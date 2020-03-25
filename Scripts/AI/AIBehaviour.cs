using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    /// <summary>
    /// An abstract Scriptable Object describing an AI Behaviour that an NPC might exhibit.
    /// </summary>
    public abstract class AIBehaviour : ScriptableObject
    {

        internal bool m_IsActive = false;
        internal GameObject m_Owner = null;

        /// <summary>
        /// Called during the AIController Start method to initialize any components needed.
        /// <param name="owner">The parent GameObject for this behaviour.</param>
        /// <return>True if the behaviour has been correctly initialized.</return>
        /// </summary>
        internal virtual bool Init(GameObject owner)
        {
            this.m_Owner = owner;
            return true;
        }

        /// <summary>
        /// Called on each update tick by the controller. This may be less frequently than
        /// the Update cycle depending on the controller configuration. Use this method to 
        /// take any action required.
        /// </summary>
        internal abstract void Tick();
    }
}
