using GnG.Exceptions.Inventory;
using GnG.Data;
using GnG.Data.BaseItem;
using GnG.UI.Inventory.Toolbar;
using UnityEngine;

namespace GnG.Core.Inventory
{
  public class InventoryManager : MonoBehaviour
  {
    [SerializeField] private InventoryData inventory;
    [SerializeField] private ToolbarUI toolbarUI;


    [SerializeField] private BaseItemSO plackart;
    [SerializeField] private BaseItemSO emerald;
    [SerializeField] private BaseItemSO leaves;
    [SerializeField] private BaseItemSO ladders;
    public InventoryData Inventory {get => inventory; private set => inventory = value;}

    void Awake() {
      inventory.Pickup(ladders, 30);
      inventory.Pickup(plackart, 1);
      inventory.Pickup(emerald, 2);
      inventory.Pickup(leaves, 4);
    }

    /* Get the inventory entry corresponding to the toolbar selection. */
    public InventoryEntry GetSelectedToolbarEntry()
    {
      int index = toolbarUI.GetSelectedIndex();
      return inventory.GetToolbarEntry(index);
    }

    /* Decrement the inventory entry corresponding to the toolbar selection. */
    public void DecrementSelectedToolbarEntry() {

      int index = toolbarUI.GetSelectedIndex();
      InventoryEntry entry = inventory.GetToolbarEntry(index);

      // Disallow decrementing null entries
      if (entry == null) {
        throw new InventoryException("Cannot decrement a toolbar entry that is null");
      }
      // Replace entry with null if the count becomes 0
      if (entry.Count == 1) {
        inventory.SetToolbarEntry(index, null);
        return;
      }
      
      entry.ChangeCountBy(-1);
      inventory.SetToolbarEntry(index, entry);
    }
  }
}
