using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MapGeneration
{
	public class MapGenerator : MonoBehaviour
	{
		private const int smoothValue = 4, fillAroundSize = 5;
		private const int randomFillPercent = 52, randomFillPercentND = 24;
		private const int smoothingIterations = 2, smoothingIterationsND = 3;
		private const float genStepTime = 0.05f;

		private Tilemap blockTilemap;
		private TileBase NonDistructableTile;

		private Biom curretBiom;
		private int2 mapSize;
		private bool[,] map, edgeMap;
		private float[,] noiseMap;

		public int2 generatedMapSize; // 75,300 medium size

		void Start()
		{
			InitializeWorldGenerator();
			StartCoroutine(GenerateMap(generatedMapSize, 0));
		}
		private void InitializeWorldGenerator()
		{
			NonDistructableTile = Resources.Load<RuleTile>("Blocks/NonDistructableTile");
			blockTilemap = transform.Find("blocks").GetComponent<Tilemap>();
		}
		private IEnumerator GenerateMap(int2 _mapSize, int biomId)
		{
			ResetPreviosGeneration();

			curretBiom = BiomList.instance.bioms[biomId];
			mapSize = _mapSize;
			map = new bool[mapSize.x, mapSize.y];
			edgeMap = new bool[mapSize.x, mapSize.y];
			noiseMap = Noise.GenerateNoiseMap(mapSize, curretBiom.noise);

			RandomMapFill(ref map, randomFillPercent); yield return new WaitForSeconds(genStepTime);
			RandomMapFill(ref edgeMap, randomFillPercentND); yield return new WaitForSeconds(genStepTime);
			SmoothMap(ref map, smoothingIterations); yield return new WaitForSeconds(genStepTime);
		    SmoothMap(ref edgeMap, smoothingIterationsND); yield return new WaitForSeconds(genStepTime);

			FillTiles(); yield return new WaitForSeconds(genStepTime);
			FillAreaAround(); yield return new WaitForSeconds(genStepTime);
			MiningManager.instance.InitializeBlockData(mapSize.x, mapSize.y);
		}
		private void RandomMapFill(ref bool[,] mapArray, int _randomFillPercent)
		{
			for (int x = 0; x < mapSize.x; x++)
		    for (int y = 0; y < mapSize.y; y++)
			{
				if (x == 0 || x == mapSize.x - 1 || y == 0 || y == mapSize.y - 1) mapArray[x, y] = true;
				else mapArray[x, y] = (Random.Range(0, 100) < _randomFillPercent) ? true : false;
			}
		}
		private void SmoothMap(ref bool[,] mapArray, int _smoothingIterations)
		{
			for (int i = 0; i < _smoothingIterations; i++)
			for (int x = 0; x < mapSize.x; x++)
			for (int y = 0; y < mapSize.y; y++)
			{
				int neighbourWallTiles = GetSurroundingWallCount(new int2(x, y), ref mapArray);
				if (neighbourWallTiles > smoothValue) mapArray[x, y] = true;
				else if (neighbourWallTiles < smoothValue) mapArray[x, y] = false;
			}
		}
		private void FillTiles()
		{
			for (int x = 0; x < mapSize.x; x++)
			for (int y = 0; y < mapSize.y; y++)
			if (map[x, y] && edgeMap[x, y] == false)
		    {
				for (int i = 0; i < curretBiom.tileSpawnChances.Length; i++)
					if (noiseMap[x, y] <= curretBiom.tileSpawnChances[i])
					{
						blockTilemap.SetTile(new Vector3Int(x, y, 0), curretBiom.tileBases[i]);
						break;
					}
			}
			else if (edgeMap[x, y]) 
				blockTilemap.SetTile(new Vector3Int(x, y, 0), NonDistructableTile); 
		}
		private int GetSurroundingWallCount(int2 pos, ref bool[,] mapArray)
		{
			int wallCount = 0;
			for (int neighbourX = pos.x - 1; neighbourX <= pos.x + 1; neighbourX++)
			{
				for (int neighbourY = pos.y - 1; neighbourY <= pos.y + 1; neighbourY++)
				{
					if (neighbourX >= 0 && neighbourX < mapSize.x && neighbourY >= 0 && neighbourY < mapSize.y)
					{
						if ((neighbourX != pos.x || neighbourY != pos.y) && mapArray[neighbourX, neighbourY])
							wallCount++;
					}
					else wallCount++;
				}
			} return wallCount;
		}
		private void ResetPreviosGeneration()
		{
			Random.InitState(Random.Range(int.MinValue, int.MaxValue));
			blockTilemap.ClearAllTiles();
		}
		private void FillAreaAround()
		{
			blockTilemap.SetTile(new Vector3Int(-fillAroundSize, -fillAroundSize - 1, 0), NonDistructableTile);
			blockTilemap.SetTile(new Vector3Int(mapSize.x + fillAroundSize - 1, mapSize.y + fillAroundSize, 0), NonDistructableTile);
			BoundsInt bounds = blockTilemap.cellBounds;
			Vector3Int fillStart = new Vector3Int(bounds.xMin, bounds.yMin + 1, 0);
			blockTilemap.BoxFill(fillStart, NonDistructableTile, fillStart.x, fillStart.y, bounds.xMax - 1, bounds.yMax - 2);
		}
	}
}
