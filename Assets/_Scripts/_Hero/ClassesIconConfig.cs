using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ClassesIconConfig : ScriptableObject
{
    [SerializeField] private List<Icon> icons;
    
    public Sprite GetIconById(string id)
    {
        foreach (var icon in icons)
        {
            if (icon.id == id)
            {
                return icon.sprite;
            }
        }

        return null;
    }


    [Serializable]
    struct Icon
    {
        public string id;
        public Sprite sprite;
    }
}
