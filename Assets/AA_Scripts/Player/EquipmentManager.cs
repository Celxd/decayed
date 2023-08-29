using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform weaponHolder;
    [SerializeField] GameObject handRifle;
    [SerializeField] GameObject handPistol;

    Inventory inventory;
    PlayerInput playerInput;
    InputAction action_primary;
    InputAction action_secondary;
    InputAction action_melee;
    InputAction action_drop;

    public int currentWeaponIndex = 2;
    [HideInInspector] public GameObject currentWeaponObject = null;


    void Start()
    {
        inventory = GetComponent<Inventory>();
        playerInput = GetComponent<PlayerInput>();

        action_primary = playerInput.actions["Weapon_Primary"];
        action_secondary = playerInput.actions["Weapon_Secondary"];
        action_melee = playerInput.actions["Weapon_Melee"];
        action_drop = playerInput.actions["Drop"];

        action_primary.performed += ctx => HandleWeaponSelection(inventory.GetWeapon(0));
        action_secondary.performed += ctx => HandleWeaponSelection(inventory.GetWeapon(1));
        action_melee.performed += ctx => HandleWeaponSelection(inventory.GetWeapon(2));
        action_drop.performed += ctx => DropWeapon();

        HandleWeaponSelection(inventory.GetWeapon(2));
    }
    
    private void EquipWeapon(Weapon weapon)
    {
        currentWeaponIndex = (int)weapon.weaponCategory;
        currentWeaponObject = Instantiate(weapon.gunModel, weaponHolder);
        currentWeaponObject.GetComponent<ItemObject>().item = weapon;

        EquipHand(currentWeaponIndex);
    }
    
    private void UnequipWeapon()
    {
        Destroy(currentWeaponObject);
        currentWeaponIndex = 2;
        DisableHand();
    }

    public void DropWeapon()
    {
        if (currentWeaponObject == null)
            return;
        Rigidbody rb = currentWeaponObject.GetComponent<Rigidbody>();

        inventory.RemoveWeapon(currentWeaponIndex);
        currentWeaponIndex = 2;
        currentWeaponObject.transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(transform.forward * 5, ForceMode.Impulse);
        currentWeaponObject = null;

        DisableHand();
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

    void EquipHand(int category)
    {
        if (category != 0 || category != 1)
            DisableHand();

        if (category == 0)  //If rifle
        {
            handRifle.SetActive(true);
            handPistol.SetActive(false);
        } 
        else if (category == 1) //If pistol
        {
            handRifle.SetActive(false);
            handPistol.SetActive(true);
        }
    }

    void DisableHand()
    {
        handRifle.SetActive(false);
        handPistol.SetActive(false);
    }
}
