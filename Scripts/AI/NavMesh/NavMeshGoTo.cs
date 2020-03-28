using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    /// <summary>
    /// Instruct an agent to go to a specific point using the NavMeshAgent if it does not already have a destination.
    /// </summary>
    [CreateAssetMenu(fileName = "NavMeshGoTo", menuName = "NeoFPS/AI/NavMesh Go To")]
    public class NavMeshGoTo : AbstractNavMeshBehaviour
    {
        [SerializeField, Tooltip("The minimum distance the agent should be away from this location before they return.")]
        float m_MinDistance = 10f;
        [SerializeField, Tooltip("The location that the agent should go to.")]
        Transform m_TargetTransform = null;

        internal override string Tick()
        {
            if (m_TargetTransform == null)
            {
                return "No target to move to.";
            }

            if (m_Agent.IsMovingToTarget())
            {
                return "Already moving to a target";
            }

            if (Vector3.Distance(m_Agent.transform.position, m_TargetTransform.position) >= m_MinDistance)
            {
                m_Agent.SetDestination(m_TargetTransform.position);
                return "";
            }

            return "Too close to the target point already."; 
        }
    }
}
