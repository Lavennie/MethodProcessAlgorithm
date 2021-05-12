using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    private List<Vector3> pickups;
    private Vector3 playerPos;
    private Quaternion playerRot;


#if UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void SyncFiles();
#endif

    private void Awake()
    {
        instance = this;
        pickups = new List<Vector3>();
        foreach (var pickup in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            pickups.Add(pickup.transform.position);
        }
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerPos = player.transform.position;
            playerRot = player.transform.rotation;
        }
    }

    public static void LoadLevel(int number)
    {
        if (number < 1)
        {
            Debug.LogError(new System.ArgumentOutOfRangeException("Level to load does not exist"));
            return;
        }
        else if (number > 10)
        {
            LoadMainMenu();
        }
        else
        {
            SceneManager.LoadScene($"Level {number}");
        }
    }
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public static void ReloadLevel()
    {
        instance.StartCoroutine(instance.ReloadLevelInternal());
    }
    private IEnumerator ReloadLevelInternal()
    {
        CodeExecutor.Stop();

        GameObject.FindWithTag("Player").transform.position = instance.playerPos;
        GameObject.FindWithTag("Player").transform.rotation = instance.playerRot;
        GameObject.FindWithTag("Player").GetComponent<Player>().ResetDummy();
        GameObject.FindWithTag("Player").GetComponent<CodeExecutor>().moveRotated = false;

        yield return null;

        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
        foreach (var pickup in instance.pickups)
        {
            Instantiate(pickups[0], pickup, Quaternion.identity, pickups[0].transform.parent);
        }
        foreach (var pickup in pickups)
        {
            Destroy(pickup);
        }

        yield return null;

        Objective.Instance.CheckForPickups();
    }
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public static int CurLevelNumber()
    {
        string[] sceneNameSplitted = SceneManager.GetActiveScene().name.Split(' ');
        return int.Parse(sceneNameSplitted[sceneNameSplitted.Length - 1]);
    }

    public static void UpdateClearCount(int clearCount)
    {
        if (clearCount <= ClearedCount())
        {
            return;
        }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Save data/");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.streamingAssetsPath + "/Save data/clearCount.save", FileMode.OpenOrCreate);
        bf.Serialize(file, new LevelsCleared(clearCount));
        file.Close();
        MemoryStream ms = new MemoryStream();
        using (ms)
        {
            bf.Serialize(ms, new LevelsCleared(clearCount));
        }
#elif UNITY_WEBGL
        PlayerPrefs.SetInt("clearCount", clearCount);
        PlayerPrefs.Save();
        SyncFiles();
#endif
    }
    public static int ClearedCount()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.streamingAssetsPath + "/Save data/clearCount.save"))
        {
            FileStream file = File.Open(Application.streamingAssetsPath + "/Save data/clearCount.save", FileMode.Open);
            LevelsCleared cleared = (LevelsCleared)bf.Deserialize(file);
            file.Close();

            return cleared.count;
        }
        else
        {
            return 0;
        }
#elif UNITY_WEBGL
        return PlayerPrefs.GetInt("clearCount", 0);
#endif
    }

    public static void ClearSaveData()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (!Directory.Exists(Application.streamingAssetsPath + "/Save data/"))
        {
            return;
        }
        
        string[] files = Directory.GetFiles(Application.streamingAssetsPath + "/Save data/");
        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i]);
        }
#elif UNITY_WEBGL
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SyncFiles();
#endif
    }

    [System.Serializable]
    public struct LevelsCleared
    {
        public int count;

        public LevelsCleared(int count)
        {
            this.count = count;
        }
    }
}
