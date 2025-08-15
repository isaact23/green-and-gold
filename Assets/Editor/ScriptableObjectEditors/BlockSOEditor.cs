// using GnG.Data;
// using UnityEditor;
// using UnityEngine;
// using GnG.Util;

// namespace Editor.ScriptableObjectEditors
// {
//   [CustomEditor(typeof(BlockSO))]
//   public class BlockSOEditor : UnityEditor.Editor
//   {
//     private SerializedProperty bottomTexture;
//     private SerializedProperty isSameFaces;
//     private SerializedProperty mainTexture;
//     private SerializedProperty topTexture;

//     private void OnEnable()
//     {
//       isSameFaces = serializedObject.FindProperty("isSameFaces");
//       mainTexture = serializedObject.FindProperty("mainTexture");
//       topTexture = serializedObject.FindProperty("topTexture");
//       bottomTexture = serializedObject.FindProperty("bottomTexture");
//     }

//     public override void OnInspectorGUI()
//     {
//       base.OnInspectorGUI();
//       serializedObject.Update();

//       EditorGUILayout.PropertyField(isSameFaces, new GUIContent("Same faces"));
//       EditorGUILayout.PropertyField(mainTexture);
//       if (!isSameFaces.boolValue)
//       {
//         EditorGUILayout.PropertyField(topTexture);
//         EditorGUILayout.PropertyField(bottomTexture);
//       }

//       serializedObject.ApplyModifiedProperties();
//     }

//     public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
//     {
//       Texture2D img = Texture2DArraySelector.Get(mainTexture.intValue);
//       return img;
//     }
//   }
// }