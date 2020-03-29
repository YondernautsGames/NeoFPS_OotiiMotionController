using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    /// <summary>
    /// A condition that can be used to control whether an AI Behaviour executes or not.
    /// </summary>
    public abstract class AICondition : ScriptableObject
    {
        [SerializeField, Tooltip("Time To Live, in seconds, for the result cache. In order to minimize performance hits the condition will be tested no more frequently than this time.")]
        private float m_CacheTTL = 0.025f;

        private bool m_CachedResult;
        internal AIBehaviour m_behaviour;

        private float m_CacheInvalidationTime = float.NegativeInfinity;

        /// <summary>
        /// Called during the AIBehaviour Init method to initialize any components needed.
        /// <param name="behaviour">The behaviour this condition instance belongs to.</param>
        /// <return>True if the condition has been correctly initialized.</return>
        /// </summary>
        internal virtual bool Init(AIBehaviour behaviour)
        {
            m_behaviour = behaviour;
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
                m_CachedResult = Test();
                m_CacheInvalidationTime = Time.realtimeSinceStartup + m_CacheTTL;
                Debug.Log(this + " cached result = " + m_CachedResult);
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
