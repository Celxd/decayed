using UnityEngine;
using System.Collections;

public class PortalTeleport : MonoBehaviour
{
    public Transform destinationPortal; 

    private bool isTeleporting = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (!isTeleporting && other.CompareTag("Player"))
        {
            isTeleporting = true;

            Vector3 destinationPosition = destinationPortal.position;
            Quaternion destinationRotation = destinationPortal.rotation; 
            other.transform.position = destinationPosition;
            other.transform.rotation = destinationRotation; 

            StartCoroutine(ResetTeleportFlag());
        }
    }

    private IEnumerator ResetTeleportFlag()
    {
        yield return new WaitForSeconds(0.5f);
        isTeleporting = false;
    }
}
