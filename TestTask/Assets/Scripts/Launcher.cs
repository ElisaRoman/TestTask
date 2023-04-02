using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public Launcher instance;

    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_InputField _roomNameFindInputField;
    [SerializeField] private TMP_Text _errorText;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private Transform _playerList;
    [SerializeField] private GameObject _playerNamePrefab;

    private void Start()
    {
        instance=this;
    }
    public void CreateRoom()//создать комнату !
    {
        if(string.IsNullOrEmpty(_roomNameInputField.text))
        {
            return;
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers =4;
        PhotonNetwork.CreateRoom(_roomNameInputField.text,roomOptions);
    }
    public void JoinRoom()
    {
        if(string.IsNullOrEmpty(_roomNameFindInputField.text))
        {
            return;
        }
        PhotonNetwork.JoinRoom(_roomNameFindInputField.text); 
    }
    public override void OnJoinedRoom()//при успешном подсоединении к комнате !
    {
        string randomName = "Player "+ Random.Range(0,1000).ToString("0000");
        PhotonNetwork.NickName=randomName;
        PhotonNetwork.LoadLevel("Game");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)//при неуспешном подсоединении к комнате
    {
        _errorText.text="Error: "+ message;
        MenuManager.instance.OpenMenu("error");
    }
    public override void OnJoinRoomFailed (short returnCode, string message)//при неуспешном подсоединении к комнате
    {
        _errorText.text="Error: "+ message;
        MenuManager.instance.OpenMenu("error");
    }
    public void ExitTheGame()
    {
        Application.Quit();
    }
}
