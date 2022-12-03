using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject _matchesViewHolder;
    [SerializeField] private HistoryMenuItem _historyMenuItemPref;
    [SerializeField] private TMP_InputField _userFullname;
    [SerializeField] private TMP_Text _totalMatchTxt;
    [SerializeField] private TMP_Text _winMatchTxt;

    private int _totalMatch;
    private int _winMatch;
    
    // Start is called before the first frame update
    void Start()
    {
        _userFullname.text = UserManager.instance.fullName;
        SendGetMatchesHistory();
    }
    
    private async void SendGetMatchesHistory(){
        
        var response = await ApiRequest.instance.SendGetMatchesRequest();
        
        if (response.success)
        {
            _totalMatch = response.matches.Count;
            foreach (var match in response.matches)
            {
                bool isWin = match.winner._id == UserManager.instance.id;
                _winMatch += isWin ? 1 : 0;
                
                string opponentName = isWin ?  match.loser.fullName : match.winner.fullName;
                var matchItem = Instantiate(_historyMenuItemPref, _matchesViewHolder.transform);
                
                matchItem.InitData(isWin, opponentName, match.round, match.createAt);
                matchItem.gameObject.SetActive(true);
            }

            _totalMatchTxt.text = _totalMatch.ToString();
            _winMatchTxt.text = _winMatch.ToString();
            
        }
        else
        {
            Debug.Log(response);
        }
    }

    
}
