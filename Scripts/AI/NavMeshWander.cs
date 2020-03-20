using NeoFPS.AI;
using UnityEngine;
using UnityEngine.AI;

namespace NeoFPS.AI
{
    /// <summary>
    /// A very simple controller for an Ootii Motion Controller NPC.
    /// The NPC will simply wander randomly using the navmesh.
    /// </summary>
    [CreateAssetMenu(fileName = "NavMeshWander", menuName = "NeoFPS/AI/NavMesh Wander")]
    public class NavMeshWander : AbstractNavMeshBehaviour
    {
        [Header("Wander Parameters")]
        [Tooltip("The minimum distance away to set the target position for each wander. Note the actual position may be a little less than this if there is an obstacle at this distance.")]
        public float minDistance = 5;
        [Tooltip("The maximum distance away to set the target position for each wander. Note the actual position may be a little more than this if there is an obstacle at this distance.")]
        public float maxDistance = 15;
        [Tooltip("The minimum angle away from forward to set the target position for each wander. Note the actual angle may be a little less than this if there is an obstacle at this angle.")]
        public float minAngle = -90;
        [Tooltip("The maximum angle away from forward to set the target position for each wander. Note the actual angle may be a little more than this if there is an obstacle at this angle.")]
        public float maxAngle = 90;
        [Tooltip("The maxium number of attempts to find a valid position to navigate to within the parameters set. Set high to increase the chances of finding a valid position in a given frame, or low to minimize performance impact per frame.")]
        public int maxRetries = 10;
        
        internal override void Tick()
        {
            if (!agent.IsMovingToTarget(0.5f))
            {
                agent.TrySetRandomTarget(minDistance, maxDistance, minAngle, maxAngle, maxRetries);
            }
        }


    }
}
