using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public static void Open(bool won)
    {
        Instance.gameObject.SetActive(true);
        if (won)
        {
            Instance.Next.gameObject.SetActive(true);
            Instance.WinLoseText.text = "WIN";
        }
        else
        {
            Instance.Next.gameObject.SetActive(false);
            Instance.WinLoseText.text = "LOSE";
        }
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
        Debug.Log("NEXT level");
    }

    private Button Replay { get { return transform.GetChild(0).GetComponent<Button>(); } }
    private Button MainMenu { get { return transform.GetChild(1).GetComponent<Button>(); } }
    private Button Next { get { return transform.GetChild(2).GetComponent<Button>(); } }
    private TextMeshProUGUI WinLoseText { get { return transform.GetChild(3).GetComponent<TextMeshProUGUI>(); } }

    private static EndScreen Instance { get { return Database.Instance?.EndScreen; } }
}
