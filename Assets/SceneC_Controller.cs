using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SceneC_Controller : MonoBehaviour
{
    [Header("Assign prefabs — HouseB must be index 0")]
    public GameObject[] modelPrefabs;
    public PlaneFinderBehaviour planeFinder;
    public Vector3 spawnOffset = new Vector3(0.5f, 0f, 0f);

    private int _nextIndex = 0;
    private readonly System.Collections.Generic.List<GameObject> _spawned = new();

    void Start()
    {
        planeFinder.OnInteractiveHitTest.AddListener(OnPlaneTapped);
    }

    void OnDestroy()
    {
        planeFinder.OnInteractiveHitTest.RemoveListener(OnPlaneTapped);
    }

    void OnPlaneTapped(HitTestResult result)
    {
        if (_nextIndex == 0)
        {
            // Always spawn House B first
            SpawnAt(0, result.Position);
            _nextIndex = 1;
            return;
        }

        if (_nextIndex >= modelPrefabs.Length)
            _nextIndex = 1; // loop back, skip house

        SpawnAt(_nextIndex, result.Position + spawnOffset * _spawned.Count);
        _nextIndex++;
    }

    void SpawnAt(int index, Vector3 position)
    {
        if (modelPrefabs[index] == null) return;
        var obj = Instantiate(modelPrefabs[index], position, Quaternion.identity);
        _spawned.Add(obj);
    }

    public void ResetAll()
    {
        foreach (var obj in _spawned) Destroy(obj);
        _spawned.Clear();
        _nextIndex = 0;
    }
}