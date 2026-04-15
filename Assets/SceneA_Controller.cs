using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SceneA_Controller : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject housePrefab;
    public GameObject markerPrefab;
    public PlaneFinderBehaviour planeFinder;

    [Header("Settings")]
    public int maxMarkers = 10;

    private GameObject _spawnedHouse;
    private int _markerCount = 0;
    private bool _housePlaced = false;

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
        if (!_housePlaced)
        {
            _spawnedHouse = Instantiate(housePrefab, result.Position, Quaternion.identity);
            _housePlaced = true;
            return;
        }

        if (_markerCount < maxMarkers)
        {
            Instantiate(markerPrefab, result.Position, Quaternion.identity);
            _markerCount++;
        }
    }

    public void ResetMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("Marker");
        foreach (var m in markers) Destroy(m);
        _markerCount = 0;
    }
}