using UnityEngine;

public class Hoe : ItemInfo
{
    private const float Damage = 2.5F;
    private const float LeftClickRadius = 1F;
    public Hoe()
    {
        Name = "괭이";
        Description = "블럭을 부실 수 있습니다.";
        ID = "hoe";
    }

    public override bool LeftUse(Player player)
    {
        player.CancelAlert("Hoe_Tip");
        player.PlaySweep();
        var nearGameObjects = Physics2D.OverlapCircleAll(player.transform.position, LeftClickRadius);
        foreach (var col in nearGameObjects)
        {
            if (col.gameObject == player) continue;
            var breakable = col.GetComponent<Breakable>();
            if(breakable == null)
            {
                var enemy = col.GetComponent<Enemy>();
                enemy?.Damaged(Damage);
            }
            else breakable.Hit(true);
        }

        return false;
    }
}
