using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelTxt;
    

    public void SetLevel(int level)
    {
        _levelTxt.text = level.ToString();
    }
}
