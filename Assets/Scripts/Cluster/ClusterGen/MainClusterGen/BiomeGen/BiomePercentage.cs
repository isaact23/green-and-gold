using GnG.Data;
using GnG.Data.WorldGeneration.Biome;
using System;

// Store biome and percentage in a pair. Used to construct a BiomeComposition.
namespace GnG.Cluster.ClusterGen.MainClusterGen.BiomeGen
{
  public class BiomePercentage
  {
    public BiomePercentage(BiomeSO biome, float percent)
    {
      if (percent < 0 || percent > 1) throw new ArgumentException("Percent must be normalized in range [0.0, 1.0]");
      Biome = biome;
      Percent = percent;
    }

    public BiomeSO Biome { get; private set; }
    public float Percent { get; private set; }
  }
}