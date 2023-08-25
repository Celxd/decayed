using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public CinemachineVirtualCamera vcam;
    [SerializeField] GameObject point;
    [SerializeField] TMP_Text ammo;
    [SerializeField] TMP_Text gun;
    [SerializeField] GameObject holePrefab;
    [SerializeField] Transform adsPosition;
    [SerializeField] Transform weaponHolder;
    [SerializeField] AudioSource _audio;

    [Header("Audio Settings")]
    [SerializeField] AudioClip shoot;
    [SerializeField] AudioClip reload;

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

    CinemachinePOV pov;
    Inventory inventory;
    EquipmentManager equipmentManager;
    PlayerInput playerInput;
    InputAction action_shoot;
    InputAction action_reload;
    InputAction mouse;
    InputAction action_aim;
    Weapon currentWeapon;
    int currentWeaponIndex = 0;
    
    Consumables currentAmmo;
    public AudioSource soundhit;
    public GameObject hitmarker;
    Coroutine fireCoroutine;
    bool melee;

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
        
        defaultFOV = vcam.m_Lens.FieldOfView;
        defaultPos = weaponHolder.localPosition;
        //hitmarker.SetActive(false);

        InitWeapon();
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
        currentWeapon = inventory.GetWeapon(currentWeaponIndex);
        currentAmmo = inventory.SearchAmmo(currentWeapon.ammoType);

        if (currentWeapon == null)
            return;

        if ((int)currentWeapon.weaponCategory == 2)
            melee = true;
        else
            melee = false;

        UpdateUI();

        currentWeapon.reloading = false;
    }

    void UpdateUI()
    {
        currentWeaponIndex = equipmentManager.currentWeaponIndex;
        currentWeapon = inventory.GetWeapon(currentWeaponIndex);
        currentAmmo = inventory.SearchAmmo(currentWeapon.ammoType);

        gun.text = currentWeapon.name.ToString();

        if (currentAmmo == null)
        {
            ammo.text = currentWeapon.currentAmmo.ToString() + " / " + "0";
        }
        else
        {
            ammo.text = currentWeapon.currentAmmo.ToString() + " / " + currentAmmo.stack * currentAmmo.restorePoint;
        }
    }
    
    public void RecoilMath()
    {
        originalVerticalValue = pov.m_VerticalAxis.Value;

        currentRecoilX = ((Random.value - 0.5f) / 2) * recoilX;
        currentRecoilY = ((Random.value - 0.5f) / 2) * (timePressed >= maxRecoilTime ? recoilY / 4 : recoilY);

        float smoothedRecoilY = Mathf.SmoothDamp(0f, Mathf.Abs(currentRecoilY), ref smoothRecoilVelY, smoothTime);
        float smoothedRecoilX = Mathf.SmoothDamp(0f, currentRecoilX, ref smoothRecoilVelX, smoothTime);

        pov.m_VerticalAxis.Value -= smoothedRecoilY;
        pov.m_HorizontalAxis.Value -= smoothedRecoilX;
    }

    IEnumerator GunRecoil()
    {
        Vector3 initPos = weaponHolder.transform.localPosition;
        Vector3 targetPos = new Vector3(
            initPos.x,
            initPos.y,
            initPos.z - 0.05f
            );

        float lerpPos = Mathf.Lerp(weaponHolder.transform.localPosition.z, targetPos.z, 1);

        weaponHolder.transform.localPosition = new Vector3(weaponHolder.transform.position.x, weaponHolder.transform.localPosition.y, lerpPos);

        yield return null;
    }

    bool IsClipped()
    {
        if (Physics.Raycast(point.transform.position, point.transform.forward, 1f))
            return true;
        else
            return false;
    }

    private bool CanShoot() => !currentWeapon.reloading && currentWeapon.currentAmmo > 0 && !IsClipped() && Cursor.lockState == CursorLockMode.Locked && Time.timeScale == 1;

    public void Shoot()
    {
        if (CanShoot() || melee)
        {
            if(melee == false)
            {
                _audio.Play();
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit, currentWeapon.range))
            {
                if(currentWeapon.name != "Hand")
                    Instantiate(holePrefab, hit.point + (hit.normal * 0.001f), Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);

                if (hit.transform.gameObject.GetComponent<Enemy>() != null)
                {
                    hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(28, hit.point);
                    //Hitactive();
                    //Invoke(nameof(Hitdisable), 0.2f);
                }
            }

            if (!melee)
            {
                RecoilMath();
                StopCoroutine(GunRecoil());
                StartCoroutine(GunRecoil());
                currentWeapon.currentAmmo--;
                if (ammo != null)
                    UpdateUI();
            }
            
        }

    }

    //private bool CanReload() => currentAmmo.stack > 0 && currentWeapon.currentAmmo < currentWeapon.magSize && !currentWeapon.reloading;

    private bool CanReload()
    {
        if (currentAmmo != null)
            return currentAmmo.stack > 0 && currentWeapon.currentAmmo < currentWeapon.magSize && !currentWeapon.reloading;
        else
        {
            return false;
        }
    }

    IEnumerator Reload()
    {
        if(CanReload())
        {
            currentWeapon.reloading = true;

            _audio.clip = reload;
            _audio.Play();

            if (ammo != null)
                ammo.text = "Reloading";

            yield return new WaitForSeconds(currentWeapon.reloadTime);

            //TODO: Make a function in WaitForSeconds that checks if gun is swapped (If true: cancel reload)
            
            currentWeapon.currentAmmo = currentAmmo.restorePoint;
            currentAmmo.stack -= 1;
            if(currentAmmo.stack <= 0)
                inventory.RemoveItem(currentAmmo);

            UpdateUI();
            InitWeapon();
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
        _audio.clip = shoot;

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
    //private void Hitactive()
    //{
    //    hitmarker.SetActive(true);
    //}
    //private void Hitdisable()
    //{
    //    hitmarker.SetActive(false);
    //}
}
