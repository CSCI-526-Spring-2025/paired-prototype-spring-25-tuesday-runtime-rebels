using UnityEngine;

public class EndlessGround : MonoBehaviour
{
    public GameObject laneSeparatorPrefab;

    [Header("Ground Settings")]
    [Tooltip("The ground segment prefab (e.g., your scaled Cube)")]
    public GameObject groundPrefab;

    [Tooltip("Length of one ground segment (should match the Z-scale of your prefab)")]
    public float segmentLength = 30f;

    [Tooltip("How many ground segments to have in the pool")]
    public int numberOfSegments = 3;

    [Header("Player Reference")]
    [Tooltip("Reference to the player transform (or camera) to check progress")]
    public Transform player;

    // Array to hold spawned ground segments
    private GameObject[] segments;

    // Next Z position to spawn a new segment
    private float spawnZ = 0f;

    void Start()
    {
        // Allocate the segments array:
        segments = new GameObject[numberOfSegments];

        for (int i = 0; i < numberOfSegments; i++)
        {
            // Instantiate the ground segment.
            GameObject groundSegment = Instantiate(groundPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);

            // Optionally, instantiate lane separators if a prefab is assigned.
            if (laneSeparatorPrefab != null)
            {
                // Slightly above the ground so they’re visible (adjust Y as needed)
                Vector3 separatorOffset = new Vector3(0, 0.051f, 0);

                // Instantiate the left separator at x = -1 relative to the ground center.
                GameObject leftSeparator = Instantiate(laneSeparatorPrefab,
                    groundSegment.transform.position + separatorOffset + new Vector3(-1, 0, 0), Quaternion.identity);
                leftSeparator.transform.parent = groundSegment.transform;

                // Instantiate the right separator at x = 1 relative to the ground center.
                GameObject rightSeparator = Instantiate(laneSeparatorPrefab,
                    groundSegment.transform.position + separatorOffset + new Vector3(1, 0, 0), Quaternion.identity);
                rightSeparator.transform.parent = groundSegment.transform;
            }

            segments[i] = groundSegment;
            spawnZ += segmentLength;
        }
    }


    void Update()
    {
        // When the player has moved beyond the first segment, recycle it.
        // (Here we check if the player's z-position minus one segment length is greater than the first segment's position.)
        if (player.position.z - segmentLength > segments[0].transform.position.z)
        {
            RecycleSegment();
        }
    }

    // Recycle the first segment by moving it to the end of the track.
    void RecycleSegment()
    {
        // Store the first segment in a temporary variable.
        GameObject firstSegment = segments[0];

        // Shift the segments left in the array.
        for (int i = 0; i < numberOfSegments - 1; i++)
        {
            segments[i] = segments[i + 1];
        }

        // Reposition the first segment to the new "end" position.
        firstSegment.transform.position = new Vector3(0, 0, spawnZ);
        segments[numberOfSegments - 1] = firstSegment;

        // Obstacle Spawning
        ObstacleSpawner spawner = firstSegment.GetComponent<ObstacleSpawner>();
        if (spawner != null)
        {
            spawner.SpawnNewObstacles();
        }

        // Update the next spawn position.
        spawnZ += segmentLength;
    }
}
