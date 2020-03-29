using com.ootii.Actors.Inventory;
using NeoFPS.AI.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI.ootii
{
    /// <summary>
    /// Equip a specific weapon set from the Ootii inventory if an enemy has been detected.
    /// </summary>
    [CreateAssetMenu(fileName = "MotionControllerEquipWeaponSet", menuName = "NeoFPS/AI/Motion Controller/Behaviour/Equip Weapon Set")]
    public class MotionControllerEquipWeapon : AIBehaviour
    {
        [SerializeField, Tooltip("The ID of the weapon set to equip.")]
        int m_WeaponSetID = 0;

        private BasicInventory m_InventorySource = null;

        /// <summary>
        /// Is the motion currently in progress m_IsMotionActive will be true.
        /// </summary>
        private bool m_IsMotionActive;

        internal override bool Init(GameObject owner)
        {
            bool isSuccess = false;
            m_InventorySource = owner.GetComponent<BasicInventory>();
            isSuccess = m_InventorySource != null;
            Debug.Assert(isSuccess, owner + " has a MotionControllerEquipWeapon AI behaviour but no BasicInventory.");
            isSuccess &= base.Init(owner);

            if (isSuccess)
            {
                IsActive = true;
                return true;
            }
            else
            {
                IsActive = false;
                return true;
            }
        }

        internal override string Tick()
        {
            if (m_InventorySource.IsWeaponSetEquipped(m_WeaponSetID))
            {
                m_IsMotionActive = false;
                return "Weapon set already equipped";
            }

            if (!m_IsMotionActive)
            {
                m_InventorySource.EquipWeaponSet(m_WeaponSetID);
                m_IsMotionActive = true;
                return "";
            }

            return "Equip motion is already active.";
        }
    }
}
