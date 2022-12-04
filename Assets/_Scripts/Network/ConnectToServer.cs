using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect to Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    { 
        //SceneManager.LoadScene("Lobby");
        Debug.Log("Join lobby");
        GameFlowManager.instance.isConnect = true;
    }
    
    
    // // Start is called before the first frame update
    // void Start()
    // {
    //     //PhotonNetwork.ConnectUsingSettings();
    // }
    //
    // public void JoinRandomRoom()
    // {
    //     PhotonNetwork.JoinRandomRoom();
    // }
    //
    //
    // #region Photon Callbacks
    // public override void OnConnected()
    // {
    //     base.OnConnected();
    //     Debug.Log("connect to server successful");
    // }
    //
    // public override void OnJoinRandomFailed(short returnCode, string message)
    // {
    //     base.OnJoinRandomFailed(returnCode, message);
    //     Debug.Log(message);
    //     CreateAndJoinRoom();
    // }
    //
    //
    //
    // public override void OnPlayerEnteredRoom(Player newPlayer)
    // {
    //     base.OnPlayerEnteredRoom(newPlayer);
    //     Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
    // }
    //
    // #endregion
    //
    //
    // #region Private
    // private void CreateAndJoinRoom()
    // {
    //     string roomName = UserManager.instance.id;
    //     
    //     RoomOptions roomOptions = new RoomOptions();
    //     roomOptions.IsOpen = true;
    //     roomOptions.IsVisible = true;
    //     roomOptions.MaxPlayers = 2;
    //     
    //     
    //     PhotonNetwork.CreateRoom(roomName, roomOptions);
    // }
    //     
    // #endregion
    
}
