using System;
using UnityEngine;
using UnityEngine.Profiling;
using Unity.Mathematics;
using GnG.Cluster;
using GnG.Avatar;

namespace GnG.World {
    /*
    Store all data related to a GnG world. This is a wrapper class to the
    cluster that provides additional data storage, such as object metadata,
    spawnpoints, etc. Also handles initializing the world.
    */
    public class World : MonoBehaviour {
        [SerializeField] private ClusterManager clusterManager;
        [SerializeField] private Motor motor;
        [SerializeField] private int3 spawnPoint;

        void Start() {
            clusterManager.Generate();
            motor.Spawn(GetSpawnPoint());
        }

        // Get the world spawn point adjusted to the nearest ground block.
        public int3 GetSpawnPoint() {
            for (int i = 0; i < 200; i++) {
                // Look upward
                int3 point = spawnPoint + new int3(0, i, 0);
                if (isSafeSpawnPoint(point))
                    return point;

                // Look downward
                point = spawnPoint - new int3(0, i, 0);
                if (isSafeSpawnPoint(point))
                    return point;
            }

            // If no valid point is found, just return the stored spawn point
            return spawnPoint;
        }

        // Determine if the spawn point fits the player safely.
        private bool isSafeSpawnPoint(int3 point) {
            return clusterManager.GetBlock(point) == null &&
                   clusterManager.GetBlock(point + new int3(0, 1, 0)) == null &&
                   clusterManager.GetBlock(point - new int3(0, 1, 0)) != null;
        }
    }
}