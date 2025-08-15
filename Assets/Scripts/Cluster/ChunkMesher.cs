using GnG.Cluster.Data;
using GnG.Data.BaseItem.Block;
using GnG.Settings;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace GnG.Cluster
{
  // General mesher for any IChunkData.
  public class ChunkMesher
  {
    // The 8 vertices in a cube
    private static readonly int3[] cubeVerts =
    {
      new(0, 0, 0),
      new(0, 0, 1),
      new(0, 1, 0),
      new(0, 1, 1),
      new(1, 0, 0),
      new(1, 0, 1),
      new(1, 1, 0),
      new(1, 1, 1)
    };

    // Vertices for each face in a cube
    private static readonly int[][] faceVerts =
    {
      new[] { 1, 3, 7, 5 }, // Forward
      new[] { 5, 7, 6, 4 }, // Right
      new[] { 4, 6, 2, 0 }, // Back
      new[] { 0, 2, 3, 1 }, // Left
      new[] { 3, 2, 6, 7 }, // Up
      new[] { 1, 5, 4, 0 } // Down
    };

    // Vectors for each face
    private static readonly int3[] dirs =
    {
      new(0, 0, 1),
      new(1, 0, 0),
      new(0, 0, -1),
      new(-1, 0, 0),
      new(0, 1, 0),
      new(0, -1, 0)
    };

    // Generate a chunk mesh. Returns null on failure.
    public static Mesh CreateMesh(int3 chunkCoord, IClusterData cluster)
    {
      // Initialize
      var vertices = new List<int3>();
      var normals = new List<int3>();
      var uv = new List<Vector3>();
      var triangles = new List<int>();
      var v = 0;

      var chunk = cluster.GetChunk(chunkCoord);
      if (chunk == null) return null;

      var chunkGlobalCoord = chunkCoord * GameSettings.CHUNK_SIZE;

      // Iterate through the non-null blocks in the chunk
      var blockIt = chunk.CoordIterator();
      foreach (var tuple in blockIt)
      {
        // Get coord and block
        int3 coord = tuple.Item1;
        BlockSO block = tuple.Item2;
        if (block == null) {
          Debug.LogError("Block is null");
        }

        // Iterate through faces
        for (var i = 0; i < 6; i++)
        {
          // Store direction the face faces
          var dir = dirs[i];

          // Get global-space coordinate of block adjacent to target block
          var adj = coord + dir + chunkGlobalCoord;

          // If adjacent block in the cluster is null, render the face.
          if (cluster.GetBlock(adj) == null)
          {
            // Add vertices and normals
            var verts = faceVerts[i];
            for (var j = 0; j < 4; j++)
            {
              var offset = cubeVerts[verts[j]];
              var vertCoord = coord + offset;
              vertices.Add(vertCoord);
              normals.Add(dir);
            }

            // Add triangles
            triangles.Add(v);
            triangles.Add(v + 2);
            triangles.Add(v + 1);
            triangles.Add(v + 2);
            triangles.Add(v);
            triangles.Add(v + 3);
            v += 4;

            // Get index of texture for the face
            int index;
            if (block.isSameFaces) {
              index = block.mainTexture;
            } else {
              if (dir.y > 0)
                index = block.topTexture;
              else if (dir.y < 0)
                index = block.bottomTexture;
              else
                index = block.mainTexture;
            }

            // Add uvs
            uv.Add(new Vector3(1, 0, index));
            uv.Add(new Vector3(1, 1, index));
            uv.Add(new Vector3(0, 1, index));
            uv.Add(new Vector3(0, 0, index));
          }
        }
      }

      // Create the Mesh object with the data
      // TODO: Optimize by doing calculations with Vector3 rather than converting here
      var mesh = new Mesh();
      mesh.vertices = vertices.Select(v => new Vector3(v.x, v.y, v.z)).ToArray();
      mesh.normals = normals.Select(n => new Vector3(n.x, n.y, n.z)).ToArray();
      mesh.SetUVs(0, uv.ToArray());
      mesh.triangles = triangles.ToArray();

      return mesh;
    }
  }
}