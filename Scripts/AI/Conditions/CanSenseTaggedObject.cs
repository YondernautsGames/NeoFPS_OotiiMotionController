using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI.Condition
{
    [CreateAssetMenu(fileName = "CanSenseTaggedObject", menuName = "NeoFPS/AI/Condition/Can Sense Tagged Object")]
    [Obsolete("Use CanSeeObject instead (which at the time of writing needs support for tags adding.")]
    public class CanSenseTaggedObject : AbstractTaggedObjectCondition
    {
        [SerializeField, Tooltip("Any player within this distance of the agent will be detected regardless of their position relative to the agent.")]
        private float m_AutomaticSensingRange = 10;

        protected override bool Test()
        {
            List<Collider> result = GetObjectsWithinSphere(m_AutomaticSensingRange);

            return result.Count > 0;
        }
    }
}
