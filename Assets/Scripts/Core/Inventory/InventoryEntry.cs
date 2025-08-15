using System;
using GnG.Data.BaseItem;

namespace GnG.Core.Inventory
{
  public class InventoryEntry
  {
    /* The type of item stored in this entry. */
    public BaseItemSO Item { get; private set; }
    /* The number of the item stored in this entry. Must be positive and nonzero. */
    public int Count { get; private set; }
    public int MaxStackSize { get => Item.maxStackSize; }

    public InventoryEntry(BaseItemSO item, int count)
    {
      this.Item = item;
      this.Count = count;

      CheckFields();
    }

    public void ChangeCountBy(int count)
    {
      this.Count += count;
      CheckFields();
    }

    public void SetCount(int count) {
      this.Count = count;
      CheckFields();
    }

    private void CheckFields()
    {
      if (Count < 1)
      {
        throw new ArgumentException("Count must be at least 1");
      }

      if (Count > Item.maxStackSize)
      {
        throw new ArgumentException("Count cannot exceed max stack height");
      }
    }
  }
}
