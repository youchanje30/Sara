using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// [RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    public float        maxHealth   { get; set; }
    public float        curHealth   { get ; set ; }
    public Rigidbody2D  rb          { get; set; }

    #region 
    
    public bool isWithoutRemoveDistance { get; set; }
    public bool isWithinStrikingDistance { get; set; }

    public AIPath aIPath;

    #endregion



    #region  State Machine Variables

    public EnemyStateMachine    stateMachine    { get; set; }
    public EnemyIdleState       idleState       { get; set; }
    public EnemyAtkState        atkState        { get; set; }

    #endregion



    public Transform target;
    public float stopFollow;
    public float rebeginFollow;


    #region Atk State Informations
    
    public Gun<Enemy> gun;
    public Transform shootTrans;
    public LayerMask layerMask;

    #endregion

    void Awake()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        atkState = new EnemyAtkState(this, stateMachine);

        target = GameObject.FindGameObjectWithTag("Player").transform;

        GameManager.instance.AddMonster(gameObject);

        

        // if(!rb)
        //     rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        maxHealth = 15;
        curHealth = maxHealth;
        stateMachine.Init(idleState);
    }

    void Update()
    {
        stateMachine.curEnemyState.FrameUpdate();
    }

    void FixedUpdate()
    {
        stateMachine.curEnemyState.PhysicsUpdate();
    }

    #region IDamageable Functions

    public void Damage(float damageAmount)
    {
        curHealth -= damageAmount;

        if(curHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        GameManager.instance.RemoveMonster(gameObject);
        gameObject.SetActive(false);

        
    }
    
    #endregion

    public IEnumerator Shoot()
    {
        while (true)
        {            
            gun.Shoot(this);
            yield return new WaitForSeconds(gun.data.reshootCoolDown);
            
        }
    }

    
    #region Movement Functions

    public virtual void MoveEnemy(Vector2 velocity)
    {
        return;

        // rb.velocity = velocity.normalized * moveSpeed;
        
    }

    public void LookDir(Vector2 velocity)
    {
        if(velocity.x == 0 && velocity.y == 0) return;

        float angleInRadians = Mathf.Atan2(velocity.y, velocity.x);
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleInDegrees);
    }

    #endregion


    #region ITriggerCheckable Functions
    
    public void SetRemoveDistanceBool(bool isWithoutRemoveDistance)
    {
        this.isWithoutRemoveDistance = isWithoutRemoveDistance;
    }

    public void SetStrikingDistanceBool(bool isWithinStrikingDistance)
    {
        this.isWithinStrikingDistance = isWithinStrikingDistance;
    }

    #endregion
}
