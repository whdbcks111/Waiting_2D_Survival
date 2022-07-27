using UnityEngine;

public class Storage : MonoBehaviour
{
    public static Storage Instance { get; private set; }
    public GameObject CraftingUI;
    private void Awake()
    {
        Instance = this;
    }
}
