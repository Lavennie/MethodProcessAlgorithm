using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public const int LEVELS_IN_ROW = 5;

    public RectTransform levelButtonPrefab;
    public int levelCount = 10;

    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject[] pcOnly;

    private void Awake()
    {
        RemakeButtons();
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        for (int i = 0; i < pcOnly.Length; i++)
        {
            pcOnly[i].SetActive(true);
        }
#elif UNITY_WEBGL
        for (int i = 0; i < pcOnly.Length; i++)
        {
            pcOnly[i].SetActive(false);
        }
#endif
    }

    public void GoToLevelSelect()
    {
        mainScreen.SetActive(false);
    }
    public void GoToMainScreen()
    {
        mainScreen.SetActive(true);
    }
    public void CloseGame()
    {
        Application.Quit();
    }

    private void RemakeButtons()
    {
        LevelContainer.DetachChildren();

        // generate level buttons
        int CLEARED_COUNT = LevelManager.ClearedCount();
        int ROW_COUNT = Mathf.CeilToInt(levelCount / LEVELS_IN_ROW);
        Vector3 startPosition = LevelContainer.position +
            (Vector3)(LevelContainer.rect.size * new Vector2(-0.5f, (levelCount > LEVELS_IN_ROW) ? 0.5f : 0.0f));
        Vector2 offset = new Vector2(LevelContainer.rect.width / (LEVELS_IN_ROW - 1), LevelContainer.rect.height / (ROW_COUNT - 1));
        for (int i = 0; i < levelCount; i++)
        {
            RectTransform level = Instantiate(levelButtonPrefab, LevelContainer);
            level.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("Level {0}", i + 1);

            if (i >= LEVELS_IN_ROW)
            {
                level.anchorMin = new Vector2((float)(i % LEVELS_IN_ROW) / (float)(LEVELS_IN_ROW - 1), 0);
                level.anchorMax = level.anchorMin;
            }
            else
            {
                level.anchorMin = new Vector2((float)(i % LEVELS_IN_ROW) / (float)(LEVELS_IN_ROW - 1), 1);
                level.anchorMax = level.anchorMin;
            }
            level.anchoredPosition = Vector2.zero;

            // if level is locked cross it, if it is open add event that loads corresponding level
            if (i > CLEARED_COUNT)
            {
                level.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                int temp = i + 1;
                level.GetComponent<ColoredElementButton>().onClick.AddListener(() => LevelManager.LoadLevel(temp));
            }
        }
    }

    public void ClearSaveData()
    {
        LevelManager.ClearSaveData();
        RemakeButtons();
    }

    private RectTransform LevelContainer { get { return (RectTransform)transform.GetChild(0); } }
}
