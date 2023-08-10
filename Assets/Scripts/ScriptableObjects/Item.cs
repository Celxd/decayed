using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Info")]
    //[SerializeField] string id;
    //public string ID { get { return id; } }
    new public string name;
    public Type type;
    public Sprite icon;

    public enum Type { Weapon, Consumables }

    protected virtual void OnValidate()
    {
        //string path = AssetDatabase.GetAssetPath(this);
        //id = AssetDatabase.GetAssetPath(this);
    }
}
