using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _pickupRange;
    [SerializeField] private LayerMask _pickupLayer;
    [SerializeField] Canvas _pickupPrompt;

    PlayerInput _playerInput;
    InputAction action_pickup;
    Camera _cam;
    Inventory _inventory;
    EquipmentManager _equipmentManager;
    PlayerShooting _playerShooting;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _cam = Camera.main;
        _inventory = GetComponent<Inventory>();
        _equipmentManager = GetComponent<EquipmentManager>();
        _playerShooting = GetComponent<PlayerShooting>();

        action_pickup = _playerInput.actions["Pickup"];
        
        action_pickup.performed += ctx => Pickup();

        _pickupPrompt.enabled = false;
    }

    private void LateUpdate()
    {
        CheckSeen();
    }

    void CheckSeen()
    {
        if (Physics.Raycast(_cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit, _pickupRange, _pickupLayer))
        {
            //Why so long?
            //Cuz the weapon is in Player/Main Camera/Holder/Weapon holder
            //Why? u said?
            //Idk all the functions that needs this so ask the past version of me
            if (hit.transform.parent?.transform.parent?.transform.parent?.transform.parent != null)
                return;

            InitPrompt(hit);
        } 
        else
            _pickupPrompt.enabled = false;
    }

    void InitPrompt(RaycastHit hit)
    {
        ItemObject m_item = hit.transform.GetComponent<ItemObject>();
        TextMeshProUGUI promptText = _pickupPrompt.transform.Find("Panel/Name").GetComponent<TextMeshProUGUI>();
        var pos = Camera.main.transform.position;

        promptText.text = m_item.item.name;
        _pickupPrompt.transform.position = hit.transform.position + Vector3.up * 0.5f;
        _pickupPrompt.transform.rotation = Quaternion.LookRotation(_pickupPrompt.transform.position - pos);

        _pickupPrompt.enabled = true;
    }

    private void Pickup()
    {
        if (Physics.Raycast(_cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit, _pickupRange, _pickupLayer)) {
            Weapon newItem = hit.transform.GetComponent<ItemObject>().item as Weapon;

            if (_inventory.GetItem((int)newItem.weaponCategory) == null)
            {
                _inventory.AddItem(newItem);
            } 
            else
            {
                _equipmentManager.DropWeapon();
                _inventory.AddItem(newItem);
                _playerShooting.InitWeapon();
            }

            Destroy(hit.transform.gameObject);
            _equipmentManager.EquipWeapon(newItem);

        }
    }
}
