using GnG.Cluster.ClusterGen.MainClusterGen.BiomeGen;
using GnG.Data.WorldGeneration.Biome;
using UnityEngine;

namespace GnG.Data.WorldGeneration.Settings
{
  
    [CreateAssetMenu(menuName = "GnG/Main World Generator/Hill Generator Settings", order = 1)]
  public class HillGeneratorSettings : ScriptableObject
  {
    [SerializeField] public float hillDensity;
    [SerializeField] public float hillAltitude;
    [SerializeField] public float hillHeight;
    [SerializeField] public float hillGrade;
    [SerializeField] public float hillWidth;
    [SerializeField] public float noiseRange;

    // When called, set this object's fields to be the weighted average of the
    // HillGeneratorSettings of the biomes in the composition.
    public void Merge(BiomeComposition composition)
    {
      ZeroFields();

      foreach (BiomePercentage bp in composition.GetBiomePercentages())
      {
        BiomeSO biome = bp.Biome;
        float percent = bp.Percent;

        HillGeneratorSettings settings = biome.hillSettings;

        this.hillDensity += settings.hillDensity * percent;
        this.hillAltitude += settings.hillAltitude * percent;
        this.hillHeight += settings.hillHeight * percent;
        this.hillGrade += settings.hillGrade * percent;
        this.hillWidth += settings.hillWidth * percent;
        this.noiseRange += settings.noiseRange * percent;
      }
    }

    public int GetHillRatio()
    {
      return Mathf.FloorToInt((hillHeight + hillAltitude) / hillDensity);
    }

    private void ZeroFields() {
      this.hillDensity = 0;
      this.hillAltitude = 0;
      this.hillHeight = 0;
      this.hillGrade = 0;
      this.hillWidth = 0;
      this.noiseRange = 0;
    }
  }
}