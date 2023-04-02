using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class RewardPanel : MonoBehaviour
{
    public TMP_Text _nameWinner;
    public TMP_Text _coinsCountWinner;
    public void LeaveArena()
    {
        Time.timeScale =1f;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }
}
