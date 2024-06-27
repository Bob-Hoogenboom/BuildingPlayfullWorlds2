using System.Collections;
using UnityEngine;

/// <summary>
/// This script handles all the playerMovement actions like attacking, moving, health and animation
/// Should be changed to a statemachine in the future but as controls are simple its not nessesary
/// </summary>
public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("General")]

    [Header("Movement")]
    [SerializeField] private GridNode _currentGridNode;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float moveSpeed = .5f;
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

    [Header("Animation")]
    [SerializeField] private Animator anim;
    private int _isWalkingHash;
    private int _isAttackingHash;
    private int _isHitHash;
    private int _isDefeatedHash;

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
        anim = GetComponentInChildren<Animator>();
        _isWalkingHash = Animator.StringToHash("Walking");
        _isAttackingHash = Animator.StringToHash("Attack");
        _isHitHash = Animator.StringToHash("Hit");
        _isDefeatedHash = Animator.StringToHash("Death");
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
            GridNode nodeHit = _hit.transform.gameObject.GetComponent<GridNode>();

            if(nodeHit != _nextGridNode)
            {
                nodeHit.ToggleGlow(true);
                _nextGridNode = nodeHit;
                RotatePlayer();
            }
            else
            {
                nodeHit.ToggleGlow(false);
                if (nodeHit.isOccupied)
                {
                    //#Switchcase for enemies/items/weapons/magic?
                    if(nodeHit.objectOnThisNode.GetComponent<EnemyBehaviour>())
                    {
                        //Unit is another entity or enemy*
                        Attack(nodeHit);
                        return;
                    }
                    else
                    {
                        //obstacle or wall
                        return;
                    }
                }
                else
                {
                    //move onto gridnode
                    Debug.Log("Move");
                    StartCoroutine(MovePlayer());
                    return;
                }
            }
        }
    }

    //Coroutine to lerp towards the new Node
    private IEnumerator MovePlayer()
    {
        _currentGridNode.SetOccupation(gameObject, false);

        anim.SetBool(_isWalkingHash, true);

        Vector3 start = _currentGridNode.transform.position;
        Vector3 goal = _nextGridNode.transform.position;

        float distance = Vector3.Distance(start, goal);
        float duration= distance/moveSpeed;

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

        _nextGridNode.SetOccupation(gameObject, true);
        _currentGridNode = _nextGridNode;
        _nextGridNode = null;

        anim.SetBool(_isWalkingHash, false);

        EndTurn();
    }

    private void RotatePlayer()
    {
        //Calculate the rotation in which the player should look
        Vector3 targetDir =  _nextGridNode.transform.position - gameObject.transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 360, 0.0f);

        //Apply rotation
        gameObject.transform.rotation = Quaternion.LookRotation(newDir);
    }

    private void Attack(GridNode enemyNode)
    {
        IDamagable iDamage = enemyNode.objectOnThisNode.GetComponent<IDamagable>();
        iDamage.Damage(attackPower);
        anim.SetTrigger(_isAttackingHash);

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
        anim.SetTrigger(_isHitHash);
        if ((m_health -= amount) <= 0)
        {
            anim.SetTrigger(_isDefeatedHash);
        }
    }
}
