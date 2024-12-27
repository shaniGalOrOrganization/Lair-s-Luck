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
    #endregion
}
