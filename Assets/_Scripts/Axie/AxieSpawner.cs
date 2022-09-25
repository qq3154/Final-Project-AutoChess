using System;
using System.Collections;
using AxieMixer.Unity;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class AxieSpawner : MonoBehaviour
{
    [SerializeField] AxieFigureController axieFigureController;

    [SerializeField] private HeroID _heroID;
    [SerializeField] private string axieId;
    
    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;

    //bool _isPlaying = false;
    //bool _isFetchingGenes = false;
    

    // Start is called before the first frame update
    void Start()
    {
        Mixer.Init();
        OnMixButtonClicked();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            OnMixButtonClicked();
        }
    }

    public void Init(HeroID heroID)
    {
        this._heroID = heroID;
        axieId = _heroProfileConfigMap.GetValueFromKey(heroID).AxieId;

        OnMixButtonClicked();
    }

    void OnMixButtonClicked()
    {
        //_isFetchingGenes = true;
        StartCoroutine(GetAxiesGenes(axieId));
    }
    

    public IEnumerator GetAxiesGenes(string axieId)
    {
        string searchString = "{ axie (axieId: \"" + axieId + "\") { id, genes, newGenes}}";
        JObject jPayload = new JObject();
        jPayload.Add(new JProperty("query", searchString));

        var wr = new UnityWebRequest("https://graphql-gateway.axieinfinity.com/graphql", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jPayload.ToString().ToCharArray());
        wr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        wr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        wr.SetRequestHeader("Content-Type", "application/json");
        wr.timeout = 10;
        yield return wr.SendWebRequest();
        if (wr.error == null)
        {
            var result = wr.downloadHandler != null ? wr.downloadHandler.text : null;
            if (!string.IsNullOrEmpty(result))
            {
                JObject jResult = JObject.Parse(result);
                string genesStr = (string)jResult["data"]["axie"]["newGenes"];
                PlayerPrefs.SetString("selectingId", axieId);
                PlayerPrefs.SetString("selectingGenes", genesStr);
             
                axieFigureController.SetGenes(axieId, genesStr);
            }
        }
        //_isFetchingGenes = false;
    }
}
