using System.Collections.Generic;
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
                    new List<NoiseData>() {
                    new NoiseData (1, 1f, 1f, 1f),
                    new NoiseData (3, 8f, 0.4f, 1.4f),
                    new NoiseData (3, 7f, 0.4f, 1.6f),
                    new NoiseData (3, 6f, 0.4f, 1.8f),
                    new NoiseData (3, 5f, 0.4f, 2f) },
                    new List<Block>() {Block.stone, Block.coal, Block.ironOre, Block.goldOre, Block.crystalOre},
                    new List<float>() { 1f, 0.42f, 0.40f, 0.38f, 0.34f }),
            };
        }
    }
    public struct Biom
    {
        public List<NoiseData> noise;
        public List<Block> blocks;
        public List<float> tileSpawnChances;
        public Biom(List<NoiseData> _noise, List<Block> _blocks, List<float> _tileSpawnChances)
        {
            noise = _noise;
            blocks = _blocks;
            tileSpawnChances = _tileSpawnChances;
        }
    }
}
