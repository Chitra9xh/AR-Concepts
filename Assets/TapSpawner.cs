using UnityEngine;

public class TapSpawner : MonoBehaviour
{
    public GameObject cityPrefab;
    public GameObject[] models;

    public float spawnRadius = 2.5f;

    private int tapCount = 0;
    private int modelIndex = 0;

    private GameObject cityInstance;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleTap();
        }
    }

    void HandleTap()
    {
        tapCount++;

        if (tapCount == 1)
        {
            SpawnCity();
        }
        else
        {
            SpawnRandomModel();
        }
    }

    void SpawnCity()
    {
        cityInstance = Instantiate(
            cityPrefab,
            transform.position,
            Quaternion.identity,
            transform
        );
    }

    void SpawnRandomModel()
    {
        if (cityInstance == null || models.Length == 0)
            return;

        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;

        Vector3 spawnPosition = cityInstance.transform.position + new Vector3(
            randomCircle.x,
            0,
            randomCircle.y
        );

        Instantiate(
            models[modelIndex % models.Length],
            spawnPosition,
            Quaternion.identity,
            cityInstance.transform
        );

        modelIndex++;
    }
}