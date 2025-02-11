using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;      // Continuous forward speed.
    public float laneSwitchSpeed = 5f;    // (Optional) Speed for smooth lane switching.
    public float laneDistance = 2f;       // Distance from center lane to left/right lane.

    // 0: left lane, 1: center lane, 2: right lane.
    private int currentLane = 1;

    void Update()
    {
        // Move forward continuously.
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Check for input to switch lanes.
        if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
        {
            currentLane--;
            MoveToLane();
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentLane < 2)
        {
            currentLane++;
            MoveToLane();
        }
    }

    // Updates the player's x position based on the current lane.
    void MoveToLane()
    {
        // Calculate target x-position: center lane (index 1) is x = 0, left (0) is -laneDistance, right (2) is laneDistance.
        float targetX = (currentLane - 1) * laneDistance;
        Vector3 pos = transform.position;
        pos.x = targetX;
        transform.position = pos;
    }
}
