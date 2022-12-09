using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Resign : MonoBehaviour
{
   public void ResignMatch()
   {
      TeamID winTeamID = (GameFlowManager.instance.playerTeam == TeamID.Blue) ? TeamID.Red : TeamID.Blue;
            
      string winner = (winTeamID == TeamID.Blue) ? MatchManager.instance.userBlue : MatchManager.instance.userRed;
      string loser = (winTeamID == TeamID.Red) ? MatchManager.instance.userBlue : MatchManager.instance.userRed;
      SendMatchRequest(winner, loser,  GameFlowManager.instance.round);
      GameFlowManager.instance.round = 0;
     
      object[] content = new object[] {winTeamID, GameFlowManager.instance.round};
      RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
      PhotonNetwork.RaiseEvent(PhotonEvent.OnMatchEnd, content, raiseEventOptions, SendOptions.SendReliable);

      
      
   }
   
   private async void SendMatchRequest(string winner, string loser, int round)
   {
      var response = await ApiRequest.instance.SendCreateMatchRequest(winner, loser, round);
        
      if (response.success)
      {
         Debug.Log(response);

      }
      else
      {
         Debug.Log(response);
      }
   }
}
