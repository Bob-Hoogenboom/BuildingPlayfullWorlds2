using System;
using System.Collections;
using System.Collections.Generic;
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
        //if gamemanager.enum.enemystate invoke every enemybehaviour to do something
    }

    public void ActivateAction()
    {

    }

}
