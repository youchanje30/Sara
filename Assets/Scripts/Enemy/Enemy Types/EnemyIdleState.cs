using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{

    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    

    public override void EnterState() 
    {
        base.EnterState();
        enemy.aIPath.canFollow = false;
    }

    public override void ExitState()
    {
        base.ExitState();
        
        // enemy.MoveEnemy(Vector2.zero);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        // enemy.MoveEnemy(enemy.endPos - enemy.transform.position);
        enemy.LookDir(enemy.aIPath.desiredVelocity);

        if ( enemy.isWithoutRemoveDistance )
        {
            enemy.Die();
        }

        if ( enemy.isWithinStrikingDistance )
        {
            enemy.stateMachine.ChangeState(enemy.atkState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
