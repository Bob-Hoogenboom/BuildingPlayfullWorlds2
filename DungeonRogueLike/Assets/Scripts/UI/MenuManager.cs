using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject victorypanel;
    [SerializeField] private GameObject losePanel;

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged; 
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        tutorialPanel.SetActive(state == GameState.Tutorial);
    }

    public async void ToggleVictoryScreen(bool isActive)
    {
        Time.timeScale = 0;
        victorypanel.SetActive(isActive);
        await Task.Delay(2000);
        BackToMainMenu();
    }

    public async void ToggleLoseScreen(bool isActive)
    {
        Time.timeScale = 0;
        losePanel.SetActive(isActive);
        await Task.Delay(2000);
        BackToMainMenu();
    }

    //Unity Action for the continue button on the Tutorial Screen
    public void ContinueToGame()
    {
        GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
