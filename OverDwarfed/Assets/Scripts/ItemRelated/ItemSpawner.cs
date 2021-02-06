using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner instance;
    public Dictionary <Item, ItemData> items;
    private GameObject itemPrefab;

    public void SpawnItem(Vector3 position, Item item, int amount)
    {
        GameObject itemToSpawn = Instantiate(itemPrefab, position, Quaternion.identity, transform);
        itemToSpawn.GetComponent<ItemSpawned>().InitializeItem(items[item], amount);
    }
    public void SpawnLootTable(Vector3 position, Item[] itemsArray, int [] chances)
    {
        if (itemsArray.Length != chances.Length) Debug.LogError("IDs doesn't match chances amount"); // FOR TESTING ERRORS IN SCR OBJECTS

        for (int i = 0; i < itemsArray.Length; i++)
        {
            if (Random.Range(1, 101) <= chances[i])
            SpawnItem(position, itemsArray[i], 1);   
        }
    }
    #region Initialization
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeItemSpawner();
    }
    private void InitializeItemSpawner()
    {
        items = new Dictionary<Item, ItemData>();
        itemPrefab = Resources.Load<GameObject>("Items/item");

        for (int i = 0; i < 100; i++)
        {
            ItemData loadedItem = Resources.Load<ItemData>("Items/" + i.ToString());
            if (loadedItem != null) items.Add((Item)i, loadedItem);
            else break;
        }
    }
    #endregion
}
