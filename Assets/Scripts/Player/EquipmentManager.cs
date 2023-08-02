using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] public Transform weaponHolder;
    private Inventory inventory;
    private PlayerInput playerInput;
    private InputAction action_primary;
    private InputAction action_secondary;
    private InputAction action_melee;
    private InputAction action_drop;

    public int currentWeaponIndex = 2;
    private GameObject currentWeaponObject = null;


    void Start()
    {
        inventory = GetComponent<Inventory>();
        playerInput = GetComponent<PlayerInput>();

        action_primary = playerInput.actions["Weapon_Primary"];
        action_secondary = playerInput.actions["Weapon_Secondary"];
        action_melee = playerInput.actions["Weapon_Melee"];
        action_drop = playerInput.actions["Drop"];

        action_primary.performed += ctx => HandleWeaponSelection(inventory.GetItem(0));
        action_secondary.performed += ctx => HandleWeaponSelection(inventory.GetItem(1));
        action_melee.performed += ctx => HandleWeaponSelection(inventory.GetItem(2));
        action_drop.performed += ctx => DropWeapon();

        HandleWeaponSelection(inventory.GetItem(2));
    }
    
    private void EquipWeapon(Weapon weapon)
    {
        currentWeaponIndex = (int)weapon.weaponCategory;
        currentWeaponObject = Instantiate(weapon.gunModel, weaponHolder);
    }
    
    private void UnequipWeapon()
    {
        Destroy(currentWeaponObject);
        currentWeaponIndex = 2;
    }

    public void DropWeapon()
    {
        if (currentWeaponObject == null)
            return;
        Rigidbody rb = currentWeaponObject.GetComponent<Rigidbody>();

        inventory.RemoveItem(currentWeaponIndex);
        currentWeaponIndex = 2;
        currentWeaponObject.transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(transform.forward * 5, ForceMode.Impulse);
        currentWeaponObject = null;
    }
    
    public void HandleWeaponSelection(Weapon weapon)
    {
        if (weapon == null)
            return;

        if (currentWeaponIndex != (int)weapon.weaponCategory)
        {
            UnequipWeapon();
            EquipWeapon(weapon);
        }
        else if (currentWeaponIndex == (int)weapon.weaponCategory)
        {
            UnequipWeapon();
        }
    }
}
