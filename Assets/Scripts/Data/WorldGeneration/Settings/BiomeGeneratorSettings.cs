using UnityEngine;

namespace GnG.Data.WorldGeneration.Settings {
    
    [CreateAssetMenu(menuName = "GnG/Main World Generator/Biome Generator Settings", order = 1)]
    public class BiomeGeneratorSettings : ScriptableObject {

        [SerializeField] public int biomeWidth = 50;
        [SerializeField] public float biomeGrade = 10f;
    }
}