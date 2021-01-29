﻿using System.Collections.Generic;
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
                new Biom (new NoiseData(3, 7f, 0.4f, 1.5f),
                    new TileBase[] {
                    MiningManager.tileTypes[2].destructionStages[3],
                    MiningManager.tileTypes[1].destructionStages[3],
                    MiningManager.tileTypes[0].destructionStages[3], },
                    new float[] { 0.42f, 0.5f, 1f})
            };
        }
    }
    public struct Biom // might put 1 player mapSize here and multiplay it by player count
    {
        public NoiseData noise;
        public TileBase[] tileBases;
        public float[] tileSpawnChances;
        public Biom(NoiseData _noise, TileBase[] _tileBases, float[] _tileSpawnChances)
        {
            noise = _noise;
            tileBases = _tileBases;
            tileSpawnChances = _tileSpawnChances;
        }
    }
}