using UnityEngine;
using UnityEngine.Events;

namespace GnG.UI.Danger {
    public class DeathPopup : MonoBehaviour {
        [SerializeField] private UnityEvent onPressRespawn;
        [SerializeField] private GameObject deathPopupPanel;

        public void Show() {
            deathPopupPanel.SetActive(true);
        }

        public void OnPressRespawn() {
            deathPopupPanel.SetActive(false);
            onPressRespawn.Invoke();
        }
    }
}
