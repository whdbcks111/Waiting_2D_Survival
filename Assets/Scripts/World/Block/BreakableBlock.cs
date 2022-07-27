using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BreakableBlock : Block, Breakable
{
    [SerializeField]
    private int _maxHealth = 1;
    private int _health;

    private GameObject _bar;
    private GameObject _back;
    
    protected SpriteRenderer _spriteRenderer;
    
    [SerializeField]
    private DropInfo[] _dropItems;
    [SerializeField]
    private string _hitSound = "bush_break";
    [SerializeField]
    private float _volume = 1f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _back = Instantiate(Resources.Load<GameObject>("ObjectHpBar"), transform);
        _bar = _back.transform.GetChild(0).gameObject;
        
        if (_maxHealth == 0) _maxHealth = 1;
        _health = _maxHealth;
    }

    public virtual void Hit(bool isPlayer)
    {
        AudioManager.Instance.PlayOneShot(_hitSound, _volume);
        StartCoroutine(nameof(HitDisplay));
        _health--;

        _back.SetActive(true);
        StartCoroutine(nameof(HealthDisplay), isPlayer);
    }

    private IEnumerator HealthDisplay(bool isPlayer) {
        float targetX = Mathf.Clamp01(_health / (float)_maxHealth);
        var scale = _bar.transform.localScale;
        while(scale.x > targetX + 0.01) {
            yield return null;
            scale.x = Mathf.Lerp(scale.x, targetX, Time.deltaTime * 8);
            _bar.transform.localScale = scale;
        }
        if (_health <= 0) Break(isPlayer);
    }

    private IEnumerator HitDisplay()
    {
        var color = _spriteRenderer.color;
        var changeColor = _spriteRenderer.color;
        changeColor.a = .5F;
        _spriteRenderer.color = changeColor;
        yield return new WaitForSeconds(.1F);
        _spriteRenderer.color = color;
    }

    public virtual void Break(bool isPlayer)
    {
        if(isPlayer)
        {
            foreach(var info in _dropItems)
            {
                if (Random.Range(0, 1F) < info.Percent)
                {
                    Player.Instance.Inventory.AddItem(info.Item);
                }
            }
        }
        Destroy(gameObject);
    }
}
