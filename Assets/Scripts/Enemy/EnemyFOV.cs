using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    [SerializeField] public float _radius;
    [SerializeField] [Range(0, 360)] public float _angle;
    [SerializeField] LayerMask _playerMask;
    public bool playerOnSight = false;

    public void StartFOV(LayerMask pMask)
    {
        StartCoroutine(FOVRoutine());
    }

    public IEnumerator FOVRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.2f);
        while(true)
        {
            yield return delay;
            FOVCheck();
        }

    }

    public void FOVCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _playerMask);

        if (rangeChecks.Length > 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < _angle / 2)
            {
                if (Physics.Raycast(transform.position, dirToTarget, out RaycastHit hit))
                {
                    if ((_playerMask.value & (1 << hit.transform.gameObject.layer)) != 0)
                        playerOnSight = true;
                    else
                        playerOnSight = false;
                }

            }
            else
                playerOnSight = false;
        }
        else if (playerOnSight)
            playerOnSight = false;
    }
}
