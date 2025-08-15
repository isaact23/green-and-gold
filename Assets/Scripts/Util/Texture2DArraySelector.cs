using System;
using UnityEditor;
using UnityEngine;

namespace GnG.Util
{
  public class Texture2DArraySelector
  {
    private static Sprite[] sprites = new Sprite[256];

    private static readonly Texture2DArray tex2DArray = AssetDatabase.LoadAssetAtPath("Assets/Textures/tilemap.png",
      typeof(Texture2DArray)) as Texture2DArray;

    public static Texture2D GetTexture(int index)
    {
      var previewTex = new Texture2D(16, 16);

      if (tex2DArray == null)
      {
        Debug.LogError("Cannot access tilemap.png");
        return previewTex;
      }

      if (index > 255) return previewTex;

      var preview = tex2DArray.GetPixels(index);
      previewTex.SetPixels(preview);
      previewTex.Apply();

      return previewTex;
    }

    public static Sprite GetSprite(int index) {
      if (index > 255) {
        throw new ArgumentException("Index out of bounds");
      }

      Sprite sprite = sprites[index];
      if (sprite != null) return sprite;

      Texture2D texture = GetTexture(index);
      sprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0, 0));
      sprites[index] = sprite;
      return sprite;
    }
  }
}
