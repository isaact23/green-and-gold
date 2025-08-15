using UnityEngine;

namespace GnG.UI.Danger {
    public class RedOverlay : MonoBehaviour {
        private Animator animator;
        private enum Mode {
            Default, LowHealth, Dead
        }

        void Awake() {
            animator = GetComponent<Animator>();
        }

        public void UpdateForHealth(int health) {
            if (health > 5) {
                setMode(Mode.Default);
            } else if (health > 0) {
                setMode(Mode.LowHealth);
            } else {
                setMode(Mode.Dead);
            }
        }

        private void setMode(Mode mode) {
            if (mode == Mode.Default) {
                animator.SetBool("isLowHealth", false);
                animator.SetBool("isDead", false);
            }
            else if (mode == Mode.LowHealth) {
                animator.SetBool("isLowHealth", true);
                animator.SetBool("isDead", false);
            }
            else if (mode == Mode.Dead) {
                animator.SetBool("isLowHealth", true);
                animator.SetBool("isDead", true);
            }
        }
    }
}