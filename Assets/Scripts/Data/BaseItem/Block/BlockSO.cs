
using GnG.TextureSelector;
using UnityEngine;
using UnityEngine.Audio;

namespace GnG.Data.BaseItem.Block
{
  [CreateAssetMenu(menuName = "GnG/Block", order = 1)]
  public class BlockSO : BaseItemSO
  {
    [SerializeField] public bool isSameFaces = true;

    [SerializeField] [TextureSelector]
    public int mainTexture;

    [SerializeField] [TextureSelector]
    public int topTexture;

    [SerializeField] [TextureSelector]
    public int bottomTexture;
    [SerializeField]
    public AudioResource placeSound;
    [SerializeField]
    public AudioResource breakSound;
  }
}