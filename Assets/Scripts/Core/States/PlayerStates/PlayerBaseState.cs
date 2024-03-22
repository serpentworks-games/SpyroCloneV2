using ScalePact.Core.StateMachines;
using UnityEngine;

namespace ScalePact.Core.States
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine stateMachine;

        protected const float kAnimatorDampTime = 0.1f;

        protected float previousFrameTime;
        protected float remainingDodgeTime;
        protected Vector2 dodgeInput;

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {

        }

        public override void Tick(float deltaTime)
        {
        }

        public override void PhysicsTick(float deltaTime)
        {

        }

        public override void Exit()
        {

        }

        public override void UpdateAnimator(float deltaTime)
        {

        }

        protected void ReturnToMovementState()
        {
            if (stateMachine.TargetScanner.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerMoveState(stateMachine));
            }
        }

        protected void MovementWithForces(Vector3 motion, float desiredMoveSpeed, float deltaTime)
        {
            Vector3 velocity = motion * desiredMoveSpeed;
            stateMachine.Rigidbody.velocity = velocity + stateMachine.ForceReceiver.Movement;

            Vector3 floorMovement = stateMachine.ForceReceiver.GetFloorMovement();
            if (floorMovement != stateMachine.Rigidbody.position && stateMachine.Rigidbody.velocity.y <= 0)
            {
                stateMachine.Rigidbody.MovePosition(floorMovement);
            }
        }

        protected Vector3 CalculateFreeLookMovementVector()
        {
            Vector3 movement = new()
            {
                x = stateMachine.InputManager.MovementVector.x,
                y = 0,
                z = stateMachine.InputManager.MovementVector.y
            };

            Vector3 adjustedMoveX = movement.x * stateMachine.MainCameraTransform.right;
            Vector3 adjustedMoveZ = movement.z * stateMachine.MainCameraTransform.forward;

            adjustedMoveX.Normalize();
            adjustedMoveZ.Normalize();

            Vector3 combined = adjustedMoveX + adjustedMoveZ;

            Vector3 moveDir = new Vector3(combined.x, 0, combined.z);
            return moveDir;
        }

        protected Vector3 CalculateTargettedMovement(float deltaTime)
        {
            Vector3 movement = new();
            if (remainingDodgeTime > 0f)
            {
                float distOverTime = stateMachine.MaxDodgeDistance / stateMachine.MaxDodgeDuration;
                movement += stateMachine.transform.right * dodgeInput.x * distOverTime;
                movement += stateMachine.transform.forward * dodgeInput.y * distOverTime;

                remainingDodgeTime = Mathf.Max(remainingDodgeTime - deltaTime, 0f);
            }
            else
            {
                movement += stateMachine.transform.right * stateMachine.InputManager.MovementVector.x;
                movement += stateMachine.transform.forward * stateMachine.InputManager.MovementVector.y;
            }
            return movement;
        }

        protected void ApplyFreeLookRotation(Vector3 motion, float deltaTime)
        {
            Vector3 normalDir = motion;

            if (motion == Vector3.zero)
            {
                normalDir = stateMachine.transform.forward;
            }
            normalDir.y = 0;

            Quaternion rot = Quaternion.LookRotation(normalDir);
            Quaternion targetRot = Quaternion.Slerp(stateMachine.transform.rotation, rot, deltaTime * stateMachine.BaseRotationSpeed);
            stateMachine.transform.rotation = targetRot;
        }

        protected void FaceTarget()
        {
            if (stateMachine.TargetScanner.CurrentTarget == null) return;
            Vector3 dirToTarget = stateMachine.TargetScanner.CurrentTarget.transform.position - stateMachine.transform.position;
            dirToTarget.y = 0;

            stateMachine.transform.rotation = Quaternion.LookRotation(dirToTarget);
        }
    }
}