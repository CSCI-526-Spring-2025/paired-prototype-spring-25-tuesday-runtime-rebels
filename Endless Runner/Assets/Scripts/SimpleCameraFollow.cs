using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;       // Now assign CameraTarget here instead of a player.
    public float followSpeed = 5f; // Adjust speed if needed.
    public float offsetY = 10f;
    public float offsetZ = -10f;

    private float fixedX;
    private Quaternion fixedRotation;

    void Start()
    {
        fixedX = transform.position.x;
        fixedRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(fixedX, target.position.y + offsetY, target.position.z + offsetZ);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.rotation = fixedRotation;
        }
    }
}
