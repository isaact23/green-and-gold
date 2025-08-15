using UnityEngine;
using UnityEngine.UI;

namespace GnG.UI.Inventory.Toolbar
{
  internal class ToolbarSelector : MonoBehaviour
  {
    [SerializeField] private RectTransform selectPanel;
    private readonly int BOX_COUNT = 8;
    private readonly int BOX_WIDTH = 52;
    private readonly int SELECTOR_X = 0;
    private readonly int TOP_Y = 0;

    private int selected;

    public int GetSelectedIndex() {
      return selected;
    }

    void Update()
    {
      selected -= Mathf.RoundToInt(Input.GetAxis("Mouse ScrollWheel"));
      selected = selected % BOX_COUNT;
      if (selected < 0) selected += BOX_COUNT;

      if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) selected = 0;
      if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) selected = 1;
      if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) selected = 2;
      if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) selected = 3;
      if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) selected = 4;
      if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) selected = 5;
      if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) selected = 6;
      if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) selected = 7;

      MoveSelector();
    }

    private void MoveSelector()
    {
      selectPanel.anchoredPosition = new Vector3(SELECTOR_X, TOP_Y - BOX_WIDTH * selected, 0);
    }
  }
}