using UnityEngine;

public class Seeds : ItemInfo
{
    [SerializeField]
    private GameObject _resource;
    
    public Seeds()
    {
        Name = "감자 씨앗";
        Description = "<color=green>감자</color> 씨앗.";
        ID = "potato_seeds";
    }

    public override bool RightUse(Player player)
    {
        if (WorldGenerator.Instance.IsAnyBlockHere(Player.Instance.transform.position)) return false;

        AudioManager.Instance.PlayOneShot("seed_use", .4f);
        var block = Instantiate(_resource, Vector3Int.FloorToInt(Player.Instance.transform.position), Quaternion.identity).GetComponent<Block>();
        block.IsDefault = false;
        block.transform.parent = WorldGenerator.Instance.transform;
        return true;
    }
}
