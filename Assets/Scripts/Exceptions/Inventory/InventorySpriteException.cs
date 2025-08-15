using System;

namespace GnG.Exceptions.Inventory {
    // Use for issues with sprites in the inventory.
    public class InventorySpriteException : InventoryException {
        public InventorySpriteException() {}
        public InventorySpriteException(string message) : base(message) {}
        public InventorySpriteException(string message, Exception inner) : base(message, inner) {}
    }
}
