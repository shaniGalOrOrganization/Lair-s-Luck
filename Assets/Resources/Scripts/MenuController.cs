using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MenuLogic;

public class MenuController : MonoBehaviour
{
    #region Logic 
    public void BTN_Login()
    {
        MenuLogic.Instance.BTN_LoginLogic();
    }

    public void BTN_Settings()
    {
        MenuLogic.Instance.BTN_SettingsLogin();
    }

    public void BTN_Back()
    {
        MenuLogic.Instance.BTN_BackLogic();
    }

    public void BTN_Register()
    {
        MenuLogic.Instance.BTN_RegisterLogic();
    }

    public void BTN_Singleplayer()
    {
        MenuLogic.Instance.BTN_SingleplayerLogic();
    }

    public void BTN_Menu()
    {
        MenuLogic.Instance.BTN_MenuLogic();
    }
    #endregion
}
