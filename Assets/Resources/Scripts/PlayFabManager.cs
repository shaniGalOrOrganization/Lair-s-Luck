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
    [Header("UI Elements")]
    public TMP_InputField usernameInputRegister;
    public TMP_InputField passwordInputRegister;
    public TMP_InputField usernameInputLogin;
    public TMP_InputField passwordInputLogin;
    public TMP_Text greetingText;

    void Start()
    {
        Debug.Log("PlayFab Initialized.");

        // Set the default greeting message to "Hello Guest"
        if (greetingText != null)
        {
            greetingText.text = "Hello Guest";
        }
        else
        {
            Debug.LogWarning("Greeting Text is not assigned in the Inspector.");
        }
    }

    public void RegisterUser()
    {
        string username = usernameInputRegister.text;
        string password = passwordInputRegister.text;

        // Validation 1: Check for empty fields
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Username or Password is empty. Please fill in both fields.");
            return; // Prevent further execution
        }

        // Validation 2: Username length (PlayFab requires 3–20 characters)
        if (username.Length < 3 || username.Length > 20)
        {
            Debug.LogError("Username must be between 3 and 20 characters.");
            return;
        }

        // Validation 3: Password length (PlayFab requires 6–100 characters)
        if (password.Length < 6 || password.Length > 100)
        {
            Debug.LogError("Password must be between 6 and 100 characters.");
            return;
        }

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
        string username = usernameInputLogin.text;
        string password = passwordInputLogin.text;

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

        // Update the greeting text to show the logged-in username
        if (greetingText != null)
        {
            greetingText.text = $"Hello {usernameInputLogin.text}";
        }

        MenuLogic.Instance.BTN_ConnectLogic();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login failed: " + error.GenerateErrorReport());
    }
}
