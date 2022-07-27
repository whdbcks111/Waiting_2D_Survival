using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    private const float RightClickRadius = 1.2F;

    public static Player Instance { get; private set; }
    private Animator _animator;

    public const float DefaultMovePower = 3F;
    public const float MaxHp = 100;
    public const float MaxFood = 100;
    public const float MaxWater = 100;
    public float HealthPoint { get; internal set; } = MaxHp;
    public float Food { get; internal set; } = MaxFood;
    public float Water { get; internal set; } = MaxWater;
    [HideInInspector]
    public float MovePower = 4F;
    [HideInInspector]
    public bool Blind = false;


    [SerializeField]
    private Image _hpDisplay;
    [SerializeField]
    private Image _foodDisplay;
    [SerializeField]
    private Image _waterDisplay;
    [SerializeField]
    private TextMeshProUGUI _hpText;
    [SerializeField]
    private TextMeshProUGUI _foodText;
    [SerializeField]
    private TextMeshProUGUI _waterText;
    [SerializeField]
    private Image[] slotImages;
    [SerializeField]
    private RectTransform slotSelector;
    [SerializeField]
    private GameObject inventoryView;
    [SerializeField]
    private GameObject inventoryContentView;
    [SerializeField]
    private GameObject inventoryItemSlotPrefab;
    [SerializeField]
    private Image defaultBlind;
    [SerializeField]
    private Image darkerBlind;
    [SerializeField]
    private Animator sweepAnimator;
    [SerializeField]
    private GameObject sweepObj;
    [SerializeField]
    private TextMeshProUGUI effectsText, alertsText;
    [SerializeField]
    private GameObject guideBook;
    [SerializeField]
    private TextMeshProUGUI positionDisplay;

    public const float FoodDecreaseSpan = 7f;
    public const float WaterDecreaseSpan = 5f;

    public int HoldItemSlot { get; private set; }
    public Inventory Inventory { get; private set; } = new();
    [HideInInspector]
    private List<StatusEffect> effects = new(), removeEffects = new();
    private List<AlertText> alerts = new();

    private bool _canLeftClick = true;
    private bool _canRightClick = true;

    private List<Block> _enteredBlock = new();

    internal ChunkCoord _chunkCoord;

    private void Awake()
    {
        Instance = this;
        Inventory.AddItem(Item.Hoe);
        Inventory.AddItem(Item.GuideBook);
        _animator = GetComponent<Animator>();
        StartCoroutine(nameof(FoodChange));
        StartCoroutine(nameof(WaterChange));
        ChangeInventoryView();

        Alert("Inv_Tip", "<color=yellow>E 또는 Tab을 눌러 가방 열기</color>", 30);
        Alert("Hoe_Tip", "<color=yellow>괭이를 들고 지형 주변에서 좌클릭하여 파밍하기</color>", 60);
    }

    public void ToggleGuide()
    {
        guideBook.SetActive(!guideBook.activeSelf);
        AudioManager.Instance.PlayOneShot("book");
    }

    private void ChangeInventoryView()
    {
        while (inventoryContentView.transform.childCount > 0)
        {
            var tr = inventoryContentView.transform.GetChild(0);
            tr.SetParent(null);
            Destroy(tr.gameObject);
        }
        foreach (var i in Inventory.Contents)
        {
            var slot = Instantiate(inventoryItemSlotPrefab, inventoryContentView.transform);
            var itemName = slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var itemDesc = slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            var itemCount = slot.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            var itemSprite = slot.transform.GetChild(1).GetComponent<Image>();
            var changeSlotBtns = new Button[]{
                slot.transform.GetChild(4).GetComponent<Button>(),
                slot.transform.GetChild(5).GetComponent<Button>(),
                slot.transform.GetChild(6).GetComponent<Button>()
            };

            itemName.SetText(i.Name);
            itemDesc.SetText(i.Description);
            itemCount.SetText(string.Format("개수 : {0}", i.Amount));
            itemSprite.sprite = i.Sprite;

            for (var j = 0; j < 3; j++)
            {
                int finalJ = j;
                changeSlotBtns[j].onClick.AddListener(() =>
                {
                    if (finalJ >= Inventory.Count) return;
                    Inventory.ChangeIndex(i, finalJ);
                });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LeftClick();
        RightClick();
        UpdateValues();
        InventoryUpdate();
        StatusEffectUpdate();
        AlertUpdate();
        PosDisplayUpdate();
        BlockEnterUpdate();
    }

    private void PosDisplayUpdate()
    {
        positionDisplay.SetText(string.Format("좌표: ({0}, {1})", (int)transform.position.x, (int)transform.position.y));
    }

    public void AddEffect(StatusEffect statusEffect)
    {
        foreach (var eff in effects)
        {
            if (eff.Info == statusEffect.Info) return;
        }
        effects.Add(statusEffect);
    }

    public void RemoveEffect(StatusEffectInfo statusEffectInfo)
    {
        foreach (var eff in effects)
        {
            if (eff.Info == statusEffectInfo) removeEffects.Add(eff);
        }
    }

    public void CancelAlert(string id) => alerts.Remove(new AlertText(id, string.Empty, 0));

    public void Alert(string id, string text, float duration=1f)
    {
        var alert = new AlertText(id, text, duration);
        var index = alerts.IndexOf(alert);
        if (index == -1)
            alerts.Add(alert);
        else
        {
            alert = alerts[index];
            alert.Text = text;
            alert.Duration = duration;
        }
    }

    private void AlertUpdate()
    {
        List<AlertText> removeAlerts = new();
        string listTextStr = "";
        foreach (var a in alerts)
        {
            listTextStr += a.Text + "\n";
            if ((a.Duration -= Time.deltaTime) <= 0) removeAlerts.Add(a);
        }
        alertsText.SetText(listTextStr.Trim());
        foreach (var a in removeAlerts) alerts.Remove(a);
    }

    private void StatusEffectUpdate()
    {
        string listTextStr = "";
        foreach (var eff in effects)
        {
            listTextStr += "<color=yellow>Lv." + eff.Level + "</color> " + eff.Info.Name + " <color=#f55>" + (int)eff.Duration + "초</color>\n";
            eff.Info.Action(eff);
            if ((eff.Duration -= Time.deltaTime) <= 0) removeEffects.Add(eff);
        }
        effectsText.SetText(listTextStr.Trim());
        foreach (var eff in removeEffects) effects.Remove(eff);

        Color c = darkerBlind.color;
        c.a = Mathf.Lerp(c.a, Blind ? 1f : 0f, Time.deltaTime * 2);
        darkerBlind.color = c;
        Blind = false;
    }

    private void ChangeHoldSlot(int slotIdx)
    {
        HoldItemSlot = slotIdx % 3;
        int selectorX = (HoldItemSlot - 1) * 102;
        Vector2 pos = slotSelector.anchoredPosition;
        pos.x = selectorX;
        slotSelector.anchoredPosition = pos;

        if (HoldItemSlot < Inventory.Count)
            Alert("HoldChange", Inventory.GetItem(HoldItemSlot)?.Name);

        guideBook.SetActive(false);
    }

    private void InventoryUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeHoldSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeHoldSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeHoldSlot(2);

        if (Input.mouseScrollDelta.y != 0f)
        {
            AudioManager.Instance.PlayOneShot("slot_change");
            ChangeHoldSlot(HoldItemSlot + (Input.mouseScrollDelta.y > 0 ? 2 : 1));
        }

        if (Inventory.IsChanged)
        {
            for (var i = 0; i < slotImages.Length; i++)
            {
                if (Inventory.Count > i)
                {
                    var item = Inventory.GetItem(i);
                    slotImages[i].transform.GetChild(1).GetComponent<Image>().sprite = item.Sprite;
                    slotImages[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(item.Amount + "");
                }
                for (var j = 0; j < slotImages[i].transform.childCount; j++)
                    slotImages[i].transform.GetChild(j).gameObject.SetActive(Inventory.Count > i);
            }
            Inventory.Updated();
            ChangeInventoryView();
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.E))
        {
            CancelAlert("Inv_Tip");
            AudioManager.Instance.PlayOneShot("slot_change");
            inventoryView.SetActive(!inventoryView.activeSelf);
        }
    }

    IEnumerator FoodChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(FoodDecreaseSpan);
            Food -= 2;
        }
    }

    IEnumerator WaterChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(WaterDecreaseSpan);
            Water -= 1;
        }
    }

    private void UpdateValues()
    {
        Food = Mathf.Clamp(Food, 0, MaxFood);
        HealthPoint = Mathf.Clamp(HealthPoint, 0, MaxHp);
        Water = Mathf.Clamp(Water, 0, MaxWater);

        _hpDisplay.fillAmount = Mathf.Clamp01(HealthPoint / MaxHp);
        _foodDisplay.fillAmount = Mathf.Clamp01(Food / MaxFood);
        _waterDisplay.fillAmount = Mathf.Clamp01(Water / MaxWater);

        _hpText.SetText(string.Format("{0}%", (int)(HealthPoint / MaxHp * 100)));
        _waterText.SetText(string.Format("{0}%", (int)(Water / MaxWater * 100)));
        _foodText.SetText(string.Format("{0}%", (int)(Food / MaxFood * 100)));

        if (Food < MaxFood * 0.25f) AddEffect(new(StatusEffectInfo.Hungry, 3, 5));
        if (Food <= 0) HealthPoint -= Time.deltaTime * 1f;
        if (Water <= 0) HealthPoint -= Time.deltaTime * 2f;

        if (HealthPoint <= 0) SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
    }

    private void BlockEnterUpdate()
    {
        var enteredBlock = new List<Block>();
        var nearGameObjects = Physics2D.OverlapCircleAll(transform.position, 1.8F);
        foreach (var col in nearGameObjects)
        {
            var block = col.gameObject.GetComponent<Block>();
            if (block == null) continue;
            block.PlayerEnter();
            enteredBlock.Add(block);
            _enteredBlock.Remove(block);
        }

        foreach (var block in _enteredBlock)
            block.PlayerExit();

        _enteredBlock = enteredBlock;
    }

    private void Move()
    {
        var direction = Vector2.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction += Vector2.right;
        }
        var walkMode = 0;
        if (direction.x > 0) walkMode = 4;
        if (direction.x < 0) walkMode = 2;
        if (direction.y > 0) walkMode = 1;
        if (direction.y < 0) walkMode = 3;
        if (walkMode != 0) _animator.SetInteger("WalkMode", walkMode);
        if (direction.magnitude == 0)
        {
            _animator.speed = 0;
        }
        else
        {

            _animator.speed = .5f;
        }
        transform.Translate(direction.normalized * Time.deltaTime * MovePower, Space.World);
        MovePower = DefaultMovePower;
    }

    private void LeftClick()
    {
        if (!_canLeftClick) return;
        if (!Input.GetMouseButton(0)) return;
        if (HoldItemSlot >= Inventory.Count) return;
        var item = Inventory.GetItem(HoldItemSlot);
        if (item.LeftUse(this)) Inventory.SubItem(item);
        _canLeftClick = false;
        StartCoroutine(nameof(LeftClickTimerStart));
    }

    private IEnumerator LeftClickTimerStart()
    {
        yield return new WaitForSeconds(0.5f);
        _canLeftClick = true;
    }

    private void RightClick()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        var nearGameObjects = Physics2D.OverlapCircleAll(transform.position, RightClickRadius);
        foreach (var col in nearGameObjects)
        {
            var interactive = col.gameObject.GetComponent<Interactive>();
            if (interactive != null)
            {
                interactive.Interact();
                return;
            }
        }

        if (!Input.GetMouseButtonDown(1)) return;
        if (!_canRightClick) return;
        if (!Input.GetMouseButton(1)) return;
        if (HoldItemSlot >= Inventory.Count) return;
        var item = Inventory.GetItem(HoldItemSlot);
        if (item.RightUse(this)) Inventory.SubItem(item);
        _canRightClick = false;
        StartCoroutine(nameof(RightClickTimerStart));
    }

    private IEnumerator RightClickTimerStart()
    {
        yield return new WaitForSeconds(0.5f);
        _canRightClick = true;
    }

    public void PlaySweep()
    {
        AudioManager.Instance.PlayOneShot("sweep", .3f);
        StartCoroutine(nameof(SweepCoroutine));
    }

    public IEnumerator SweepCoroutine()
    {
        sweepObj.SetActive(true);
        sweepAnimator.SetTrigger("Sweep");
        while (sweepAnimator.GetCurrentAnimatorStateInfo(0).length >
            sweepAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime) yield return null;
        yield return new WaitForSeconds(0.2f);
        sweepObj.SetActive(false);
    }
}
