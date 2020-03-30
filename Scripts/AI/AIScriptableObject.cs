using NeoFPS.AI.Behaviour;
using NeoFPS.AI.Condition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    /// <summary>
    /// A ScriptableObject that is used for AIBehaviours and AIConditions.
    /// </summary>
    public abstract class AIScriptableObject : ScriptableObject
    {
        private Blackboard m_Blackboard;
        private Transform m_Transform;

        private Blackboard blackboard
        {
            get
            {
                if (m_Blackboard == null)
                {
                    AIBehaviour behaviour = this as AIBehaviour;
                    if (behaviour != null)
                    {
                        m_Blackboard = behaviour.m_Owner.GetComponent<Blackboard>();
                    }
                    else
                    {
                        AICondition condition = this as AICondition;
                        if (condition != null)
                        {
                            m_Blackboard = condition.m_Behaviour.m_Owner.GetComponent<Blackboard>();
                        }
                    }
                    Debug.Assert(m_Blackboard != null, "Any object with an AIController component must also have a Blackboard component.");
                }
                return m_Blackboard;
            }
        }

        /// <summary>
        /// Get the transform of the agent.
        /// </summary>
        protected Transform transform
        {
            get
            {
                if (m_Transform == null)
                {
                    AIBehaviour behaviour = this as AIBehaviour;
                    if (behaviour != null)
                    {
                        m_Transform = behaviour.m_Owner.transform;
                    }
                    else
                    {
                        AICondition condition = this as AICondition;
                        if (condition != null)
                        {
                            m_Transform = condition.m_Behaviour.m_Owner.transform;
                        }
                    }
                    Debug.Assert(m_Transform != null, "Cannot find the transform for the agent.");
                }
                return m_Transform;
            }
        }

        public GameObject GetVariable(string name)
        {
            return blackboard.GetVariable(name);
        }

        public void SetVariable(string name, GameObject gameObject)
        {
            blackboard.SetVariable(name, gameObject);
        }
    }
}
