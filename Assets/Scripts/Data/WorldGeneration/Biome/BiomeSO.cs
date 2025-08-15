using GnG.Data.WorldGeneration.Settings;
using UnityEngine;

namespace GnG.Data.WorldGeneration.Biome {

    [CreateAssetMenu(menuName = "GnG/Biome", order = 1)]
    public class BiomeSO : ScriptableObject {
        [SerializeField] public string name;
        [SerializeField] public HillGeneratorSettings hillSettings;
        [SerializeField] public TreeGeneratorSettings treeSettings;
    }
}