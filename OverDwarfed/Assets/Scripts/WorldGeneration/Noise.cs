using UnityEngine;

public static class Noise
{
	public static float[,] GenerateNoiseMap(int size, NoiseData data)
	{
		float[,] noiseMap = new float[size, size];

		for (int y = 0; y < size; y++)
		{
			for (int x = 0; x < size; x++)
			{
				float impact = 1;
				float spread = 1;
				float noiseValue = 0;

				for (int i = 0; i < data.octaves; i++)
				{
					float sampleX = (x + data.offset.x) / data.scale * spread;
					float sampleY = (y - data.offset.y) / data.scale * spread;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
					noiseValue += perlinValue * impact;

					impact *= data.impact;
					spread *= data.spread;
				}
				noiseMap[x, y] = noiseValue;
			}
		}
		return noiseMap;
	}
	public struct NoiseData
	{
		public int octaves;
		public float scale, impact, spread;
		public Vector2 offset;
		public NoiseData(int _octaves, float _scale, float _impact, float _spread, Vector2 _offset)
		{
			octaves = _octaves;
			scale = _scale;
			impact = _impact;
			spread = _spread;
			offset = _offset;
		}
	}
}