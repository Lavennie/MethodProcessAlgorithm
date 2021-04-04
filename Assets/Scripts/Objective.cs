using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] levelAddons;
    private ILevelAddon[] addons;

    private List<GameObject> pickups = new List<GameObject>();
    private int pickedCount = 0;

    private void OnEnable()
    {
        addons = new ILevelAddon[levelAddons.Length];
        for (int i = 0; i < addons.Length; i++)
        {
            addons[i] = (ILevelAddon)levelAddons[i];
        }

        CheckForPickups();
    }

    public void StartLevel()
    {
        for (int i = 0; i < levelAddons.Length; i++)
        {
            levelAddons[i].enabled = true;
        }
    }

    void Update()
    {
        if (CodeExecutor.Instance == null || !CodeExecutor.Instance.enabled) { return; }

        for (int i = 0; i < addons.Length; i++)
        {
            if (addons[i].RecheckForPickups)
            {
                CheckForPickups();
                addons[i].RecheckForPickups = false;
            }
        }

        if (pickups.Count == 0)
        {
            for (int i = 0; i < addons.Length; i++)
            {
                if (pickedCount < addons[i].MinPickupCount)
                {
                    return;
                }
            }
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

    private void CheckForPickups()
    {
        pickups.Clear();
        pickups.AddRange(GameObject.FindGameObjectsWithTag("Pickup"));
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
        Instance.pickedCount++;
        Destroy(pickup);
    }

    public static Objective Instance { get { return Database.Instance?.Objective; } }
}


public interface ILevelAddon
{
    bool RecheckForPickups { get; set; }
    int MinPickupCount { get; }
}