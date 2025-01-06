using System.Collections;
using System.Collections.Generic;
// using System.Diagnostics;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI; // Required for UI elements
using TMPro; // Add this namespace for TMP_InputField


public class PlayFabManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("PlayFab Initialized.");
    }

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    public void RegisterUser()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        var request = new PlayFab.ClientModels.RegisterPlayFabUserRequest
        {
            Username = username,
            Password = password,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);

    }
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    { 
        Debug.Log("Registration successful!");
        MenuLogic.Instance.BTN_SignUpLogic();
    }

    void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError("Registration failed: " + error.GenerateErrorReport());
    }

    public void LoginUser()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Username or Password is empty. Please fill in both fields.");
            return; // Prevent further execution
        }

        var request = new LoginWithPlayFabRequest
        {
            Username = username,
            Password = password
        };

        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successful! PlayFab ID: " + result.PlayFabId);
        MenuLogic.Instance.BTN_ConnectLogic();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login failed: " + error.GenerateErrorReport());
    }
}
