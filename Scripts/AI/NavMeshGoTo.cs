using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    [CreateAssetMenu(fileName = "NavMeshGoTo", menuName = "NeoFPS/AI/NavMesh Go To")]
    public class NavMeshGoTo : AbstractNavMeshBehaviour
    {
        [SerializeField, Tooltip("The maximum distance the player is allowed to be from the destination before this behaviour will fire.")]
        float triggerDistance = 10f;
        [SerializeField, Tooltip("The location that the agent should go to.")]
        Transform targetTransform;

        internal override void Tick()
        {
            if (targetTransform == null && agent.destination != targetTransform.position)
            {
                return;
            }

            if (Vector3.Distance(Owner.transform.position, targetTransform.position) >= triggerDistance)
            {
                agent.SetDestination(targetTransform.position);
            }
        }
    }
}
