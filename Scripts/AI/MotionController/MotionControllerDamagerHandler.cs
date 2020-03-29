using com.ootii.Actors.AnimationControllers;
using com.ootii.Actors.Combat;
using com.ootii.Messages;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace NeoFPS.AI.ootii
{
    /// <summary>
    /// A damage handler for use on Ootii Motion Controller managed characters (NPCs).
    /// This will handle damage for Neo FPS and send messages to the Motion Controller as well.
    /// </summary>
    public class MotionControllerDamagerHandler : BasicDamageHandler
    {
        MotionController m_MotionController;

        protected override void Awake()
        {
            base.Awake();
            m_MotionController = GetComponentInParent<MotionController>();
            Debug.Assert(m_MotionController != null, gameObject + " has a MotionControllerDamageHandler but no motionController component.");
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
    }
}