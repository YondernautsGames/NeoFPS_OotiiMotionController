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
        public float m_MinDistance = 5;
        [Tooltip("The maximum distance away to set the target position for each wander. Note the actual position may be a little more than this if there is an obstacle at this distance.")]
        public float m_MaxDistance = 15;
        [Tooltip("The minimum angle away from forward to set the target position for each wander. Note the actual angle may be a little less than this if there is an obstacle at this angle.")]
        public float m_MinAngle = -90;
        [Tooltip("The maximum angle away from forward to set the target position for each wander. Note the actual angle may be a little more than this if there is an obstacle at this angle.")]
        public float m_MaxAngle = 90;
        [Tooltip("The maxium number of attempts to find a valid position to navigate to within the parameters set. Set high to increase the chances of finding a valid position in a given frame, or low to minimize performance impact per frame.")]
        public int m_MaxRetries = 10;
        
        internal override void Tick()
        {
            if (!m_Agent.IsMovingToTarget(0.5f))
            {
                m_Agent.TrySetRandomTarget(m_MinDistance, m_MaxDistance, m_MinAngle, m_MaxAngle, m_MaxRetries);
            }
        }


    }
}
