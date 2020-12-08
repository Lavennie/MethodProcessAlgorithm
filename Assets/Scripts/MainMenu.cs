using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public const int LEVELS_IN_ROW = 5;

    public RectTransform levelButtonPrefab;
    public int levelCount = 10;

    private void Awake()
    {
        int ROW_COUNT = Mathf.CeilToInt(levelCount / LEVELS_IN_ROW);
        Vector3 startPosition = LevelContainer.position + 
            (Vector3)(LevelContainer.rect.size * new Vector2(-0.5f, (levelCount > LEVELS_IN_ROW) ? 0.5f : 0.0f));
        Vector2 offset = new Vector2(LevelContainer.rect.width / (LEVELS_IN_ROW - 1), LevelContainer.rect.height / (ROW_COUNT - 1));
        for (int i = 0; i < levelCount; i++)
        {
            if (!SceneManager.GetSceneByName(string.Format("Level {0}", i + 1)).IsValid())
            {
                continue;
            }
            RectTransform level = Instantiate(levelButtonPrefab,  LevelContainer);
            level.position = startPosition + new Vector3(i % LEVELS_IN_ROW * offset.x, (i / LEVELS_IN_ROW) * -offset.y, 0);
            level.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("Level {0}", i + 1);
            level.GetComponent<Button>().onClick.AddListener(delegate { LoadLevel(i); });
        }
    }

    private void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(string.Format("Level {0}", levelIndex));
    }

    private RectTransform LevelContainer { get { return (RectTransform)transform.GetChild(0); } }
}
