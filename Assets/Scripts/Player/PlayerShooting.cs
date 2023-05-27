using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject point;
    [SerializeField] TMP_Text ammo;
    [SerializeField] GameObject holePrefab;

    [Header("Settings")]
    [Range(0, 3f)] float recoilX;
    [Range(0, 7f)] float recoilY;
    [Range(0, 10f)] float maxRecoilTime;

    private Inventory inventory;
    private EquipmentManager equipmentManager;
    private PlayerInput playerInput;
    InputAction action_shoot;
    InputAction action_reload;
    InputAction mouse;
    private Weapon currentWeapon;
    public int currentWeaponIndex = 0;

    float currentRecoilX;
    float currentRecoilY;
    float timePressed;
    float totalAmmo;
    
    

    Coroutine fireCoroutine;

    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();
        equipmentManager = GetComponent<EquipmentManager>();

        action_shoot = playerInput.actions["Shoot"];
        action_reload = playerInput.actions["Reload"];
        mouse = playerInput.actions["Look"];

        action_shoot.started += ctx => StartFiring();
        action_shoot.canceled += ctx => StopFiring();
        action_reload.started += ctx => StartCoroutine(Reload());

        currentWeaponIndex = equipmentManager.currentWeaponIndex;
        
        ammo.text = currentWeapon.currentAmmo.ToString();
    }

    private void Update()
    {
        if (currentWeaponIndex != equipmentManager.currentWeaponIndex)
            InitWeapon();
    }

    public void InitWeapon()
    {
        currentWeaponIndex = equipmentManager.currentWeaponIndex;
        currentWeapon = inventory.GetItem(currentWeaponIndex);
        
        totalAmmo = currentWeapon.magCount * currentWeapon.magSize;
        ammo.text = currentWeapon.currentAmmo.ToString() + " / " + totalAmmo.ToString();
        currentWeapon.reloading = false;
    }
    
    public void RecoilMath()
    {
        currentRecoilX = ((Random.value - 0.5f) / 2) * recoilX;
        currentRecoilY = ((Random.value - 0.5f) / 2) * (timePressed >= maxRecoilTime ? recoilY / 4 : recoilY);
        //TODO: Apply recoil to camera (hint: u already marked the solution in chrome in case u forgoor)
    }
    
    bool IsClipped()
    {
        if (Physics.Raycast(point.transform.position, point.transform.forward, 1f))
            return true;
        else
            return false;
    }

    private bool CanShoot() => !currentWeapon.reloading && currentWeapon.currentAmmo > 0 && !IsClipped();

    public void Shoot()
    {
        if (CanShoot())
        {

            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit))
            {
                Debug.Log(hit.transform.name);
                Instantiate(holePrefab, hit.point + (hit.normal * 0.001f), Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);
            }
            Debug.DrawRay(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)), Camera.main.transform.forward * currentWeapon.range, Color.red, currentWeapon.range);

            RecoilMath();
            currentWeapon.currentAmmo--;
            ammo.text = currentWeapon.currentAmmo.ToString() + " / " + totalAmmo.ToString();
        }

    }

    private bool CanReload() => totalAmmo > 0 && currentWeapon.currentAmmo < currentWeapon.magSize && !currentWeapon.reloading;

    IEnumerator Reload()
    {
        if(CanReload())
        {
            currentWeapon.reloading = true;
            ammo.text = "Reloading";
            yield return new WaitForSeconds(currentWeapon.reloadTime);
            currentWeapon.currentAmmo = currentWeapon.magSize;
            totalAmmo -= currentWeapon.magSize;
            currentWeapon.magCount -= 1;
            ammo.text = currentWeapon.currentAmmo.ToString() + "/" + totalAmmo.ToString();
            currentWeapon.reloading = false;
        }
    }

    public IEnumerator RapidFire()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(1 / (currentWeapon.fireRate / 60));
        }
    }

    IEnumerator PressedTime()
    {
        while (true)
        {
            timePressed += Time.deltaTime;
            timePressed = timePressed >= maxRecoilTime ? maxRecoilTime : timePressed;
            yield return null;
        }
    }

    void StartFiring()
    {
        if (currentWeapon.weaponType == WeaponType.Rifle || currentWeapon.weaponType == WeaponType.Submachine)
        {
            fireCoroutine = StartCoroutine(RapidFire());
            StartCoroutine(PressedTime());
        }
        else
        {
            Shoot();
        }
    }

    void StopFiring()
    {
        if (fireCoroutine != null)
            StopCoroutine(fireCoroutine);

        timePressed = 0;
    }
}
