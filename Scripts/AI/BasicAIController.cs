using NeoFPS.AI.Behaviour;
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
        List<AIBehaviour> m_Behaviours = new List<AIBehaviour>();
        [SerializeField, Tooltip("Tick optimal frequency represents how often, in game time, the AI will reconsider its current actions.")]
        [Range(0.01f, 5f)]
        float m_TickFrequency = 0.2f;
        [SerializeField, Tooltip("Is this controller active and processing behaviours it contains?")]
        internal bool m_IsActive = true;
        [Header("Debug")]
        [SerializeField, Tooltip("Log successful behaviour execution debug info to the console.")]
        bool m_DebugSuccessfulToConsole = false;
        [SerializeField, Tooltip("Log successful behaviour execution debug info to the console.")]
        bool m_DebugUnsuccessfulToConsole = false;

        protected IHealthManager m_HealthManager;

        float m_NextTick;

        protected virtual void Awake()
        {
            m_HealthManager = GetComponent<IHealthManager>();
        }

        protected virtual void Start()
        {
            for (int i = 0; i < m_Behaviours.Count; i++)
            {
                m_Behaviours[i] = Instantiate(m_Behaviours[i]); // instantiate so that a single SO is not shared across GameObjects
                m_Behaviours[i].Init(gameObject);
            }
        }

        protected virtual void Update()
        {
            if (!m_IsActive)
            {
                return;
            }

            // REFACTOR: Is Time.time the best option, or Time.realtimeSinceLevelWasLoaded? 
            // If the reduced tickrate is intended as an optimisation, then it should be realtime. 
            // If it's intended for something like a simple reaction time, then it should be time so that it gets slower if the player can mess with the time scale.
            if (Time.time > m_NextTick)
            {
                string result;
                for (int i = 0; i < m_Behaviours.Count; i++)
                {
                    if (m_Behaviours[i].IsActive)
                    {
                        result = m_Behaviours[i].Tick();
                        if (string.IsNullOrEmpty(result) && m_DebugSuccessfulToConsole) {
                            Debug.Log(m_Behaviours[i] + " behaviour from the " + m_GroupName + " group fired succesfully.");
                        } else if (!string.IsNullOrEmpty(result) && m_DebugUnsuccessfulToConsole)
                        {
                            Debug.Log(m_Behaviours[i] + " from the " + m_GroupName + " group did not fire because " + result);
                        }

                        if (string.IsNullOrEmpty(result))
                        {
                            break;
                        }
                    } else if (m_DebugUnsuccessfulToConsole)
                    {
                        Debug.Log(m_Behaviours[i] + " from the " + m_GroupName + " group did not fire because it is either inactive or the required conditions were not met.");
                    }
                }
                m_NextTick = Time.time + m_TickFrequency;
            }
        }
    }
}
