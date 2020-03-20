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
        string groupName = "AI Behaviour Group";
        [SerializeField, Tooltip("The behaviours that this AI might execute in each tick.")]
        List<AIBehaviour> behaviours;
        [SerializeField, Tooltip("Tick optimal frequency represents how often, in game time, the AI will reconsider its current actions.")]
        float tickFrequency = 0.2f;
        [SerializeField, Tooltip("Is this controller active and processing behaviours it contains?")]
        public bool IsActive = true;

        float nextTick;

        void Start()
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                behaviours[i] = Instantiate(behaviours[i]); // instantiate so that a single SO is not shared across GameObjects
                behaviours[i].Init(gameObject);
            }
        }

        void Update()
        {
            if (!IsActive)
            {
                return;
            }

            if (Time.time > nextTick)
            {
                for (int i = 0; i < behaviours.Count; i++)
                {
                    if (behaviours[i].IsActive)
                    {
                        behaviours[i].Tick();
                    }
                }
                nextTick = Time.time + tickFrequency;
            }
        }
    }
}
