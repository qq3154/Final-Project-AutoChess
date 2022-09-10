using System.Collections;
using System.Collections.Generic;
using Observer;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.PostEvent(EventID.OnGamePlayStart);
        
        //Play intro or something then start wave
        
        
        
        this.PostEvent(EventID.OnWaveStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
