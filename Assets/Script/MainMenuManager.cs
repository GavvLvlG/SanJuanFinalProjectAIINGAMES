using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Gameplay";

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
