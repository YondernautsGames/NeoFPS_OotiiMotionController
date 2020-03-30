using NeoFPS.AI.Behaviour;
using UnityEngine;
using UnityEngine.AI;

namespace NeoFPS.AI
{
    public abstract class AbstractNavMeshBehaviour : AIBehaviour
    {
        protected NavMeshAgent m_Agent = null;

        internal override bool Init(GameObject owner, BasicAIController controller)
        {
            m_Agent = owner.GetComponent<NavMeshAgent>();
            bool isSuccess = m_Agent != null;
            Debug.Assert(isSuccess, owner + " has a NavMeshWander behaviour but no NaveMeshAgent component.");
            isSuccess &= base.Init(owner, controller);

            if (isSuccess)
            {
                IsActive = true;
                return true;
            } else
            {
                IsActive = false;
                return true;
            }
        }
    }
}