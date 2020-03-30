using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI.Condition
{
    /// <summary>
    /// Checks to see if a particular Game Object can be sensed by the agent.
    /// </summary>
    [CreateAssetMenu(fileName = "CanSenseObject", menuName = "NeoFPS/AI/Condition/Can Sense Object")]
    public class CanSenseObject : AICondition
    {
        [SerializeField, Tooltip("The name of the variable containing the game object we are attempting to sense.")]
        string m_VariableName = "Target";
        [SerializeField, Tooltip("Any player within this distance of the agent will be detected regardless of their position relative to the agent.")]
        float m_AutomaticSensingRange = 10;

        protected override bool Test()
        {
            GameObject target = GetVariable(m_VariableName);
            if (target == null)
            {
                return false;
            }
            ObjectFilter filter = new ObjectFilter();
            filter.gameObject = target;

            List<Collider> result = Senses.GetObjectsWithinSphere(m_Behaviour.m_Owner.transform.position, m_AutomaticSensingRange, filter);

            return result.Count > 0;
        }
    }
}
