using GnG.Audio;
using GnG.Cluster;
using GnG.Core;
using GnG.Enums;
using GnG.Exceptions;
using GnG.Util;
using GnG.Core.Inventory;
using GnG.Core.Stats;
using GnG.Data;
using GnG.Data.Avatar;
using GnG.Data.BaseItem.Block;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace GnG.Avatar
{
  // The player controller
  public class Motor : MonoBehaviour
  {
    [SerializeField] private ClusterManager clusterManager;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameObject camObj;
    [SerializeField] private OneshotSoundManager soundPlayer;
    [SerializeField] private MotorSettings motorSettings;
    [SerializeField] private HealthManager healthManager;
    private CharacterController controller;
    private int raycastMask;
    private Vector3 vel;
    private bool isFrozen;

    // Spawn in the player at the given position.
    public void Spawn(int3 pos) {
      transform.position = new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z + 0.5f);
      Unfreeze();
    }

    public void Freeze() {isFrozen = true;}
    public void Unfreeze() {isFrozen = false;}

    void Awake()
    {
      controller = GetComponent<CharacterController>();
      raycastMask = LayerMask.GetMask("Terrain");
      Unfreeze();
    }

    void Update()
    {
      if (!isFrozen)
      {
        ApplyLook();
        ApplyJump();
        ApplyWalking();

        HandleClick();
      }
      else
      {
        vel.x *= 0.8f;
        vel.z *= 0.8f;
      }

      // Apply lateral movement
      controller.Move(vel * Time.deltaTime);

      ApplyGravity();
    }

    private void ApplyWalking()
    {
      vel.x = 0;
      vel.z = 0;

      vel += Input.GetAxis("Horizontal") * motorSettings.moveSpeed * transform.right;
      vel += Input.GetAxis("Vertical") * motorSettings.moveSpeed * transform.forward;
    }

    private void ApplyGravity()
    {
      if (controller.isGrounded) {
        if (-vel.y > 1) {
          int damage = (int) -vel.y;
          damage = Math.Max(0, damage - 10);
          damage /= 2;
          healthManager.TakeDamage(damage, DamageType.Falling);
        }
        vel.y = 0;
      } else {
        vel.y -= motorSettings.gravity * Time.deltaTime;
        if (vel.y < -motorSettings.maxFallSpeed)
          vel.y = -motorSettings.maxFallSpeed;
      }
    }

    private void ApplyJump()
    {
      if (controller.isGrounded && Input.GetButton("Jump")) {
        vel.y = motorSettings.jumpSpeed;
      }
    }

    private void ApplyLook()
    {
      // Rotate left/right
      var playerRot = transform.eulerAngles;
      playerRot.y += Input.GetAxis("Mouse X") * motorSettings.mouseSensitivity;
      transform.eulerAngles = playerRot;

      // Rotate up/down
      var camRot = camObj.transform.eulerAngles;
      camRot.x -= Input.GetAxis("Mouse Y") * motorSettings.mouseSensitivity;
      if (camRot.x > 90 && camRot.x < 180)
        camRot.x = 90;
      else if (camRot.x >= 180 && camRot.x < 270) camRot.x = 270;
      camObj.transform.eulerAngles = camRot;
    }

    private void HandleClick()
    {
      // Left click to break
      if (Input.GetMouseButtonDown(0))
      {
        RaycastHit hit;
        if (Raycast(out hit))
        {
          float3 mid = hit.point - hit.normal * 0.5f;
          var target = new int3(Mathf.FloorToInt(mid.x), Mathf.FloorToInt(mid.y), Mathf.FloorToInt(mid.z));

          BlockSO block = clusterManager.GetBlock(target);
          if (inventoryManager.Inventory.Pickup(block, 1) != 0) {
            throw new UnhandledEventException("Cannot pick up block - inventory full");
          }

          clusterManager.SetBlock(target, null);

          if (block.breakSound != null)
            soundPlayer.PlayAtPoint(block.breakSound, hit.point, SoundType.Block);
        }
      }

      // Right click to place
      if (Input.GetMouseButtonDown(1))
      {
        // The toolbar selected item must be 1 or more Blocks
        InventoryEntry entry = inventoryManager.GetSelectedToolbarEntry();
        if (entry == null || entry.Item is not BlockSO) return;

        BlockSO block = (BlockSO) entry.Item;
        inventoryManager.DecrementSelectedToolbarEntry();

        RaycastHit hit;
        if (Raycast(out hit))
        {
          float3 mid = hit.point + hit.normal * 0.5f;
          var target = new int3(Mathf.FloorToInt(mid.x), Mathf.FloorToInt(mid.y), Mathf.FloorToInt(mid.z));

          clusterManager.SetBlock(target, block);

          if (block.placeSound != null)
            soundPlayer.PlayAtPoint(block.placeSound, hit.point, SoundType.Block);
        }
      }
    }

    // Raycast from the camera
    private bool Raycast(out RaycastHit hit)
    {
      var ray = new Ray(camObj.transform.position, camObj.transform.forward);
      return Physics.Raycast(ray, out hit, motorSettings.blockInteractDistance, raycastMask);
    }
  }
}