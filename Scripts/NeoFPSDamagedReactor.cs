using com.ootii.Actors.Combat;
using com.ootii.Actors.LifeCores;
using com.ootii.Base;
using com.ootii.Messages;
using com.ootii.Reactors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.OotiiMotionController
{
    [Serializable]
    [BaseName("Neo FPS Damaged Reactor")]
    [BaseDescription("Neo FPS reactor for handling messages of type DamageMessage. Damage inflicted by a character controlled by Ootii Motion Controller will be transferred to the Neo FPS character.")]
    public class NeoFPSDamagedReactor : ReactorAction    
    {
        [NonSerialized]
        protected ActorCore mActorCore = null;
        private IDamageHandler mDamageHandler;

        public NeoFPSDamagedReactor() : base()
        {
            _ActivationType = 0;
        }

        public NeoFPSDamagedReactor(GameObject rOwner) : base(rOwner)
        {
            _ActivationType = 0;
            mActorCore = rOwner.GetComponent<ActorCore>();
        }

        public override void Awake()
        {
            if (mOwner != null)
            {
                mActorCore = mOwner.GetComponent<ActorCore>();
            }
        }

        /// <summary>
        /// Used to test if the reactor should process
        /// </summary>
        /// <returns></returns>
        public override bool TestActivate(IMessage rMessage)
        {
            if (!base.TestActivate(rMessage)) { return false; }

            if (mActorCore == null || !mActorCore.IsAlive) { return false; }

            DamageMessage lDamageMessage = rMessage as DamageMessage;
            if (lDamageMessage == null) { return false; }

            if (lDamageMessage.Damage == 0f) { return false; }

            if (lDamageMessage.ID != 0)
            {
                if (lDamageMessage.ID == CombatMessage.MSG_DEFENDER_KILLED) { return false; }
                if (lDamageMessage.ID != CombatMessage.MSG_DEFENDER_ATTACKED) { return false; }
            }

            CombatMessage lCombatMessage = rMessage as CombatMessage;
            if (lCombatMessage != null && lCombatMessage.Defender.transform.root.gameObject != mActorCore.gameObject) { return false; }

            mDamageHandler = lCombatMessage.Defender.GetComponent<IDamageHandler>();
            if (mDamageHandler == null)
            {
                Debug.LogError(mOwner + " recieved a damage message with a hit on " + lCombatMessage.Defender.transform + " but there is no IDamageHandler attached. Damage is ignored.");
                return false;
            }

            mMessage = lDamageMessage;

            return true;
        }

        /// <summary>
        /// Called when the reactor is first activated
        /// </summary>
        /// <returns>Determines if other reactors should process.</returns>
        public override bool Activate()
        {
            base.Activate();
            CombatMessage combatMessage = (CombatMessage)mMessage;
            IDamageSource damageSource = (IDamageSource)combatMessage.Weapon;
            NeoFPSWeaponCore weapon = (NeoFPSWeaponCore)combatMessage.Weapon;

            mDamageHandler.AddDamage(combatMessage.Damage, damageSource);

            // Get character kicker
            var character = mOwner.GetComponent<NeoFPS.ICharacter>();
            if (character != null)
            {
                var kicker = character.headTransformHandler.GetComponent<NeoFPS.AdditiveKicker>();
                if (kicker != null)
                {
                    // Kick the camera position & rotation
                    float kickDuration = 0.25f;
                    float kickRotation = 5f;
                    kicker.KickPosition(combatMessage.HitDirection * combatMessage.ImpactPower, kickDuration);
                    kicker.KickRotation(Quaternion.AngleAxis(kickRotation, Vector3.Cross(combatMessage.HitDirection, Vector3.up)), kickDuration);
                }
            }

            //Debug.Log(damageMessage.Sender + " hit for " + ((DamageMessage)mMessage).Damage + " on " + mOwner);
            Deactivate();
            return true;
        }

        /// <summary>
        /// Called when the reactor is meant to be deactivated
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();

            mMessage = null;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Called when the inspector needs to draw
        /// </summary>
        public override bool OnInspectorGUI(UnityEditor.SerializedObject rTargetSO, UnityEngine.Object rTarget)
        {
            _EditorShowActivationType = false;
            return false;
        }
#endif
    }
}
