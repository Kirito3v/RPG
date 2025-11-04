using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ItemSlotUI : MonoBehaviour
{
    //[SerializeField] private Image imgBG;
    //[SerializeField] private Image imgFG;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    
    
    [SerializeField] private InventoryItem item;

    public void UpdateSlot(InventoryItem newitem)
    {
        item = newitem;

        //imgBG.enabled = true;
        //imgFG.enabled = true;
        //itemImage.enabled = true;
        //itemText.enabled = true;

        if (item != null)
        {
            itemImage.sprite = item.itemData.icon;

            if (item.stackSize > 1)
                itemText.text = item.stackSize.ToString();
            else
                itemText.text = string.Empty;
        }
    }
}
