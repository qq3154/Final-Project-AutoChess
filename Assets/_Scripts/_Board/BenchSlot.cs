using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchSlot : MonoBehaviour
{
    [SerializeField] private TeamID _teamID;
    [SerializeField] private Hero _hero;

    public bool isUse;


    public void SetHero(Hero hero)
    {
        _hero = hero;
    }

    public Hero GetHero()
    {
        return _hero;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
