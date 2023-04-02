using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float X,Y;
    private float speed=5f;
    private float maxHealth=1f;
    Rigidbody2D rb;
    private Joystick joystick;
    private PhotonView view;
    private bool isF=true;
    private Animator anim;
    private TMP_Text _countCoinsText;
    public TMP_Text _namePlayerText;
    private int timeSrawnBullet=80;
    public Transform positionSpawnBullet;
    private Slider sliderHealth;
    private RewardPanel rewardPanel;
    private float offset=0.4f;
    private Quaternion rotateTransf;
    private int timer;
    private bool startPlay=false;
    private RoomManager roomManager;
    private float minX=-9f, minY=-5, maxX=9f, maxY=5;

    void Start()
    {
        view=GetComponent<PhotonView>();
        joystick=GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        _countCoinsText=GameObject.FindGameObjectWithTag("CoinsText").GetComponent<TMP_Text>();
        roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        GameObject rewPan = GameObject.FindGameObjectWithTag("RewardPanel");
        if(rewPan!=null)
        {
            rewardPanel=GameObject.FindGameObjectWithTag("RewardPanel").GetComponent<RewardPanel>();
            rewardPanel.gameObject.SetActive(false);
        }
        sliderHealth=GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        sliderHealth.value=maxHealth;
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        timer=timeSrawnBullet;
        _namePlayerText.text=view.Owner.NickName;
    }
     void Update()
    {
        if(view!=null)
        {
        CheckPlayers();
        if(startPlay)
        {
        float rotateZ=Mathf.Atan2(joystick.Vertical,joystick.Horizontal)*Mathf.Rad2Deg;//joystick.Horizontal,joystick.Vertical
        rotateTransf=Quaternion.Euler(0f,0f,rotateZ+offset);
        X=joystick.Horizontal*speed;
        Y=joystick.Vertical*speed;
        if(view.IsMine)
        {
            if(timer<0)
            {
                SpawnBullet();
                timer=timeSrawnBullet;
            }
            if(X==0)
            {
                anim.SetBool("IsRun",false);
            }
            else if (X!=0 || Y!=0)
            {
                anim.SetBool("IsRun",true);
            }
            if(X>0 && !isF)
                Flip();
            else if (X<0 && isF)
                Flip();
            
        }
        timer--;
        }
        }
    }
     void FixedUpdate()
    {
         if(view!=null && view.IsMine && startPlay)
        {
            rb.velocity=new Vector2(X,Y);
            transform.position=new Vector3
            (
                Mathf.Clamp(transform.position.x,minX,maxX),
                Mathf.Clamp(transform.position.y,minY,maxY),
                transform.position.z
            );
        }
    } 
    private void Flip()
    {
        isF =! isF;
        transform.Rotate(0f,180f,0f);
        _namePlayerText.transform.Rotate(0f,180f,0f);
    }    
     void OnTriggerEnter2D(Collider2D Obj)
    {
        if(view!=null)
        {
            if(view.IsMine)
            {
                if(Obj.tag=="Coin")
                {
                    _countCoinsText.text=(int.Parse(_countCoinsText.text)+1).ToString();
                    view.RPC("DestroyObg",RpcTarget.All,Obj.GetComponent<PhotonView>().ViewID);
                }
                if(Obj.tag=="Bullet")
                {
                    if(Obj.GetComponent<Bullet>().viewPlayerID==0)
                    {
                        ChangeHealth(Obj.GetComponent<Bullet>().damage);
                        if(view!=null)
                            view.RPC("DestroyObg",RpcTarget.All,Obj.GetComponent<PhotonView>().ViewID);
                    }
                }
            }
        }
    }
    void ChangeHealth(float _damage)
    {
        if(sliderHealth.value-_damage<=0)
        {
            sliderHealth.value=0;
            roomManager.Leave();
            view=null;
        }
        else
            sliderHealth.value-=_damage;
            
    } 
    void CheckPlayers()
    {
        if(!startPlay)
        {
            Player[] players = PhotonNetwork.PlayerList;
            if(players.Length>1)
            {
                startPlay=true;
            }
        }
        if(startPlay)
        {
            Player[] players = PhotonNetwork.PlayerList;
            if(players.Length<=1)
            {
                rewardPanel.gameObject.SetActive(true);
                Time.timeScale =0f;
                rewardPanel._nameWinner.text=PhotonNetwork.NickName;
                rewardPanel._coinsCountWinner.text=_countCoinsText.text;
                startPlay=!startPlay;
            }
        }
    }
    void SpawnBullet()
    {
        var _bullet =PhotonNetwork.Instantiate("Bullet",positionSpawnBullet.position,rotateTransf);
         _bullet.GetComponent<Bullet>().viewPlayerID=view.ViewID;
        if(transform.forward.z==-1)
        {
            view.RPC("ChangeDirection",RpcTarget.All,_bullet.GetComponent<PhotonView>().ViewID,true);
        }
        else
        {
            view.RPC("ChangeDirection",RpcTarget.All,_bullet.GetComponent<PhotonView>().ViewID,false);
        }
    }
    [PunRPC]
    private void DestroyObg(int gO)
    {
        GameObject pr = PhotonView.Find(gO).gameObject;
        pr.SetActive(false);
        Destroy(pr);
    }
    [PunRPC]
    private void ChangeDirection(int gO,bool _dir)
    {
        PhotonView.Find(gO).gameObject.GetComponent<Bullet>().dir=_dir;
    }
}