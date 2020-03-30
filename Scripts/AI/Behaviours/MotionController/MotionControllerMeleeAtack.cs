using com.ootii.Actors.AnimationControllers;
using com.ootii.Actors.Combat;
using com.ootii.Messages;
using NeoFPS.AI.Behaviour;
using System.Collections;
using UnityEngine;

namespace NeoFPS.AI.OotiiMotionController
{
    /// <summary>
    /// Attack a target with an equipped melee weapon. If no weapon is eqiupped then the
    /// attack will fail.
    /// </summary>
    [CreateAssetMenu(fileName = "MotionControllerMeleeAttack", menuName = "NeoFPS/AI/Motion Controller/Behaviour/Melee Attack")]
    public class MotionControllerMeleeAtack : AIBehaviour
    {
        [SerializeField, Tooltip("The name of the variable containing the game object to seek.")]
        string m_Target = "Target";
        [SerializeField, Tooltip("The minimum time between attacks.")]
        float m_Cooldown = 1f;

        private MotionController m_MotionController;
        float m_NextAttackTime = 0;

        internal override bool Init(GameObject owner, BasicAIController controller)
        {
            bool isSuccess = false;
            m_MotionController = owner.GetComponent<MotionController>();
            isSuccess = m_MotionController != null;
            Debug.Assert(isSuccess, owner + " has a MotionControllerMeleeAttack AI behaviour but no MotionController.");
            isSuccess &= base.Init(owner, controller);

            if (isSuccess)
            {
                IsActive = true;
                return true;
            }
            else
            {
                IsActive = false;
                return true;
            }
        }

        internal override string Tick()
        {
            if (Time.time < m_NextAttackTime)
            {
                return "Melee attack is on cooldown.";
            }

            GameObject target = GetVariable(m_Target);
            if (target == null)
            {
                return "No target specified.";
            }

            HandleMeleeAttackMotion(target);
            m_NextAttackTime = Time.time + m_Cooldown;

            return "";
        }

        private void HandleMeleeAttackMotion(GameObject target)
        {
            CombatMessage message = CombatMessage.Allocate();
            if (message != null)
            {
                message.ID = CombatMessage.MSG_COMBATANT_ATTACK;
                message.Attacker = m_Owner;
                message.Defender = target;
                m_MotionController.SendMessage(message);
                CombatMessage.Release(message);
            }

        }
    }
}
