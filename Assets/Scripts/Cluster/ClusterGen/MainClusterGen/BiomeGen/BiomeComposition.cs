using GnG.Data.BaseItem;
using GnG.Data.WorldGeneration.Biome;
using System;
using System.Collections.Generic;

using UnityEngine;

// Store a series of biomes and their percentages. Percentages add up to 100% (1.0)
// and represent the biomes in a single x-z column.
namespace GnG.Cluster.ClusterGen.MainClusterGen.BiomeGen
{
  public class BiomeComposition
  {
    private readonly List<BiomePercentage> biomePercentages;

    public BiomeComposition(List<BiomePercentage> biomePercentages)
    {
      this.biomePercentages = biomePercentages;

      // Ensure total is 1
      float total = 0;
      foreach (var bp in biomePercentages) total += bp.Percent;

      // 100% +/- 0.01% is acceptable range
      if (Mathf.Abs(1f - total) > 0.0001f) throw new ArgumentException("Biome percentages must add up to 1");
    }

    public IEnumerable<BiomePercentage> GetBiomePercentages()
    {
      return biomePercentages;
    }

    // Get the Biome with the highest composition percentage.
    public BiomeSO GetTopBiome()
    {
      BiomeSO maxBiome = null;
      var maxPercent = 0f;
      foreach (var bp in biomePercentages)
        if (bp.Percent > maxPercent)
        {
          maxBiome = bp.Biome;
          maxPercent = bp.Percent;
        }

      return maxBiome;
    }
  }
}