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

[System.Serializable]
struct EmailVeriReqJson
{
    public string email;

    public EmailVeriReqJson(string email)
    {
        this.email = email;
    }
}

[System.Serializable]
struct EmailVeriResJson
{
    public int status;
    public string err_message;
}

[System.Serializable]
struct CodeVeriReqJson
{
    public string email;
    public string code;
    public string password;

    public CodeVeriReqJson(string email, string code, string password)
    {
        this.email = email;
        this.code = code;
        this.password = password;
    }
}

[System.Serializable]
struct CodeVeriResJson
{
    public int status;
    public string err_message;
}

public class LoginAndSignUp : MonoBehaviour
{
    public GameObject signUpPanel;
    public GameObject loginPanel;
    public GameObject emailVeriPanel;
    public GameObject codeVeriPanel;
    public GameObject resetPasswordPanel;
    public GameObject deleteButton;

    public InputField signUpEmailInput;
    public InputField signUpPasswordInput;
    public InputField signUpRenterPasswordInput;
    public InputField signUpUserNameInput;

    public InputField loginEmailInput;
    public InputField loginPasswordInput;

    public InputField forgetPasswordEmailInput;
    public InputField forgetPasswordCodeInput;
    public InputField resetPasswordNewPasswordInput;
    public InputField resetPasswordRenterPasswordInput;

    public Text errorMessageText;

    //render the signUpPanel in the front
    public void ShowSignUpPanel()
    {
        signUpPanel.transform.SetAsLastSibling();
        deleteButton.transform.SetAsLastSibling();
        errorMessageText.transform.SetAsLastSibling();
        errorMessageText.text = "";
    }

    //render the loginPanel in the front
    public void ShowLoginPanel()
    {
        loginPanel.transform.SetAsLastSibling();
        deleteButton.transform.SetAsLastSibling();
        errorMessageText.transform.SetAsLastSibling();
        errorMessageText.text = "";
    }

    public void ShowEmailVeriPanel()
    {
        emailVeriPanel.transform.SetAsLastSibling();
        deleteButton.transform.SetAsLastSibling();
        errorMessageText.transform.SetAsLastSibling();
        errorMessageText.text = "";
    }

    public void ShowCodeVeriPanel()
    {
        codeVeriPanel.transform.SetAsLastSibling();
        deleteButton.transform.SetAsLastSibling();
        errorMessageText.transform.SetAsLastSibling();
        errorMessageText.text = "";
    }

    public void ShowRestPasswordPanel()
    {
        resetPasswordPanel.transform.SetAsLastSibling();
        deleteButton.transform.SetAsLastSibling();
        errorMessageText.transform.SetAsLastSibling();
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
                    WebReq.email = email;
                    WorkShopEvents.loginEvent?.Invoke(email);
                }
            }
        }
    }

    public void RequestEmailVeri()
    {
        Debug.Log("RequestEmailVeri");
        string email = forgetPasswordEmailInput.text;
        StartCoroutine(RequestEmailVeriCoro(email));
    }

    IEnumerator RequestEmailVeriCoro(string email)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "account/emailVeri", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new EmailVeriReqJson(email)));

            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                EmailVeriResJson res = JsonUtility.FromJson<EmailVeriResJson>(www.downloadHandler.text);

                if (res.err_message != null)
                {
                    Debug.Log(res.err_message);
                    errorMessageText.text = res.err_message;
                }
                else
                {
                    Debug.Log(res.status);
                    ShowCodeVeriPanel();
                }
            }
        }
    }

    public void RequestCodeVeri()
    {
        Debug.Log("RequestCodeVeri");
        string email = forgetPasswordEmailInput.text;
        string code = forgetPasswordCodeInput.text;
        string password = resetPasswordNewPasswordInput.text;
        string passwordRenter = resetPasswordRenterPasswordInput.text;
        if (!password.Equals(passwordRenter))
        {
            errorMessageText.text = "renter password not match";
            return;
        }
        StartCoroutine(RequestCodeVeriCoro(email, code, password));
    }

    IEnumerator RequestCodeVeriCoro(string email, string code, string password)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "account/codeVeri", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new CodeVeriReqJson(email, code, password)));
            Debug.Log(JsonUtility.ToJson(new CodeVeriReqJson(email, code, password)));

            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                CodeVeriResJson res = JsonUtility.FromJson<CodeVeriResJson>(www.downloadHandler.text);

                if (res.err_message != null)
                {
                    Debug.Log(res.err_message);
                    errorMessageText.text = res.err_message;
                }
                else
                {
                    Debug.Log(res.status);
                    ShowRestPasswordPanel();
                }
            }
        }
    }
}
