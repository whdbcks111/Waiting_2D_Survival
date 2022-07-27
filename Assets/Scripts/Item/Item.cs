using UnityEngine;

public class Item
{
    public static readonly Hoe Hoe = Resources.Load<GameObject>("Item/Hoe").GetComponent<Hoe>();
    public static readonly GuideBook GuideBook = Resources.Load<GameObject>("Item/GuideBook").GetComponent<GuideBook>();
    public static readonly Food Bean = Resources.Load<GameObject>("Item/Bean").GetComponent<Food>();
    public static readonly Food Potato = Resources.Load<GameObject>("Item/Potato").GetComponent<Food>();
    public static readonly Food SweetPotato = Resources.Load<GameObject>("Item/Sweet Potato").GetComponent<Food>();
    public static readonly Food Tomato = Resources.Load<GameObject>("Item/Tomato").GetComponent<Food>();
    public static readonly Food RandomSeed = Resources.Load<GameObject>("Item/Random Seed").GetComponent<Food>();
    public static readonly HealingKit HealingKit = Resources.Load<GameObject>("Item/Healing Kit").GetComponent<HealingKit>();
    public static readonly ItemInfo Stone = Resources.Load<GameObject>("Item/Stone").GetComponent<ItemInfo>();
    public static readonly ItemInfo Coal = Resources.Load<GameObject>("Item/Coal").GetComponent<ItemInfo>();
    public static readonly ItemInfo Stick = Resources.Load<GameObject>("Item/Stick").GetComponent<ItemInfo>();
    public static readonly Water Water = Resources.Load<GameObject>("Item/Water").GetComponent<Water>();
    public static readonly PollutionWater PollutionWater = Resources.Load<GameObject>("Item/Pollution Water").GetComponent<PollutionWater>();
    public static readonly Fence Fence = Resources.Load<GameObject>("Item/Fence").GetComponent<Fence>();
    public static readonly ItemInfo Fabric = Resources.Load<GameObject>("Item/Fabric").GetComponent<ItemInfo>();
    public static readonly ItemInfo SosBeacon = Resources.Load<GameObject>("Item/Sos Beacon").GetComponent<ItemInfo>();
    public static readonly Bandage Bandage = Resources.Load<GameObject>("Item/Bandage").GetComponent<Bandage>();
    public static readonly ItemInfo Filter = Resources.Load<GameObject>("Item/Filter").GetComponent<ItemInfo>();
    public static readonly ItemInfo CookedPotato = Resources.Load<GameObject>("Item/Cooked Potato").GetComponent<ItemInfo>();
    public static readonly ItemInfo CookedSweetPotato = Resources.Load<GameObject>("Item/Cooked Sweet Potato").GetComponent<ItemInfo>();
}
