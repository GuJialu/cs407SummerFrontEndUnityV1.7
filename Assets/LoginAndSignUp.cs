using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
struct SignUpReqJson
{
    public string email;
    public string password;
    public string username;

    public SignUpReqJson(string email, string password, string username)
    {
        this.email = email;
        this.password = password;
        this.username = username;
    }
}

[System.Serializable]
struct SignUpResJson
{
    public int status;
    public string err_message;
}

[System.Serializable]
struct LoginReqJson
{
    public string email;
    public string password;

    public LoginReqJson(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}

[System.Serializable]
struct LoginResJson
{
    public int status;
    public string token;
    public string err_message;
}

public class LoginAndSignUp : MonoBehaviour
{
    public GameObject signUpPanel;
    public GameObject loginPanel;

    public InputField signUpEmailInput;
    public InputField signUpPasswordInput;
    public InputField signUpRenterPasswordInput;
    public InputField signUpUserNameInput;

    public InputField loginEmailInput;
    public InputField loginPasswordInput;

    public Text errorMessageText;

    //render the signUpPanel in the front
    public void ShowSignUpPanel()
    {
        signUpPanel.SetActive(true);
        loginPanel.SetActive(false);
        errorMessageText.text = "";
    }

    //render the loginPanel in the front
    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
        errorMessageText.text = "";
    }

    //send the sign up requset
    public void RequestSignUp()
    {
        Debug.Log("RequestSignUp");

        string email = signUpEmailInput.text;
        string password = signUpPasswordInput.text;
        string renterPassword = signUpRenterPasswordInput.text;
        string username = signUpUserNameInput.text;

        //check input validation
        //...

        StartCoroutine(RequestSignUpCoro(email, password, username));
    }

    IEnumerator RequestSignUpCoro(string email, string password, string username)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "account/registration", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new SignUpReqJson(email, password, username)));

            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                SignUpResJson res = JsonUtility.FromJson<SignUpResJson>(www.downloadHandler.text);
                if (res.err_message != null)
                {
                    errorMessageText.text = res.err_message;
                }
                else
                {
                    errorMessageText.text = "success";
                }
            }
        }
    }

    //send the login request
    public void RequestLogin()
    {
        Debug.Log("RequestLogin");

        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;

        StartCoroutine(RequestLoginCoro(email, password));
    }

    IEnumerator RequestLoginCoro(string email, string password)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "account/login", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new LoginReqJson(email, password)));

            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                LoginResJson res = JsonUtility.FromJson<LoginResJson>(www.downloadHandler.text);
                
                if (res.err_message!=null)
                {
                    Debug.Log(res.err_message);
                    errorMessageText.text = res.err_message;
                }
                else
                {
                    WebReq.bearerToken = res.token;
                    WorkShopEvents.loginEvent?.Invoke(email);
                }
            }
        }
    }
}
