using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private float pickupRange;
    [SerializeField] private LayerMask pickupLayer;

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
