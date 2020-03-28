using com.ootii.Actors.AnimationControllers;
using com.ootii.Actors.Combat;
using com.ootii.Actors.LifeCores;
using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NeoFPS
{
    /// <summary>
    /// A damage handler for use on Ootii Motion Controller managed characters (NPCs).
    /// This will handle damage for Neo FPS and send messages to the Motion Controller as well.
    /// </summary>
    public class MotionControllerDamagerHandler : BasicDamageHandler
    {
        MotionController m_MotionController;
        private MotionControllerMotion m_DeathMotion;
        private NavMeshAgent m_NavMeshAgent;

        protected override void Awake()
        {
            base.Awake();
            m_MotionController = GetComponentInParent<MotionController>();
            Debug.Assert(m_MotionController != null, gameObject + " has a MotionControllerDamageHandler but no motionController component.");

            m_NavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            m_HealthManager.onIsAliveChanged += OnIsAliveChanged;
        }

        private void OnDisable()
        {
            m_HealthManager.onIsAliveChanged -= OnIsAliveChanged;
        }

        public override DamageResult AddDamage(float damage)
        {
            DamageResult result = base.AddDamage(damage);
            StartCoroutine(HandleDamageMotion(damage));
            return result;
        }

        public override DamageResult AddDamage(float damage, IDamageSource source)
        {
            DamageResult result = base.AddDamage(damage, source);
            StartCoroutine(HandleDamageMotion(damage));
            return result;
        }

        private IEnumerator HandleDamageMotion(float damage)
        {
            CombatMessage message = CombatMessage.Allocate();
            if (message != null)
            {
                message.ID = EnumMessageID.MSG_COMBAT_DEFENDER_DAMAGED;
                message.Damage = damage * m_Multiplier;
                message.Defender = gameObject;
                m_MotionController.SendMessage(message);
                message.Release();
            }

            yield return null;
        }

        private void OnIsAliveChanged(bool alive)
        {
            if (alive) return;

            CombatMessage message = CombatMessage.Allocate();
            if (message != null)
            {
                message.ID = EnumMessageID.MSG_COMBAT_DEFENDER_KILLED;
                message.StyleIndex = -1; // Random
                message.Defender = gameObject;
                m_MotionController.SendMessage(message);
                message.Release();
            }

            if (m_NavMeshAgent != null)
            {
                m_NavMeshAgent.isStopped = true;
            }

            if (message.IsHandled)
            {
                m_DeathMotion = message.Recipient as MotionControllerMotion;
            }
        }

        private void Update()
        {
            if (m_DeathMotion != null && !m_DeathMotion.IsActive) // death motion has completed
            {
                m_MotionController.enabled = false;
                m_MotionController.ActorController.enabled = false;
                if (m_NavMeshAgent != null)
                {
                    m_NavMeshAgent.enabled = false;
                }
            }
        }
    }
}