using GnG.Core;
using GnG.Core.Inventory;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GnG.UI.Inventory.Toolbar
{
  public class ToolbarUI : MonoBehaviour
  {
    [SerializeField] private InventoryData inventory;
    [SerializeField] private ToolbarSelector toolbarSelector;
    [SerializeField] private ItemPanel[] itemPanels;

    public int GetSelectedIndex() {
      return toolbarSelector.GetSelectedIndex();
    }

    // Called whenever the inventory updates
    public void OnUpdateInventory() {
      for (int i = 0; i < itemPanels.Length; i++) {

        ItemPanel itemPanel = itemPanels[i];
        InventoryEntry entry = inventory.GetToolbarEntry(i);

        itemPanel.ShowEntry(entry);
      }
    }
  }
}
