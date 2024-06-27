using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Enemy, IDamagable
{
    [Header("Attack")]
    [SerializeField] private int attackForce = 1;

    [Header("Detection")]
    [SerializeField] private LayerMask detectionMasks; //Select Grid-Mask
    [SerializeField] private float rayDistance = 3f;

    public override void EnemyAction()
    {
        var list = GetOBJOnNode();

        //check every node to see if there is a player or fellow enemy nearby
        foreach (var gridNode in list)
        {
            if (gridNode.objectOnThisNode != null)
            {
                if (gridNode.objectOnThisNode.transform.gameObject.CompareTag("Player"))
                {
                    playerNode = gridNode;
                    states = EnemyStates.Attack;
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
        if (walkableNodes.Count > 0 && states != EnemyStates.Attack)
        {
            states = EnemyStates.Move;
        }
        else if (states != EnemyStates.Attack)
        {
            states = EnemyStates.Idle;
        }

        switch (states)
        {
            case EnemyStates.Idle:
                EnemyIdle();
                break;
            case EnemyStates.Move:
                StartCoroutine(EnemyMove());
                break;
            case EnemyStates.Attack:
                EnemyAttack(attackForce);
                break;
            default:
                Debug.LogError("End of Switchcase no valid State given");
                break;
        }
    }

    private List<GridNode> GetOBJOnNode()
    {
        List<GridNode> ObjectList = new List<GridNode>();

        RaycastHit hit;
        //left
        if (Physics.Raycast(gameObject.transform.position, Vector3.left, out hit, rayDistance, detectionMasks))
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
