using NeoFPS.AI.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI.Condition
{
    /// <summary>
    /// A condition that can be used to control whether an AI Behaviour executes or not.
    /// </summary>
    public abstract class AICondition : AIScriptableObject
    {
        [SerializeField, Tooltip("Negate the condition.")]
        private bool negate = false;
        [SerializeField, Tooltip("Time To Live, in seconds, for the result cache. In order to minimize performance hits " +
            "the condition will be tested no more frequently than this time. Note, " +
            "however, that the controller may have an tick frequency that is slower than this TTL value, " +
            "in this situation the controllers tick freuqncy will take precedence.")]
        private float m_CacheTTL = 0.025f;

        private bool m_CachedResult;
        internal AIBehaviour m_Behaviour;

        private float m_CacheInvalidationTime = float.NegativeInfinity;

        /// <summary>
        /// Called during the AIBehaviour Init method to initialize any components needed.
        /// <param name="behaviour">The behaviour this condition instance belongs to.</param>
        /// <return>True if the condition has been correctly initialized.</return>
        /// </summary>
        internal virtual bool Init(AIBehaviour behaviour)
        {
            m_Behaviour = behaviour;
            return true;
        }

        /// <summary>
        /// Get the result of testing this condition. The result will be cached
        /// for the time specified in `cacheTTL`.
        /// </summary>
        /// <returns>True if the condition is satisfied, otherwise false.</returns>
        public bool GetResult()
        {
            if (Time.realtimeSinceStartup > m_CacheInvalidationTime)
            {
                if (negate)
                {
                    m_CachedResult = !Test();
                }
                else
                {
                    m_CachedResult = Test();
                }
                m_CacheInvalidationTime = Time.realtimeSinceStartup + m_CacheTTL;
            }
            return m_CachedResult;
        }

        /// <summary>
        /// Test to find the current result of this condition. The result will be 
        /// cached and should be accessed through `GetResult()`.
        /// </summary>
        /// <returns>True if the condition is satisfied, otherwise false.</returns>
        protected abstract bool Test();
    }
}
