using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArrow : MonoBehaviour
{
    public float arrowLifetime = 2f;       // How long the arrow stays visible
    public float circleRadius = 3f;        // The distance from the player where the arrow will appear (circle radius)
    private Transform player;
    private Transform spawner;
    private Camera mainCamera;              // Reference to the main camera

    void Start()
    {
        // Find the player by tag (assuming the player GameObject has the "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get the main camera
        mainCamera = Camera.main;
    }

    public void Initialize(Transform spawner)
    {
        this.spawner = spawner;
        Destroy(gameObject, arrowLifetime);  // Destroy the arrow after its lifetime
    }

    void Update()
    {
        if (spawner != null && player != null && mainCamera != null)
        {
            // Calculate the direction from the player to the spawner
            Vector3 direction = (spawner.position - player.position).normalized;

            // Position the arrow at a certain distance (circleRadius) from the player in the direction of the spawner
            Vector3 offsetPosition = player.position + direction * circleRadius;

            // Apply the offset while keeping the Y position of the arrow slightly above the player
            transform.position = new Vector3(offsetPosition.x, player.position.y + 2f, offsetPosition.z); // Adjust the Y offset as needed

            // Make the arrow face the camera
            Vector3 lookDirection = mainCamera.transform.position - transform.position;
            lookDirection.y = 0;  // Zero out the Y component to keep it level with the ground
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);  // Smoothly rotate to face the camera
        }
    }
}


