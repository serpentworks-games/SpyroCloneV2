using UnityEngine;
using ScalePact.Core.StateMachines;
using UnityEngine.EventSystems;
using UnityEditor.Callbacks;

namespace ScalePact.Core.States
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            //stateMachine.InputManager.JumpEvent += SwitchStateToJump;
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.InputManager.MovementVector == Vector2.zero)
            {
                stateMachine.Animator.SetFloat(HashIDs.BaseVelocityHash, 0, kAnimatorDampTime, deltaTime);
                return;
            }
            stateMachine.Animator.SetFloat(HashIDs.BaseVelocityHash, 1, kAnimatorDampTime, deltaTime);

            base.Tick(deltaTime);
        }

        public override void PhysicsTick(float deltaTime)
        {
            Vector3 moveDir = CalculateMoveVector();

            Vector3 velocity = moveDir * stateMachine.BaseMoveSpeed;

            ApplyGravity(velocity);

            ApplyRotation(deltaTime, moveDir);

            base.PhysicsTick(deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
            //stateMachine.InputManager.JumpEvent -= SwitchStateToJump;
        }

        void SwitchStateToJump()
        {
            stateMachine.SwitchState(new PlayerJumpState(stateMachine));
        }

        private Vector3 CalculateMoveVector()
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

        private void ApplyRotation(float deltaTime, Vector3 moveDir)
        {
            Vector3 normalDir = moveDir;

            if (moveDir == Vector3.zero)
            {
                normalDir = stateMachine.transform.forward;
            }
            normalDir.y = 0;

            Quaternion rot = Quaternion.LookRotation(normalDir);
            Quaternion targetRot = Quaternion.Slerp(stateMachine.transform.rotation, rot, deltaTime * stateMachine.BaseRotationSpeed);
            stateMachine.transform.rotation = targetRot;
        }

        private void ApplyGravity(Vector3 velocity)
        {
            Vector3 gravity = new();
            gravity += Physics.gravity.y * Time.deltaTime * Vector3.up;

            stateMachine.Rigidbody.velocity = velocity + gravity;

            Vector3 floorMovement = new Vector3(stateMachine.Rigidbody.position.x, stateMachine.FindFloor().y + stateMachine.FloorOffsetY, stateMachine.Rigidbody.position.z);
            if (floorMovement != stateMachine.Rigidbody.position && stateMachine.Rigidbody.velocity.y <= 0)
            {
                stateMachine.Rigidbody.MovePosition(floorMovement);
                gravity.y = 0;
            }
        }

    }
}