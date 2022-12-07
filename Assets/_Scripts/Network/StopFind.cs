using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class StopFind : MonoBehaviour
{
    public void StopFindMatch()
    {
        PhotonNetwork.LeaveRoom();
    }
}
