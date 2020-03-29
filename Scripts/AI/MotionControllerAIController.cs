using com.ootii.Actors.AnimationControllers;
using com.ootii.Actors.Combat;
using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NeoFPS.AI.ootii
{
    public class MotionControllerAIController : BasicAIController
    {
        private MotionController m_MotionController;
        private MotionControllerMotion m_DeathMotion;
        private NavMeshAgent m_NavMeshAgent;

        protected override void Awake()
        {
            base.Awake();
            m_MotionController = GetComponent<MotionController>();
            Debug.Assert(m_MotionController != null, gameObject + " has a MotionControllerAIController but no motionController component.");

            m_NavMeshAgent = gameObject.GetComponentInParent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            m_HealthManager.onIsAliveChanged += OnIsAliveChanged;
        }

        private void OnDisable()
        {
            m_HealthManager.onIsAliveChanged -= OnIsAliveChanged;
        }

        private void OnIsAliveChanged(bool alive)
        {
            m_IsActive &= alive;

            if (alive) return;

            CombatMessage message = CombatMessage.Allocate();
            if (message != null)
            {
                message.ID = EnumMessageID.MSG_COMBAT_DEFENDER_KILLED;
                message.StyleIndex = -1; // Random
                message.Defender = m_MotionController.gameObject;
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

        protected override void Update()
        {
            base.Update();

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
