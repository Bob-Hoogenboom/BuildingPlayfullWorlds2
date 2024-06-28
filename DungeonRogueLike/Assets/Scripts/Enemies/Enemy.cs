using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyStates
{
    Idle,
    Move,
    Attack
}

public class Enemy : MonoBehaviour
{
    public EnemyStates states;
    public GridNode _currentGridNode;
    protected List<GridNode> walkableNodes = new List<GridNode>();

    protected GridNode playerNode;
    public bool isDead = false;
    public int m_health = 3;
    protected float moveSpeed = 5f;
    [SerializeField] private AnimationCurve curve;

    [Header("Effects")]
    public UnityEvent onDamageTaken;


    public int Health
    {
        get => m_health;
        set => m_health = value;
    }


    public void StartEnemyAction()
    {
        if (isDead) return;
        EnemyAction();
    }
    

    public virtual void EnemyAction() { }


    protected void EnemyIdle()
    {
        EndOfTurn();
    }


    protected IEnumerator EnemyMove()
    {
        _currentGridNode.SetOccupation(gameObject, false);

        int randomNode = Random.Range(0, walkableNodes.Count);
       
        Vector3 start = _currentGridNode.transform.position;
        Vector3 goal = walkableNodes[randomNode].transform.position;

        float distance = Vector3.Distance(start, goal);
        float duration = distance / moveSpeed;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            //Current Interpolation value
            float current = elapsedTime / duration;
            //The interpolation value in teh curve
            float curveValue = curve.Evaluate(current);

            gameObject.transform.position = Vector3.Lerp(start, goal, curveValue);
            yield return null;
        }
        //Fail-Save to ensure the player lands on the goal position
        gameObject.transform.position = goal;

        _currentGridNode = walkableNodes[randomNode];
        transform.position = _currentGridNode.GetComponent<Transform>().position;

        _currentGridNode.SetOccupation(gameObject, true);

        EndOfTurn();
    }


    protected void EnemyAttack(int hitpoints)
    {
        IDamagable iDamagable = playerNode.objectOnThisNode.GetComponent<IDamagable>();
        Debug.Log(iDamagable);

        iDamagable.Damage(hitpoints);

        EndOfTurn();
    }


    public void Damage(int amount)
    {
        onDamageTaken.Invoke();

        if ((m_health -= amount) <= 0)
        {
            _currentGridNode.SetOccupation(gameObject, false);
            isDead = true;
            StartCoroutine(Defeated());
        }
    }


    private void EndOfTurn()
    {
        walkableNodes.Clear();
        states = EnemyStates.Idle;
        playerNode = null;
    }

    private IEnumerator Defeated()
    {
        yield return new WaitForSeconds(.5f);

        gameObject.SetActive(false);
        yield return null;  
    }
}
