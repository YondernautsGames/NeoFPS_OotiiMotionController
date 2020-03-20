using com.ootii.Actors.AnimationControllers;
using com.ootii.Actors.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NeoFPS.AI.OotiiMotionController
{
    /// <summary>
    /// Checks to see if the Neo FPS character is alive. If it is do nothing otherwise play the
    /// death animation and destroy the object.
    /// </summary>
    [CreateAssetMenu(fileName = "IsAlive", menuName = "NeoFPS/AI/IsAlive")]
    public class MotionControllerIsAlive : AIBehaviour
    {
        [SerializeField, Tooltip("The time to delay before destroying the object.")]
        float destroyDelay = 5;

        private IHealthManager healthManager;
        private MotionController motionController;

        internal override bool Init(GameObject owner)
        {
            IsActive = base.Init(owner);
            healthManager = owner.GetComponent<IHealthManager>();
            IsActive &= healthManager != null;
            Debug.Assert(IsActive, owner + " has an IsAlive behaviour but no IHealthManager component.");

            motionController = Owner.GetComponent<MotionController>();
            IsActive &= motionController != null;
            Debug.Assert(IsActive, owner + " has an IsAlive behaviour but no motionController component.");

            return IsActive;
        }

        internal override void Tick()
        {
            if (healthManager.isAlive)
            {
                return;
            }

            NavMeshAgent agent = Owner.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.isStopped = true;
            }

            CombatMessage message = CombatMessage.Allocate();
            if (message != null)
            {
                message.ID = 1108; // DEATH
                message.StyleIndex = -1; // Random
                message.Defender = Owner;

                motionController.SendMessage(message);
            }

            Destroy(Owner, destroyDelay);

            StopAllBehaviourControllers();
        }

        /// <summary>
        /// Disable all Behaviour Controllers so that no more behaviours will be
        /// triggered.
        /// </summary>
        private void StopAllBehaviourControllers()
        {
            BasicAIController[] controllers = Owner.GetComponents<BasicAIController>();
            for (int i = 0; i < controllers.Length; i++)
            {
                controllers[i].IsActive = false;
            }
        }
    }
}
