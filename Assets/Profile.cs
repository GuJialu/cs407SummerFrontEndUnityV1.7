using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
struct ProfileReqJson
{
    public string email;

    public ProfileReqJson(string email)
    {
        this.email = email;
    }
}

[System.Serializable]
struct ProfileResJson
{
    public string status;
    public string username;
    public string email;
    public string description;
    public int icon;
}

public class Profile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //LoadUserInfo(); don't use the start(), the Info needs email which will be provided by the parent module
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //init the profile page, will be called by the parent module
    void LoadUserInfo(string email)
    {
        // invoke the RequestProfileCoro below
    }

    IEnumerator RequestProfileCoro(string email)
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

                //change the icon, username and description
                //...
            }
        }
    }

    public void UploadButtonPressed()
    {
        // Go to Uploads page
        Debug.Log("Pressed Upload Button in Profile Page");
    }

    public void NextPage()
    {

        Debug.Log("Next Page");
    }

    public void PreviousPage()
    {
        Debug.Log("Previous Page");

    }

    public void ChangeIcon()
    {
        Debug.Log("you clicked change icon!");
        //startcoror()
        
    }

    IEnumerator ChangeIconEnum()
    {
        //send webrequest;
        //change you icon locally
        yield return null;

    }
}
