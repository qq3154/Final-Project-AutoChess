using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HomeMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _userFullnameTxt;
    [SerializeField] private TMP_Text _goldTxt;
    
    // Start is called before the first frame update
    void Start()
    {
        // _userFullnameTxt.text = UserManager.instance.fullName;
        // _goldTxt.text = UserManager.instance.gold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _userFullnameTxt.text = UserManager.instance.fullName;
        _goldTxt.text = UserManager.instance.gold.ToString();
    }
}
