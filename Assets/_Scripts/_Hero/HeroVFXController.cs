using System.Collections;
using UnityEngine;

public class HeroVFXController : MonoBehaviour
{
    [SerializeField] private GameObject SelectVFX;
    [SerializeField] private GameObject LevelupVFX;
    
    public void SetSelectVFXEnable(bool isEnable)
    {
        SelectVFX.SetActive(isEnable);
    }

    public void PlayLevelUpVFX()
    {
        StartCoroutine(IE_PlayLevelUpVFX());
    }

    IEnumerator IE_PlayLevelUpVFX()
    {
        LevelupVFX.SetActive(true);
        yield return new WaitForSeconds(1);
        LevelupVFX.SetActive(false);
    }
}
