using UnityEngine;

public class TwoPlayerManager : MonoBehaviour
{
    [Header("Player Setup")]
    public GameObject playerPrefab;  // The player prefab (a cuboid placeholder)
    public Transform spawnPoint;     // SpawnPoint to define the y and z coordinates

    // Define the x-positions for lanes 1, 2, 3.
    public float[] laneX = new float[] { -2f, 0f, 2f };

    // Store the two player instances and their current lane numbers.
    private GameObject player1, player2;
    private int laneP1, laneP2;

    [Header("Camera Target")]
    public Transform cameraTarget;   // Drag the CameraTarget GameObject here in the Inspector

    void Start()
    {
        // --- SPAWN TWO PLAYERS IN RANDOM LANES ---
        int[] lanes = new int[] { 1, 2, 3 };

        // Shuffle the lanes array (simple Fisher–Yates shuffle)
        for (int i = 0; i < lanes.Length; i++)
        {
            int rnd = Random.Range(i, lanes.Length);
            int temp = lanes[i];
            lanes[i] = lanes[rnd];
            lanes[rnd] = temp;
        }

        // Pick the first two lanes from the shuffled array
        laneP1 = lanes[0];
        laneP2 = lanes[1];

        // Use the spawnPoint's y and z; assign x based on lane positions
        Vector3 pos1 = new Vector3(laneX[laneP1 - 1], spawnPoint.position.y, spawnPoint.position.z);
        Vector3 pos2 = new Vector3(laneX[laneP2 - 1], spawnPoint.position.y, spawnPoint.position.z);

        // Instantiate the two players
        player1 = Instantiate(playerPrefab, pos1, Quaternion.identity);
        player2 = Instantiate(playerPrefab, pos2, Quaternion.identity);

        // Set the cameraTarget's initial position
        UpdateCameraTarget();
    }

    void Update()
    {
        HandleInput();
        UpdateCameraTarget();
    }

    // Computes the midpoint between the two players and updates the camera target.
    void UpdateCameraTarget()
    {
        if (player1 != null && player2 != null && cameraTarget != null)
        {
            Vector3 midPoint = (player1.transform.position + player2.transform.position) / 2f;
            cameraTarget.position = midPoint;
        }
    }

    // Handle lane switching input according to the following rules:
    // (1,2): Only D works → right player from lane 2 goes to lane 3.
    // (2,3): Only A works → left player from lane 2 goes to lane 1.
    // (1,3): Both keys work.
    //         - A moves the right player (lane 3) to lane 2.
    //         - D moves the left player (lane 1) to lane 2.
    void HandleInput()
    {
        // Determine which lane is on the left and which is on the right.
        int leftLane = Mathf.Min(laneP1, laneP2);
        int rightLane = Mathf.Max(laneP1, laneP2);

        // Process A key presses
        if (Input.GetKeyDown(KeyCode.A))
        {
            // If configuration is (2,3)
            if (leftLane == 2 && rightLane == 3)
            {
                // Move the player in lane 2 (left player) to lane 1.
                if (laneP1 == 2)
                {
                    laneP1 = 1;
                    UpdatePlayerPosition(player1, laneP1);
                }
                else if (laneP2 == 2)
                {
                    laneP2 = 1;
                    UpdatePlayerPosition(player2, laneP2);
                }
            }
            // If configuration is (1,3)
            else if (leftLane == 1 && rightLane == 3)
            {
                // Move the player in lane 3 (right player) to lane 2.
                if (laneP1 == 3)
                {
                    laneP1 = 2;
                    UpdatePlayerPosition(player1, laneP1);
                }
                else if (laneP2 == 3)
                {
                    laneP2 = 2;
                    UpdatePlayerPosition(player2, laneP2);
                }
            }
            // If configuration is (1,2), pressing A does nothing.
        }

        // Process D key presses
        if (Input.GetKeyDown(KeyCode.D))
        {
            // If configuration is (1,2)
            if (leftLane == 1 && rightLane == 2)
            {
                // Move the player in lane 2 (right player) to lane 3.
                if (laneP1 == 2)
                {
                    laneP1 = 3;
                    UpdatePlayerPosition(player1, laneP1);
                }
                else if (laneP2 == 2)
                {
                    laneP2 = 3;
                    UpdatePlayerPosition(player2, laneP2);
                }
            }
            // If configuration is (1,3)
            else if (leftLane == 1 && rightLane == 3)
            {
                // Move the player in lane 1 (left player) to lane 2.
                if (laneP1 == 1)
                {
                    laneP1 = 2;
                    UpdatePlayerPosition(player1, laneP1);
                }
                else if (laneP2 == 1)
                {
                    laneP2 = 2;
                    UpdatePlayerPosition(player2, laneP2);
                }
            }
            // If configuration is (2,3), pressing D does nothing.
        }
    }

    // Helper function to update a player's x position based on its lane number.
    void UpdatePlayerPosition(GameObject player, int laneNumber)
    {
        Vector3 pos = player.transform.position;
        pos.x = laneX[laneNumber - 1];
        player.transform.position = pos;
    }
}
