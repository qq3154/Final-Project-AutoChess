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
    
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    
    Axie2dBuilder builder => Mixer.Builder;
    
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
        skeletonAnimation.gameObject.layer = LayerMask.NameToLayer("Board");
      
    }

    public void SetGenes()
    {
        var (key, body, classIdx, classValue) = ("beast-04", "body-bigyak", 0, 4);
        
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
       
        
        mySkeletonAnimation.name = "b";
        mySkeletonAnimation.transform.localPosition = _position;
        mySkeletonAnimation.transform.SetParent(transform, false);
        mySkeletonAnimation.transform.localScale = _localScale;
        mySkeletonAnimation.timeScale = 0.5f;
        mySkeletonAnimation.skeleton.FindSlot("shadow").Attachment = null;
        mySkeletonAnimation.state.SetAnimation(0, "action/idle/normal", true);
        mySkeletonAnimation.skeleton.ScaleX = (_flipX ? -1 : 1) * Mathf.Abs(mySkeletonAnimation.skeleton.ScaleX);
        
        mySkeletonAnimation.GetComponent<MeshRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        mySkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = 2;
        
    }

    

   
}

