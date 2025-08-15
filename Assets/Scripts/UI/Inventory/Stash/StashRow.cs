using GnG.Core.Inventory;
using UnityEngine;

namespace GnG.UI.Inventory.Stash {
    public class StashRow : MonoBehaviour {
        [SerializeField] private ItemPanel[] itemPanels;
        [SerializeField] private int rowNumber;

        public void UpdateItems(InventoryData inventory) {
            for (int i = 0; i < itemPanels.Length; i++) {

                ItemPanel itemPanel = itemPanels[i];
                InventoryEntry entry = inventory.GetStashEntry(rowNumber, i);

                itemPanel.ShowEntry(entry);
            }
        }
    }
}
