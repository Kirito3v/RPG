using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class EquipmentSlotUI : ItemSlotUI
{
    [SerializeField] private RectTransform slotTransform;

    public EquipmentType slotType;

    private void Start()
    {
        slotTransform = GetComponentInParent<RectTransform>();
    }

    private void OnValidate()
    {
        slotTransform.name = "Equipment Slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.itemData == null)
            return;

        Inventory.Instance.UnEquip(item.itemData as ItemDataEquipment);
        Inventory.Instance.AddItem(item.itemData as ItemDataEquipment);

        ClearSlot();
    }
}
