using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] private List <MenuAction> _menus;

    private void Awake()
    {
        instance=this;
    }
    public void OpenMenu(string menuName)
    {
        foreach (MenuAction menu in _menus)
        {
            if(menu.menuName==menuName)//нашлось меню, которое нужно открыть
            {
                menu.Open();
            }
            else
            {
                menu.Close();
            }
        }
    }
}
