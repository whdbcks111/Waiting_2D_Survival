public class Food : ItemInfo
{
    public int FoodValue = 40;
    public int WaterValue = 0;
    public Food()
    {
        Name = "고구마";
        Description = "배고픔 <color=blue>40</color> 회복";
        ID = "sweet_potato";
    }

    public override bool RightUse(Player player)
    {
        AudioManager.Instance.PlayOneShot("food");
        player.Food += FoodValue;
        player.Water += WaterValue;
        return true;
    }
}
