using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    
    [SerializeField] private InventoryItem item;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item.itemData.itemType == ItemType.Equipment)
            Inventory.Instance.EquipItem(item.itemData);
    }

    public void UpdateSlot(InventoryItem newitem)
    {
        item = newitem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.itemData.icon;

            if (item.stackSize > 1)
                itemText.text = item.stackSize.ToString();
            else
                itemText.text = string.Empty;
        }
    }

    public InventoryItem GetItem() => item;
}
