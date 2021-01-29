using Random = UnityEngine.Random;
using Unity.Mathematics;
using UnityEngine;

namespace MapGeneration
{
	public static class Noise
	{
		public static float[,] GenerateNoiseMap(int2 size, NoiseData data)
		{
			float[,] noiseMap = new float[size.x, size.y];
			Vector2 offset = new Vector2(Random.Range(0, 100000), Random.Range(0, 100000));

			for (int y = 0; y < size.y; y++)
			{
				for (int x = 0; x < size.x; x++)
				{
					float impact = 1, spread = 1, noiseValue = 0;

					for (int i = 0; i < data.octaves; i++)
					{
						float sampleX = (x + offset.x) / data.scale * spread;
						float sampleY = (y - offset.y) / data.scale * spread;

						float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
						noiseValue += perlinValue * impact;

						impact *= data.impact;
						spread *= data.spread;
					}
					noiseMap[x, y] = Mathf.Clamp(noiseValue, 0f, 1f);
				}
			}
			return noiseMap;
		}
	}
	public struct NoiseData
	{
		public int octaves;
		public float scale, impact, spread;
		public NoiseData(int _octaves, float _scale, float _impact, float _spread)
		{
			octaves = _octaves; scale = _scale;
			impact = _impact; spread = _spread;
		}
	}
}
