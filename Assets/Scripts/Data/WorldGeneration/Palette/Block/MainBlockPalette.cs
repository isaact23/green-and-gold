using GnG.Data.BaseItem.Block;
using UnityEngine;

namespace GnG.Data.WorldGeneration.Palette.Block {
    
    [CreateAssetMenu(menuName = "GnG/Main World Generator/Main Block Palette", order = 1)]
    public class MainBlockPalette : ScriptableObject {
        [SerializeField] public BlockSO grass;
        [SerializeField] public BlockSO dirt;
        [SerializeField] public BlockSO stone;
        [SerializeField] public BlockSO leaves;
        [SerializeField] public BlockSO log;
        [SerializeField] public BlockSO ash;
        [SerializeField] public BlockSO sand;

    }
}
