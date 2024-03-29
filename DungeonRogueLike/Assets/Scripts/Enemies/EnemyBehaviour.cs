using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Idle,
    Move,
    Attack
}

public class EnemyBehaviour : MonoBehaviour, IDamagable
{
    [Header("GENERAL")]
    public States states;

    [Header("Detection")]
    public GridNode _currentGridNode;
    [SerializeField] private LayerMask detectionMasks; //Grid-Mask
    [SerializeField] private float rayDistance = 3f;

    [Header("MoveState")]
    [SerializeField] private List<GridNode> walkableNodes = new List<GridNode>();

    [Header("AttackState")]
    [SerializeField] private GridNode playerNode;
    [SerializeField] private int m_health = 3;
    [SerializeField] private int attackPower = 1;

    public int Health 
    {
        get => m_health;
        set => m_health = value;
    }

    public void EnemyAction()
    {
        var list = GetOBJOnNode();

        //check every node to see if there is a player or fellow enemy nearby
        foreach (var gridNode in list)
        {
            if(gridNode.objectOnThisNode != null)
            {
                if (gridNode.objectOnThisNode.transform.gameObject.CompareTag("Player"))
                {
                    playerNode = gridNode;
                    states = States.Attack;
                    break;
                }
            }
            else
            {
                //if the node is empty add it to a list of walkable nodes
                walkableNodes.Add(gridNode);
            }
        }

        //if the list of walkable nodes is higher then 0 and no player was detected walk : otherwise stay in Idle state
        if (walkableNodes.Count > 0 && states != States.Attack)
        {
            states = States.Move;
        }
        else if (states != States.Attack) 
        {
            states = States.Idle;
        }


        switch (states)
        {
            case States.Idle:
                EnemyIdle();
                break;
            case States.Move:
                EnemyMove();
                break;
            case States.Attack:
                EnemyAttack();
                break;
            default:
                Debug.LogError("End of Switchcase no valid State given");
                break;
        }
    }

    //simply does nothing but can be filled with an effect later
    private void EnemyIdle()
    {
        EndOfTurn();
    }

    //removes this object from the current node
    //moves to the next node
    //sets the new node as the new currentnode
    private void EnemyMove()
    {
        _currentGridNode.SetOccupation(gameObject, false);

        int nextNode = Random.Range(0, walkableNodes.Count);
        _currentGridNode = walkableNodes[nextNode];
        transform.position = _currentGridNode.GetComponent<Transform>().position;

        _currentGridNode.SetOccupation(gameObject, true);

        EndOfTurn();
    }

    private void EnemyAttack()
    {
        IDamagable iDamagable = playerNode.gameObject.GetComponent<IDamagable>();

        iDamagable.Damage(attackPower);
        EndOfTurn();
    }

    private List<GridNode> GetOBJOnNode()
    {
        List<GridNode> ObjectList = new List<GridNode>();

        RaycastHit hit;
        //left
        if(Physics.Raycast(gameObject.transform.position, Vector3.left, out hit, rayDistance, detectionMasks))
        {
            ObjectList.Add(hit.transform.gameObject.GetComponent<GridNode>());
        }

        //right
        if (Physics.Raycast(gameObject.transform.position, Vector3.right, out hit, rayDistance, detectionMasks))
        {
            ObjectList.Add(hit.transform.gameObject.GetComponent<GridNode>());
        }

        //forward
        if (Physics.Raycast(gameObject.transform.position, Vector3.forward, out hit, rayDistance, detectionMasks))
        {
            ObjectList.Add(hit.transform.gameObject.GetComponent<GridNode>());
        }

        //back
        if (Physics.Raycast(gameObject.transform.position, Vector3.back, out hit, rayDistance, detectionMasks))
        {
            ObjectList.Add(hit.transform.gameObject.GetComponent<GridNode>());
        } 

        return ObjectList;
    }

    //here we reset the values at the end of a turn, it will also allow us to play ahn effect or sound later on
    private void EndOfTurn()
    {
        walkableNodes.Clear();
        states = States.Idle;
    }

    private void OnDrawGizmos()
    {
        //left
        Gizmos.DrawRay(transform.position, Vector3.left * rayDistance);
        //right
        Gizmos.DrawRay(transform.position, Vector3.right * rayDistance);
        //forward
        Gizmos.DrawRay(transform.position, Vector3.forward * rayDistance);
        //back
        Gizmos.DrawRay(transform.position, Vector3.back * rayDistance);
    }

    public void Damage(int amount)
    {
        
    }
}
