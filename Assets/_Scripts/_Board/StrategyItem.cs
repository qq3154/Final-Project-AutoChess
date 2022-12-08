using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StrategyItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _count;
    
    [Header("Ref")]
    [SerializeField] private ClassesIconConfig _classesIconConfig;

    public void SetStrategy(string name, int count)
    {
        _name.text = name;
        _count.text = count.ToString();
        _icon.sprite = _classesIconConfig.GetIconById(name);
    }
}
