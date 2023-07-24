using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CheckLOS : MonoBehaviour
{
    //fyi, LOS stands for "Line Of Sight"

    SphereCollider m_SphereCollider;
    float m_Fov = 90f;
    LayerMask m_LOSLayer;

    public delegate void GainSightEvent(Transform target);
    public event GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(Transform target);
    public event LoseSightEvent OnLoseSight;

    Coroutine m_CheckLOSCoroutine;

    private void Awake()
    {
        m_SphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckLOSBool(other.transform))
        {

        }
    }

    private bool CheckLOSBool(Transform target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, direction);
        if (dotProduct > Mathf.Cos(m_Fov)) {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, m_SphereCollider.radius, m_LOSLayer))
            {
                OnGainSight?.Invoke(target);
                return true;
            }
        }
        return false;
    }
}
