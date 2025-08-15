// Interface for basic hill generator

namespace GnG.Cluster.ClusterGen.MainClusterGen.HillGen
{
  public interface IHillGenerator
  {
    // Given an x/z coordinate, determine the height of the column
    int GetYFromXZ(int x, int z);
  }
}