using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI.Condition
{
    /// <summary>
    /// Checks to see if a particular Game Object can be sensed by the agent.
    /// 
    /// The agent supports a field of view. For a good tutorial on how this works see 
    /// Sebastion Lague's Video https://www.youtube.com/watch?v=rQG9aUWarwE which was
    /// the initial inspiration for some of the code in this script.
    /// 
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "CanSenseObject", menuName = "NeoFPS/AI/Condition/Can Sense Object")]
    public class CanSenseObject : AICondition
    {
        [Header("Target Criteria")]
        [SerializeField, Tooltip("The name of the variable containing a specific game object we are attempting to sense. If this is null then all objects matching the criteria below will be searched for. If this is set the search will be limited to this object.")]
        string m_VariableName = "Target";
        [SerializeField, Tooltip("The layers that contain all objects that we want to sense.")]
        LayerMask m_TargetMask;

        [Header("Sight")]
        [SerializeField, Tooltip("Any player within this radius of the agent may be seen by the agent. Note, being within this radius does not mean the object will be seen, it must also be visible via the line of sight checks. Setting this to 0 essentially makes the agent blind.")]
        float m_ViewRadius = 10;
        [SerializeField, Tooltip("When checking for line of sight to an object the target must be within this angle of forward on of the agent.")]
        [Range(0, 360)]
        float m_ViewAngle = 45;
        [SerializeField, Tooltip("The layers that contain all objects that can obstruct line of sight to an object.")]
        LayerMask m_LineOfSightObstacleMask;

        List<Transform> sensedTargets = new List<Transform>();

        private Vector3 DirectionFromAngle(float angleInDegrees)
        {
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        protected override bool Test()
        {
            sensedTargets.Clear();

            GameObject target = GetVariable(m_VariableName);
            ObjectFilter filter = new ObjectFilter();
            filter.gameObject = target;

            List<Transform> result = Senses.FindVisibleTargets(m_Behaviour.m_Owner.transform, m_Behaviour.m_Owner.transform.position, m_ViewRadius, m_ViewAngle, m_LineOfSightObstacleMask, filter);

            return result.Count > 0;
        }

        private void OnValidate()
        {
            if (m_ViewRadius < 0)
            {
                m_ViewRadius = 0;
            }
        }
    }
}
