using GnG.Cluster.ClusterGen.MainClusterGen.BiomeGen;
using GnG.Cluster.Data;
using GnG.Data.BaseItem.Block;
using GnG.Data.WorldGeneration.Palette.Block;
using GnG.Data.WorldGeneration.Settings;
using UnityEngine;
using Random = System.Random;

namespace GnG.Cluster.ClusterGen.MainClusterGen.TreeGen
{
  public class TreeGenerator
  {
    private readonly MainBlockPalette blocks;
    private readonly IBiomeGenerator biomeGenerator;
    private readonly Random random;
    private readonly int worldWidth;
    private int seed;

    public TreeGenerator(MainBlockPalette blocks, IBiomeGenerator biomeGenerator, int worldWidth, int seed)
    {
      this.blocks = blocks;
      this.biomeGenerator = biomeGenerator;
      this.worldWidth = worldWidth;
      this.seed = seed;
      random = new Random(seed);
    }

    public void PlantTrees(IClusterData clusterData)
    {
      // Remember coordinates of planted trees
      var plantedTrees = new bool[worldWidth][];
      for (var x = 0; x < worldWidth; x++)
      {
        plantedTrees[x] = new bool[worldWidth];
        for (var z = 0; z < worldWidth; z++)
          if (canPlant(plantedTrees, x, z) && isNoisePoint(x, z))
          {
            plantTree(clusterData, x, z);
            plantedTrees[x][z] = true;
          }
      }

      var end = Time.realtimeSinceStartup;
    }

    private void plantTree(IClusterData cluster, int x, int z)
    {
      var y = 0;
      while (cluster.GetBlock(x, y, z) != null) y++;

      for (var j = 0; j < 9; j++)
      {
        if (j < 6)
          cluster.SetBlock(x, y + j, z, blocks.log);

        if (j > 2)
        {
          var leafRange = (8 - j) / 2;
          for (var i = -leafRange; i <= leafRange; i++)
          for (var k = -leafRange; k <= leafRange; k++)
          {
            if (j < 6 && i == 0 && k == 0) continue;

            if (cluster.GetBlock(x + i, y + j, z + k) == null)
              cluster.SetBlock(x + i, y + j, z + k, blocks.leaves);
          }
        }
      }
    }

    private bool canPlant(bool[][] plantedTrees, int x, int z)
    {
      if (x > 0)
      {
        if (plantedTrees[x - 1][z]) return false;
        if (z > 0)
          if (plantedTrees[x - 1][z - 1])
            return false;

        if (z < worldWidth - 1)
          if (plantedTrees[x - 1][z + 1])
            return false;
      }

      if (z > 0)
        if (plantedTrees[x][z - 1])
          return false;

      return true;
    }

    private bool isNoisePoint(int x, int z)
    {
      var biomeComposition = biomeGenerator.GetBiomesFromXZ(x, z);
      var settings = ScriptableObject.CreateInstance<TreeGeneratorSettings>();
      settings.Merge(biomeComposition);

      if (settings.treeDensity == 0) return false;
      var treeRatio = Mathf.FloorToInt(1000 / settings.treeDensity);
      if (treeRatio == 0) return true;

      return random.Next() % treeRatio == 0;
    }
  }
}