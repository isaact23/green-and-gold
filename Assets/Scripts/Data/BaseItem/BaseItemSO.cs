using UnityEngine;

namespace GnG.Data.BaseItem
{
  public class BaseItemSO : ScriptableObject
  {
    [SerializeField] public string displayName;
    [SerializeField] public int maxStackSize;
    [SerializeField] public Sprite toolbarSprite;
  }
}