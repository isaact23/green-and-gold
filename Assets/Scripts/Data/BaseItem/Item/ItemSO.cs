
using UnityEngine;

namespace GnG.Data.BaseItem.Item
{
  [CreateAssetMenu(menuName = "GnG/Item", order = 1)]
  public class ItemSO : BaseItemSO
  {
    [SerializeField] public Sprite sprite;
  }
}