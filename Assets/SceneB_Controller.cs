using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SceneB_Controller : MonoBehaviour
{
    [Header("Assign in Inspector — order: HouseB, Godzilla, Dragon, Transformer")]
    public GameObject[] modelPrefabs;
    public PlaneFinderBehaviour planeFinder;

    [Header("Circle Settings")]
    public float circleRadius = 0.5f;

    [Header("Scale Settings (match prefab order)")]
    public Vector3[] modelScales;   

    private bool _spawned = false;

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
        if (_spawned) return;

        int count = modelPrefabs.Length;
        for (int i = 0; i < count; i++)
        {
            if (modelPrefabs[i] == null) continue;

            float angle = i * (360f / count) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)) * circleRadius;

            GameObject obj = Instantiate(modelPrefabs[i], result.Position + offset, Quaternion.identity);

            // 👇 APPLY SCALE IF PROVIDED
            if (modelScales != null && i < modelScales.Length)
                obj.transform.localScale = modelScales[i];
        }

        _spawned = true;
    }
}