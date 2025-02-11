using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the player moves forward

    void Update()
    {
        // Move the player forward (along the Z-axis) at a constant speed
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
