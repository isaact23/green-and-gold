using GnG.Cluster.ClusterGen.MainClusterGen.BiomeGen;
using GnG.Cluster.ClusterGen.MainClusterGen.HillGen;
using GnG.Cluster.ClusterGen.MainClusterGen.TreeGen;
using GnG.Cluster.Data;
using GnG.Data.BaseItem.Block;
using GnG.Data.WorldGeneration.Palette;
using GnG.Data.WorldGeneration.Settings;

using UnityEngine.Profiling;

// Generator for clusters (world terrain)
namespace GnG.Cluster.ClusterGen.MainClusterGen
{
  public class MainClusterGenerator : IClusterGenerator
  {
    private readonly MainMasterPalette palette;
    private readonly int worldWidth;
    private readonly IBiomeGenerator biomeGen;
    private readonly IHillGenerator hillGen;
    private readonly TreeGenerator treeGen;

    public MainClusterGenerator(MainMasterPalette palette, WorldGeneratorSettings settings)
    {
      this.palette = palette;
      this.worldWidth = settings.worldWidth;
      biomeGen = new BiomeGenerator(palette.biomes, settings.biomeSettings, worldWidth, settings.seed);
      hillGen = new HillGenerator(biomeGen, worldWidth, settings.seed + 1);
      treeGen = new TreeGenerator(palette.blocks, biomeGen, worldWidth, settings.seed + 2);
    }

    public IClusterData Create()
    {
      Profiler.BeginSample("Create cluster");

      // Create empty cluster
      IClusterData cluster = new ClusterData();

      for (var x = 0; x < worldWidth; x++)
      for (var z = 0; z < worldWidth; z++)
      {
        // Call generation subsystems
        var height = hillGen.GetYFromXZ(x, z);
        //int height = 0;
        var biome = biomeGen.GetBiomesFromXZ(x, z).GetTopBiome();

        // Set the blocks in the chunk
        for (var y = 0; y <= height; y++)
        {
          BlockSO blockType;
          if (y == height)
            blockType = palette.blocks.grass;
          else if (y > height - 4)
            blockType = palette.blocks.dirt;
          else
            blockType = palette.blocks.stone;

          cluster.SetBlock(x, y, z, blockType);
        }
      }

      treeGen.PlantTrees(cluster);

      Profiler.EndSample();

      return cluster;
    }
  }
}