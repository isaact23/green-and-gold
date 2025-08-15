using GnG.Exceptions.Inventory;
using GnG.Data.BaseItem;
using GnG.Data.BaseItem.Block;
using GnG.Data.BaseItem.Item;
using UnityEngine;

namespace GnG.Util {
    public class BaseItemSpriteFinder {
        // Given an item, find the corresponding sprite to display in the inventory.
        public static Sprite FindSpriteFor(BaseItemSO baseItem) {

            if (baseItem.toolbarSprite != null) {
                return baseItem.toolbarSprite;
            }
            else if (baseItem is BlockSO) {
                BlockSO block = (BlockSO) baseItem;
                return Texture2DArraySelector.GetSprite(block.mainTexture);
            }
            else if (baseItem is ItemSO) {
                ItemSO item = (ItemSO) baseItem;
                return item.sprite;
            }
            else {
                throw new InventorySpriteException("Failed to resolve texture for item" + baseItem.displayName);
            }
        }
    }
}
