using UnityEngine;

public class GuideBook : ItemInfo
{
    public GuideBook()
    {
        Name = "가이드북";
        Description = "<color=yellow>당신에게 도움이 될지도</color>";
    }
    
    public override bool RightUse(Player player)
    {
        player.ToggleGuide();
        return false;
    }
}
