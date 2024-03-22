using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace ScalePact.Forces
{
    public class EnemyForceReceiver : ForceReceiver
    {
        CharacterController controller;
        NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (verticalVelocity < 0f && IsGrounded())
            {
                verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, Drag);

            if (agent != null)
            {
                if (impact.sqrMagnitude < 0.2f * 0.2f)
                {
                    impact = Vector3.zero;
                    agent.enabled = true;
                }
            }

            
        }

        public override bool IsGrounded()
        {
            if (controller.isGrounded) return true;
            else return false;
        }

        public override void AddForce(Vector3 forceToAdd)
        {
            impact += forceToAdd;

            if (agent != null)
            {
                agent.enabled = false;
            }
        }
    }
}