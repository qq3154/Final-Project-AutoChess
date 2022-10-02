using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class AxieAnimationController : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _skeletonAnimation;


    public void Set(SkeletonAnimation skeletonAnimation)
    {
        _skeletonAnimation = skeletonAnimation;
    }
    

    public void SetRun(float duration)
    {
        _skeletonAnimation.state.SetAnimation(0, "action/move-forward", false).TimeScale = duration * 1.5f;
    }
}
