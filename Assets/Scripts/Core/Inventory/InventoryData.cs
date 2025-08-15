using GnG.Data;
using GnG.Data.BaseItem;
using GnG.Exceptions.Inventory;
using GnG.Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GnG.Core.Inventory
{
  public class InventoryData : MonoBehaviour
  {
    [SerializeField] private UnityEvent onChange;
  
    /* Entries with no items are null. */
    private InventoryEntry[] toolbarEntries = new InventoryEntry[GameSettings.TOOLBAR_CAPACITY];
    private InventoryEntry[] inventoryEntries = new InventoryEntry[GameSettings.STASH_CAPACITY];

    /* Get the toolbar entry from the given index. */
    public InventoryEntry GetToolbarEntry(int index) {
      checkToolbarIndex(index);
      return toolbarEntries[index];
    }

    /* Get the stash entry for the given row and column. */
    public InventoryEntry GetStashEntry(int row, int col) {
      checkInventoryIndex(row, col);
      return inventoryEntries[getInventoryIndex(row, col)];
    }

    /* Set the item and count for an element in the toolbar. */
    public void SetToolbarEntry(int index, InventoryEntry entry) {
      checkToolbarIndex(index);
      toolbarEntries[index] = entry;
      
      onChange.Invoke();
    }

    /* Append a number of a type of item to the inventory. If it
       doesn't completely fit, return the number of excess items,
       else return 0. */
    public int Pickup(BaseItemSO item, int count) {

      if (item == null) {
        throw new InventoryArgumentException("item must not be null");
      }

      if (count == 0) return 0;
      int remaining = count;

      // Loop through inventory entries and update counts of matching items.
      var matchingEntries = findEntries(item);
      foreach (InventoryEntry entry in matchingEntries) {

        // If the entry can't fit the remaining elements,
        if (entry.Count + remaining > entry.MaxStackSize) {
          int numAdded = entry.MaxStackSize - entry.Count;
          remaining -= numAdded;
          entry.ChangeCountBy(numAdded);

        // If the entry fits the remaining elements,
        } else {
          entry.ChangeCountBy(remaining);
          remaining = 0;
          break;
        }
      }

      // If no matching inventory entries, see if there is empty space to create entries
      for (int i = 0; remaining > 0 && i < toolbarEntries.Length; i++) {
        if (toolbarEntries[i] == null) {
          int entryCount = Math.Min(item.maxStackSize, remaining);
          toolbarEntries[i] = new InventoryEntry(item, entryCount);
          remaining = remaining - entryCount;
        }
      }
      for (int i = 0; remaining > 0 && i < inventoryEntries.Length; i++) {
        if (inventoryEntries[i] == null) {
          int entryCount = Math.Min(item.maxStackSize, remaining);
          inventoryEntries[i] = new InventoryEntry(item, entryCount);
          remaining = remaining - entryCount;
        }
      }

      // The number of leftover items is returned.
      onChange.Invoke();
      return remaining;
    }

    /* Find inventory entries whose item matches the given item. */
    private IEnumerable<InventoryEntry> findEntries(BaseItemSO item) {
      for (int i = 0; i < toolbarEntries.Length; i++)
      {
        if (toolbarEntries[i] != null && toolbarEntries[i].Item == item) yield return toolbarEntries[i];
      }
      for (int i = 0; i < inventoryEntries.Length; i++) {
        if (inventoryEntries[i] != null && inventoryEntries[i].Item == item) yield return inventoryEntries[i];
      }
    }

    /* Validate a toolbar index to be within the range of [0, toolbarSize). */
    private void checkToolbarIndex(int index) {
      if (index < 0 || index >= GameSettings.STASH_CAPACITY)
        throw new InventoryArgumentException("Invalid index " + index + " for toolbar access");
    }

    /* Get the inventory index given a row/column. */
    private int getInventoryIndex(int row, int col) {
      checkInventoryIndex(row, col);
      return col + (row * GameSettings.STASH_WIDTH);
    }

    /* Validate an inventory row/col to be within the range defined in GameSettings. */
    private void checkInventoryIndex(int row, int col) {
      if (row < 0 || col < 0 || row >= GameSettings.STASH_HEIGHT || col >= GameSettings.STASH_WIDTH) {
        throw new InventoryArgumentException("Invalid index " + row + " " + col + " for stash access");
      }
    }
  }
}
