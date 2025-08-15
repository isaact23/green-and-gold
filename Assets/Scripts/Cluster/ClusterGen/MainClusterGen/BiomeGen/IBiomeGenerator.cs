// Interface for biome composition generator

namespace GnG.Cluster.ClusterGen.MainClusterGen.BiomeGen
{
  public interface IBiomeGenerator
  {
    // Get biome composition for x/z coordinate (a column)
    public BiomeComposition GetBiomesFromXZ(int x, int z);
  }
}