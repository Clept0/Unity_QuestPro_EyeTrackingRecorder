using System.Collections.Generic;
using UnityEngine;

public class HeatmapGenerator : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem gazePointParticles; // Particle system to be spawned at gaze point
    private OVREyeGaze ovrEyeGaze;

    [SerializeField]
    private float spawnCooldown = 0.1f; // The cooldown between each spawn of the particle system
    private float spawnTimer;

    [SerializeField]
    private int maxGazePoints = 1000; // Maximum number of gaze points
    private Queue<ParticleSystem> gazePoints = new Queue<ParticleSystem>(); // Collection of active gaze points

    [SerializeField]
    private Color minColor = Color.blue; // Color representing low density
    [SerializeField]
    private Color maxColor = Color.red; // Color representing high density

    [SerializeField]
    private float colorChangeSpeed = 1.0f; // Speed at which color changes from min to max color

    private Dictionary<Vector3Int, int> gazeDensity = new Dictionary<Vector3Int, int>(); // Holds the density of gaze points
    [SerializeField]
    private int gridSize = 5; // Size of the grid cell

    [SerializeField]
    private float spawnOffset = 0.1f; // Offset for the spawning location of the particle system

    private void Awake()
    {
        ovrEyeGaze = GetComponent<OVREyeGaze>(); // Access the OVREyeGaze component
    }

    void Update()
    {
        if (!ovrEyeGaze.EyeTrackingEnabled) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer < spawnCooldown) return;

        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.forward, out hit)) return;

        var gazePosition = hit.point + hit.normal * spawnOffset; // Add offset to the gaze point
        var gridPosition = Vector3Int.FloorToInt(gazePosition / gridSize);

        // Update density
        if (!gazeDensity.ContainsKey(gridPosition))
            gazeDensity[gridPosition] = 0;
        gazeDensity[gridPosition]++;

        // Calculate color based on density
        float density = gazeDensity[gridPosition];
        Color color = Color.Lerp(minColor, maxColor, density / (maxGazePoints * colorChangeSpeed));

        // Create gaze point with particles
        var newGazePoint = Instantiate(gazePointParticles, gazePosition, Quaternion.identity);
        var mainModule = newGazePoint.main;
        mainModule.startColor = color;

        gazePoints.Enqueue(newGazePoint);
        if (gazePoints.Count > maxGazePoints)
        {
            var oldestGazePoint = gazePoints.Dequeue();
            Destroy(oldestGazePoint.gameObject); // Destroy the oldest gaze point
        }

        spawnTimer = 0f;
    }
}
