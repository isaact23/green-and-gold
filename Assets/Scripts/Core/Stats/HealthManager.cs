using GnG.Enums;
using GnG.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace GnG.Core.Stats {
  public class HealthManager : MonoBehaviour {
    [SerializeField] private UnityEvent<int> onChange;
    [SerializeField] private UnityEvent onDeath;

    private int health;

    void Awake() {
      health = GameSettings.DEFAULT_HEARTS * 2;
    }
    void Start() {
      onChange.Invoke(health);
    }

    public int GetHealth() {
      return health;
    }

    public void ChangeHealthBy(int diff) {
      if (diff == 0) return;

      // Once health is 0, this object is immutable
      if (health == 0) return;

      if (health + diff <= 0) {
        Die();
      } else {
        health += diff;
      }

      onChange.Invoke(health);
    }

    public void TakeDamage(int damage, DamageType damageType) {
      ChangeHealthBy(-damage);
    }

    public void Die() {
      // Prevent dying twice
      if (health == 0) return;

      health = 0;
      onDeath.Invoke();
      onChange.Invoke(health);
    }
  }
}