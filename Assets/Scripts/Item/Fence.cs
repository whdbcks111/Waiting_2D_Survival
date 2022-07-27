using UnityEngine;

public class Fence : ItemInfo
{
    public Fence()
    {
        Name = "울타리";
        Description = "울타리?";
    }
    
    public override bool RightUse(Player player)
    {
        if (WorldGenerator.Instance.IsAnyBlockHere(Player.Instance.transform.position)) return false;
        var block = Instantiate(Blocks.Fence.gameObject, Vector3Int.FloorToInt(Player.Instance.transform.position), Quaternion.identity).GetComponent<Block>();
        block.IsDefault = false;
        block.transform.parent = WorldGenerator.Instance.transform;
        AudioManager.Instance.PlayOneShot("wood_break", 0.4f);
        return true;
    }
}
