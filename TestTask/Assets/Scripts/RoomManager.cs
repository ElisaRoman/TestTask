using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }
}
