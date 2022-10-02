using System.Collections.Generic;
using AxieCore.AxieMixer;
using AxieMixer.Unity;
using Spine;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;


public class AxieFigureController : MonoBehaviour
{
    [SerializeField] private bool _flipX;
    [SerializeField] private Vector3 _localScale;
    [SerializeField] private Vector3 _position;
    [SerializeField] private string _layerName;
    [SerializeField] private int _layerOrder;
    
    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    
    Axie2dBuilder builder => Mixer.Builder;
    
    public void SetGenes(AxieProfile axieProfile)
    {
        // var (key, body, classIdx, classValue) = ("beast-04", "body-bigyak", 0, 4);

        var key = axieProfile.key;
        var body = axieProfile.body;
        var classIdx = axieProfile.classIdx;
        var classValue = axieProfile.classValue;
        
        
        var characterClass = (CharacterClass)classIdx;
        string finalBody = body;
        string keyAdjust = key.Replace("-06", "-02").Replace("-12", "-04");
        var adultCombo = new Dictionary<string, string> {
            {"back", key },
            {"body", finalBody },
            {"ears", key },
            {"ear", key },
            {"eyes", keyAdjust },
            {"horn", key },
            {"mouth", keyAdjust },
            {"tail", key },
            {"body-class", characterClass.ToString() },
            {"body-id", " 2727 " },
        };
        
        float scale = 0.0018f;
        byte colorVariant = (byte)builder.GetSampleColorVariant(characterClass, classValue);
        
        var builderResult = builder.BuildSpineAdultCombo(adultCombo, colorVariant, scale, true);
        
        var mySkeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(builderResult.skeletonDataAsset);
       
        
        mySkeletonAnimation.transform.localPosition = _position;
        mySkeletonAnimation.transform.SetParent(transform, false);
        mySkeletonAnimation.transform.localScale = _localScale;
        mySkeletonAnimation.timeScale = 0.5f;
        mySkeletonAnimation.skeleton.FindSlot("shadow").Attachment = null;
        mySkeletonAnimation.state.SetAnimation(0, "action/idle/normal", true);
        mySkeletonAnimation.skeleton.ScaleX = (_flipX ? -1 : 1) * Mathf.Abs(mySkeletonAnimation.skeleton.ScaleX);
        
        mySkeletonAnimation.GetComponent<MeshRenderer>().sortingLayerID = SortingLayer.NameToID(_layerName);
        mySkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = _layerOrder;

        _skeletonAnimation = mySkeletonAnimation.GetComponent<SkeletonAnimation>();


    }

    
    public void SetRun(float duration)
    {
        _skeletonAnimation.state.SetAnimation(0, "action/move-forward", false).TimeScale = duration * 1.5f;
    }
    

   
}

