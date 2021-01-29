using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using System;

namespace MapGeneration
{
	public class MapGenerator : MonoBehaviour
	{
		private const int smoothValue = 4, randomFillPercent = 52;
		private const float genStepTime = 0.05f;

		private Tilemap blockTilemap;

		private Biom curretBiom;
		private int2 mapSize;
		private bool[,] map;
		private float[,] noiseMap;

		public int smoothingIterations; // 2 best
		public int2 generatedMapSize; // 75,300 medium size

		void Start()
		{
			InitializeWorldGenerator();
			StartCoroutine(GenerateMap(generatedMapSize, 0));
		}
		private void InitializeWorldGenerator()
		{
			blockTilemap = transform.Find("blocks").GetComponent<Tilemap>();
		}
		private IEnumerator GenerateMap(int2 _mapSize, int biomId)
		{
			ResetPreviosGeneration();

			curretBiom = BiomList.instance.bioms[biomId];
			mapSize = _mapSize;
			map = new bool[mapSize.x, mapSize.y];
			noiseMap = Noise.GenerateNoiseMap(mapSize, curretBiom.noise);

			RunMethodInCycle(RandomMapFill);
			for (int i = 0; i < smoothingIterations; i++)
			{
				RunMethodInCycle(SmoothMap);
				yield return new WaitForSeconds(genStepTime);
			}
			RunMethodInCycle(FillTiles);

			MiningManager.instance.InitializeBlockData(mapSize.x, mapSize.y);
		}
		private int RandomMapFill(int2 pos)
		{
			if (pos.x == 0 || pos.x == mapSize.x - 1 || pos.y == 0 || pos.y == mapSize.y - 1) map[pos.x, pos.y] = true;
			else map[pos.x, pos.y] = (Random.Range(0, 100) < randomFillPercent) ? true : false; return 0;
		}
		private int SmoothMap(int2 pos)
		{
			int neighbourWallTiles = GetSurroundingWallCount(pos);
			if (neighbourWallTiles > smoothValue) map[pos.x, pos.y] = true;
			else if (neighbourWallTiles < smoothValue) map[pos.x, pos.y] = false; return 0;
		}
		private int FillTiles(int2 pos)
		{
			if (map[pos.x, pos.y])
			for (int i = 0; i < curretBiom.tileSpawnChances.Length; i++)
			if (noiseMap[pos.x, pos.y] <= curretBiom.tileSpawnChances[i])
			{
				blockTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), curretBiom.tileBases[i]);
				break;
			}
			return 0;
		}
		private int GetSurroundingWallCount(int2 pos)
		{
			int wallCount = 0;
			for (int neighbourX = pos.x - 1; neighbourX <= pos.x + 1; neighbourX++)
			{
				for (int neighbourY = pos.y - 1; neighbourY <= pos.y + 1; neighbourY++)
				{
					if (neighbourX >= 0 && neighbourX < mapSize.x && neighbourY >= 0 && neighbourY < mapSize.y)
					{
						if ((neighbourX != pos.x || neighbourY != pos.y) && map[neighbourX, neighbourY])
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
		private void RunMethodInCycle(Func<int2, int> func)
		{
			for (int x = 0; x < mapSize.x; x++)
				for (int y = 0; y < mapSize.y; y++)
					func(new int2(x, y));
		}
	}
}
