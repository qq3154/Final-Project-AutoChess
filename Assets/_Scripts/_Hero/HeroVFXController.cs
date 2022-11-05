using UnityEngine;

public class HeroVFXController : MonoBehaviour
{
    [SerializeField] private GameObject SelectVFX;
    
    public void SetSelectVFXEnable(bool isEnable)
    {
        SelectVFX.SetActive(isEnable);
    }
}
