using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipPrevention : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] GameObject clipProjector;
    [SerializeField] float checkDistance;
    [SerializeField] Vector3 newDirection;

    float lerpPos;
    RaycastHit hit;
    
    private void Update()
    {
        Debug.DrawRay(clipProjector.transform.position, clipProjector.transform.forward, Color.green);
        if (Physics.Raycast(clipProjector.transform.position, clipProjector.transform.forward, out hit, checkDistance, 5))
        {
            lerpPos = 1 - (hit.distance / checkDistance);
        }
        else
        {
            lerpPos = 0;
        }
        Mathf.Clamp01(lerpPos);
        
        transform.localRotation = Quaternion.Lerp(
            Quaternion.Euler(Vector3.zero),
            Quaternion.Euler(newDirection),
            lerpPos);
    }
}
