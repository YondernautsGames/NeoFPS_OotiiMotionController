using UnityEngine;
using UnityEngine.AI;

namespace NeoFPS.AI
{
    public abstract class AbstractNavMeshBehaviour : AIBehaviour
    {
        protected NavMeshAgent m_Agent;

        internal override bool Init(GameObject owner)
        {
            m_IsActive = base.Init(owner);

            m_Agent = owner.GetComponent<NavMeshAgent>();
            m_IsActive = m_Agent != null;
            Debug.Assert(m_IsActive, owner + " has a NavMeshWander behaviour but no NaveMeshAgent component.");

            return m_IsActive;
        }
    }
}