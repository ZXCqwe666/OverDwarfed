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
		private const int randomFillPercent = 52, randomFillPercentEdge = 24;
		private const int smoothingIterations = 2, smoothingIterationsEdge = 3;
		private const float genStepTime = 0.1f;

		private Tilemap blockTilemap;
		private TileBase NonDistructableTile;
		private TilemapCollider2D blocksCollider;

		private Biom curretBiom;
		private int2 mapSize;

		public int2 generatedMapSize; // 75,300 medium size

		public GameObject spawnPointPrefab;
		public SpawnPointData spData;
		public int randomSamplesForSpawnPoints;

		void Start()
		{
			InitializeWorldGenerator();
			StartCoroutine(GenerateMap(generatedMapSize, 0));
		}
		private void InitializeWorldGenerator()
		{
			NonDistructableTile = Resources.Load<RuleTile>("Blocks/NonDistructableTile");
			blockTilemap = transform.Find("blocks").GetComponent<Tilemap>();
			blocksCollider = blockTilemap.GetComponent<TilemapCollider2D>();
		}
		private IEnumerator GenerateMap(int2 _mapSize, int biomId)
		{
			ResetPreviosGeneration();
			MiningManager.instance.InitializeBlockData(_mapSize);

			curretBiom = BiomList.instance.bioms[biomId];
			mapSize = _mapSize;

			bool[,] map = new bool[mapSize.x, mapSize.y];
			bool[,] edgeMap = new bool[mapSize.x, mapSize.y];

			RandomMapFill(ref map, randomFillPercent); yield return new WaitForSeconds(genStepTime);
			RandomMapFill(ref edgeMap, randomFillPercentEdge); yield return new WaitForSeconds(genStepTime);
			SmoothMap(ref map, smoothingIterations); yield return new WaitForSeconds(genStepTime);
		    SmoothMap(ref edgeMap, smoothingIterationsEdge); yield return new WaitForSeconds(genStepTime);

			FillTiles(ref map, ref edgeMap); yield return new WaitForSeconds(genStepTime);
			FillAreaAround(); yield return new WaitForSeconds(genStepTime);
			CarveStartingArea(ref map); yield return new WaitForSeconds(genStepTime);

			TemporaryPortalGeneration(ref map); yield return new WaitForSeconds(genStepTime);
			InitializePathfindingGrid(ref map); yield return new WaitForSeconds(genStepTime);

			blocksCollider.enabled = true;
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
			for (int i = 0; i < curretBiom.blocks.Count; i++)
			{
				float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, curretBiom.noise[i]);

				for (int x = 0; x < mapSize.x; x++)
				for (int y = 0; y < mapSize.y; y++)
				if (map[x, y] && edgeMap[x, y] == false)
				{
					if (noiseMap[x, y] <= curretBiom.tileSpawnChances[i])
						MiningManager.instance.SetBlock(curretBiom.blocks[i], x, y);
				}
			}
			for (int x = 0; x < mapSize.x; x++)
			for (int y = 0; y < mapSize.y; y++)
			{
				if(edgeMap[x, y])
				{
					blockTilemap.SetTile(new Vector3Int(x, y, 0), NonDistructableTile);
					MiningManager.instance.SetBlock(Block.empty, x, y);
				}
				else if (map[x, y] == false)
				MiningManager.instance.SetBlock(Block.empty, x, y);
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
			WaveSpawner.instance.spawnPoints.Clear();
			blockTilemap.ClearAllTiles();
			blocksCollider.enabled = false;
		}
		private void FillAreaAround()
		{
			blockTilemap.SetTile(new Vector3Int(-fillAroundSize, -fillAroundSize - 1, 0), NonDistructableTile);
			blockTilemap.SetTile(new Vector3Int(mapSize.x + fillAroundSize - 1, mapSize.y + fillAroundSize, 0), NonDistructableTile);
			BoundsInt bounds = blockTilemap.cellBounds;
			Vector3Int fillStart = new Vector3Int(bounds.xMin, bounds.yMin + 1, 0);
			blockTilemap.BoxFill(fillStart, NonDistructableTile, fillStart.x, fillStart.y, bounds.xMax - 1, bounds.yMax - 2);
		}
		private void CarveStartingArea(ref bool[,] map) /// BETA SHIT
		{
			int areaDiameter = 15; // 15 10  temp values
			int2 startPosition = new int2(10, mapSize.y / 2 - areaDiameter / 2);

			for (int x = startPosition.x; x < startPosition.x + areaDiameter; x++)
			for (int y = startPosition.y; y < startPosition.y + areaDiameter; y++)
			{
				map[x, y] = false;
				blockTilemap.SetTile(new Vector3Int(x, y, 0), null);
			}
					
			FindObjectOfType<PlayerController>().transform.position = //// TEMPORARY PLAYER SEARCH
				new Vector3(startPosition.x + areaDiameter / 2, startPosition.y + areaDiameter / 2, 0f);

			// ++++ PLACE BUILDINGS AT START
		}
		private void InitializePathfindingGrid(ref bool[,] map)
		{
			for (int x = 0; x < mapSize.x; x++)
	        for (int y = 0; y < mapSize.y; y++)
				map[x, y] = !map[x, y];
			Pathfinding.pathGrid = new PathGrid(mapSize, map);
		}
		private void TemporaryPortalGeneration(ref bool[,] map) // beta roflan
		{
			for (int i = 0; i < randomSamplesForSpawnPoints; i++)
			{
				Vector3 randomSpot = new Vector3(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y), 0f);
				if (map[(int)randomSpot.x, (int)randomSpot.y] == false)
				{
					SpawnPoint point = Instantiate(spawnPointPrefab, randomSpot + Utility.halfVector, Quaternion.identity, transform).GetComponent<SpawnPoint>();
					point.InitializeSpawnPoint(spData);
					WaveSpawner.instance.spawnPoints.Add(point);
				}
			}
		}
	}
}
