using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class EnemyAtkState : EnemyState
{

    public EnemyAtkState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        
    }

    

    public override void EnterState() 
    {
        base.EnterState();
        enemy.LookDir(enemy.target.position - enemy.transform.position);
        enemy.StartCoroutine(nameof(enemy.Shoot));
        enemy.aIPath.canFollow = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.StopCoroutine(nameof(enemy.Shoot));
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();


        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, enemy.target.position - enemy.transform.position, Vector3.Distance(enemy.target.position, enemy.transform.position), enemy.layerMask);
        if(hit)
        {
            enemy.LookDir(enemy.aIPath.desiredVelocity);
            enemy.aIPath.canFollow = true;
        }
        else
        {
            enemy.LookDir(enemy.target.position - enemy.transform.position);
            if(Vector3.Distance(enemy.transform.position, enemy.target.position) <= enemy.stopFollow)
                enemy.aIPath.canFollow = false;
            else if(Vector3.Distance(enemy.transform.position, enemy.target.position) >= enemy.rebeginFollow)
                enemy.aIPath.canFollow = true;
        }


        if ( !enemy.isWithinStrikingDistance )
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
