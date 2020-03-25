using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    [CreateAssetMenu(fileName = "NavMeshGoTo", menuName = "NeoFPS/AI/NavMesh Go To")]
    public class NavMeshGoTo : AbstractNavMeshBehaviour
    {
        [SerializeField, Tooltip("The maximum distance the player is allowed to be from the destination before this behaviour will fire.")]
        float m_TriggerDistance = 10f;
        [SerializeField, Tooltip("The location that the agent should go to.")]
        Transform m_TargetTransform;

        internal override void Tick()
        {
            if (m_TargetTransform == null && m_Agent.destination != m_TargetTransform.position)
            {
                return;
            }

            if (Vector3.Distance(m_Owner.transform.position, m_TargetTransform.position) >= m_TriggerDistance)
            {
                m_Agent.SetDestination(m_TargetTransform.position);
            }
        }
    }
}
