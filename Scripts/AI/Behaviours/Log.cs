using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI.Behaviour
{
    /// <summary>
    /// Log something to the console. The intention is to use this when developing conditions.
    /// Create your condition, add it to a Log behaviour and test.
    /// </summary>
    [CreateAssetMenu(fileName = "Log", menuName = "NeoFPS/AI/Debug Log")]
    public class Log : AIBehaviour
    {
        [SerializeField, Tooltip("The message to log to the console.")]
        string message = "Debug Log Message";

        internal override string Tick()
        {
            Debug.Log(message);
            return "";
        }
    }
}
