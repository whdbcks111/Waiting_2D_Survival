using System.Collections.Generic;
using UnityEngine;

public class Blocks
{
    public static readonly Block Bean = Resources.Load<GameObject>("Bean").GetComponent<Block>();
    public static readonly Block Potato = Resources.Load<GameObject>("Potato").GetComponent<Block>();
    public static readonly Block SweetPotato = Resources.Load<GameObject>("Sweet Potato").GetComponent<Block>();
    public static readonly Block Tomato = Resources.Load<GameObject>("Tomato").GetComponent<Block>();
    public static readonly Block Fence = Resources.Load<GameObject>("Fence").GetComponent<Block>();
    public static readonly Block Bush = Resources.Load<GameObject>("Bush").GetComponent<Block>();
    public static readonly Block Stone = Resources.Load<GameObject>("Stone").GetComponent<Block>();
    public static readonly Block Crafting = Resources.Load<GameObject>("Crafting").GetComponent<Block>();
}
