// using GnG.Data;
// using UnityEditor;
// using UnityEngine;

// namespace Editor.ScriptableObjectEditors
// {
//   [CustomEditor(typeof(ItemSO))]
//   public class ItemSOEditor : UnityEditor.Editor
//   {
//     public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
//     {
//       var item = (ItemSO)target;

//       if (item == null || item.Sprite == null)
//         return new Texture2D(16, 16);

//       var sprite = item.Sprite;
//       return AssetPreview.GetAssetPreview(sprite);
//     }
//   }
// }