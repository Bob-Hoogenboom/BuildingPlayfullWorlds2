using UnityEngine;
using System;
using System.Threading.Tasks;

/// <summary>
/// TaroDev Tutorial: https://www.youtube.com/watch?v=4I0vonyqMi8
/// </summary>
/// 
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState state;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;    

    }

    private void Start()
    {
        UpdateGameState(GameState.Tutorial);
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Tutorial:
                HandleSelectTutorial();
                break;
            case GameState.PlayerTurn:
                HandlePlayerTurn();
                break;
            case GameState.EnemyTurn:
                HandleEnemyTurn();
                break;
            case GameState.Decide:
                HandleDecide();
                break;
            case GameState.Victory:
                HandelVictory();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(newState);

    }

    private void HandelVictory()
    {
        MenuManager.Instance.ToggleVictoryScreen(true);
    }

    private void HandleLose()
    {
        MenuManager.Instance.ToggleLoseScreen(true);
    }

    private async void HandleDecide()
    {
        EnemyBehaviour[] enemies = FindObjectsOfType<EnemyBehaviour>();
        PlayerController player = FindObjectOfType<PlayerController>();

        await Task.Delay(500);

        if (enemies.Length <= 0) UpdateGameState(GameState.Victory);
        else if (player == null) UpdateGameState(GameState.Lose);
        else UpdateGameState(GameState.PlayerTurn);
    }

    private async void HandleEnemyTurn()
    {
        await Task.Delay(2000);

        EnemyManager.Instance.ActivateAction();

        await Task.Delay(2000);
    }

    private void HandlePlayerTurn()
    {

    }

    private void HandleSelectTutorial()
    {

    }
}

public enum GameState
{
    Tutorial,
    PlayerTurn,
    EnemyTurn,
    Decide,
    Victory,
    Lose
}
