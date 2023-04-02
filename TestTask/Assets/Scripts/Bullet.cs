using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    private float speed=2f;
    public float damage=0.05f;
    public int viewPlayerID;
    public bool dir=false;

    void Start()
    {
        InvokeRepeating("DeleteBullet", 10f, 0f);
    }
    void Update()
    {
        if(!dir)
        {
            transform.Translate(Vector2.right*speed*Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left*speed*Time.deltaTime);
        }
    }
    void DeleteBullet()
    {
        if(gameObject!=null)
            Destroy(gameObject);
    }
    void OnDestroy()
    {
        CancelInvoke();
    }
}
