using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] GameObject point;
    [SerializeField] TMP_Text ammo;
    [SerializeField] TMP_Text gun;
    [SerializeField] GameObject holePrefab;
    [SerializeField] Transform adsPosition;
    [SerializeField] Transform weaponHolder;

    [Header("Recoil Settings")]
    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float maxRecoilTime;
    float currentRecoilX;
    float currentRecoilY;
    float recoilDuration = 0.1f;
    float smoothTime = 0.1f;
    float smoothRecoilVelX;
    float smoothRecoilVelY;
    float timePressed;
    float originalVerticalValue;

    [Header("Aim Settings")]
    [SerializeField] float zoomRatio;
    [SerializeField] float aimAnimationSpeed;
    [HideInInspector] public bool ads = false;
    float defaultFOV;
    Vector3 defaultPos;

    private CinemachinePOV pov;
    private Inventory inventory;
    private EquipmentManager equipmentManager;
    private PlayerInput playerInput;
    InputAction action_shoot;
    InputAction action_reload;
    InputAction mouse;
    InputAction action_aim;
    private Weapon currentWeapon;
    private int currentWeaponIndex = 0;
    
    float totalAmmo;
    public AudioSource soundhit;

    Coroutine fireCoroutine;

    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();
        equipmentManager = GetComponent<EquipmentManager>();
        pov = vcam.GetCinemachineComponent<CinemachinePOV>();

        action_shoot = playerInput.actions["Shoot"];
        action_reload = playerInput.actions["Reload"];
        mouse = playerInput.actions["Look"];
        action_aim = playerInput.actions["Aim"];

        action_shoot.started += ctx => StartFiring();
        action_shoot.canceled += ctx => StopFiring();
        action_reload.started += ctx => StartCoroutine(Reload());
        action_aim.started += ctx => ads = !ads;

        currentWeaponIndex = equipmentManager.currentWeaponIndex;
        
        if (currentWeapon != null)
        {
            ammo.text = currentWeapon.currentAmmo.ToString();
            
        }
        ammo.text = "";
        gun.text = "";

        defaultFOV = vcam.m_Lens.FieldOfView;
        defaultPos = weaponHolder.localPosition;
    }

    private void Update()
    {
        if (currentWeaponIndex != equipmentManager.currentWeaponIndex)
            InitWeapon();

        if (ads)
        {
            weaponHolder.localPosition = Vector3.Lerp(weaponHolder.localPosition, adsPosition.localPosition, aimAnimationSpeed * Time.deltaTime);
            SetFieldOfView(Mathf.Lerp(vcam.m_Lens.FieldOfView, defaultFOV / zoomRatio, aimAnimationSpeed * Time.deltaTime));
        }
        else
        {
            weaponHolder.localPosition = Vector3.Lerp(weaponHolder.localPosition, defaultPos, aimAnimationSpeed * Time.deltaTime);
            SetFieldOfView(Mathf.Lerp(vcam.m_Lens.FieldOfView, defaultFOV, aimAnimationSpeed * Time.deltaTime));
        }
    }

    void SetFieldOfView(float fov)
    {
        vcam.m_Lens.FieldOfView = fov;
    }

    public void InitWeapon()
    {
        currentWeaponIndex = equipmentManager.currentWeaponIndex;
        currentWeapon = inventory.GetItem(currentWeaponIndex);

        if (currentWeapon == null)
            return;

        gun.text = currentWeapon.name.ToString();
        totalAmmo = currentWeapon.magCount * currentWeapon.magSize;
        ammo.text = currentWeapon.currentAmmo.ToString() + " / " + totalAmmo.ToString();
        currentWeapon.reloading = false;
    }
    
    public void RecoilMath()
    {
        originalVerticalValue = pov.m_VerticalAxis.Value;

        currentRecoilX = ((Random.value - 0.5f) / 2) * recoilX;
        currentRecoilY = ((Random.value - 0.5f) / 2) * (timePressed >= maxRecoilTime ? recoilY / 4 : recoilY);

        //float smoothedRecoilY = Mathf.Lerp(0f, Mathf.Abs(currentRecoilY), timePressed);
        //float smoothedRecoilX = Mathf.Lerp(0f, currentRecoilX, timePressed);

        float smoothedRecoilY = Mathf.SmoothDamp(0f, Mathf.Abs(currentRecoilY), ref smoothRecoilVelY, smoothTime);
        float smoothedRecoilX = Mathf.SmoothDamp(0f, currentRecoilX, ref smoothRecoilVelX, smoothTime);

        pov.m_VerticalAxis.Value -= smoothedRecoilY;
        pov.m_HorizontalAxis.Value -= smoothedRecoilX;
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
                Instantiate(holePrefab, hit.point + (hit.normal * 0.001f), Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);
                if (hit.transform.gameObject.GetComponent<Enemy>() != null)
                {
                    hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(28, hit.point);
                    Debug.Log(hit.transform.gameObject.GetComponent<Enemy>().health);
                }
                    
            }

            RecoilMath();
            currentWeapon.currentAmmo--;
            if(ammo != null)
                ammo.text = currentWeapon.currentAmmo.ToString() + " / " + totalAmmo.ToString();
        }

    }

    private bool CanReload() => totalAmmo > 0 && currentWeapon.currentAmmo < currentWeapon.magSize && !currentWeapon.reloading;

    IEnumerator Reload()
    {
        if(CanReload())
        {
            currentWeapon.reloading = true;
            if (ammo != null)
                ammo.text = "Reloading";
            yield return new WaitForSeconds(currentWeapon.reloadTime);
            //TODO: Make a function in WaitForSeconds that checks if gun is swapped (If true: cancel reload)
            currentWeapon.currentAmmo = currentWeapon.magSize;
            totalAmmo -= currentWeapon.magSize;
            currentWeapon.magCount -= 1;
            if(ammo != null)
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
        if (currentWeapon == null)
            return;

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
        //float currentVerticalValue = pov.m_VerticalAxis.Value;
        //float targetVerticalValue = originalVerticalValue;
        //float smoothReturnVelY = 0.1f;
        //float smoothReturnTime = 0.5f;

        //float smoothedVerticalValue = Mathf.SmoothDamp(currentVerticalValue, targetVerticalValue, ref smoothReturnVelY, smoothReturnTime);
        //pov.m_VerticalAxis.Value -= smoothedVerticalValue;
    }
}
