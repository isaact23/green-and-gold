using GnG.Core;
using GnG.Core.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace GnG.UI.Inventory.Stash
{
  public class StashUI : MonoBehaviour
  {
    [SerializeField] private InventoryData inventory;
    [SerializeField] private UnityEvent onOpenStash;
    [SerializeField] private UnityEvent onCloseStash;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private StashRow[] stashRows;

    private bool isOpen;

    // Called whenever the inventory updates
    public void OnUpdateInventory() {
      for (int i = 0; i < stashRows.Length; i++) {
        stashRows[i].UpdateItems(inventory);
      }
    }

    // Hide stash without invoking onCloseStash
    public void CloseStash() {
      closeStash();
    }

    private void openStashAndInvoke()
    {
      openStash();
      onOpenStash.Invoke();
    }

    private void openStash() {
      isOpen = true;
      inventoryPanel.SetActive(true);
    }

    private void closeStashAndInvoke()
    {
      closeStash();
      onCloseStash.Invoke();
    }

    private void closeStash() {
      isOpen = false;
      inventoryPanel.SetActive(false);
    }

    private void toggleStashAndInvoke()
    {
      if (isOpen)
      {
        closeStashAndInvoke();
      }
      else
      {
        openStashAndInvoke();
      }
    }

    void Awake()
    {
      closeStashAndInvoke();
    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.E))
        toggleStashAndInvoke();
    }
  }
}