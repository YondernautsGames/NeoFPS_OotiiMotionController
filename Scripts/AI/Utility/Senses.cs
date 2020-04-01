using System;
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
        /// Find all objects matching a filter that are within Line of Site.
        /// </summary>
        public static List<Transform> FindVisibleTargets(Transform viewer, Vector3 eyesPosition, float radius, float fovAngle, LayerMask obstacleMask, ObjectFilter filter)
        {
            List<Transform> visibleTargets = new List<Transform>();
            if (radius == 0) { return visibleTargets; }

            List<Collider> targetsInViewRadius = GetObjectsWithinSphere(eyesPosition, radius, filter);

            for (int i = 0; i < targetsInViewRadius.Count; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - eyesPosition).normalized;
                if (Vector3.Angle(viewer.forward, dirToTarget) <= fovAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(eyesPosition, target.position);
                    if (!Physics.Raycast(eyesPosition, dirToTarget, distanceToTarget, obstacleMask))
                    {
                        Debug.Log(viewer + " can see " + target);
                        visibleTargets.Add(target);
                    } else
                    {
                        Debug.Log(viewer + " can't see " + target);
                    }
                }
            }

            return visibleTargets;
        }

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
        /// A specific GameObject that we are looking for. If this is null then all objects matching other criteria will be discovered.
        /// </summary>
        public GameObject gameObject;
        /// <summary>
        /// A tag that must be applied to an object for it to be sensed. If this is null or empty then any tag will be accepted.
        /// </summary>
        public string tag;
        /// <summary>
        /// The layers that contain all objects that we want to sense.
        /// </summary>
        public LayerMask targetMask;
    }
}
