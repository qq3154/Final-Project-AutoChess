using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelTxt;
    [SerializeField] private Image _levelIcon;
    [SerializeField] private List<Sprite> _icons; 

    public void SetLevel(int level)
    {
        _levelTxt.text = level.ToString();
        _levelIcon.sprite = _icons[level - 1];
    }
}
