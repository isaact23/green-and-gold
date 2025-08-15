using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
  public class ChunkSaveEditor
  {
    private static readonly string clusterPath = "Assets/Cluster/";

    [MenuItem("GNG/Save Cluster Prefab")]
    public static void SaveClusterPrefab()
    {
      if (Directory.Exists(clusterPath))
        Directory.Delete(clusterPath, true);
      Directory.CreateDirectory(clusterPath);

      var cluster = GameObject.Find("Main Cluster");
      foreach (var filter in cluster.GetComponentsInChildren<MeshFilter>())
      {
        var path = clusterPath + filter.gameObject.name + " Mesh.asset";
        AssetDatabase.CreateAsset(filter.sharedMesh, path);
      }

      PrefabUtility.SaveAsPrefabAsset(cluster, clusterPath + "Cluster.prefab");

      Debug.Log("Cluster prefab saved");
    }
  }
}