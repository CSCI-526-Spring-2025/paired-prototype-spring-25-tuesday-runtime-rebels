using UnityEngine;
using System.Collections;

public class TwoPlayerManager : MonoBehaviour
{
    [Header("Player Setup")]
    public GameObject playerPrefab;
    public Transform spawnPoint;

    public float[] laneX = new float[] { -2f, 0f, 2f };

    private GameObject player1, player2;
    private int laneP1, laneP2;

    [Header("Camera Target")]
    public Transform cameraTarget;

    [Header("Jump and Crouch Settings")]
    public float jumpHeight = 2f;
    public float jumpDuration = 0.5f;
    public float crouchScale = 0.5f;
    public float crouchDuration = 0.5f;

    [Header("Materials")]
    public Material blueMaterial;
    public Material greenMaterial;

    void Start()
    {
        // --- SPAWN TWO PLAYERS IN RANDOM LANES ---
        int[] lanes = new int[] { 1, 2, 3 };

        // Shuffle lanes
        for (int i = 0; i < lanes.Length; i++)
        {
            int rnd = Random.Range(i, lanes.Length);
            int temp = lanes[i];
            lanes[i] = lanes[rnd];
            lanes[rnd] = temp;
        }

        // Assign two random lanes
        laneP1 = lanes[0];
        laneP2 = lanes[1];

        // Spawn players
        Vector3 pos1 = new Vector3(laneX[laneP1 - 1], spawnPoint.position.y, spawnPoint.position.z);
        Vector3 pos2 = new Vector3(laneX[laneP2 - 1], spawnPoint.position.y, spawnPoint.position.z);

        player1 = Instantiate(playerPrefab, pos1, Quaternion.identity);
        player2 = Instantiate(playerPrefab, pos2, Quaternion.identity);

        // Assign different colors
        AssignMaterial(player1, blueMaterial);
        AssignMaterial(player2, greenMaterial);

        // Update camera target
        UpdateCameraTarget();
    }

    // Helper function to assign material
    void AssignMaterial(GameObject player, Material material)
    {
        Renderer renderer = player.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    void Update()
    {
        HandleInput();
        UpdateCameraTarget();
    }

    void UpdateCameraTarget()
    {
        if (player1 != null && player2 != null && cameraTarget != null)
        {
            Vector3 midPoint = (player1.transform.position + player2.transform.position) / 2f;
            cameraTarget.position = midPoint;
        }
    }

    void HandleInput()
    {
        int leftLane = Mathf.Min(laneP1, laneP2);
        int rightLane = Mathf.Max(laneP1, laneP2);

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (leftLane == 2 && rightLane == 3)
            {
                if (laneP1 == 2) { laneP1 = 1; UpdatePlayerPosition(player1, laneP1); }
                else if (laneP2 == 2) { laneP2 = 1; UpdatePlayerPosition(player2, laneP2); }
            }
            else if (leftLane == 1 && rightLane == 3)
            {
                if (laneP1 == 3) { laneP1 = 2; UpdatePlayerPosition(player1, laneP1); }
                else if (laneP2 == 3) { laneP2 = 2; UpdatePlayerPosition(player2, laneP2); }
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (leftLane == 1 && rightLane == 2)
            {
                if (laneP1 == 2) { laneP1 = 3; UpdatePlayerPosition(player1, laneP1); }
                else if (laneP2 == 2) { laneP2 = 3; UpdatePlayerPosition(player2, laneP2); }
            }
            else if (leftLane == 1 && rightLane == 3)
            {
                if (laneP1 == 1) { laneP1 = 2; UpdatePlayerPosition(player1, laneP1); }
                else if (laneP2 == 1) { laneP2 = 2; UpdatePlayerPosition(player2, laneP2); }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Jump(player1));
            StartCoroutine(Crouch(player2));
        }
    }

    void UpdatePlayerPosition(GameObject player, int laneNumber)
    {
        Vector3 pos = player.transform.position;
        pos.x = laneX[laneNumber - 1];
        player.transform.position = pos;
    }

    IEnumerator Jump(GameObject player)
    {
        // Capture the current y value as the ground level.
        float groundY = player.transform.position.y;
        float elapsedTime = 0f;

        // Move upward: gradually increase only the y component.
        while (elapsedTime < jumpDuration)
        {
            float newY = Mathf.Lerp(groundY, groundY + jumpHeight, elapsedTime / jumpDuration);
            Vector3 pos = player.transform.position;  // Get the current x and z.
            pos.y = newY;  // Only update y.
            player.transform.position = pos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure we reach the peak.
        Vector3 peakPos = player.transform.position;
        peakPos.y = groundY + jumpHeight;
        player.transform.position = peakPos;

        yield return new WaitForSeconds(0.2f);  // Hold the peak briefly.

        // Move downward: gradually decrease y back to groundY.
        elapsedTime = 0f;
        while (elapsedTime < jumpDuration)
        {
            float newY = Mathf.Lerp(groundY + jumpHeight, groundY, elapsedTime / jumpDuration);
            Vector3 pos = player.transform.position;  // Current x and z remain.
            pos.y = newY;
            player.transform.position = pos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Make sure the final y is exactly the ground level.
        Vector3 finalPos = player.transform.position;
        finalPos.y = groundY;
        player.transform.position = finalPos;
    }



    IEnumerator Crouch(GameObject player)
    {
        float elapsedTime = 0f;
        Vector3 originalScale = player.transform.localScale;
        Vector3 crouchScaleVec = new Vector3(originalScale.x, crouchScale, originalScale.z);

        while (elapsedTime < crouchDuration)
        {
            player.transform.localScale = Vector3.Lerp(originalScale, crouchScaleVec, elapsedTime / crouchDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.localScale = crouchScaleVec;
        yield return new WaitForSeconds(0.2f);

        elapsedTime = 0f;
        while (elapsedTime < crouchDuration)
        {
            player.transform.localScale = Vector3.Lerp(crouchScaleVec, originalScale, elapsedTime / crouchDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.localScale = originalScale;
    }
}
