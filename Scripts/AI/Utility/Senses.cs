using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    /// <summary>
    /// Methods for coding senses such as sight and sound.
    /// </summary>
    public static class Senses
    {
        /// <summary>
        /// Get all objects matching the required criteria within a sphere centered on the agents position.
        /// </summary>
        /// <param name="centre">The center point of the spehere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="includeFilter">A filter that identifies results that should be included in the results.</param>
        /// <param name="result">All colliders within the sphere that match the filter</param>
        public static List<Collider> GetObjectsWithinSphere(Vector3 centre, float radius, ObjectFilter includeFilter)
        {
            List<Collider> result = new List<Collider>();

            Collider[] hitColliders = Physics.OverlapSphere(centre, radius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (includeFilter.gameObject != null && GameObject.ReferenceEquals(includeFilter.gameObject, hitColliders[i].gameObject))
                {
                    result.Add(hitColliders[i]);
                }
                else if (!string.IsNullOrEmpty(includeFilter.tag))
                {
                    if (hitColliders[i].CompareTag(includeFilter.tag))
                    {
                        result.Add(hitColliders[i]);
                    }
                    else
                    {
                        result.Add(hitColliders[i]);
                    }
                }
            }

            return result;
        }
    }

    public struct ObjectFilter
    {
        /// <summary>
        /// A tag that must be applied to the object. If null then any tag will be accepted.
        /// </summary>
        public string tag;
        /// <summary>
        /// A specific GameObject.
        /// </summary>
        public GameObject gameObject;
    }
}
