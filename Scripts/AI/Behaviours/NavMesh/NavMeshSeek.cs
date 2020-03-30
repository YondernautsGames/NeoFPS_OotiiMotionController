using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    /// <summary>
    /// The NavMeshAgent will seek towards a specified target.
    /// </summary>
    [CreateAssetMenu(fileName = "NavMeshSeek", menuName = "NeoFPS/AI/NavMesh Seek")]
    public class NavMeshSeek : AbstractNavMeshBehaviour
    {
        [SerializeField, Tooltip("The name of the variable containing the game object to seek.")]
        string m_Target = "Target";
    
        /// REFACTOR: move the player sensing into a condition MotionControllerCanSensePlayer, but how do we get the sensed value into this behaviour

        internal override string Tick()
        {
            GameObject go = GetVariable(m_Target);
            if (go == null)
            {
                return "No game object set in the variable: " + m_Target;
            }

            if (m_Agent.SetNearestDestination(go.transform.position))
            {
                return "";
            }

            return "Unable to get a volid position on the NavMesh for " + go;
        }
    }
}
