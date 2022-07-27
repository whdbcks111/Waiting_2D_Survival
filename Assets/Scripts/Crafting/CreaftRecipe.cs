using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CraftRecipe {
    public CraftItemInfo itemInfo;
    public List<CraftItemInfo> requireItems;
}