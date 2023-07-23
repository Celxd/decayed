using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]
public class EnemyFOVDisplay : Editor
{
    private void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov._radius);

        Vector3 viewAngle1 = AngleDirection(fov.transform.eulerAngles.y, -fov._angle / 2);
        Vector3 viewAngle2 = AngleDirection(fov.transform.eulerAngles.y, fov._angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle1 * fov._radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle2 * fov._radius);

        if (fov.playerOnSight)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.transform.forward);
            Debug.Log("Player seen");
        }
    }

    private Vector3 AngleDirection(float eulerY, float angleInDegrees) {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
