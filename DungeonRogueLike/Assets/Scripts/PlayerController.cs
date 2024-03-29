using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("General")]
    private GameObject playerOBJ;
    [SerializeField] private GridNode _currentGridNode;
    private GridNode _nextGridNode;

    [Header("Detection")]
    [SerializeField] private LayerMask gridNodeMask;
    [SerializeField] private float rayLength;
    private RaycastHit _hit;
    private Vector3 _rayDir;
    private bool _playerTurn;

    [Header("Attack")]
    [SerializeField] private int m_health = 3;
    [SerializeField] private int attackPower = 1;
    [SerializeField] private GameObject[] heartSprites;
    private int healthIndex;

    public int Health 
    { 
        get => m_health ;
        set => m_health = value;
    }

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        _playerTurn = (state == GameState.PlayerTurn);
    }

    private void Start()
    {
        playerOBJ = gameObject;
        healthIndex = m_health;
    }

    private void Update()
    {
        if (!_playerTurn) return;
        //WASD raycast to check gridnodes if occupied
        //North
        if (Input.GetKeyDown(KeyCode.W))
        {
            _rayDir = Vector3.forward;
            EmitRaycastCheck(_rayDir);
        }

        //East
        if (Input.GetKeyDown(KeyCode.D))
        {
            _rayDir = Vector3.right;
            EmitRaycastCheck(_rayDir);
        }

        //South
        if (Input.GetKeyDown(KeyCode.S))
        {
            _rayDir = -Vector3.forward;
            EmitRaycastCheck(_rayDir);
        }

        //West
        if (Input.GetKeyDown(KeyCode.A))
        {
            _rayDir = -Vector3.right;
            EmitRaycastCheck(_rayDir);
        }
    }

    private void EmitRaycastCheck(Vector3 dir)
    {
        if (Physics.Raycast(transform.position, dir, out _hit,  rayLength, gridNodeMask))
        {
            var nodeHit = _hit.transform.gameObject.GetComponent<GridNode>();

            if(nodeHit != _nextGridNode)
            {
                nodeHit.ToggleGlow(true);
                _nextGridNode = nodeHit; 
            }
            else
            {
                nodeHit.ToggleGlow(false);
                if (nodeHit.isOccupied)
                {
                    //switchcase for enemies/items/weapons/magic?
                    if(nodeHit.objectOnThisNode.GetComponent<EnemyBehaviour>())
                    {
                        //Unit is another entity or enemy*
                        Debug.Log("Attack");
                        Attack(nodeHit);
                        return;
                    }
                    else
                    {
                        //Debug.Log("Obstacle detected");
                        return;
                    }
                }
                else
                {
                    //move onto gridnode
                    Move();
                    return;
                }
            }
        }
    }

    private void Move()
    {
        _currentGridNode.SetOccupation(playerOBJ, false);
        playerOBJ.transform.position = _nextGridNode.transform.position;
        _nextGridNode.SetOccupation(playerOBJ, true);
        _currentGridNode = _nextGridNode;
        _nextGridNode = null;

        EndTurn();
    }

    private void Attack(GridNode enemyNode)
    {
        IDamagable iDamage = enemyNode.objectOnThisNode.GetComponent<IDamagable>();
        iDamage.Damage(attackPower);
        EndTurn();
    }

    public void SetFirstGridNode(GridNode node)
    {
        _currentGridNode = node;
    }

    private void EndTurn()
    {
        GameManager.Instance.UpdateGameState(GameState.EnemyTurn);
        _playerTurn = false;
    }

    public void Damage(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            heartSprites[healthIndex - 1].SetActive(false);
            healthIndex -= 1;
        }

        if ((m_health -= amount) <= 0)
        {
            Destroy(gameObject);
        }
    }
}
