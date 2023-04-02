using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviourPunCallbacks
{
    public override void OnJoinedRoom()//при успешном подсоединении к комнате
    {
        CheckPlayerCount();
    }
    public override void OnPlayerEnteredRoom(Player player)
    {
        CheckPlayerCount();
    }
    void CheckPlayerCount()
    {
        Player[] players = PhotonNetwork.PlayerList;
        if(players.Length>1)
        {
            PhotonNetwork.LoadLevel(2);
        }
    }
}
