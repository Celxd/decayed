using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hit : MonoBehaviour
{
    [SerializeField]
    private float _maximumForce;

    [SerializeField]
    private float _maximumForceTime;

    private float _timeMouseButtonDown;

    private Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _timeMouseButtonDown = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Bandit_1 bandit_1 = hitInfo.collider.GetComponentInParent<Bandit_1>();

                if (bandit_1 != null)
                {
                    float mouseButtonDownDuration = Time.time - _timeMouseButtonDown;
                    float forcePercentage = mouseButtonDownDuration / _maximumForceTime;
                    float forceMagnitude = Mathf.Lerp(1, _maximumForce, forcePercentage);

                    Vector3 forceDirection = bandit_1.transform.position - _camera.transform.position;
                    forceDirection.y = 1;
                    forceDirection.Normalize();

                    Vector3 force = forceMagnitude * forceDirection;

                    bandit_1.TriggerRagdoll(force, hitInfo.point);
                }
            }
        }
    }
}
