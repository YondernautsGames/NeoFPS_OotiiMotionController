using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    /// <summary>
    /// The NavMeshAgent will try to find a specified target or type of target.
    /// If a suitable target can be sensed then the agent will move towards it.
    /// 
    /// If more than one item is detected the nearest will be used.
    /// </summary>
    [CreateAssetMenu(fileName = "NavMeshSeek", menuName = "NeoFPS/AI/NavMesh Seek")]
    public class NavMeshSeek : AbstractNavMeshBehaviour
    {
        [SerializeField, Tooltip("The tag that must be attached to an object for it to be considered a target. If empty then any tag will be accepted. To be detected targets must have a collider attached.")]
        string m_TargetTag = "";
        [SerializeField, Tooltip("The range within which objects will automatically be sensed. Any object that matches other search parameters within this range will be detected regardless of their position.")]
        float m_AutomaticSensingRange = 10f;
    
        /// REFACTOR: move the player sensing into a condition MotionControllerCanSensePlayer, but how do we get the sensed value into this behaviour

        internal override string Tick()
        {
            List<Collider> result = new List<Collider>();
            GetAutomaticallyDetected(result);

            for (int i = 0; i < result.Count; i++)
            {
                if (m_Agent.destination != result[i].gameObject.transform.position)
                {
                    if (m_Agent.SetNearestDestination(result[i].gameObject.transform.position))
                    {
                        return "";
                    }
                } 
            }

            return "Can't sense a target";
        }

        private void GetAutomaticallyDetected(List<Collider> result)
        {
            Collider[] hitColliders = Physics.OverlapSphere(m_Owner.transform.position, m_AutomaticSensingRange);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (!string.IsNullOrEmpty(m_TargetTag))
                {
                    if (hitColliders[i].CompareTag(m_TargetTag))
                    {
                        result.Add(hitColliders[i]);
                    }
                }
                else
                {
                    result.Add(hitColliders[i]);
                }
            }
        }
    }
}
