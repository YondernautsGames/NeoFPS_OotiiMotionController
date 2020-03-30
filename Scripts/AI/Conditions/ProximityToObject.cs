using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NeoFPS.AI.Condition
{
    /// <summary>
    /// Looks to see if a specific object is within a given distance range.
    /// </summary>
    [CreateAssetMenu(fileName = "ProximityToObject", menuName = "NeoFPS/AI/Condition/Proximity To Object")]
    public class ProximityToObject : AICondition
    {
        [SerializeField, Tooltip("The name of the variable containing the game object to seek.")]
        string m_Target = "Target";
        [SerializeField, Tooltip("Minimum distance the object must be.")]
        float minDistance = 1.0f;
        [SerializeField, Tooltip("Maximum distance the object must be.")]
        float maxDistance = 2.5f;

        protected override bool Test()
        {
            ObjectFilter filter = new ObjectFilter();
            filter.gameObject = GetVariable(m_Target);

            List<Collider> include = Senses.GetObjectsWithinSphere(transform.position, maxDistance, filter);
            List<Collider> exclude = Senses.GetObjectsWithinSphere(transform.position, minDistance, filter);
            IEnumerable<Collider> result = exclude.Intersect(include);
            return result.Count<Collider>() > 0;
        }
    }
}
