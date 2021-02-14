using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private List<GameObject> pickups = new List<GameObject>();

    private void OnEnable()
    {
        pickups.Clear();
        pickups.AddRange(GameObject.FindGameObjectsWithTag("Pickup"));
    }

    void Update()
    {
        if (CodeExecutor.Instance == null || !CodeExecutor.Instance.enabled) { return; }

        if (pickups.Count == 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        enabled = false;
        CodeExecutor.Instance.enabled = false;
        CodeExecutor.Instance.ResetToIdle();
        EndScreen.Open(true);
    }

    public static GameObject PickupNearestTo(Vector3 point)
    {
        float minDist = float.MaxValue;
        GameObject nearest = null;
        foreach (var pickup in Instance.pickups)
        {
            float dist = Vector3.Distance(point, pickup.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = pickup;
            }
        }
        return nearest;
    }

    public static void PickPickup(GameObject pickup)
    {
        Instance.pickups.Remove(pickup);
        Destroy(pickup);
    }

    public static Objective Instance { get { return Database.Instance?.Objective; } }
}
