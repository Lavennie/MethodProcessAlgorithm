using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private List<GameObject> pickups = new List<GameObject>();
    private int pickedCount = 0;

    private void Awake()
    {
        CheckForPickups();
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
        AudioManager.PlaySoundEffect(AudioManager.SoundEffects.Win);
        CodeSave.ClearSave($"saveData{LevelManager.CurLevelNumber()}.save");
        LevelManager.UpdateClearCount(LevelManager.CurLevelNumber());
    }

    public void CheckForPickups()
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

        ParticleSystem.MainModule main = Instantiate(Database.Instance.pickupPS, pickup.transform.position, Quaternion.identity).main;
        main.startColor = Database.GetColor(ColorPalette.Slot.InverseLight);

        AudioManager.PlaySoundEffect(AudioManager.SoundEffects.Pickup);
    }

    public static Objective Instance { get { return Database.Instance?.Objective; } }
}