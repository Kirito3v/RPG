using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private InventoryUI UI;
    public System.Action OnInventoryChanged;

    public List<InventoryItem> equipment = new();
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDict = new();

    public List<InventoryItem> inventory = new();
    public Dictionary<ItemData, InventoryItem> inventoryDict = new();

    public List<InventoryItem> stash = new();
    public Dictionary<ItemData, InventoryItem> stashDict = new();

    private void Awake()
    {
        Instance = Instance == null ? this : DestroyAndReturnNull();
    }

    private void Start()
    {
        UI = InventoryUI.Instance;
    }

    private Inventory DestroyAndReturnNull()
    {
        Destroy(gameObject);
        return null;
    }

    #region Inventory Management
    public void EquipItem(ItemData _item)
    {
        if (_item is not ItemDataEquipment newEquipment)
            return;

        InventoryItem newItem = new(newEquipment);
        ItemDataEquipment oldEquipment = null;

        // Find existing equipment of same type
        foreach (var kvp in equipmentDict)
        {
            if (kvp.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = kvp.Key;
                break;
            }
        }

        // Swap old equipment
        if (oldEquipment != null)
        {
            UnEquip(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDict[newEquipment] = newItem;
        newEquipment.AddModifiers();

        RemoveItem(_item, inventory, inventoryDict);
        OnInventoryChanged?.Invoke();
        //UI.UpdateUI();
    }

    public void UnEquip(ItemDataEquipment itemToRemove)
    {
        if (equipmentDict.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDict.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    public void AddItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.Equipment:
                AddItemToCollection(item, inventory, inventoryDict);
                break;
            case ItemType.Material:
                AddItemToCollection(item, stash, stashDict);
                break;
        }

        OnInventoryChanged?.Invoke();
        //UI.UpdateUI();
    }

    private void AddItemToCollection<T>(T item, List<InventoryItem> list, Dictionary<T, InventoryItem> dict) where T : ItemData
    {
        if (dict.TryGetValue(item, out InventoryItem value))
            value.AddStack();
        else
        {
            InventoryItem newItem = new(item);
            list.Add(newItem);
            dict[item] = newItem;
        }
    }

    //public void RemoveItem(ItemData item)
    //{
    //    if (inventoryDict.TryGetValue(item, out InventoryItem inv))
    //    {
    //        if (inv.stackSize <= 1)
    //        {
    //            inventory.Remove(inv);
    //            inventoryDict.Remove(item);
    //        }
    //        else inv.RemoveStack();
    //    }

    //    if (stashDict.TryGetValue(item, out InventoryItem stashVal))
    //    {
    //        if (stashVal.stackSize <= 1)
    //        {
    //            stash.Remove(stashVal);
    //            stashDict.Remove(item);
    //        }
    //        else stashVal.RemoveStack();
    //    }

    //    UI.UpdateUI();
    //}

    public void RemoveItem<T>(T item, List<InventoryItem> list, Dictionary<T, InventoryItem> dict)
    {
        if (dict.TryGetValue(item, out InventoryItem val))
        {
            if (val.stackSize <= 1)
            {
                list.Remove(val);
                dict.Remove(item);
            }
            else val.RemoveStack();
        }

        OnInventoryChanged?.Invoke();
        //UI.UpdateUI();
    }
    #endregion
}
