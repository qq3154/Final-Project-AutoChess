using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelTxt;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevel(int level)
    {
        _levelTxt.text = level.ToString();
    }
}
