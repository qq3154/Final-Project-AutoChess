using AxieMixer.Unity;
using Spine;
using Spine.Unity;
using UnityEngine;


public class AxieFigureController : MonoBehaviour
{
    [SerializeField] private bool _flipX;
    [SerializeField] private Vector3 _localScale;
    [SerializeField] private Vector3 _position;
    
    private SkeletonAnimation skeletonAnimation;
    
    // public bool flipX
    // {
    //     get
    //     {
    //         return _flipX;
    //     }
    //     set
    //     {
    //         _flipX = value;
    //         if (skeletonAnimation != null)
    //         {
    //             skeletonAnimation.skeleton.ScaleX = (_flipX ? -1 : 1) * Mathf.Abs(skeletonAnimation.skeleton.ScaleX);
    //         }
    //     }
    // }

    private void Awake()
    {
        skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
    }

    public void SetGenes(string id, string genes)
    {
        if (string.IsNullOrEmpty(genes)) return;

        
        Mixer.SpawnSkeletonAnimation(skeletonAnimation, id, genes);

        skeletonAnimation.transform.localPosition = _position;
        skeletonAnimation.transform.SetParent(transform, false);
        skeletonAnimation.transform.localScale = _localScale;
        skeletonAnimation.skeleton.ScaleX = (_flipX ? -1 : 1) * Mathf.Abs(skeletonAnimation.skeleton.ScaleX);
        skeletonAnimation.timeScale = 0.5f;
        skeletonAnimation.skeleton.FindSlot("shadow").Attachment = null;
        skeletonAnimation.state.SetAnimation(0, "action/idle/normal", true);
      
    }

    

   
}

