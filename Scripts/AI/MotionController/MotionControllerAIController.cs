using com.ootii.Actors.AnimationControllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI.ootii
{
    public class MotionControllerAIController : BasicAIController
    {
        private MotionController m_MotionController;

        protected override void Awake()
        {
            base.Awake();
            m_MotionController = GetComponent<MotionController>();
            Debug.Assert(m_MotionController != null, gameObject + " has a MotionControllerAIController but no motionController component.");
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
        }
    }
}
