using GnG.Settings;
using Unity.Mathematics;
using UnityEngine;

namespace GnG.Util
{
  public class CoordConvert
  {
    /// <summary>
    ///   Get chunk coordinate from a global coordinate.
    /// </summary>
    /// <param name="pos">A coordinate in world space.</param>
    /// <returns>The chunk coordinate of the chunk containing pos.</returns>
    public static int3 GetChunkCoord(int3 pos)
    {
      // Calculate chunk coords
      var x = Mathf.FloorToInt((float)pos.x / GameSettings.CHUNK_SIZE);
      var y = Mathf.FloorToInt((float)pos.y / GameSettings.CHUNK_SIZE);
      var z = Mathf.FloorToInt((float)pos.z / GameSettings.CHUNK_SIZE);
      return new int3(x, y, z);
    }

    /// <summary>
    ///   Convert a global coordinate to a local coordinate (relative to its chunk).
    /// </summary>
    /// <param name="pos">A coordinate in world space.</param>
    /// <returns>The equivalent coordinate in local space relative to its containing chunk.</returns>
    public static int3 GetLocalCoord(int3 pos)
    {
      var x = pos.x % GameSettings.CHUNK_SIZE;
      var y = pos.y % GameSettings.CHUNK_SIZE;
      var z = pos.z % GameSettings.CHUNK_SIZE;

      if (x < 0) x += GameSettings.CHUNK_SIZE;
      if (y < 0) y += GameSettings.CHUNK_SIZE;
      if (z < 0) z += GameSettings.CHUNK_SIZE;

      return new int3(x, y, z);
    }
  }
}