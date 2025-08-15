using GnG.Cluster.ClusterGen.MainClusterGen.BiomeGen;
using GnG.Data.WorldGeneration.Settings;
using System;
using System.Collections.Generic;
using GnG.Settings;
using Unity.Mathematics;
using UnityEngine;
using GnG.Util;
using Random = System.Random;

// A basic hill generator
namespace GnG.Cluster.ClusterGen.MainClusterGen.HillGen
{
  public class HillGenerator : IHillGenerator
  {
    private readonly IBiomeGenerator biomeGenerator;

    // Store noise blocks in chunks. int3's are chunk coordinates, while float3's are in GLOBAL space.
    private readonly Dictionary<int3, List<float3>> noise;
    private readonly Random random;
    private int seed;

    public HillGenerator(IBiomeGenerator biomeGenerator, int worldWidth, int seed)
    {
      this.biomeGenerator = biomeGenerator;
      this.seed = seed;
      random = new Random(seed);
      noise = new Dictionary<int3, List<float3>>();
    }

    // Given an x/z coordinate, determine the height of the column
    public int GetYFromXZ(int x, int z)
    {
      var biomeComposition = biomeGenerator.GetBiomesFromXZ(x, z);
      var settings = ScriptableObject.CreateInstance<HillGeneratorSettings>();
      settings.Merge(biomeComposition);

      var hillRatio = settings.GetHillRatio();
      var center = new Vector3(x, Mathf.FloorToInt(settings.hillAltitude + settings.hillHeight / 2), z);

      float ySum = 0;
      float totalInf = 0;

      // Get enumerable of noise points and distances from the origin
      var coordIt = getNoiseWithin(center, settings.noiseRange);

      foreach (var tuple in coordIt)
      {
        Vector3 coord = tuple.Item1;
        var distance = tuple.Item2;

        // Influence function for hill generator is the logistic function. This greatly
        // impacts how the hills are formed.
        var influence = logistic(distance, 1, -settings.hillGrade, settings.hillWidth);

        ySum += coord.y * influence;
        totalInf += influence;
      }

      // Calculate weighted average
      if (totalInf == 0) return 0;
      return Mathf.FloorToInt(ySum / totalInf);
    }

    private float logistic(float x, float supremum, float grade, float midpoint)
    {
      var denom = 1 + Mathf.Exp(-grade * (x - midpoint));
      if (denom == 0) return 0;
      return supremum / denom;
    }

    /// <summary>
    ///   Iterator for noise within a certain distance of an origin.
    /// </summary>
    /// <param name="origin">The center of noise to query.</param>
    /// <param name="dist">Max distance from center to get noise.</param>
    /// <returns>
    ///   IEnumerator for Vector3s paired with their distances from the origin,
    ///   where all points are within the specified distance of the origin.
    /// </returns>
    private IEnumerable<Tuple<float3, float>> getNoiseWithin(float3 origin, float maxDist)
    {
      var noiseWithin = new List<Tuple<float3, float>>();

      var chunkPos = CoordConvert.GetChunkCoord(
        new int3(
          Mathf.FloorToInt(origin.x),
          Mathf.FloorToInt(origin.y),
          Mathf.FloorToInt(origin.z)
        )
      );
      var chunkDist = Mathf.CeilToInt(maxDist / GameSettings.CHUNK_SIZE);

      for (var x = chunkPos.x - chunkDist; x < chunkPos.x + chunkDist; x++)
      for (var y = chunkPos.y - chunkDist; y < chunkPos.y + chunkDist; y++)
      for (var z = chunkPos.z - chunkDist; z < chunkPos.z + chunkDist; z++)
      {
        var otherChunkPos = new int3(x, y, z);
        var points = getNoiseForChunk(otherChunkPos);

        foreach (var point in points)
        {
          var dist = math.distance(point, origin);
          if (dist < maxDist)
            noiseWithin.Add(new Tuple<float3, float>(point, dist));
        }
      }

      return noiseWithin;
    }

    /// <summary>
    ///   Get noise for a chunk. Generate it if it doesn't exist.
    /// </summary>
    /// <param name="chunkPos">The chunk coordinate of the chunk to generate noise for.</param>
    /// <returns>The list of noise points for the chunk.</returns>
    private List<float3> getNoiseForChunk(int3 chunkPos)
    {
      // Check if they already exist
      if (noise.TryGetValue(chunkPos, out var points)) return points;

      // Generate points randomly
      points = new List<float3>();

      var lowerBound = chunkPos * GameSettings.CHUNK_SIZE;
      var upperBound = (chunkPos + new int3(1, 1, 1)) * GameSettings.CHUNK_SIZE;
      for (var x = lowerBound.x; x < upperBound.x; x++)
      for (var z = lowerBound.z; z < upperBound.z; z++)
      {
        var biomeComposition = biomeGenerator.GetBiomesFromXZ(x, z);
        var settings = ScriptableObject.CreateInstance<HillGeneratorSettings>();
        settings.Merge(biomeComposition);
        var hillRatio = settings.GetHillRatio();

        for (var y = lowerBound.y; y < upperBound.y; y++)
          if (random.Next() % hillRatio == 0)
            points.Add(new Vector3(x, y, z));
      }

      // Save points
      noise[chunkPos] = points;
      return points;
    }
  }
}