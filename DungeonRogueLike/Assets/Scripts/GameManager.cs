using UnityEngine;
using System;
using System.Threading.Tasks;

public enum GameState
{
    Tutorial,
    PlayerTurn,
    EnemyTurn,
    Decide,
    Victory,
    Lose
}

/// <summary>
/// A simple game manager script that gives the player control of their turn, then the enemies and 
/// then checks if one of both parties is defeated
/// TaroDev Tutorial: https://www.youtube.com/watch?v=4I0vonyqMi8
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState state;

    public static event Action<GameState> OnGameStateChanged;

    private BossBehaviour _boss;
    private PlayerController _player;

    private void Awake()
    {
        Instance = this;    

    }

    private void Start()
    {
        UpdateGameState(GameState.Tutorial);
        _boss = FindObjectOfType<BossBehaviour>();
        _player = FindObjectOfType<PlayerController>();
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
        await Task.Delay(250);

        if (_boss.isDead == true) UpdateGameState(GameState.Victory);
        else if (_player.Health <= 0) UpdateGameState(GameState.Lose);
        else UpdateGameState(GameState.PlayerTurn);
    }

    private async void HandleEnemyTurn()
    {
        await Task.Delay(500);

        EnemyManager.Instance.EnemyTurn();

        await Task.Delay(500);
    }

    private void HandlePlayerTurn()
    {

    }

    private void HandleSelectTutorial()
    {
        Time.timeScale = 1;
    }
}
