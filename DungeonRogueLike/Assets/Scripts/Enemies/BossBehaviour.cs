using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The behaviour of bosses like Max an Brutus, these have a bigger range then other enemies
/// also more effects and attack variants can be added by sepereating the boss from the standard enemies
/// </summary>
public class BossBehaviour : Enemy, IDamagable
{
    [Header("Attack")]
    [SerializeField] private int attackForce = 1;

    [Header("Detection")]
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private LayerMask detectionMasks; //Select Grid-Mask
    [SerializeField] private Vector3 boxSize;
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

        Vector3 Origin = transform.position;
        Quaternion rotation = transform.rotation;


        RaycastHit[] hits = Physics.BoxCastAll(Origin, boxSize, transform.forward, rotation, 0, detectionMasks);

        foreach (RaycastHit hit in hits)
        {
            GridNode node = hit.collider.GetComponent<GridNode>();
            if (node != null)
            { 
                ObjectList.Add(hit.transform.gameObject.GetComponent<GridNode>());
                
            }
        }

        return ObjectList;
    }

    private void OnDrawGizmos()
    {
        //left
        Gizmos.DrawWireCube(transform.position, boxSize*2);
    }
}
