using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI
{
    [CreateAssetMenu(fileName = "MotionControllerCanSenseTaggedObject", menuName = "NeoFPS/AI/Motion Controller/Condition/Can Sense Tagged Object")]
    public class CanSenseTaggedObject : AICondition
    {
        [SerializeField, Tooltip("The tag that must be attached to an object for it to be considered a target. If empty then any tag will be accepted. To be detected targets must have a collider attached.")]
        string m_TargetTag = "";
        [SerializeField, Tooltip("Any player within this distance of the agent will be detected regardless of their position relative to the agent.")]
        private float m_AutomaticSensingRange = 10;

        protected override bool Test()
        {
            List<Collider> result = new List<Collider>();
            GetAutomaticallyDetected(result);

            return result.Count > 0;
        }

        private void GetAutomaticallyDetected(List<Collider> result)
        {
            Collider[] hitColliders = Physics.OverlapSphere(m_behaviour.m_Owner.transform.position, m_AutomaticSensingRange);
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
