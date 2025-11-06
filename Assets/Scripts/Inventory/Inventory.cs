using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDict;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDict;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDict;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotRoot;
    [SerializeField] private Transform stashSlotRoot;
    [SerializeField] private Transform equipmentSlotRoot;
    [SerializeField] private RectTransform inventoryScroll;
    [SerializeField] private GameObject itemSlotPrefab;

    private ObjectPool<GameObject> slotPool;
    private ItemSlotUI[] inventoryItemSlot;
    private ItemSlotUI[] stashItemSlot;
    private EquipmentSlotUI[] equipmentSlot;

    private void Awake()
    {
        Instance = Instance == null ? this : DestroyAndReturnNull();

        InitializePools();
    }

    private void Start()
    {
        equipment = new List<InventoryItem>();
        equipmentDict = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventory = new List<InventoryItem>();
        inventoryDict = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDict = new Dictionary<ItemData, InventoryItem>();

        //itemSlot = inventorySlotRoot.GetComponentsInChildren<ItemSlotUI>();
        equipmentSlot = equipmentSlotRoot.GetComponentsInChildren<EquipmentSlotUI>();
    }

    private Inventory DestroyAndReturnNull()
    {
        Destroy(gameObject);
        return null;
    }

    public void EquipItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDict)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }

        if (oldEquipment != null)
        {
            UnEquip(oldEquipment);
            AddItem(oldEquipment);
        }


        equipment.Add(newItem);
        equipmentDict.Add(newEquipment, newItem);
        RemoveItem(_item);

        UpdateUI(newItem);
    }

    private void UnEquip(ItemDataEquipment itemToRemove)
    {
        if (equipmentDict.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDict.Remove(itemToRemove);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item.itemType == ItemType.Equipment)
            AddToInventory(item);
        else if (item.itemType == ItemType.Material)
            AddToStash(item);
        UpdateUI(null);
    }

    private void AddToStash(ItemData item)
    {
        if (stashDict.TryGetValue(item, out InventoryItem value))
            value.AddStack();
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stash.Add(newItem);
            stashDict.Add(item, newItem);
        }
    }

    private void AddToInventory(ItemData item)
    {
        if (inventoryDict.TryGetValue(item, out InventoryItem value))
            value.AddStack();
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventory.Add(newItem);
            inventoryDict.Add(item, newItem);
        }
    }

    public void RemoveItem(ItemData item)
    {
        if (inventoryDict.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDict.Remove(item);
            }
            else
                value.RemoveStack();
        }

        if (stashDict.TryGetValue(item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDict.Remove(item);
            }
            else
                stashValue.RemoveStack();
        }

        UpdateUI(null);
    }

    private void UpdateUI(InventoryItem itemToRemove)
    {
        UpdateSlotUI(itemToRemove);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryScroll.GetComponent<RectTransform>());
    }

    private void UpdateSlotUI(InventoryItem itemToRemove = null)
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDict)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }

        RemoveEquipmentUI(itemToRemove);

        inventoryItemSlot = inventorySlotRoot.GetComponentsInChildren<ItemSlotUI>();
        stashItemSlot = stashSlotRoot.GetComponentsInChildren<ItemSlotUI>();
        
        ClearSlotUI();

        for (int i = 0; i < inventory.Count; i++)
        {
            GameObject s = slotPool.Get();
            s.transform.SetParent(inventorySlotRoot, false);
            ItemSlotUI slot = s.GetComponentInChildren<ItemSlotUI>();
            slot.UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            GameObject s = slotPool.Get();
            s.transform.SetParent(stashSlotRoot, false);
            ItemSlotUI slot = s.GetComponentInChildren<ItemSlotUI>();
            slot.UpdateSlot(stash[i]);
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryScroll.GetComponent<RectTransform>());
    }

    private void ClearSlotUI()
    {
        foreach (Transform slot in inventorySlotRoot)
            slotPool.Release(slot.gameObject);

        foreach (Transform slot in stashSlotRoot)
            slotPool.Release(slot.gameObject);
    }

    private void RemoveEquipmentUI(InventoryItem itemToRemove)
    {
        if (itemToRemove == null)
            return;

        inventoryItemSlot = inventorySlotRoot.GetComponentsInChildren<ItemSlotUI>();
        stashItemSlot = stashSlotRoot.GetComponentsInChildren<ItemSlotUI>();

        foreach (ItemSlotUI slot in inventoryItemSlot)
            if (slot != null && slot.GetItem() == itemToRemove)
                slotPool.Release(slot.gameObject);

        //foreach (ItemSlotUI slot in stashItemSlot)
        //    if (slot != null && slot.GetItem() == itemToRemove)
        //        slotPool.Release(slot.gameObject);
    }

    private void InitializePools()
    {
        slotPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(itemSlotPrefab);
                obj.SetActive(false);
                return obj;
            },
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 100
        );
    }
}
