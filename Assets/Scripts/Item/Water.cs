using UnityEngine;

public class Water : ItemInfo
{
    [SerializeField]
    protected float WaterValue = 20;
    public Water()
    {
        Name = "물";
        Description = "<color=lightblue>물</color>";
        ID = "water";
    }

    public override bool RightUse(Player player)
    {
        if(player.Water < Player.MaxWater)
        {
            AudioManager.Instance.PlayOneShot("water_use");
            player.Water += WaterValue;
            return true;
        }
        return false;
    }
}
