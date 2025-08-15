using GnG.Data.WorldGeneration.Palette.Biome;
using GnG.Data.WorldGeneration.Palette.Block;
using System;
using UnityEngine;

namespace GnG.Data.WorldGeneration.Palette {
    
    [CreateAssetMenu(menuName = "GnG/Main World Generator/Main Master Palette", order = 1)]
    public class MainMasterPalette : ScriptableObject {
        [SerializeField] public MainBiomePalette biomes;
        [SerializeField] public MainBlockPalette blocks;
    }
}