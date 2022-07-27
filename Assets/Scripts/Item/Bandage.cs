using UnityEngine;

public class Bandage : ItemInfo
{
    public Bandage()
    {
        Name = "붕대";
        ID = "bandage";
    }

    public override bool RightUse(Player player)
    {
        AudioManager.Instance.PlayOneShot("bandage_use");
        player.RemoveEffect(StatusEffectInfo.Blood);
        return true;
    }
}
