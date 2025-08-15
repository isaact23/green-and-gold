using System;

namespace GnG.Exceptions.Inventory {
    public class InventoryArgumentException : InventoryException {
        public InventoryArgumentException() {}
        public InventoryArgumentException(string message) : base(message) {}
        public InventoryArgumentException(string message, Exception inner) : base(message, inner) {}
    }
}
