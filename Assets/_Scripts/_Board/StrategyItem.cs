using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StrategyItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _count;
    [SerializeField] private GameObject _tooltip;
    [SerializeField] private TMP_Text _descriptionTxt;
    
    [Header("Ref")]
    [SerializeField] private ClassesIconConfig _classesIconConfig;

    public void SetStrategy(string name, int count)
    {
        _name.text = name;
        _count.text = count.ToString();
        _icon.sprite = _classesIconConfig.GetIconById(name);
        _tooltip.SetActive(false);
        switch (name)
        {
            case "Dragon":
                _descriptionTxt.text = "+25 Attack Damage for each Dragon in team";
                break;
            case "Elf":
                _descriptionTxt.text = "+50 Ultimate Damage for each Elf in team";
                break;
            case "Beast":
                _descriptionTxt.text = "+0.3 Attack Speed for each Beast in team";
                break;
            case "Naga":
                _descriptionTxt.text = "+70 Health for each Naga in team";
                break;
            case "Warrior":
                _descriptionTxt.text = "+50 Health for each Warrior in team";
                break;
            case "Knight":
                _descriptionTxt.text = "+5% Health for each Knight in team";
                break;
            case "Mage":
                _descriptionTxt.text = "-10 Max Mana for each Mage in team";
                break;
            case "Hunter":
                _descriptionTxt.text = "+20 Attack Damage for each Hunter in team";
                break;
            default: 
                Debug.LogWarning("Not found class " + name);
                break;
        }
       
    }


    public void OnOpenTooltip()
    {
        _tooltip.SetActive(true);
    }


    public void OnCloseTooltip()
    {
        _tooltip.SetActive(false);
    }
}
