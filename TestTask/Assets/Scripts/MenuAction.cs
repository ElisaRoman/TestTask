using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAction : MonoBehaviour
{
    public string menuName;//имя меню

    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
