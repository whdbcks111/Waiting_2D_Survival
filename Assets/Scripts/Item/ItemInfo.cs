using System;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public string Name;
    public string Description;
    public Sprite Sprite;

    public string ID;

    public int Amount;

    public static bool operator==(ItemInfo a, object b)
    {
        if (b is not ItemInfo other) return false;
        return a.ID == other.ID;
    }
    public static bool operator !=(ItemInfo a, object other) => !(a == other);

    public override bool Equals(object obj)
    {
        return this == obj;
    }

    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }

    public virtual bool LeftUse(Player player) => false;
    public virtual bool RightUse(Player player) => false;
}
