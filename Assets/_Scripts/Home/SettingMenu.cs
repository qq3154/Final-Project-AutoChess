using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private Slider _bgVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    [SerializeField] private Image _bgVolumeIcon;
    [SerializeField] private Image _sfxVolumeIcon;
    
    [SerializeField] private Image _bgVolumeHandle;
    [SerializeField] private Image _sfxVolumeHandle;

    [SerializeField] private Sprite _unmuteIcon;
    [SerializeField] private Sprite _muteIcon;
    
    [SerializeField] private Sprite _handleOnIcon;
    [SerializeField] private Sprite _handleOffIcon;

    // Start is called before the first frame update
    void Start()
    {
        if (SoundManager.instance.isBgMute)
        {
            _bgVolumeIcon.sprite = _muteIcon;
            _bgVolumeHandle.sprite = _handleOffIcon;
            _bgVolumeSlider.value = 0;
        }
        else
        {
            _bgVolumeIcon.sprite = _unmuteIcon;
            _bgVolumeHandle.sprite = _handleOnIcon;
        }
        
        
        if (SoundManager.instance.isSFXMute)
        {
            _sfxVolumeIcon.sprite = _muteIcon;
            _sfxVolumeHandle.sprite = _handleOffIcon;
            _sfxVolumeSlider.value = 0;
        }
        else
        {
            _sfxVolumeIcon.sprite = _unmuteIcon;
            _sfxVolumeHandle.sprite = _handleOnIcon;
        }

        _bgVolumeSlider.value = SoundManager.instance.bgVolume;
        _sfxVolumeSlider.value = SoundManager.instance.sfxVolume;
    }

    private void Update()
    {
        SoundManager.instance.bgVolume = _bgVolumeSlider.value;
        SoundManager.instance.bgSource.volume = _bgVolumeSlider.value;
        if (_bgVolumeSlider.value == 0)
        {
            _bgVolumeIcon.sprite = _muteIcon;
            _bgVolumeHandle.sprite = _handleOffIcon;
        }
        else
        {
            
            _bgVolumeIcon.sprite = _unmuteIcon;
            _bgVolumeHandle.sprite = _handleOnIcon;
        }
        
        SoundManager.instance.sfxVolume = _sfxVolumeSlider.value;
        SoundManager.instance.sfxSource.volume = _sfxVolumeSlider.value;
        if (_sfxVolumeSlider.value == 0)
        {
            _sfxVolumeIcon.sprite = _muteIcon;
            _sfxVolumeHandle.sprite = _handleOffIcon;
        }
        else
        {
            
            _sfxVolumeIcon.sprite = _unmuteIcon;
            _sfxVolumeHandle.sprite = _handleOnIcon;
        }
    }
}
