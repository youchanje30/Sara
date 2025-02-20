using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace PlayerState
{
    public class Idle : State<Player>
    {
        public override void Enter(Player entity)
        {
            DBG.DebugerNN.Debug("Enter of Idle");
            if ( entity.curSpeed != entity.maxSpeed )
            {
                float changeTime = 1f * (1 - (entity.curSpeed - entity.minSpeed) / (entity.maxSpeed - entity.minSpeed));
                
                if(entity.speedTween != null)
                    entity.speedTween.Kill();
                if(entity.colorTween != null)
                    entity.colorTween.Kill();

                entity.speedTween = DOTween.To(() => entity.curSpeed,
                x => entity.curSpeed = x, entity.maxSpeed, changeTime)
                .OnComplete(() => DBG.DebugerNN.Debug("Normal Speed"));
                entity.colorTween = entity.weaponHead.DOColor(entity.idleHeadColor, changeTime);
            }
        }

        public override void Exit(Player entity)
        {
            DBG.DebugerNN.Debug("Exit of Idle");
        }

        public override void FrameUpdate(Player entity)
        {
            if (Input.GetMouseButtonDown(0))
                entity.ChangeState(PlayerStates.Combat);

            // DBG.DebugerNN.Debug("FrameUpdate of Idle");

            Vector2 moveVec = Vector2.zero;
            if(entity.inputController.isRightArrowPressed)
                moveVec.x ++;
            if(entity.inputController.isLeftArrowPressed)
                moveVec.x --;
            if(entity.inputController.isUpArrowPressed)
                moveVec.y ++;
            if(entity.inputController.isDownArrowPressed)
                moveVec.y --;

            entity.Move(moveVec.normalized);
            
        }

        public override void PhysicsUpdate(Player entity)
        {

        }


        
    }

    public class Combat : State<Player>
    {
        public override void Enter(Player entity)
        {
            DBG.DebugerNN.Debug("Enter of Combat");

            if ( entity.curSpeed != entity.minSpeed )
            {
                float changeTime = 0.5f * (entity.curSpeed - entity.minSpeed) / (entity.maxSpeed - entity.minSpeed);

                if(entity.speedTween != null)
                    entity.speedTween.Kill();
                if(entity.colorTween != null)
                    entity.colorTween.Kill();
                
                entity.speedTween = DOTween.To(() => entity.curSpeed,
                x => entity.curSpeed = x, entity.minSpeed, changeTime);

                entity.colorTween = entity.weaponHead.DOColor(entity.combatHeadColor, changeTime)
                .OnComplete(() => entity.StartCoroutine(nameof(entity.Shoot)));
            }
        }

        public override void Exit(Player entity)
        {
            DBG.DebugerNN.Debug("Exit of Combat");
            entity.StopCoroutine(nameof(entity.Shoot));
        }

        public override void FrameUpdate(Player entity)
        {
            if (Input.GetMouseButtonUp(0))
                entity.ChangeState(PlayerStates.Idle);
                
            // DBG.DebugerNN.Debug("FrameUpdate of Combat");

            Vector2 moveVec = Vector2.zero;
            if(entity.inputController.isRightArrowPressed)
                moveVec.x ++;
            if(entity.inputController.isLeftArrowPressed)
                moveVec.x --;
            if(entity.inputController.isUpArrowPressed)
                moveVec.y ++;
            if(entity.inputController.isDownArrowPressed)
                moveVec.y --;

            entity.Move(moveVec.normalized);
        }

        public override void PhysicsUpdate(Player entity)
        {

        }
    }



    public class Dead : State<Player>
    {
        public override void Enter(Player entity)
        {

        }

        public override void Exit(Player entity)
        {

        }

        public override void FrameUpdate(Player entity)
        {

        }

        public override void PhysicsUpdate(Player entity)
        {

        }
    }

}