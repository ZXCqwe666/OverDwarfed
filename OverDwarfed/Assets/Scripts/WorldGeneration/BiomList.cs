using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace MapGeneration
{
    public class BiomList : MonoBehaviour
    {
        public static BiomList instance;
        public List<Biom> bioms;

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            bioms = new List<Biom>()
            {
                new Biom (
                    new NoiseData[] {
                    new NoiseData (1, 1f, 1f, 1f),
                    new NoiseData (3, 8f, 0.4f, 1.4f),
                    new NoiseData (3, 7f, 0.4f, 1.6f),
                    new NoiseData (3, 6f, 0.4f, 1.8f),
                    new NoiseData (3, 5f, 0.4f, 2f) },
                    new TileBase[] {
                    MiningManager.tileTypes[0].destructionStages[3],
                    MiningManager.tileTypes[1].destructionStages[3],
                    MiningManager.tileTypes[2].destructionStages[3],
                    MiningManager.tileTypes[3].destructionStages[3],
                    MiningManager.tileTypes[4].destructionStages[3] },
                    new float[] { 1f, 0.42f, 0.40f, 0.38f, 0.34f })
            };
        }
    }
    public struct Biom // might put 1 player mapSize here and multiplay it by player count
    {
        public NoiseData[] noise;
        public TileBase[] tileBases;
        public float[] tileSpawnChances;
        public Biom(NoiseData[] _noise, TileBase[] _tileBases, float[] _tileSpawnChances)
        {
            noise = _noise;
            tileBases = _tileBases;
            tileSpawnChances = _tileSpawnChances;
        }
    }
}
