using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HeroProfileConfigMap : ScriptableObject
{
    #region Data
    public List<KeyValue> list;
    

    [System.Serializable]
    public class KeyValue
    {
        public string id;
        public HeroConfig heroConfig;       
    }
    #endregion//Data

    #region Public - get data
    Dictionary<string, HeroConfig> _fromListTomap
        = new Dictionary<string, HeroConfig>();
    Dictionary<string, HeroConfig> FromListToMap
    {
        get
        {
            // not convert from list to map yet
            if (_fromListTomap.Count == 0)
            {
                for (int n = 0; n < list.Count; n++)
                {
                    var item = list[n];
                    _fromListTomap.Add(item.id, item.heroConfig);
                }
            }
            return _fromListTomap;
        }
    }

    public HeroConfig GetValueFromKey(string id)
    {
        var result = FromListToMap[id];
        // validate data
        if (result == null)
        {
            Debug.LogError($"Not add item for ID [{id}] yet");
            return null;
        }
        return result;
    }

    #endregion //Public - get data

}

[System.Serializable]
public class HeroConfig
{
    [Header("Stats")]
    [SerializeField] public HeroStats HeroStats;
        
    [Header("Axie")]
    [SerializeField] public AxieProfile axieProfile;
}