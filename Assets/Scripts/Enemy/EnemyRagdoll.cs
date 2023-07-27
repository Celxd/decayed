using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    Rigidbody[] _ragdollRigidbodies;
    Animator _animator;
    CharacterController _characterController;

    void Awake()
    {
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        DisableRagdoll();
    }

    private void DisableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        _animator.enabled = true;
        _characterController.enabled = true;
    }

    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    {
        EnableRagdoll();

        Rigidbody hitRigidbody = _ragdollRigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPoint)).First();

        hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    private void EnableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        _animator.enabled = false;
        _characterController.enabled = false;
    }

    public void RagdollBehavior()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
            rigidbody.isKinematic = true;
    }
}
