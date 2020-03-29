using NeoFPS.AI.Behaviour;
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

        float m_NextAttackTime = 0;

        internal override string Tick()
        {
            if (Time.time < m_NextAttackTime)
            {
                return "Melee attack is on cooldown.";
            }

            Debug.Log("Melee Attack");
            m_NextAttackTime = Time.time + m_Cooldown;

            return "";
        }
    }
}
