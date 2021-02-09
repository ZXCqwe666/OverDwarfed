﻿using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using PathFinder;

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
			bool[,] map = new bool[mapSize.x, mapSize.y];
			bool[,] edgeMap = new bool[mapSize.x, mapSize.y];

			RandomMapFill(ref map, randomFillPercent); yield return new WaitForSeconds(genStepTime);
			RandomMapFill(ref edgeMap, randomFillPercentND); yield return new WaitForSeconds(genStepTime);
			SmoothMap(ref map, smoothingIterations); yield return new WaitForSeconds(genStepTime);
		    SmoothMap(ref edgeMap, smoothingIterationsND); yield return new WaitForSeconds(genStepTime);

			FillTiles(ref map, ref edgeMap); yield return new WaitForSeconds(genStepTime);
			FillAreaAround(); yield return new WaitForSeconds(genStepTime);
			CarveStartingArea(); yield return new WaitForSeconds(genStepTime);

			for (int x = 0; x < mapSize.x; x++)
		    for (int y = 0; y < mapSize.y; y++)
			map[x, y] = !map[x, y];
			Pathfinding.pathGrid = new PathGrid(mapSize, map);

			MiningManager.instance.InitializeBlockData(mapSize.x, mapSize.y);
			blockTilemap.GetComponent<TilemapCollider2D>().enabled = true;
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
		private void FillTiles(ref bool[,] map, ref bool[,] edgeMap)
		{
			for (int i = 0; i < curretBiom.tileBases.Length; i++)
			{
				float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, curretBiom.noise[i]);

				for (int x = 0; x < mapSize.x; x++)
				for (int y = 0; y < mapSize.y; y++)
				if (map[x, y] && edgeMap[x, y] == false)
				{
					if (noiseMap[x, y] <= curretBiom.tileSpawnChances[i])
						blockTilemap.SetTile(new Vector3Int(x, y, 0), curretBiom.tileBases[i]);
				}
				else if (edgeMap[x, y])
				blockTilemap.SetTile(new Vector3Int(x, y, 0), NonDistructableTile);
			}
		}
		private int GetSurroundingWallCount(int2 pos, ref bool[,] map)
		{
			int wallCount = 0;
			for (int neighbourX = pos.x - 1; neighbourX <= pos.x + 1; neighbourX++)
			for (int neighbourY = pos.y - 1; neighbourY <= pos.y + 1; neighbourY++)
			if (neighbourX >= 0 && neighbourX < mapSize.x && neighbourY >= 0 && neighbourY < mapSize.y)
			{
				if ((neighbourX != pos.x || neighbourY != pos.y) && map[neighbourX, neighbourY])
					wallCount++;
			}
			else wallCount++;
			return wallCount;
		}
		private void ResetPreviosGeneration()
		{
			Random.InitState(Random.Range(int.MinValue, int.MaxValue));
			blockTilemap.GetComponent<TilemapCollider2D>().enabled = false;
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
		private void CarveStartingArea() /// BETA SHIT
		{
			int areaDiameter = 15; // 15 10  temp values
			int2 startPosition = new int2(10, mapSize.y / 2 - areaDiameter / 2);

			for(int x = startPosition.x; x < startPosition.x + areaDiameter; x++)
			for (int y = startPosition.y; y < startPosition.y + areaDiameter; y++)
					blockTilemap.SetTile(new Vector3Int(x, y, 0), null);
					
			FindObjectOfType<PlayerController>().transform.position = //// TEMPORARY PLAYER SEARCH
				new Vector3(startPosition.x + areaDiameter / 2, startPosition.y + areaDiameter / 2, 0f);

		   // ++++ PLACE BUILDINGS AT START
		}
	}
}
