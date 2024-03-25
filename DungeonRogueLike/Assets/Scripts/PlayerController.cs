using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject playerOBJ;
    [SerializeField] private GridNode _currentGridNode;
    private GridNode _nextGridNode;

    [SerializeField] private LayerMask gridNodeMask;
    [SerializeField] private float rayLength;
    private RaycastHit _hit;
    private Vector3 _rayDir;


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
        
    }

    private void Start()
    {
        playerOBJ = gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
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
                    if(nodeHit.objectOnThisNode.CompareTag("Unit"))
                    {
                        //Unit is another entity or enemy*
                        Debug.Log("Attack");
                    }
                    else
                    {
                        Debug.Log("Obstacle detected");
                        return;
                    }
                }
                else
                {
                    //move onto gridnode
                    _currentGridNode.SetOccupation(playerOBJ, false);
                    playerOBJ.transform.position = _nextGridNode.transform.position;
                    _nextGridNode.SetOccupation(playerOBJ, true);
                    _currentGridNode = _nextGridNode;
                    _nextGridNode = null;
                }
            }
        }
    }

    public void SetFirstGridNode(GridNode node)
    {
        _currentGridNode = node;
    }


}
