using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlotInfo : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    public Image Icon => _icon;
    
    [SerializeField]
    private Transform _requiredItemSlot;
    public Transform RequiredItemSlot => _requiredItemSlot;
    
    [SerializeField]
    private TextMeshProUGUI _name;
    public TextMeshProUGUI Name => _name;
    
    [SerializeField]
    private TextMeshProUGUI _description;
    public TextMeshProUGUI Description => _description;

    [SerializeField]
    private Button _button;
    public Button Button => _button;

    [SerializeField]
    private TextMeshProUGUI _cannotCraft;
    public TextMeshProUGUI CannotCraft => _cannotCraft;
}
