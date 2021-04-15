using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public static void Open()
    {
        Instance.gameObject.SetActive(true);
        Instance.Next.gameObject.SetActive(LevelManager.CurLevelNumber() < 10);
    }
    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void NextLevel()
    {
        LevelManager.LoadLevel(LevelManager.CurLevelNumber() + 1);
    }

    private ColoredElementButton Replay { get { return transform.GetChild(0).GetComponent<ColoredElementButton>(); } }
    private ColoredElementButton MainMenu { get { return transform.GetChild(1).GetComponent<ColoredElementButton>(); } }
    private ColoredElementButton Next { get { return transform.GetChild(2).GetComponent<ColoredElementButton>(); } }
    private TextMeshProUGUI WinLoseText { get { return transform.GetChild(3).GetComponent<TextMeshProUGUI>(); } }

    private static EndScreen Instance { get { return Database.Instance?.EndScreen; } }
}
