using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NeoFPS.AI.Condition
{
    /// <summary>
    /// Test whether the agent is within minimum and maximum range of a tagged object.
    /// </summary>
    [CreateAssetMenu(fileName = "ProximityToTaggedObject", menuName = "NeoFPS/AI/Condition/Proximity To Tagged Object")]
    public class ProximityToTaggedObject : AbstractTaggedObjectCondition
    {
        [SerializeField, Tooltip("Minimum distance the object must be.")]
        float minDistance = 1.0f;
        [SerializeField, Tooltip("Maximum distance the object must be.")]
        float maxDistance = 1.0f;

        protected override bool Test()
        {
            List<Collider> include = GetObjectsWithinSphere(maxDistance);
            List<Collider> exclude = GetObjectsWithinSphere(minDistance);
            IEnumerable<Collider> result = exclude.Intersect(include);
            return result.Count<Collider>() > 0;
        }
    }
}
