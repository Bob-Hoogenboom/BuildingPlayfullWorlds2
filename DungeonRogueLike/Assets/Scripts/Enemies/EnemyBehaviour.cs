using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Idle,
    Move,
    Attack
}

public class EnemyBehaviour : MonoBehaviour
{
    [Header("GENERAL")]
    public States states;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask detectionMasks;

    [SerializeField] private List<GridNode> walkableNodes = new List<GridNode>();

    public void EnemyAction()
    {
        var list = GetOBJOnNode();
        foreach (var x in list)
        {
            Debug.Log(x.ToString());
        }

        //check every node to see if there is a player or fellow enemy nearby
        foreach (var gridNode in list)
        {
            if(gridNode.objectOnThisNode != null)
            {
                if (gridNode.objectOnThisNode.transform.gameObject.CompareTag("Player"))
                {
                    states = States.Attack;
                    break;
                }
            }
            else
            {
                Debug.Log("Add node");
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
                Debug.Log("Enemy" + name + "Invoke Idle State");
                EnemyIdle();
                break;
            case States.Move:
                Debug.Log("Enemy" + name + "Invoke Move State");
                EnemyMove();
                break;
            case States.Attack:
                Debug.Log("Enemy" + name + "Invoke Attack State");
                EnemyAttack();
                break;
            default:
                Debug.LogError("End of Switchcase no valid State given");
                break;
        }
    }

    private void EnemyIdle()
    {
        EndOfTurn();
    }

    private void EnemyMove()
    {
        int nextNode = Random.Range(0, walkableNodes.Count);

        GridNode node = walkableNodes[nextNode];

        transform.position = node.GetComponent<Transform>().position;

        EndOfTurn();
    }

    private void EnemyAttack()
    {
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

}
