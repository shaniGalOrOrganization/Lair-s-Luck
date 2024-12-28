using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuParent;

    #region logic 
    public void BTN_Login()
    {
        MenuLogic.Instance.BTN_LoginLogic();
    }

    public void BTN_Settings()
    {
        MenuLogic.Instance.BTN_SettingsLogin();
    }
    #endregion
}
