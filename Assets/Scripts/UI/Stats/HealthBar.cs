using GnG.Core.Stats;
using GnG.Enums;
using GnG.Settings;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GnG.UI.Stats
{
  public class HealthBar : MonoBehaviour
  {
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartHalf;
    [SerializeField] private Sprite heartEmpty;

    /* Update the heart sprites to reflect the current health. */
    public void UpdateSprites(int health)
    {
      int runningHealth = health;
      for (int i = 0; i < hearts.Length; i++) {
        if (runningHealth == 0) {
          hearts[i].sprite = heartEmpty;
        }
        else if (runningHealth == 1) {
          hearts[i].sprite = heartHalf;
          runningHealth = 0;
        } else {
          hearts[i].sprite = heartFull;
          runningHealth -= 2;
        }
      }
    }
  }
}
