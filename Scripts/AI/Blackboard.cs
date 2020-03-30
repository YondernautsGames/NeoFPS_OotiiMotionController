using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    /// <summary>
    /// The Blackboard is used to store global variables. Variables stored here
    /// are available to all behaviours and conditions regardless of which
    /// controller they live in.
    /// </summary>
    public class Blackboard : MonoBehaviour
    {
        internal GameObject m_Target;

        /// <summary>
        /// Get the value of a blackborad variable.
        /// </summary>
        /// <param name="name">The name of the blackboard variable to retrieve.</param>
        /// <returns>THe blackboard variable.</returns>
        public GameObject GetVariable(string name)
        {
            if (name == "Target")
            {
                return m_Target;
            }
            else
            {
                throw new NotImplementedException("Getting any variable other than 'Target' is not currently supported.");
            }
        }

        /// <summary>
        /// Get the value of a blackborad variable.
        /// </summary>
        /// <param name="name">The name of the blackboard variable to retrieve.</param>
        /// <param name="value">The value of the blackboard variable.</param>
        public void SetVariable(string name, GameObject value)
        {
            if (name == "Target")
            {
                m_Target = value;
            }
            else
            {
                throw new NotImplementedException("Setting any variable other than 'Target' is not currently supported.");
            }
        }
    }
}
