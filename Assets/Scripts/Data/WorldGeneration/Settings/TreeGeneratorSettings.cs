using GnG.Cluster.ClusterGen.MainClusterGen.BiomeGen;
using GnG.Data.WorldGeneration.Biome;
using UnityEngine;

namespace GnG.Data.WorldGeneration.Settings
{
  
    [CreateAssetMenu(menuName = "GnG/Main World Generator/Tree Generator Settings", order = 1)]
  public class TreeGeneratorSettings : ScriptableObject
  {
    [SerializeField] public float treeDensity;

    // When called, set this object's fields to be the weighted average of the
    // TreeGeneratorSettings of the biomes in the composition.
    public void Merge(BiomeComposition composition)
    {
      ZeroFields();

      foreach (BiomePercentage bp in composition.GetBiomePercentages())
      {
        BiomeSO biome = bp.Biome;
        float percent = bp.Percent;

        TreeGeneratorSettings settings = biome.treeSettings;

        this.treeDensity += settings.treeDensity * percent;
      }
    }

    private void ZeroFields()
    {
      this.treeDensity = 0;
    }
  }
}
