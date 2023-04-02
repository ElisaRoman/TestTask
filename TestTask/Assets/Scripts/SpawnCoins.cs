using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class SpawnCoins : MonoBehaviour
{
    public GameObject coinObject;
    public float minX, minY, maxX, maxY;
    private float timeSpawn=2.2f;
    private bool once=false;
    void Update()
    {
        Player[] players = PhotonNetwork.PlayerList;
        if(players.Length<=1)
        {
            CancelInvoke();
        }
        else if (!once)
        {
            InvokeRepeating("SpawnCoin", 0f, timeSpawn); 
            once=true; 
        }
    }
    void SpawnCoin()
    {
        Vector2 randomPosition = new Vector2 (Random.Range(minX,maxX),Random.Range(minY,maxY));
        PhotonNetwork.Instantiate(coinObject.name,randomPosition, Quaternion.identity);
    }
}
