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
        /// <returns>Returns true if the agent is currently moving to or calculating a path to a target.</returns>
        public static bool IsMovingToTarget(this NavMeshAgent agent)
        {
            if (agent.pathPending) return true;
            if (!agent.hasPath) return false;
            // If Agent successfully reaches destination
            if (agent.path.status == NavMeshPathStatus.PathComplete && agent.remainingDistance <= agent.stoppingDistance) return false;
            // If invalid destination(off mesh or unreachable)
            if (agent.path.status == NavMeshPathStatus.PathInvalid) return false;
            // If Agent tried and failed to reach destination
            if (agent.path.status == NavMeshPathStatus.PathPartial) return false;
            
            return true;
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
                isValid = SetNearestDestination(agent, randomPoint);
                retries++;
            }
            return isValid;
        }

        /// <summary>
        /// Set the NavMeshAgent destination to a point on the NavMesh nearest to the destination point.
        /// </summary>
        /// <param name="destination">The place we want to get to.</param>
        /// <returns>True if the destination was set succesfully.</returns>
        public static bool SetNearestDestination(this NavMeshAgent agent, Vector3 destination)
        {
            if (!agent.enabled) return false;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(destination, out hit, 1.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
