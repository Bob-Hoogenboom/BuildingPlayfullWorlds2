using System.Collections;
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



    public void ActionSwitch()
    {
        //if player is not in range
        int currentAction = RandomAction(0,1);
        States states = (States)currentAction;


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

    private int RandomAction(int min, int max)
    {
        int randoInt = Random.Range(min, max);
        return randoInt;
    }

    private void EnemyIdle()
    {

    }

    private void EnemyMove()
    {
        var list = GetOBJOnNode();
        
        //for loop that checks the walkable space


        


    }

    private List<GameObject> GetOBJOnNode()
    {
        List<GameObject> ObjectList = new List<GameObject>();

        RaycastHit hit;
        if(Physics.Raycast(gameObject.transform.position, Vector3.left, out hit, rayDistance, 3))
        {
            ObjectList.Add(hit.transform.gameObject.GetComponent<GridNode>().objectOnThisNode);
        }
        if (Physics.Raycast(gameObject.transform.position, Vector3.right, out hit, rayDistance, 3))
        {
            ObjectList.Add(hit.transform.gameObject.GetComponent<GridNode>().objectOnThisNode);
        }
        if (Physics.Raycast(gameObject.transform.position, Vector3.forward, out hit, rayDistance, 3))
        {
            ObjectList.Add(hit.transform.gameObject.GetComponent<GridNode>().objectOnThisNode);
        }
        if (Physics.Raycast(gameObject.transform.position, Vector3.back, out hit, rayDistance, 3))
        {
            ObjectList.Add(hit.transform.gameObject.GetComponent<GridNode>().objectOnThisNode);
        }

        return ObjectList;
    }

    private void EnemyAttack()
    {

    }
}
