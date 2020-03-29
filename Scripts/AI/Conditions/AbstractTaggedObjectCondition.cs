using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI.Condition
{
    public abstract class AbstractTaggedObjectCondition : AICondition
    {
        [SerializeField, Tooltip("The tag that must be attached to an object for it to be considered a target. If empty then any tag will be accepted. To be detected targets must have a collider attached.")]
        internal string m_TargetTag = "";

        /// <summary>
        /// Get all objects matching the required criteria within a sphere centered on the agents position.
        /// </summary>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="result">T</param>
        internal List<Collider> GetObjectsWithinSphere(float radius)
        {
            List<Collider> result = new List<Collider>();

            Collider[] hitColliders = Physics.OverlapSphere(m_behaviour.m_Owner.transform.position, radius);
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

            return result;
        }
    }
}
