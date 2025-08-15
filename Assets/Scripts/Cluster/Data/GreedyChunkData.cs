using GnG.Data.BaseItem.Block;
using GnG.Data.WorldGeneration.Biome;
using System;
using System.Collections.Generic;
using GnG.Settings;
using Unity.Mathematics;

// Store chunk data in an array (most basic way).
namespace GnG.Cluster.Data
{
  [Serializable]
  public class GreedyChunkData : IChunkData
  {
    private BlockSO[][][] blocks;
    private readonly int size = GameSettings.CHUNK_SIZE;

    // Create an empty chunk.
    public GreedyChunkData()
    {
      blocks = new BlockSO[size][][];

      for (var i = 0; i < size; i++)
      {
        blocks[i] = new BlockSO[size][];

        for (var j = 0; j < size; j++)
        {
          blocks[i][j] = new BlockSO[size];

          for (var k = 0; k < size; k++) blocks[i][j][k] = null;
        }
      }
    }

    public void SetBlock(int3 pos, BlockSO BlockSO)
    {
      validatePos(pos);

      blocks[pos.x][pos.y][pos.z] = BlockSO;
    }

    public BlockSO GetBlock(int3 pos)
    {
      validatePos(pos);

      return blocks[pos.x][pos.y][pos.z];
    }

    public int GetSize()
    {
      return size;
    }

    // Iterator for all non-default blocks in the chunk.
    // Coordinates are in local space.
    public IEnumerable<Tuple<int3, BlockSO>> CoordIterator()
    {
      for (var i = 0; i < size; i++)
      for (var j = 0; j < size; j++)
      for (var k = 0; k < size; k++)
      {
        var block = blocks[i][j][k];
        if (block != null) yield return new Tuple<int3, BlockSO>(new int3(i, j, k), block);
      }
    }

    private void validatePos(int3 pos)
    {
      if (pos.x < 0 || pos.y < 0 || pos.z < 0 ||
          pos.x >= size || pos.y >= size || pos.z >= size)
        throw new IndexOutOfRangeException("Block coordinate is out of bounds");
    }
  }
}