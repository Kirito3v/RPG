using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
