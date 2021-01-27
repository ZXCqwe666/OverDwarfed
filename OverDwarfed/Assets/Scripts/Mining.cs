using UnityEngine;
using UnityEngine.Tilemaps;

public class Mining : MonoBehaviour
{
    public Tilemap tilemap;
    void Start() 
    { 
        tilemap = GameObject.Find("Grid").transform.Find("Tilemap").GetComponent<Tilemap>(); 
    }
    void Update() 
    { 
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        if (Input.GetMouseButtonDown(0))
        { 
            Vector3Int selectedTile = tilemap.WorldToCell(point); 
            tilemap.SetTile(selectedTile, null); 
        } 
    }
}
