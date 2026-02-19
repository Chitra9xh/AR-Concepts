using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MultiObjectPlacement : MonoBehaviour
{
    [Header("Prefabs to Spawn")]
    public GameObject[] objectPrefabs; // Assign prefabs in the Inspector

    private int currentIndex = 0; // Keeps track of which object to place next
    private PlaneFinderBehaviour planeFinderBehaviour;
    private ContentPositioningBehaviour contentPositioningBehaviour;

    void Start()
    {
        // Get both components from Plane Finder
        planeFinderBehaviour = GetComponent<PlaneFinderBehaviour>();
        contentPositioningBehaviour = GetComponent<ContentPositioningBehaviour>();

        // Subscribe to hit test event
        planeFinderBehaviour.OnInteractiveHitTest.AddListener(OnHitTest);
    }

    // Called when the user taps on a detected plane
    private void OnHitTest(HitTestResult result)
    {
        if (objectPrefabs == null || objectPrefabs.Length == 0)
        {
            Debug.LogWarning("No prefabs assigned!");
            return;
        }

        // If we've reached the end, start over
        if (currentIndex >= objectPrefabs.Length)
        {
            currentIndex = 0;
            Debug.Log("Restarting prefab placement from beginning.");
        }

        // Select the next prefab to spawn
        GameObject prefabToPlace = objectPrefabs[currentIndex];

        // Place the prefab at the hit test position
        contentPositioningBehaviour.PositionContentAtPlaneAnchor(result);
        Instantiate(prefabToPlace, result.Position, result.Rotation);

        currentIndex++; // Move to next prefab
    }
}
