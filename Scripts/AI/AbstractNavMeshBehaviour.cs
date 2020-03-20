using UnityEngine;
using UnityEngine.AI;

namespace NeoFPS.AI
{
    public abstract class AbstractNavMeshBehaviour : AIBehaviour
    {
        protected NavMeshAgent agent;

        internal override bool Init(GameObject owner)
        {
            IsActive = base.Init(owner);

            agent = owner.GetComponent<NavMeshAgent>();
            IsActive = agent != null;
            Debug.Assert(IsActive, owner + " has a NavMeshWander behaviour but no NaveMeshAgent component.");

            return IsActive;
        }
    }
}