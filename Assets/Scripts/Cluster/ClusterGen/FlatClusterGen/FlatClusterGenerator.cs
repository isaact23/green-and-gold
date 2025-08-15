using GnG.Cluster.Data;
using GnG.Data.BaseItem.Block;

namespace GnG.Cluster.ClusterGen.FlatClusterGen
{
  public class FlatClusterGenerator : IClusterGenerator
  {
    private readonly BlockSO groundBlock;
    private readonly int width;

    public FlatClusterGenerator(BlockSO groundBlock, int width) {
      this.width = width;
    }

    public IClusterData Create()
    {
      IClusterData cluster = new ClusterData();
      for (var x = 0; x < width; x++)
      for (var z = 0; z < width; z++)
        cluster.SetBlock(x, 0, z, groundBlock);

      return cluster;
    }
  }
}