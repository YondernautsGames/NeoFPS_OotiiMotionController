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
        [SerializeField, Tooltip("The minimum time between attacks.")]
        float m_Cooldown = 1f;

        private MotionController m_MotionController;
        float m_NextAttackTime = 0;

        internal override bool Init(GameObject owner)
        {
            bool isSuccess = false;
            m_MotionController = owner.GetComponent<MotionController>();
            isSuccess = m_MotionController != null;
            Debug.Assert(isSuccess, owner + " has a MotionControllerMeleeAttack AI behaviour but no MotionController.");
            isSuccess &= base.Init(owner);

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

            HandleMeleeAttackMotion();
            m_NextAttackTime = Time.time + m_Cooldown;

            return "";
        }

        private void HandleMeleeAttackMotion()
        {
            // FIXME: Can't be finding objects with tag here, need to use a shared variable
            GameObject target = GameObject.FindGameObjectWithTag("Player");

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
