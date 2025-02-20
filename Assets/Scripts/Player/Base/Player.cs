using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PlayerState;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using JetBrains.Annotations;


public enum PlayerStates { Idle, Combat, Dead }

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float maxSpeed { get; set; } = 6f;
    public float minSpeed { get; set; } = 4f;
    public float curSpeed { get; set; } = 10f;
    public Tween speedTween;
    public Tween colorTween;
    public SpriteRenderer weaponHead;
    public Color combatHeadColor;
    public Color idleHeadColor;

    [SerializeField] Rigidbody2D rigid;

    public PlayerInputController inputController;


    #region Player States
    public StateMachine<Player> stateMachine { get; set; }
    private State<Player>[] states;
    public PlayerStates curState { get; set; }

    #endregion

    #region Gun Informations

    public Gun<Player> gun;
    [SerializeField] GunData[] gunDatas;
    public Transform shootTrans;


    #endregion

    public float maxHealth { get; set; }
    public float curHealth { get; set; }
    
    [SerializeField] int curWeaponNum;

    [SerializeField] Slider hpBar;

    void Awake()
    {
        if(!rigid)
            rigid = GetComponent<Rigidbody2D>();

        
        #region State Machine Setting
        
        states = new State<Player>[3];
        states[(int)PlayerStates.Idle] = new PlayerState.Idle();
        states[(int)PlayerStates.Combat] = new PlayerState.Combat();
        states[(int)PlayerStates.Dead] = new PlayerState.Dead();

        stateMachine = new StateMachine<Player>();
		stateMachine.Setup(this, states[(int)PlayerStates.Idle]);

        ChangeState(PlayerStates.Idle);

        #endregion


        maxHealth = 10;
        curHealth = maxHealth;
        hpBar.maxValue = maxHealth;
    }

    void Update()
    {
        SetMouseVec();
        SetHpBar();
        ChangeWeaponType();
        stateMachine.FrameUpdate();
    }

    private void ChangeWeaponType()
    {
        if(!inputController.isWeaponDownPressd && !inputController.isWeaponUpPressd)
            return;

        if(inputController.isWeaponDownPressd)
            curWeaponNum --;

        if(inputController.isWeaponUpPressd)
            curWeaponNum ++;

        if(curWeaponNum < 0)
            curWeaponNum = gunDatas.Length - 1;
        else if(curWeaponNum >= gunDatas.Length)
            curWeaponNum = 0;

        gun.data = gunDatas[curWeaponNum];
    }

    private void SetHpBar()
    {
        hpBar.value = curHealth;
    }

    void PhysicsUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public void ChangeState(PlayerStates newState)
	{
		curState = newState;

		stateMachine.ChangeState(states[(int)newState]);
		// stateMachine.ChangeState(newState);
	}

    public virtual void Move(Vector2 velocity)
    {
        rigid.velocity = velocity * curSpeed;

        if(velocity.x == 0 && velocity.y == 0) return;
        
        float angleInRadians = Mathf.Atan2(velocity.y, velocity.x);
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, 0, angleInDegrees);
    }

    void SetMouseVec()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 pos = transform.position;

        float x = mousePos.x - pos.x;
        float y = mousePos.y - pos.y;

        Vector2 lookPos = new Vector2(x, y).normalized;
        float lookAngle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, lookAngle);
    }

    public void GetDamage(float damage)
    {
        curHealth -= damage;

        if(curHealth <= 0)
            Die();
    }

    

    public IEnumerator Shoot()
    {
        while (true)
        {            
            gun.Shoot(this);
            // yield return new WaitForSeconds(gun.data.reshootCoolDown);
            yield return Time.deltaTime;
        }
    }

    void Die()
    {
        SceneManager.LoadScene("Main");
    }
}
