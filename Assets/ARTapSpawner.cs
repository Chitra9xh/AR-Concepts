using UnityEngine;
using Vuforia;
using System.Collections.Generic;

public class ARTapSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]

    [Tooltip("Total number of objects allowed to spawn")]
    public int totalSpawnLimit = 10;

    [Tooltip("Drag and drop prefabs here")]
    public List<GameObject> prefabsToSpawn = new List<GameObject>();

    [Tooltip("Drag GroundPlaneStage here")]
    public Transform spawnParent;

    private PlaneFinderBehaviour planeFinder;

    private int spawnedCount = 0;

    private List<GameObject> randomizedPrefabs;

    void Start()
    {
        planeFinder = FindObjectOfType<PlaneFinderBehaviour>();

        if (planeFinder == null)
        {
            Debug.LogError("PlaneFinder not found in scene");
            return;
        }

        planeFinder.OnInteractiveHitTest.AddListener(OnPlaneHit);

        randomizedPrefabs = new List<GameObject>(prefabsToSpawn);

        ShuffleList(randomizedPrefabs);
    }

    void Update()
    {
        if (spawnedCount >= totalSpawnLimit)
            return;

        if (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began)
        {
            planeFinder.PerformHitTest(Input.GetTouch(0).position);
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            planeFinder.PerformHitTest(Input.mousePosition);
        }
#endif
    }

    void OnPlaneHit(HitTestResult result)
    {
        if (spawnedCount >= totalSpawnLimit)
            return;

        int index = spawnedCount % randomizedPrefabs.Count;

        GameObject prefab = randomizedPrefabs[index];

        GameObject spawnedObject =
            Instantiate(
                prefab,
                result.Position,
                result.Rotation
            );

        if (spawnParent != null)
            spawnedObject.transform.parent = spawnParent;

        spawnedCount++;
    }

    void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];

            int randomIndex =
                Random.Range(i, list.Count);

            list[i] = list[randomIndex];

            list[randomIndex] = temp;
        }
    }
}
