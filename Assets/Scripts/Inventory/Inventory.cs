using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<InventoryItem> equipment = new();
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDict = new();

    public List<InventoryItem> inventory = new();
    public Dictionary<ItemData, InventoryItem> inventoryDict = new();

    public List<InventoryItem> stash = new();
    public Dictionary<ItemData, InventoryItem> stashDict = new();

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotRoot;
    [SerializeField] private Transform stashSlotRoot;
    [SerializeField] private Transform equipmentSlotRoot;
    [SerializeField] private RectTransform inventoryScroll;
    [SerializeField] private GameObject itemSlotPrefab;

    private ObjectPool<GameObject> equipmentSlotPool;
    private ObjectPool<GameObject> stashSlotPool;
    private EquipmentSlotUI[] equipmentSlot;

    private void Awake()
    {
        Instance = Instance == null ? this : DestroyAndReturnNull();

        InitializePools();
    }

    private void Start()
    {
        equipmentSlot = equipmentSlotRoot.GetComponentsInChildren<EquipmentSlotUI>();
        UpdateUI();
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

        RemoveItem(_item);
        UpdateUI();
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

        UpdateUI();
    }

    //private void AddToStash(ItemData item)
    //{
    //    if (stashDict.TryGetValue(item, out InventoryItem value))
    //        value.AddStack();
    //    else
    //    {
    //        InventoryItem newItem = new(item);
    //        stash.Add(newItem);
    //        stashDict[item] = newItem;
    //    }
    //}

    //private void AddToInventory(ItemData item)
    //{
    //    if (inventoryDict.TryGetValue(item, out InventoryItem value))
    //        value.AddStack();
    //    else
    //    {
    //        InventoryItem newItem = new(item);
    //        inventory.Add(newItem);
    //        inventoryDict[item] = newItem;
    //    }
    //}

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

    public void RemoveItem(ItemData item)
    {
        if (inventoryDict.TryGetValue(item, out InventoryItem inv))
        {
            if (inv.stackSize <= 1)
            {
                inventory.Remove(inv);
                inventoryDict.Remove(item);
            }
            else inv.RemoveStack();
        }

        if (stashDict.TryGetValue(item, out InventoryItem stashVal))
        {
            if (stashVal.stackSize <= 1)
            {
                stash.Remove(stashVal);
                stashDict.Remove(item);
            }
            else stashVal.RemoveStack();
        }

        UpdateUI();
    }
    #endregion

    #region UI Management
    private void UpdateUI()
    {
        ClearSlotUI();
        UpdateEquipmentUI();
        UpdateInventoryUI();
        UpdateStashUI();

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryScroll);
    }

    private void UpdateEquipmentUI()
    {
        foreach (var slot in equipmentSlot)
        {
            slot.ClearSlot();
            foreach (var kvp in equipmentDict)
            {
                if (kvp.Key.equipmentType == slot.slotType)
                {
                    slot.UpdateSlot(kvp.Value);
                    break;
                }
            }
        }
    }

    private void UpdateInventoryUI()
    {
        foreach (var item in inventory)
        {
            GameObject s = equipmentSlotPool.Get();
            s?.transform.SetParent(inventorySlotRoot, false);
            var slot = s.GetComponentInChildren<ItemSlotUI>();
            slot?.UpdateSlot(item);
        }
    }

    private void UpdateStashUI()
    {
        foreach (var item in stash)
        {
            GameObject s = stashSlotPool.Get();
            s?.transform.SetParent(stashSlotRoot, false);
            var slot = s.GetComponentInChildren<ItemSlotUI>();
            slot?.UpdateSlot(item);
        }
    }

    private void ClearSlotUI()
    {
        foreach (Transform child in inventorySlotRoot)
            if (child.gameObject.activeInHierarchy)
                equipmentSlotPool.Release(child.gameObject);

        foreach (Transform child in stashSlotRoot)
            if (child.gameObject.activeInHierarchy)
                stashSlotPool.Release(child.gameObject);
    }

    private void InitializePools()
    {
        equipmentSlotPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(itemSlotPrefab);
                obj.SetActive(false);
                return obj;
            },
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 100
        );

        stashSlotPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(itemSlotPrefab);
                obj.SetActive(false);
                return obj;
            },
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 100
        );
    }
    #endregion
}
