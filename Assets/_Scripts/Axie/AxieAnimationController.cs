using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class AxieAnimationController : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    

    public void SetRun(float duration)
    {
        _skeletonAnimation.state.SetAnimation(0, "action/run", false).TimeScale = duration * 1.5f;
    }
}
