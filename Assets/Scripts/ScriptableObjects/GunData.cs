using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class GunData : ScriptableObject
{
    [Header("Gun Info")]
    public string gunName;

    [Header("Gun Stats")]
    public float fireRate;
    public float range;
    public int magSize;
    public float reloadTime;

    //[HideInInspector]
    public float currentAmmo;

    [Header("Gun Effects")]
    public GameObject gunModel;

    [Header("Audio Effects")]
    public AudioClip Shooting;
    public AudioClip Reloading;

    //[HideInInspector]
    public bool reloading;
}
