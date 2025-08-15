using GnG.Cluster.Data;

namespace GnG.Cluster.ClusterGen
{
  public interface IClusterGenerator
  {
    public IClusterData Create();
  }
}