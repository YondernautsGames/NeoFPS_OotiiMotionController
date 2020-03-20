using com.ootii.Actors.Combat;
using com.ootii.Actors.LifeCores;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.OotiiMotionController
{
    public class NeoFPSWeaponCore : WeaponCore, NeoFPS.IDamageSource
    {

        #region Neo FPS IDamageSource
        public DamageFilter outDamageFilter {
            get { return NeoFPS.DamageFilter.AllDamageAllTeams; }
            set { }
        }

        public IController controller
        {
            get { return null; }
        }

        public Transform damageSourceTransform
        {
            get { return transform; }
        }

        public string description
        {
            get { return transform.name; }
        }
        #endregion

            /// <summary>
            /// Raised when the impact occurs
            /// </summary>
            /// <param name="rHitInfo">CombatHit structure detailing the hit information.</param>
            /// <param name="rAttackStyle">ICombatStyle that details the combat style being used.</param>
        protected override void OnImpact(CombatHit rHitInfo, ICombatStyle rAttackStyle = null)
        {
            // Test impact is now rolling up to the parent object but this means we are trying to apply the damage to the actorcore rather than the collider actually hit
            // see Test impact below
            IHealthManager lHealthManager = rHitInfo.Collider.gameObject.GetComponentInParent<IHealthManager>();
            GameObject defender = ((MonoBehaviour)lHealthManager).gameObject;
            mDefenders.Add(defender);

            mImpactCount++;

            Transform lHitTransform = GetClosestTransform(rHitInfo.Point, rHitInfo.Collider.transform);
            Vector3 lHitDirection = Quaternion.Inverse(lHitTransform.rotation) * (rHitInfo.Point - lHitTransform.position).normalized;

            CombatMessage lMessage = CombatMessage.Allocate();
            lMessage.Attacker = mOwner;
            lMessage.Defender = rHitInfo.Collider.gameObject;
            lMessage.Weapon = this;
            lMessage.Damage = GetAttackDamage(Random.value, (rAttackStyle != null ? rAttackStyle.DamageModifier : 1f));
            lMessage.ImpactPower = GetAttackImpactPower();
            lMessage.HitPoint = rHitInfo.Point;
            lMessage.HitDirection = lHitDirection;
            lMessage.HitVector = rHitInfo.Vector;
            lMessage.HitTransform = lHitTransform;
            lMessage.AttackIndex = mAttackStyleIndex;
            lMessage.CombatStyle = rAttackStyle;

            ActorCore lAttackerCore = (mOwner != null ? mOwner.GetComponentInParent<ActorCore>() : null);
            ActorCore lDefenderCore = defender.gameObject.GetComponentInParent<ActorCore>();

            lMessage.ID = CombatMessage.MSG_ATTACKER_ATTACKED;

            if (lAttackerCore != null)
            {
                lAttackerCore.SendMessage(lMessage);
            }

#if USE_MESSAGE_DISPATCHER || OOTII_MD
            MessageDispatcher.SendMessage(lMessage);
#endif

            lMessage.ID = CombatMessage.MSG_DEFENDER_ATTACKED;

            if (lDefenderCore != null)
            {   
                lDefenderCore.SendMessage(lMessage);

#if USE_MESSAGE_DISPATCHER || OOTII_MD
                MessageDispatcher.SendMessage(lMessage);
#endif
            }

            if (lAttackerCore != null)
            {
                lAttackerCore.SendMessage(lMessage);

#if USE_MESSAGE_DISPATCHER || OOTII_MD
                MessageDispatcher.SendMessage(lMessage);
#endif
            }

            OnImpactComplete(lMessage);

            CombatMessage.Release(lMessage);
        }

        /// <summary>
        /// Test each of the combatants to determine if an impact occured. 
        /// 
        /// FIXME: A Neo FPS character
        /// has multiple possible hit zones, therefore we should have a way of determining
        /// which is actually hit. Right now we just hit the first found. This might be
        /// enough depending on how they are ordered coming into this method.
        /// </summary>
        /// <param name="rCombatTargets">Targets who we may be impacting</param>
        /// <param name="rAttackStyle">ICombatStyle that details the combat style being used.</param>
        /// <returns>The number of impacts that occurred</returns>
        public override int TestImpact(List<CombatTarget> rCombatTargets, ICombatStyle rAttackStyle = null)
        {
            mImpactCount = 0;

            float lMaxReach = 0f;

            if (mOwner != null)
            {
                ICombatant lCombatant = mOwner.GetComponent<ICombatant>();
                if (lCombatant != null) { lMaxReach = lCombatant.MaxMeleeReach; }
            }

            for (int i = 0; i < rCombatTargets.Count; i++)
            {
                CombatTarget lTarget = rCombatTargets[i];

                // Stop if we don't have a valid target
                if (lTarget == CombatTarget.EMPTY) { continue; }

                // Stop if we already hit the Neo FPS Character
                IHealthManager lHealthManager = lTarget.Collider.gameObject.GetComponentInParent<IHealthManager>();
                if (lHealthManager == null) {
                    Debug.LogError("Testing for weapon impact impact on " + lTarget.Collider + " but cannot find an IHealthManager in parents. Ignoring.");
                    continue;
                }
                GameObject go = ((MonoBehaviour)lHealthManager).gameObject;
                if (mDefenders.Contains(go)) { continue; }

                // Stop if we're out of range
                float lDistance = Vector3.Distance(lTarget.ClosestPoint, mTransform.position);
                if (lDistance > _MaxRange + lMaxReach) { continue; }

                Vector3 lVector = (mTransform.position - mLastPosition).normalized;
                if (lVector.sqrMagnitude == 0 && mOwner != null) { lVector = mOwner.transform.forward; }

                mLastHit.Collider = lTarget.Collider;
                mLastHit.Point = lTarget.ClosestPoint;
                mLastHit.Normal = -lVector;
                mLastHit.Vector = lVector;
                mLastHit.Distance = lTarget.Distance;
                mLastHit.Index = mImpactCount;

                OnImpact(mLastHit, rAttackStyle);
            }

            return mImpactCount;
        }
    }
}
