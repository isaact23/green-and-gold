using GnG.Data;
using GnG.Data.BaseItem;
using GnG.Data.BaseItem.Block;
using GnG.Settings;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using GnG.Util;

// Store a 3D array of chunks.
namespace GnG.Cluster.Data
{
  public class ClusterData : IClusterData
  {
    private readonly Dictionary<int3, IChunkData> chunks;
    private readonly int chunkSize = GameSettings.CHUNK_SIZE;

    public ClusterData()
    {
      chunks = new Dictionary<int3, IChunkData>();
    }

    // Set a block anywhere in the cluster. Create a new chunk if necessary.
    public void SetBlock(int3 pos, BlockSO blockType)
    {
      var chunk = getChunkFromCoord(pos);

      // If chunk doesn't exist, create it
      if (chunk == null) chunk = createChunk(pos);

      var local = CoordConvert.GetLocalCoord(pos);
      chunk.SetBlock(local, blockType);
    }

    public void SetBlock(int x, int y, int z, BlockSO blockType)
    {
      SetBlock(new int3(x, y, z), blockType);
    }

    // Get a block from anywhere in the cluster
    public BlockSO GetBlock(int3 pos)
    {
      var chunk = getChunkFromCoord(pos);

      // Assume a block in a nonexistent chunk is null
      if (chunk == null) return null;

      var local = CoordConvert.GetLocalCoord(pos);
      return chunk.GetBlock(local);
    }

    public BlockSO GetBlock(int x, int y, int z)
    {
      return GetBlock(new int3(x, y, z));
    }

    // Get a chunk by chunk coordinate. Return null if
    // the chunk could not be found.
    public IChunkData GetChunk(int3 coord)
    {
      if (chunks.TryGetValue(coord, out var value)) return value;
      return null;
    }

    // Iterator for all non-default blocks in the cluster.
    // Coordinates are in global space.
    public IEnumerable<Tuple<int3, BlockSO>> CoordIterator()
    {
      // Iterate through all chunks
      foreach (var chunkDataPos in chunks)
      {
        var chunkCoord = chunkDataPos.Key;
        var chunk = chunkDataPos.Value;

        // Convert coord to global space block coordinate
        var globalCoord = chunkCoord * chunkSize;

        // Iterate through non-default blocks in the chunk
        var chunkIt = chunk.CoordIterator();
        foreach (var tuple in chunkIt)
        {
          // Add global space block coordinate before yielding
          var newCoord = tuple.Item1 + globalCoord;
          var block = tuple.Item2;

          yield return new Tuple<int3, BlockSO>(newCoord, block);
        }
      }
    }

    // Iterator for all chunk coordinates in the cluster.
    public IEnumerable<int3> ChunkIterator()
    {
      foreach (var chunkPos in chunks.Keys) yield return chunkPos;
    }

    // Get the chunk from a global coordinate, or null if it doesn't exist.
    private IChunkData getChunkFromCoord(int3 pos)
    {
      var chunkCoord = CoordConvert.GetChunkCoord(pos);
      return GetChunk(chunkCoord);
    }

    // Create a chunk that contains the given global coordinate
    private IChunkData createChunk(int3 pos)
    {
      // Create chunk and chunk coord
      IChunkData chunk = new GreedyChunkData();
      var chunkCoord = CoordConvert.GetChunkCoord(pos);

      // Insert into dictionary
      chunks[chunkCoord] = chunk;

      return chunk;
    }
  }
}