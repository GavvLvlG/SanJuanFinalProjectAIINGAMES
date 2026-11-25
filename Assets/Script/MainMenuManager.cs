using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Gameplay";
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject highscorePanel;

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenHighscorePanel()
    {
        mainMenuPanel.SetActive(false);
        highscorePanel.SetActive(true);
    }

    public void CloseHighscorePanel()
    {
        highscorePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
