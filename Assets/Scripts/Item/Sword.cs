using UnityEngine;

public class Sword : ItemInfo
{
    private const float LeftClickRadius = 1.5F;
    private const float Damage = 15F;
    public Sword()
    {
        Name = "검";
        Description = "새것처럼 보이는 검. 날이 잘 서있다.";
        ID = "sword";
    }

    public override bool LeftUse(Player player)
    {
        player.PlaySweep();
        var nearGameObjects = Physics2D.OverlapCircleAll(player.transform.position, LeftClickRadius);
        foreach (var col in nearGameObjects)
        {
            if (col.gameObject == player) continue;
            var enemy = col.GetComponent<Enemy>();
            enemy?.Damaged(Damage);
        }
        return false;
    }
}
