using UnityEngine;

public class CraftingBlock : Block, Interactive
{
    [SerializeField]
    private GameObject _craftingUI;
    private bool _active;

    public void Interact()
    {
        AudioManager.Instance.PlayOneShot("door");
        _active = true;
        Storage.Instance.CraftingUI.SetActive(true);
    }

    public void DeInteract()
    {
        Storage.Instance.CraftingUI.SetActive(false);
        _active = false;
    }

    protected override void PlayerExit2d()
    {
        DeInteract();
    }

    protected override void PlayerStay2d()
    {
        Player.Instance.Alert("CraftingBlock_Tip", "우클릭으로 <color=orange>작업대</color>에 상호작용", 0.1f);
    }

    private void Update()
    {
        if (_active && Input.GetKeyDown(KeyCode.Escape))
        {
            DeInteract();
        }
    }
}
