using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    /// <summary>
    /// A very basic AI controller intended for demo's and prototypes. Each cahracter can have multiple controllers
    /// each providing a different group of behaviours.
    /// </summary>
    public class BasicAIController : MonoBehaviour
    {
        [SerializeField, Tooltip("The name of this group of behaviours.")]
        string m_GroupName = "AI Behaviour Group";
        [SerializeField, Tooltip("The behaviours that this AI might execute in each tick.")]
        List<AIBehaviour> m_Behaviours;
        [SerializeField, Tooltip("Tick optimal frequency represents how often, in game time, the AI will reconsider its current actions.")]
        float m_TickFrequency = 0.2f;
        [SerializeField, Tooltip("Is this controller active and processing behaviours it contains?")]
        public bool m_IsActive = true;

        float m_NextTick;

        void Start()
        {
            for (int i = 0; i < m_Behaviours.Count; i++)
            {
                m_Behaviours[i] = Instantiate(m_Behaviours[i]); // instantiate so that a single SO is not shared across GameObjects
                m_Behaviours[i].Init(gameObject);
            }
        }

        void Update()
        {
            if (!m_IsActive)
            {
                return;
            }

            if (Time.time > m_NextTick)
            {
                for (int i = 0; i < m_Behaviours.Count; i++)
                {
                    if (m_Behaviours[i].m_IsActive)
                    {
                        m_Behaviours[i].Tick();
                    }
                }
                m_NextTick = Time.time + m_TickFrequency;
            }
        }
    }
}
