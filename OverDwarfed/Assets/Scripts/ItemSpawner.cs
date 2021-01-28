using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner instance;
    private List<ItemData> items;

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
        for (int i = 0; i < 100; i++)
        {
            ItemData loadedItem = Resources.Load<ItemData>("Items/" + i.ToString());
            if (loadedItem != null)
                items.Add(loadedItem);
            else break;
        }
    }
    public void SpawnItem(Vector3 position, int id)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>("Items/Item"), position + GetRandomOffset(), Quaternion.identity, transform);
        item.GetComponent<Item>().InitializeItem(items[id]);
    }
    public void SpawnLootTable(Vector3 position, int[] ids, int [] chances)
    {
        if (ids.Length != chances.Length) Debug.LogError("IDs doesn't match chances amount");

        for (int i = 0; i < ids.Length; i++)
        {
            if (Random.Range(0, 101) < chances[i])
            {
                SpawnItem(position, ids[i]);
            }
        }
    }
    private Vector3 GetRandomOffset()
    {
        return new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f),0);
    }
}
