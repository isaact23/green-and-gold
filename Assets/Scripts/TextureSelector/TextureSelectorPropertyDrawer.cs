using UnityEditor;
using UnityEngine;
using GnG.Util;

namespace GnG.TextureSelector
{
  [CustomPropertyDrawer(typeof(TextureSelectorAttribute))]
  public class TextureSelectorPropertyDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var textureSelector = attribute as TextureSelectorAttribute;

      var textureIndex = property.intValue;
      var texture = Texture2DArraySelector.GetTexture(textureIndex);

      EditorGUI.DrawPreviewTexture(new Rect(position.x, position.y, 16, 16), texture);
      EditorGUI.PropertyField(new Rect(position.x + 40, position.y, position.width, position.height), property);
    }
  }
}