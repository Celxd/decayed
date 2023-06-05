using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float pickupRange;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] Canvas pickupPrompt;
    

    private PlayerInput playerInput;
    private InputAction action_pickup;
    private Camera cam;
    private Inventory inventory;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
        inventory = GetComponent<Inventory>();

        action_pickup = playerInput.actions["Pickup"];
        
        action_pickup.performed += ctx => Pickup();

        pickupPrompt.enabled = false;
    }

    private void LateUpdate()
    {
        CheckSeen();
    }

    void CheckSeen()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit, pickupRange, pickupLayer))
        {
            InitPrompt(hit);
            Debug.Log(hit.transform.gameObject.name);
        } else
        {
            pickupPrompt.enabled = false;
        }
    }

    void InitPrompt(RaycastHit hit)
    {
        ItemObject m_item = hit.transform.GetComponent<ItemObject>();
        TextMeshProUGUI promptText = pickupPrompt.transform.Find("Panel/Name").GetComponent<TextMeshProUGUI>();
        var pos = Camera.main.transform.position;

        promptText.text = m_item.item.name;
        pickupPrompt.transform.position = hit.transform.position + Vector3.up * 0.5f;
        pickupPrompt.transform.rotation = Quaternion.LookRotation(pickupPrompt.transform.position - pos);

        pickupPrompt.enabled = true;
    }

    private void Pickup()
    {
        Debug.DrawRay(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)), Camera.main.transform.forward * pickupRange, Color.red, pickupRange);
        if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit, pickupRange, pickupLayer)) {
            Debug.Log("EEEEE");
            Weapon newItem = hit.transform.GetComponent<ItemObject>().item as Weapon;
            inventory.AddItem(newItem);
            Destroy(hit.transform.gameObject);
        }
    }
}
