using GnG.Data.BaseItem.Block;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

// Store data for a single chunk.
namespace GnG.Cluster.Data
{
  public interface IChunkData
  {
    public void SetBlock(int3 pos, BlockSO BlockSO);
    public BlockSO GetBlock(int3 pos);
    public int GetSize();

    // Iterator for all non-null blocks in the cluster.
    // Coordinates are in local space.
    public IEnumerable<Tuple<int3, BlockSO>> CoordIterator();
  }
}