using System.Collections.Generic;
using ScalePact.Editor;
using ScalePact.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace ScalePact.Combat
{
    public class Damageable : MonoBehaviour
    {
        public struct DamageMessage
        {
            public MonoBehaviour damager;
            public int damageAmount;
            public Vector3 damageFromDirection;
            public Vector3 damageSource;
            public bool throwing;

            public bool shouldStopCamera;
        }

        [SerializeField] int maxHealth = 3;
        [SerializeField] bool isImmortal = false;
        [SerializeField] float maxInvulnerabilityTime = 0.5f;
        [Range(0, 360)][SerializeField] float hitAngle = 360.0f;
        [Range(0, 360)][SerializeField] float hitForwardRotation = 360.0f;

        [SerializeField] UnityEvent OnReceiveDamage, OnDeath, OnHitWhileInvulnerable, OnBecomeVulnerable, OnResetDamage;

        [EnforceType(typeof(IMessageReceiver))][SerializeField] List<MonoBehaviour> OnDamageMessageReceivers;

        public bool IsDead { get => isDead; } 

        bool isInvunerable;
        bool isDead;
        int currentHealth;

        float timeSinceLastHit = Mathf.Infinity;

        System.Action schedule;

        private void Awake() {
            ResetDamage();
        }

        private void Update()
        {
            if (isInvunerable)
            {
                timeSinceLastHit += Time.deltaTime;
                if (timeSinceLastHit > maxInvulnerabilityTime)
                {
                    timeSinceLastHit = 0;
                    isInvunerable = false;
                    OnBecomeVulnerable?.Invoke();
                }
            }
        }

        public void ResetDamage()
        {
            currentHealth = maxHealth;
            isInvunerable = false;
            isDead = false;
            timeSinceLastHit = 0.0f;
            OnResetDamage?.Invoke();
        }

        public void ApplyDamage(DamageMessage data)
        {
            if (isDead) return;
            if (currentHealth <= 0) return;

            if (isInvunerable)
            {
                OnHitWhileInvulnerable?.Invoke();
                return;
            }

            Vector3 forward = transform.forward;
            forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

            Vector3 posToDamager = data.damageSource - transform.position;
            posToDamager -= transform.up * Vector3.Dot(transform.up, posToDamager);

            //If the damage came from outside the hit angle, ignore it
            if (Vector3.Angle(forward, posToDamager) > hitAngle * 0.5f) return;

            isInvunerable = true;

            if (!isImmortal) currentHealth -= data.damageAmount;

            //If current hp is now zero or lower, schedule the death event
            //This avoids a race condition if objects kill each other at the same time
            if (currentHealth <= 0) 
            {
                isDead = true;
                schedule += OnDeath.Invoke;
            }
            else OnReceiveDamage?.Invoke();

            //Determine if the message should be passed as DEAD or DAMAGED depending on current hp
            var messageType = currentHealth <= 0 ? MessageType.DEAD : MessageType.DAMAGED;

            //Loop through the list of receivers, sending the message and data along for each
            for (int i = 0; i < OnDamageMessageReceivers.Count; i++)
            {
                IMessageReceiver receiver = OnDamageMessageReceivers[i] as IMessageReceiver;
                receiver.OnReceiveMessage(messageType, this, data);
            }
        }

        //At the end of the frame, process the scheduled events and clear the listeners
        private void LateUpdate()
        {
            if (schedule != null)
            {
                schedule();
                schedule = null;
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Vector3 forward = transform.forward;
            forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

            if (Event.current.type == EventType.Repaint)
            {
                UnityEditor.Handles.color = Color.blue;
                UnityEditor.Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(forward), 1.0f, EventType.Repaint);
            }

            UnityEditor.Handles.color = new Color(1, 0, 0, 0.5f);
            forward = Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, forward, hitAngle, 1.0f);
        }
#endif
    }
}