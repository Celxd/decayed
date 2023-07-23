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
    

    private PlayerInput _playerInput;
    private InputAction action_pickup;
    private Camera _cam;
    private Inventory _inventory;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _cam = Camera.main;
        _inventory = GetComponent<Inventory>();

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
            InitPrompt(hit);
            Debug.Log(hit.transform.gameObject.name);
        } else
        {
            _pickupPrompt.enabled = false;
        }
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
        Debug.DrawRay(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)), Camera.main.transform.forward * _pickupRange, Color.red, _pickupRange);
        if (Physics.Raycast(_cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit, _pickupRange, _pickupLayer)) {
            Debug.Log("EEEEE");
            Weapon newItem = hit.transform.GetComponent<ItemObject>().item as Weapon;
            _inventory.AddItem(newItem);
            Destroy(hit.transform.gameObject);
        }
    }
}
