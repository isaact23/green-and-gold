using System;
using System.Collections.Generic;
using GnG.Data.WorldGeneration.Biome;
using GnG.Data.WorldGeneration.Palette.Biome;
using GnG.Data.WorldGeneration.Settings;
using Unity.Mathematics;
using Random = System.Random;

// A basic biome composition generator

namespace GnG.Cluster.ClusterGen.MainClusterGen.BiomeGen
{
  public class BiomeGenerator : IBiomeGenerator
  {
    private readonly MainBiomePalette biomes;
    private readonly BiomeGeneratorSettings settings;
    private readonly List<BiomePoint> points;
    private readonly Random random;

    public BiomeGenerator(MainBiomePalette biomes, BiomeGeneratorSettings settings, int worldWidth, int seed)
    {
      this.biomes = biomes;
      this.settings = settings;
      
      points = new List<BiomePoint>();
      random = new Random(seed);

      var biomeSparsity = settings.biomeWidth * settings.biomeWidth;

      // TODO: Do not hardcode this
      BiomeSO[] biomeArray = {biomes.grasslands, biomes.desert, biomes.forest, biomes.mountains};

      for (var x = 0; x < worldWidth; x++)
      for (var z = 0; z < worldWidth; z++)
        if (random.Next() % biomeSparsity == 0)
        {
          var index = random.Next() % 4;
          var biome = biomeArray[index];

          var point = new BiomePoint
          {
            point = new float2(x, z),
            biome = biome
          };
          points.Add(point);
        }
    }

    // Get biome composition for x/z coordinate (a column)
    public BiomeComposition GetBiomesFromXZ(int x, int z)
    {
      float2 origin = new float2(x, z);

      // Keep track of which biomes influence the given point the most
      Dictionary<BiomeSO, float> biomeInf = new Dictionary<BiomeSO, float>();
      float totalInf = 0;

      foreach (BiomePoint biomePoint in points)
      {
        float distance = math.distance(biomePoint.point, origin);

        // The influence function determines how much a BiomePoint affects the
        // origin point. This function is critical to terrain generation.
        float influence = math.pow(distance + 10, -settings.biomeGrade);

        // Record influence associated with the biome
        totalInf += influence;
        var biome = biomePoint.biome;
        if (biomeInf.TryGetValue(biome, out float oldInf))
          biomeInf[biome] = oldInf + influence;
        else
          biomeInf.Add(biome, influence);
      }

      // Convert biome influence levels to biome compositions
      List<BiomePercentage> biomePercentages = new List<BiomePercentage>();

      // Default case
      if (totalInf == 0)
      {
        biomePercentages.Add(new BiomePercentage(biomes.grasslands, 1f));
        return new BiomeComposition(biomePercentages);
      }

      foreach (var biomeInfPair in biomeInf)
      {
        var biome = biomeInfPair.Key;
        var inf = biomeInfPair.Value;
        var percentage = inf / totalInf;
        biomePercentages.Add(new BiomePercentage(biome, percentage));
      }

      return new BiomeComposition(biomePercentages);
    }

    // Associate an x/z coordinate with a Biome
    private struct BiomePoint
    {
      public float2 point;
      public BiomeSO biome;
    }
  }
}