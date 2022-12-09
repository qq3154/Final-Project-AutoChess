using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HistoryMenuItem : MonoBehaviour
{
    [SerializeField] private GameObject _winIcon;
    [SerializeField] private GameObject _loseIcon;
    [SerializeField] private TMP_Text _usernameTxt;
    [SerializeField] private TMP_Text _roundTxt;
    [SerializeField] private TMP_Text _dateTxt;
    [SerializeField] private TMP_Text _rewardTxt;
    



    public void InitData(bool isWin, string username, int round, string date)
    {
        string myDate = date.Substring(0, date.Length-5).Replace('T', ' ');
        
        _winIcon.SetActive(isWin);
        _loseIcon.SetActive(!isWin);
        _usernameTxt.text = username;
        _roundTxt.text = "Round: " + round.ToString();
        _dateTxt.text = myDate;
        _rewardTxt.text = isWin ? "200" : "0";

    }
}
