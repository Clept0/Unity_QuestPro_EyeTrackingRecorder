using System.Collections.Generic;
using UnityEngine;

public class HeatmapGenerator : MonoBehaviour
{
    public GameObject gazePointPrefab;
    private OVREyeGaze ovrEyeGaze;

    public float spawnCooldown = 0.1f;
    private float spawnTimer;

    public int maxGazePoints = 1000;
    private Queue<GameObject> gazePoints = new Queue<GameObject>();

    public float gazePointDistance = 10.0f;

    public Color minColor = Color.blue; // Farbe für niedrige Dichte
    public Color maxColor = Color.red; // Farbe für hohe Dichte

    public float colorChangeSpeed = 1.0f; // Geschwindigkeit der Färbung

    private Dictionary<Vector3Int, int> gazeDensity = new Dictionary<Vector3Int, int>();
    public int gridSize = 5; // Größe der Grid-Zellen

    private void Awake()
    {
        ovrEyeGaze = GetComponent<OVREyeGaze>();
    }

    void Update()
    {
        if (!ovrEyeGaze.EyeTrackingEnabled) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer < spawnCooldown) return;

        var gazePosition = transform.position + transform.forward * gazePointDistance;
        var gridPosition = Vector3Int.FloorToInt(gazePosition / gridSize);

        // Dichte aktualisieren
        if (!gazeDensity.ContainsKey(gridPosition))
            gazeDensity[gridPosition] = 0;
        gazeDensity[gridPosition]++;

        // Farbe basierend auf der Dichte berechnen
        float density = gazeDensity[gridPosition];
        Color color = Color.Lerp(minColor, maxColor, density / (maxGazePoints * colorChangeSpeed));

        // Gaze-Punkt mit Farbe erstellen
        var newGazePoint = Instantiate(gazePointPrefab, gazePosition, Quaternion.identity);
        newGazePoint.GetComponent<Renderer>().material.color = color;

        gazePoints.Enqueue(newGazePoint);
        if (gazePoints.Count > maxGazePoints)
        {
            var oldestGazePoint = gazePoints.Dequeue();
            Destroy(oldestGazePoint);
        }

        spawnTimer = 0f;
    }
}
