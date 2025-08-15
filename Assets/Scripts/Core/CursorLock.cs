using GnG.Cluster;
using UnityEngine;

namespace GnG.Core
{
  public class CursorLock : MonoBehaviour
  {
    [SerializeField] private Texture2D cursorTexture;

    public void Lock() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Unlock() {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Awake()
    {
      Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
      Lock();
    }
  }
}
