using UnityEngine;
using UnityEngine.AI;

namespace NeoFPS.AI
{
    /// <summary>
    /// Extension methods for working with the NavMesh based NPCs in NeoFPS.
    /// </summary>
    public static class NavMeshExtension 
    {

        /// <summary>
        /// Test if the agent is currently moving to a target.
        /// </summary>
        /// <param name="reachedDistance">If the agent has a target but is already within this distance then return false</param>
        /// <returns>Returns true if the agent is currently moving to or calcualting a pat to a target.</returns>
        public static bool IsMovingToTarget(this NavMeshAgent agent, float reachedDistance)
        {
            return agent.remainingDistance > reachedDistance || agent.hasPath || agent.pathPending;
        }

        /// <summary>
        /// Attempt to set a random target within a distance and angle range (from forward).
        /// </summary>
        /// <param name="minDistance">The minimum distance away the target should be</param>
        /// <param name="maxDistance">The maximum distance away the target should be.</param>
        /// <param name="minAngle">The minimum angle from forward the target should be.</param>
        /// <param name="maxAngle">The maximum angle from forward the target should be.</param>
        /// <param name="maxRetries">The number of attempts to find a valid target within the given parameters before returning false</param>
        /// <returns>True if a valid target point was found.</returns>
        public static bool TrySetRandomTarget(this NavMeshAgent agent, float minDistance, float maxDistance, float minAngle, float maxAngle, int maxRetries)
        {
            bool isValid = false;
            int retries = 0;

            while (!isValid && retries < maxRetries)
            {
                float distance = Random.Range(minDistance, maxDistance);
                float rot = Random.Range(minAngle, maxAngle);

                Vector3 randomPoint = agent.transform.position + Quaternion.AngleAxis(rot, Vector3.up) * agent.transform.forward * distance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    isValid = true;
                }
                retries++;
            }
            return isValid;
        }
    }
}
