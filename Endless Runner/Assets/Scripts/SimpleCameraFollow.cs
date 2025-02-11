using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform player;         // Drag your player here in the Inspector.
    public Vector3 offset = new Vector3(0, 10, -10); // Adjust offset as needed.

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
            transform.LookAt(player);
        }
    }
}
