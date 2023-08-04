using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float turnSpeed = 2f;
    public float changeDirectionTime = 3f;

    private float timer = 0f;
    private Vector3 targetDirection;

    void Start()
    {
        targetDirection = GetRandomDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > changeDirectionTime)
        {
            targetDirection = GetRandomDirection();
            timer = 0f;
        }

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), turnSpeed * Time.deltaTime);
    }

    Vector3 GetRandomDirection()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        return new Vector3(x, z).normalized;
    }

}
