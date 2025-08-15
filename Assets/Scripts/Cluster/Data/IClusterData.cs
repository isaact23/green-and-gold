using GnG.Data.BaseItem.Block;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

// Store data for a full cluster of chunks.
namespace GnG.Cluster.Data
{
  public interface IClusterData
  {
    public void SetBlock(int3 pos, BlockSO BlockSO);
    public void SetBlock(int x, int y, int z, BlockSO BlockSO);
    public BlockSO GetBlock(int3 pos);
    public BlockSO GetBlock(int x, int y, int z);
    public IChunkData GetChunk(int3 coord);

    // Iterator for all non-null blocks in the cluster.
    // Coordinates are in global space.
    public IEnumerable<Tuple<int3, BlockSO>> CoordIterator();

    // Iterator for all chunk coordinates in the cluster.
    public IEnumerable<int3> ChunkIterator();
  }
}