using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI.Behaviour
{
    /// <summary>
    /// Finds a game object that matches a set of criteria and stores the 
    /// result in a variable called Target in the controllers blackboard.
    /// </summary>
    [CreateAssetMenu(fileName = "SetTarget", menuName = "NeoFPS/AI/Blackboard/Set Target")]
    public class SetTargetVariable : AIBehaviour
    {
        [SerializeField, Tooltip("Time To Live, in seconds, for the result cache. In order to minimize " +
            "performance hits this behaviour will be executred no more frequently than this time. Note, " +
            "however, that the controller may have an tick frequency that is slower than this TTL value, " +
            "in this situation the controllers tick freuqncy will take precedence.")]
        private float m_CacheTTL = 0.025f;
        [SerializeField, Tooltip("The tag to use to identify the target object.")]
        private string m_Tag = "Player";

        private float m_CacheInvalidationTime;

        internal override string Tick()
        {
            if (Time.realtimeSinceStartup > m_CacheInvalidationTime)
            {
                GameObject target = GameObject.FindGameObjectWithTag(m_Tag);
                if (target == null)
                {
                    return "Unable to find a GameObject that matches the specified criteria.";
                }
                else
                {
                    SetVariable("Target", target);
                }

                m_CacheInvalidationTime = Time.realtimeSinceStartup + m_CacheTTL;
                return "";
            } else
            {
                return "Cache has not been invalidated yet.";
            }
        }
    }
}
