using GnG.Core.Inventory;
using GnG.Util;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GnG.UI.Inventory {
    public class ItemPanel : MonoBehaviour {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;

        public void ShowEntry(InventoryEntry entry) {
            if (entry != null) {
                Sprite sprite = BaseItemSpriteFinder.FindSpriteFor(entry.Item);

                text.text = entry.Count.ToString();
                image.sprite = sprite;
                image.enabled = true;
                text.enabled = true;
            
            } else {
                image.enabled = false;
                text.enabled = false;
            }
        }
    }
}