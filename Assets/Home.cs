using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Home : MonoBehaviour
{
    public GameObject iconButton;
    public GameObject loginButton;
    public GameObject logoutButton;

    public GameObject loginSignUpPanelPrefab;

    // Start is called before the first frame update
    void Start()
    {
        WorkShopEvents.loginEvent.AddListener(OnLogin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenProfilePanel()
    {

    }

    public void OpenLoginSignUpPanel()
    {
        Instantiate(loginSignUpPanelPrefab, transform.parent);
        logoutButton.SetActive(true);
        loginButton.SetActive(false);
    }

    public void Logout()
    {
        WebReq.bearerToken = null;
        logoutButton.SetActive(false);
        loginButton.SetActive(true);
    }

    public void ReturnToMainSence()
    {

    }

    public void OnLogin(string email)
    {
        StartCoroutine(RequestProfileIconCoro(email));
    }

    IEnumerator RequestProfileIconCoro(string email)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "profile/viewAll", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new ProfileReqJson(email)));

            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                ProfileResJson res = JsonUtility.FromJson<ProfileResJson>(www.downloadHandler.text);

                iconButton.GetComponent<Image>().color = Color.red;// change the color as an example since icon is not implmented yet
            }
        }
    }
}
