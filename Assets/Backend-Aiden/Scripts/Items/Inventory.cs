using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemList itemList;

    [SerializeField] private InventoryData inventoryData;
    [SerializeField] private GameObject[] startingItems;
    [SerializeField] private int startingItemCount = 10;

    void Start()
    {
        if (inventoryData.Value == null) inventoryData.Value = new System.Collections.Generic.Dictionary<GameObject, int>();

        if (inventoryData.Value.Count == 0)
        {
            foreach (var item in startingItems)
            {
                inventoryData.Value[item] = startingItemCount;
            }
        }
    }

    /// <summary>
    /// Adds a number of a specific item to the inventory by its ID.
    /// </summary>
    /// <param name="itemID">ID of the item to add</param>
    /// <param name="count">Count of the item to add</param>
    /// <returns>True if the item was added to the inventory, false if no item with that ID exists.</returns>
    public bool AddItem(int itemID, int count)
    {
        GameObject item = GetItemByID(itemID);
        if (item == null) return false;

        if (inventoryData.Value.ContainsKey(item))
        {
            inventoryData.Value[item] += count;
        }
        else
        {
            inventoryData.Value[item] = count;
        }
        return true;
    }

    /// <summary>
    /// Get the count of an item in the inventory by its ID.
    /// </summary>
    /// <returns>The amount of the item in the inventory. -1 if no item with that ID exists</returns>
    public int GetItemCount(int itemID)
    {
        GameObject item = GetItemByID(itemID);
        if (item == null) return -1;

        return inventoryData.Value.ContainsKey(item) ? inventoryData.Value[item] : 0;
    }

    public bool HasItem(int itemID)
    {
        GameObject item = GetItemByID(itemID);
        if (item == null) return false;

        return inventoryData.Value.ContainsKey(item) && inventoryData.Value[item] > 0;
    }

    /// <summary>
    /// Removes a number of a specific item from the inventory by its ID.
    /// </summary>
    /// <param name="itemID">The item to remove from</param>
    /// <param name="count">The amount of the item in the inventory</param>
    public void RemoveItem(int itemID, int count)
    {
        GameObject item = GetItemByID(itemID);
        if (item == null) return;

        if (inventoryData.Value.ContainsKey(item))
        {
            inventoryData.Value[item] -= count;
            if (inventoryData.Value[item] < 0)
            {
                inventoryData.Value[item] = 0;
            }
        }
    }

    private GameObject GetItemByID(int itemID)
    {
        if (itemID < 0 || itemID >= itemList.items.Length)
        {
            Debug.LogError($"Item with ID {itemID} does not exist in ItemList.");
            return null;
        }
        return itemList.items[itemID];
    }
}
