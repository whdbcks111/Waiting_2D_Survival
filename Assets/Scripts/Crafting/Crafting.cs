using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Crafting : MonoBehaviour
{
    public static Crafting Instance { get; private set; }
    [SerializeField]
    private GameObject recipeContent;
    [SerializeField]
    private GameObject recipePrefab;
    [SerializeField]
    private List<CraftRecipe> recipes;

    private CraftingSlotInfo[] CraftingSlotItems;

    void Awake()
    {
        var requiredItemSlot = Resources.Load<GameObject>("RequiredItemSlot").GetComponent<Image>();
        CraftingSlotItems = new CraftingSlotInfo[recipes.Count];
        Instance = this;
        for(var i = 0; i < recipes.Count; i++)
        {
            var recipe = recipes[i];
            var slot = CraftingSlotItems[i]
                = Instantiate(recipePrefab, recipeContent.transform).GetComponent<CraftingSlotInfo>();
            
            foreach (var info in recipe.requireItems) {
                var img = Instantiate(requiredItemSlot, slot.RequiredItemSlot);
                img.sprite = info.ItemInfo.Sprite;
                img.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(info.Amount + "");
            }

            slot.Icon.sprite = recipe.itemInfo.ItemInfo.Sprite;
            slot.Name.SetText(recipe.itemInfo.ItemInfo.Name);
            slot.Description.SetText(recipe.itemInfo.ItemInfo.Description);

            slot.Button.onClick.AddListener(() => {
                CraftItem(recipe);
            });
        }
    }

    private void Update()
    {
        for(var i = 0; i < recipeContent.transform.childCount; i++)
        {
            var slot = CraftingSlotItems[i];
            var recipe = recipes[i];
            slot.CannotCraft.gameObject.SetActive(!CanCraft(recipe));
        }
    }

    public CraftRecipe GetRecipe(string name)
    {
        foreach(var recipe in recipes)
        {
            if(recipe.itemInfo.ItemInfo.Name == name) return recipe;
        }
        return null;
    }

    public bool CanCraft(CraftRecipe recipe)
    {
        foreach (var i in recipe.requireItems)
        {
            if(Player.Instance.Inventory.GetAmount(i.ItemInfo) < i.Amount)
                return false;
        }
        return true;
    }

    public void CraftItem(CraftRecipe recipe)
    {
        if(!CanCraft(recipe)) 
        {
            AudioManager.Instance.PlayOneShot("unable_alert", 0.4f);
            return;
        }
        AudioManager.Instance.PlayOneShot("craft", 0.4f);
        foreach (var i in recipe.requireItems) {
            Player.Instance.Inventory.SubItem(i.ItemInfo, i.Amount);
        }
        Player.Instance.Inventory.AddItem(recipe.itemInfo.ItemInfo, recipe.itemInfo.Amount);
    }
}
