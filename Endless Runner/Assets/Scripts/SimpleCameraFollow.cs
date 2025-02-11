using UnityEngine;

public class SimpleCameraFollowFixedRotation : MonoBehaviour
{
    public Transform player;       // Assign your player GameObject in the Inspector.
    public float followSpeed = 5f; // Adjust for how quickly the camera catches up.

    // Offsets for the vertical (y) and forward/backward (z) positioning.
    public float offsetY = 10f;
    public float offsetZ = -10f;

    private float fixedX;          // The camera's fixed horizontal (x) position.
    private Quaternion fixedRotation;  // The desired fixed rotation.

    void Start()
    {
        // Store the camera's initial x position so it remains fixed.
        fixedX = transform.position.x;

        // Store the camera's initial rotation to maintain it.
        fixedRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate a target position that uses the player's y and z plus the offsets, but keeps x fixed.
            Vector3 targetPosition = new Vector3(fixedX, player.position.y + offsetY, player.position.z + offsetZ);

            // Smoothly move the camera to the target position.
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            // Maintain a fixed rotation (remove the dynamic LookAt).
            transform.rotation = fixedRotation;
        }
    }
}
