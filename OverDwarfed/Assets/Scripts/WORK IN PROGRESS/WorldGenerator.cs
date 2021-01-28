using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private Tilemap blockTilemap;
    public TileBase stoneTile;
    private CompositeCollider2D compositeCollider;

    private void Start()
    {
        InitializeWorldGenerator();
        GenerateMap(10, 10);
    }
    private void InitializeWorldGenerator()
    {
        blockTilemap = transform.Find("blocks").GetComponent<Tilemap>();
        compositeCollider = blockTilemap.GetComponent<CompositeCollider2D>();
    }
    private void GenerateMap(int sizeX, int sizeY)
    {
        for(int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                if(Random.Range(0,4f) > 1.5f)
                blockTilemap.SetTile(new Vector3Int(x, y, 0), stoneTile);
                else blockTilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
        compositeCollider.GenerateGeometry();

        MiningManager.instance.InitializeBlockData(sizeX, sizeY);
    }
}
