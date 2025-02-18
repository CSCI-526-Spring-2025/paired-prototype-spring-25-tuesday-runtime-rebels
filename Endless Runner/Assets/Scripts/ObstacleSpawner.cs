using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public GameObject jumpObstaclePrefab;  // ✅ Jumpable obstacle (Green)
    public GameObject crouchObstaclePrefab; // ✅ Crouchable obstacle (Red)

    public Transform[] spawnPoints; // ✅ 3 lane spawn points

    private float crouchObstacleHeight = 1.7f; // 🚀 Adjust this height for crouching under

    void Start()
    {
        SpawnNewObstacles();
    }

    public void SpawnNewObstacles()
    {
        // Clear previous obstacles before spawning new ones
        foreach (Transform spawnPoint in spawnPoints)
        {
            foreach (Transform child in spawnPoint)
            {
                Destroy(child.gameObject);
            }
        }

        // Decide which lanes will get obstacles (each lane has a 50% chance)
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (Random.value > 0.2f) // ✅ 80% chance to spawn an obstacle
            {
                GameObject obstaclePrefab = (Random.value > 0.5f) ? jumpObstaclePrefab : crouchObstaclePrefab;
                GameObject obstacle = Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity);

                // 🚨 If it's a CrouchObstacle, adjust its Y-position
                if (obstaclePrefab == crouchObstaclePrefab)
                {
                    Vector3 newPosition = obstacle.transform.position;
                    newPosition.y += crouchObstacleHeight; // ✅ Raise the crouch obstacle
                    obstacle.transform.position = newPosition;
                }

                obstacle.transform.localScale = new Vector3(1f, 1f, 1f); // ✅ Reset scale
            }
        }
    }
}
