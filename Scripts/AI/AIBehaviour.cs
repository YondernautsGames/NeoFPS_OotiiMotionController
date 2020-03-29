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
        /// <summary>
        /// Is this behaviour active?
        /// </summary>
        internal bool m_IsActive = true;
        /// <summary>
        /// The owning NPC for this AI Behaviour instance.
        /// </summary>
        internal GameObject m_Owner = null;

        /// <summary>
        /// Called during the AIController Start method to initialize any components needed.
        /// <param name="owner">The parent GameObject for this behaviour.</param>
        /// <return>True if the behaviour has been correctly initialized.</return>
        /// </summary>
        internal virtual bool Init(GameObject owner)
        {
            this.m_Owner = owner;
            return m_IsActive;
        }

        /// <summary>
        /// Called on each update tick by the controller. This may be less frequently than
        /// the Update cycle depending on the controller configuration. Use this method to 
        /// take any action required.
        /// </summary>
        /// <returns>An empty string if the behaviour fired or a reason that the behaviour did not fire.</returns>
        internal abstract string Tick();
    }
}
