using UnityEngine;

public class HealingKit : ItemInfo
{
    public HealingKit()
    {
        Name = "구급키트";
        Description = "체력 <color=red>30</color> 회복";
        ID = "healing_kit";
    }

    public override bool RightUse(Player player)
    {
        if (player.HealthPoint < Player.MaxHp)
        {
            AudioManager.Instance.PlayOneShot("healing_kit_use");
            player.HealthPoint += 30;
            return true;
        }
        return false;
    }
}
