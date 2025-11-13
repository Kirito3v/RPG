using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class InventoryUI : UI
{
    public static InventoryUI Instance { get; private set; }
    private Inventory inventory;

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

    protected override void Start()
    {
        equipmentSlot = equipmentSlotRoot.GetComponentsInChildren<EquipmentSlotUI>();
        inventory = Inventory.Instance;
        
        base.Start();

        Inventory.Instance.OnInventoryChanged += UpdateUI;
        UpdateUI();
    }

    private InventoryUI DestroyAndReturnNull()
    {
        Destroy(gameObject);
        return null;
    }

    public override void SwitchTo(GameObject menu)
    {
        base.SwitchTo(menu);
        if (menu != null)
            UpdateUI();
    }

    private void OnDisable()
    {
        if (Inventory.Instance != null)
            Inventory.Instance.OnInventoryChanged -= UpdateUI;
    }

    #region UI Management
    public void UpdateUI()
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
            foreach (var kvp in inventory.equipmentDict)
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
        if (inventorySlotRoot.gameObject.activeInHierarchy)
            foreach (var item in inventory.inventory)
            {
                GameObject s = equipmentSlotPool.Get();
                s?.transform.SetParent(inventorySlotRoot, false);
                var slot = s.GetComponentInChildren<ItemSlotUI>();
                slot?.UpdateSlot(item);
            }
    }

    private void UpdateStashUI()
    {
        if (stashSlotRoot.gameObject.activeInHierarchy)
            foreach (var item in inventory.stash)
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
