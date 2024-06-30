using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;


    //get instance of every enemybehaviour in the scene


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
 
    }

    public void EnemyTurn()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
           enemy.StartEnemyAction();
        }
  
        GameManager.Instance.UpdateGameState(GameState.Decide);
    }
}
