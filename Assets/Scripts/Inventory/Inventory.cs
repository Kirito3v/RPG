using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDict;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotRoot;
    [SerializeField] private GameObject itemSlotPrefab;
    private ItemSlotUI[] itemSlot;

    private void Awake()
    {
        Instance = Instance == null ? this : DestroyAndReturnNull();
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDict = new Dictionary<ItemData, InventoryItem>();

        //itemSlot = inventorySlotRoot.GetComponentsInChildren<ItemSlotUI>();
    }

    private Inventory DestroyAndReturnNull()
    {
        Destroy(gameObject);
        return null;
    }

    public void AddItem(ItemData item) 
    {
        if (inventoryDict.TryGetValue(item, out InventoryItem value))
            value.AddStack();
        else
        {
            GameObject newSlot = Instantiate(itemSlotPrefab, inventorySlotRoot);
            InventoryItem newItem = new InventoryItem(item);
            inventoryItems.Add(newItem);
            inventoryDict.Add(item, newItem);
        }
        UpdateSlotUI();
    }

    public void RemoveItem(ItemData item)
    {
        if (inventoryDict.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDict.Remove(item);
            }
            else
                value.RemoveStack();
        }
        UpdateSlotUI();
    }

    private void UpdateSlotUI()
    {
        itemSlot = inventorySlotRoot.GetComponentsInChildren<ItemSlotUI>();

        for (int i = 0; i < inventoryItems.Count; i++)
            itemSlot[i].UpdateSlot(inventoryItems[i]);
    }
}
