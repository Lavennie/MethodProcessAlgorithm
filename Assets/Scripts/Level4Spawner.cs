using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Spawner : MonoBehaviour, ILevelAddon
{
    [SerializeField] private int spawnCount = 10;
    private int originalSpawnCount;
    [SerializeField] private float spawnWidth = 40.0f;
    [SerializeField] private float spawnSpeed = 1.0f;
    [SerializeField] private float spawnedSpeed = 10.0f;
    [SerializeField] private GameObject pickupPrefab;
    private float timer = 0.0f;

    private bool recheckForPickups = false;

    private void Awake()
    {
        originalSpawnCount = spawnCount;
        timer = spawnSpeed;
    }

    void Update()
    {
        if (spawnCount <= 0)
        {
            enabled = false;
            return;
        }

        if (timer > spawnSpeed)
        {
            GameObject pickup = Instantiate(pickupPrefab, transform);
            pickup.transform.position = transform.position + new Vector3(Random.Range(-spawnWidth / 2.0f, spawnWidth / 2.0f), 0, 0);
            pickup.AddComponent<MovingPickup>().translation = new Vector3(0, 0, -spawnedSpeed);
            recheckForPickups = true;
            timer = 0.0f;
            spawnCount--;
        }

        timer += Time.deltaTime;
    }

    public bool RecheckForPickups 
    { 
        get { return recheckForPickups; }
        set { recheckForPickups = value; }
    }
    public int MinPickupCount { get { return originalSpawnCount; } }
}