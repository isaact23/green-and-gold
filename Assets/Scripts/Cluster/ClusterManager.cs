using GnG.Data.BaseItem;
using GnG.Data.BaseItem.Block;
using GnG.Data.WorldGeneration.Palette;
using GnG.Data.WorldGeneration.Settings;
using GnG.Cluster.ClusterGen;
using GnG.Cluster.ClusterGen.MainClusterGen;
using GnG.Cluster.ClusterGen.FlatClusterGen;
using GnG.Cluster.Data;
using GnG.Settings;
using GnG.Util;

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace GnG.Cluster
{

  // TODO: Rename to just 'Cluster'
  public class ClusterManager : MonoBehaviour
  {
    [SerializeField] private Material texMat;
    [SerializeField] private MainMasterPalette palette;
    [SerializeField] private WorldGeneratorSettings settings;

    private Dictionary<int3, GameObject> chunks;
    private IClusterData clusterData;
    private GameObject clusterObj;
    private IClusterGenerator mainClusterGen;
    private int layer;

    private void Awake()
    {
      mainClusterGen = new MainClusterGenerator(palette, settings);
      layer = LayerMask.NameToLayer("Terrain");
      chunks = new Dictionary<int3, GameObject>();
      //mainClusterGen = new FlatClusterGenerator();
    }

    // Get the block type at the given coordinate.
    public BlockSO GetBlock(int3 coord) {
      return clusterData.GetBlock(coord);
    }

    // Update a block and regenerate terrain.
    public void SetBlock(int3 coord, BlockSO block)
    {
      clusterData.SetBlock(coord, block);
      var chunkCoord = CoordConvert.GetChunkCoord(coord);
      regenChunk(chunkCoord);

      // If adjacent to other chunks, regenerate them too
      var local = CoordConvert.GetLocalCoord(coord);

      void Regen(int3 dir)
      {
        regenChunk(chunkCoord + dir);
      }

      var edgeCoord = GameSettings.CHUNK_SIZE - 1;
      if (local.x == 0) Regen(new int3(-1, 0, 0));
      if (local.y == 0) Regen(new int3(0, -1, 0));
      if (local.z == 0) Regen(new int3(0, 0, -1));
      if (local.x == edgeCoord) Regen(new int3(1, 0, 0));
      if (local.y == edgeCoord) Regen(new int3(0, 1, 0));
      if (local.z == edgeCoord) Regen(new int3(0, 0, 1));
    }

    // Generate new terrain.
    public void Generate()
    {
      destroyCluster();

      clusterData = mainClusterGen.Create();
      clusterObj = new GameObject();
      clusterObj.name = "Main Cluster";
      clusterObj.layer = layer;

      var chunkIt = clusterData.ChunkIterator();
      foreach (var chunkCoord in chunkIt) regenChunk(chunkCoord);
    }

    // Regenerate a chunk.
    private void regenChunk(int3 chunkCoord)
    {
      var mesh = ChunkMesher.CreateMesh(chunkCoord, clusterData);

      if (mesh == null) return;
      mesh.name = "Chunk Mesh " + chunkCoord;

      GameObject obj;
      if (!chunks.TryGetValue(chunkCoord, out obj))
      {
        // Initialize chunk game object

        obj = new GameObject();
        obj.layer = layer;
        obj.name = "Chunk " + chunkCoord;

        obj.transform.position = new Vector3(
          chunkCoord.x * GameSettings.CHUNK_SIZE,
          chunkCoord.y * GameSettings.CHUNK_SIZE,
          chunkCoord.z * GameSettings.CHUNK_SIZE
        );

        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshCollider>();
        obj.AddComponent<MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = texMat;

        obj.transform.SetParent(clusterObj.transform);

        chunks[chunkCoord] = obj;
      }

      // Swap out mesh
      var meshFilter = obj.GetComponent<MeshFilter>();
      var meshCollider = obj.GetComponent<MeshCollider>();
      var oldMesh = meshFilter.sharedMesh;
      meshFilter.sharedMesh = mesh;
      meshCollider.sharedMesh = mesh;

      if (oldMesh != null)
        // Free mesh from memory
        DestroyImmediate(oldMesh);
    }

    private void destroyCluster()
    {
      if (clusterObj != null)
      {
        var filters = clusterObj.GetComponentsInChildren<MeshFilter>();
        foreach (var filter in filters) DestroyImmediate(filter.sharedMesh);
        Destroy(clusterObj);
      }
    }
  }
}