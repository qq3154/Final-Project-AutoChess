using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] public AudioSource bgSource;
    [SerializeField] public AudioSource sfxSource;

    [SerializeField] public bool isBgMute = false;
    [SerializeField] public bool isSFXMute = false;
    [SerializeField] public float bgVolume;
    [SerializeField] public float sfxVolume;

    protected override void DoOnStart()
    {
        bgSource.mute = isBgMute;
        sfxSource.mute = isSFXMute;
        bgSource.volume = bgVolume;
        sfxSource.volume = sfxVolume;
    }
    
   
    
}
