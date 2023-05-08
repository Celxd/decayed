using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GunData gunData;
    [SerializeField] public GameObject player;
    [SerializeField] GameObject point;
    Transform camPos;
    
    [Header("Settings")]
    [Range(0, 3f)] float recoilX;
    [Range(0, 7f)] float recoilY;
    [Range(0, 10f)] float maxRecoilTime;
    
    float currentRecoilX;
    float currentRecoilY;

    float timePressed;

    InputAction action_shoot;
    InputAction action_reload;
    InputAction mouse;

    Coroutine fireCoroutine;

    // Start is called before the first frame update
    private void Start()
    {
        action_shoot = player.GetComponent<PlayerInput>().actions["Shoot"];
        action_reload = player.GetComponent<PlayerInput>().actions["Reload"];
        mouse = player.GetComponent<PlayerInput>().actions["Look"];

        camPos = Camera.main.transform;

        action_shoot.started += ctx => StartFiring();
        action_shoot.canceled += ctx => StopFiring();

        action_reload.started += ctx => StartCoroutine(Reload());

        gunData.currentAmmo = gunData.magSize;
    }

    public void RecoilMath()
    {
        currentRecoilX = ((Random.value - 0.5f) / 2) * recoilX;
        currentRecoilY = ((Random.value - 0.5f) / 2) * (timePressed >= maxRecoilTime ? recoilY / 4 : recoilY);
        //camPos.Rotate(new Vector3(camPos.eulerAngles.x - Mathf.Abs(currentRecoilY), camPos.eulerAngles.y - currentRecoilX, 0));
        camPos.localEulerAngles = new Vector3(camPos.localEulerAngles.x - Mathf.Abs(currentRecoilY), camPos.localEulerAngles.y - currentRecoilX, 0);

    }

    bool IsClipped()
    {
        if(Physics.Raycast(point.transform.position, point.transform.forward, 1f))
            return true;
        else
            return false;
    }

    private bool CanShoot() => !gunData.reloading && gunData.currentAmmo > 0 && !IsClipped();

    public void Shoot()
    {
        if(CanShoot())
        {
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0)), out RaycastHit hit))
            {
                Debug.Log(hit.transform.name);
                
            }
            Debug.DrawRay(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)), Camera.main.transform.forward * gunData.range, Color.red, gunData.range);

            RecoilMath();
            gunData.currentAmmo--;
            OnGunShot();
        }
        
    }

    IEnumerator Reload()
    {
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }
    
    public void OnGunShot()
    {
        //Executes after Shoot(), take hit as parameter and damage the hit.
        //Start damage decrease when hit object is more than gunData.range
    }

    public IEnumerator RapidFire()
    {
        while(true)
        {
            Shoot();
            yield return new WaitForSeconds(1/ (gunData.fireRate / 60));
        }
    }

    IEnumerator PressedTime()
    {
        while(true)
        {
            timePressed += Time.deltaTime;
            timePressed = timePressed >= maxRecoilTime ? maxRecoilTime : timePressed;
            yield return null;
        }
    }

    void StartFiring()
    {
        fireCoroutine = StartCoroutine(RapidFire());
        StartCoroutine(PressedTime());
    }
    
    void StopFiring()
    {
        if (fireCoroutine != null)
            StopCoroutine(fireCoroutine);

        timePressed = 0;
    }
}
