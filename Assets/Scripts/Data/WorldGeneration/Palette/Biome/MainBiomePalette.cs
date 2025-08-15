using UnityEngine;
using GnG.Data.WorldGeneration.Biome;
using System.Collections.Generic;

namespace GnG.Data.WorldGeneration.Palette.Biome {
    
    [CreateAssetMenu(menuName = "GnG/Main World Generator/Main Biome Palette", order = 1)]
    public class MainBiomePalette : ScriptableObject {
        [SerializeField] public BiomeSO grasslands;
        [SerializeField] public BiomeSO forest;
        [SerializeField] public BiomeSO desert;
        [SerializeField] public BiomeSO mountains;
        [SerializeField] public BiomeSO ocean;
    }
}
