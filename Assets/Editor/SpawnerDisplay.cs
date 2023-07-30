using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class SpawnerDisplay : Editor
{
    private void OnSceneGUI()
    {
        EnemySpawner spawn = (EnemySpawner)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(spawn.transform.position, Vector3.up, Vector3.forward, 360, spawn.m_checkRadius);
    }
}
