using UnityEngine;

public class PollutionWater : Water
{
    public PollutionWater()
    {
        ID = "pollution_water";
        WaterValue = 15;
    }

    public override bool RightUse(Player player)
    {
        var success = base.RightUse(player);
        if(success && Random.Range(0, 1F) < .3F)
        {
            player.AddEffect(new(StatusEffectInfo.StomachAche, 1, 10));
        }
        return success;
    }
}
