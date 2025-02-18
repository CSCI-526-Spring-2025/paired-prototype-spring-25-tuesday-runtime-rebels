using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour // ✅ Change from NewBehaviourScript to PlayerCollision
{
    private bool collisionEnabled = false; // 🚨 Collision is disabled at the start

    void Start()
    {
        Debug.Log(gameObject.name + " PlayerCollision script is active!");
        Invoke("EnableCollision", 1.5f); // ✅ Enable collision after 1.5 seconds
    }

    void EnableCollision()
    {
        collisionEnabled = true; // ✅ Now we can detect collisions
        Debug.Log("Collision detection is now enabled!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!collisionEnabled) return; // 🚨 Ignore collisions if delay is active

        Debug.Log(gameObject.name + " hit " + other.gameObject.name); // Debug log

        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("🚨 COLLISION DETECTED! Stopping game...");
            StopGame();
        }
    }

    void StopGame()
    {
        Debug.Log("🛑 GAME STOPPED! Time Scale = 0");
        Time.timeScale = 0f; // ✅ This pauses the game
    }
}
