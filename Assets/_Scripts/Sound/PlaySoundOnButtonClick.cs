using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnButtonClick : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private Button _btn;

    private void Start()
    {
        _btn.onClick.AddListener(PlaySoundOneShot);
    }

    void PlaySoundOneShot()
    {
        AudioSource source = SoundManager.instance.sfxSource;
        if (source.isPlaying)
        {
            source.Stop();
            
        }
        source.PlayOneShot(_audioClip);
    }
}
