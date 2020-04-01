using com.ootii.Actors.Combat;
using com.ootii.Actors.Inventory;
using NeoFPS.AI.Behaviour;
using UnityEngine;

namespace NeoFPS.AI.ootii
{
    /// <summary>
    /// Unequip the currently equipped weapon set.
    /// </summary>
    [CreateAssetMenu(fileName = "MotionControllerUnequipWeaponSet", menuName = "NeoFPS/AI/Motion Controller/Behaviour/Unequip Weapon Set")]
    public class MotionControllerUnequipWeapon : AIBehaviour
    {
        private BasicInventory m_InventorySource = null;

        /// <summary>
        /// Is the motion currently in progress m_IsMotionActive will be true.
        /// </summary>
        private bool m_IsMotionActive;
        private Combatant m_Combatant;

        internal override bool Init(GameObject owner, BasicAIController controller)
        {
            bool isSuccess = false;
            m_InventorySource = owner.GetComponent<BasicInventory>();
            isSuccess = m_InventorySource != null;
            Debug.Assert(isSuccess, owner + " has a MotionControllerEquipWeapon AI behaviour but no BasicInventory component.");

            m_Combatant = owner.GetComponent<Combatant>();
            isSuccess = m_Combatant != null;
            Debug.Assert(isSuccess, owner + " has a MotionControllerUnquipWeapon AI behaviour but no Combatant component.");

            isSuccess &= base.Init(owner, controller);

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
            if (m_Combatant.PrimaryWeapon != null)
            {
                m_InventorySource.StoreWeaponSet();
                m_IsMotionActive = true;
                return "";
            }

            return "Equip motion is already active.";
        }
    }
}
