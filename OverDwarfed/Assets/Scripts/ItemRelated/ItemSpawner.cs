using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner instance;
    public List<ItemData> items;
    private GameObject itemPrefab;

    public void SpawnItem(Vector3 position, int id, int amount)
    {
        GameObject item = Instantiate(itemPrefab, position /*+ RandomOffset() */, Quaternion.identity, transform);
        item.GetComponent<Item>().InitializeItem(items[id], amount);
    }
    public void SpawnLootTable(Vector3 position, int[] ids, int [] chances)
    {
        if (ids.Length != chances.Length) Debug.LogError("IDs doesn't match chances amount"); // FOR TESTING ERRORS IN SCR OBJECTS

        for (int i = 0; i < ids.Length; i++)
        {
            if (Random.Range(1, 101) <= chances[i])
            SpawnItem(position, ids[i], 1);   
        }
    }
    private Vector3 RandomOffset()
    {
        return new Vector3(Random.Range(-0.45f, 0.45f), Random.Range(-0.45f, 0.45f), 0f);
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
        items = new List<ItemData>();
        itemPrefab = Resources.Load<GameObject>("Items/item");

        for (int i = 0; i < 100; i++)
        {
            ItemData loadedItem = Resources.Load<ItemData>("Items/" + i.ToString());
            if (loadedItem != null) items.Add(loadedItem);
            else break;
        }
    }
    #endregion
}
