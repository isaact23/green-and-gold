using UnityEngine;

namespace GnG.Data.Avatar
{
  [CreateAssetMenu(fileName = "Avatar Motor Settings", menuName = "Avatar Motor Settings", order = 1)]
  public class MotorSettings : ScriptableObject
  {
    public float moveSpeed = 7;
    public float maxFallSpeed = 50;
    public float gravity = 30;
    public float jumpSpeed = 8;
    public float mouseSensitivity = 3;
    public float blockInteractDistance = 5;
  }
}