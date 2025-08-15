using System;
using UnityEngine;

namespace GnG.Data.WorldGeneration.Settings {
    
    [CreateAssetMenu(menuName = "GnG/Main World Generator/World Generator Settings", order = 1)]
    public class WorldGeneratorSettings : ScriptableObject {
        [SerializeField] public BiomeGeneratorSettings biomeSettings;
        [SerializeField] public int worldWidth;
        [SerializeField] public int seed;
    }
}
