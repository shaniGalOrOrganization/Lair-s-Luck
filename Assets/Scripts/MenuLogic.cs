using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuLogic : MonoBehaviour
{
    #region Variables
    public enum GameScreens
    {
        MainMenu, Login, Settings, SignUp, Singleplayer
    };

    private Dictionary<string, GameObject> _unityScreens;
    private static MenuLogic instance;
    private GameScreens _curScreen;
    private Stack<GameScreens> screenStack;
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        _curScreen = GameScreens.MainMenu;
        screenStack = new Stack<GameScreens>();
        _unityScreens = new Dictionary<string, GameObject>();
        GameObject[] curGameObject = GameObject.FindGameObjectsWithTag("unityScreens");
        foreach (GameObject obj in curGameObject)
        {
            _unityScreens.Add(obj.name, obj);
        }

        _unityScreens["Screen_Login"].SetActive(false);
        _unityScreens["Screen_Settings"].SetActive(false);
        _unityScreens["Screen_Singleplayer"].SetActive(false);
        _unityScreens["Screen_SignUp"].SetActive(false);
    }
    #endregion

    #region Login
    public static MenuLogic Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("SC_MenuLogic").GetComponent<MenuLogic>();
            }
            return instance;
        }
    }

    private void changeScreen(GameScreens newScreen, bool pushToStack = true)
    {
        if (_curScreen != newScreen)
        {
            if (pushToStack)
            {
                screenStack.Push(_curScreen);
            }
            _unityScreens["Screen_" + _curScreen].SetActive(false);
            _curScreen = newScreen;
            _unityScreens["Screen_" + _curScreen].SetActive(true);
        }
    }

    public void BTN_LoginLogic()
    {
        changeScreen(GameScreens.Login);
    }

    public void BTN_SettingsLogin()
    {
        changeScreen(GameScreens.Settings);
    }

    public void BTN_BackLogic()
    {
        if (screenStack.Count > 0)
        {
            GameScreens prevScreen = screenStack.Pop();
            changeScreen(prevScreen, false);
        }
    }

    public void BTN_RegisterLogic()
    {
        changeScreen(GameScreens.SignUp);
    }

    public void BTN_SignUpLogic()
    {
        changeScreen(GameScreens.Login);
    }

    public void BTN_ConnectLogic()
    {
        changeScreen(GameScreens.MainMenu);
    }
    #endregion
}
