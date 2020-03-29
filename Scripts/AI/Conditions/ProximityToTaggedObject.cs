using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI.Condition
{
    /// <summary>
    /// Test whether the agent is within minimum and maximum range of a tagged object.
    /// </summary>
    [CreateAssetMenu(fileName = "ProximityToTaggedObject", menuName = "NeoFPS/AI/Condition/Proximity To Tagged Object")]
    public class ProximityToTaggedObject : AICondition
    {
        protected override bool Test()
        {
            throw new System.NotImplementedException();
        }
    }
}
