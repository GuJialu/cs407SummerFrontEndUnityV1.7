using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using UnityEngine.UI;

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

[System.Serializable]
struct ProfileUpdateReqJson
{
    public string email;
    public string description;
    public int icon;
    public string username;

    public ProfileUpdateReqJson(string email, string description, int icon, string username)
    {
        this.email = email;
        this.description = description;
        this.icon = icon;
        this.username = username;
    }
}

public class Profile : MonoBehaviour
{
    private int iconNumber;

    public Sprite[] icons;
    public GameObject IconButton;
    public GameObject DesText;
    public SpriteAtlas spriteAtlas;
    public InputField inputField;
    public Transform content;
    public GameObject SelectIconButtonPrefab;
    public GameObject ScorllView;
    public Text usernameText;

    public GameObject signUpAndLoginPanelPrefab;
    public GameObject filePagePanel;
    public GameObject uploadPanel;
    public GameObject favoritesbutton;
    public GameObject FavoritesPanelPrefab;
    // Start is called before the first frame update
    void Start()
    {
        icons = new Sprite[spriteAtlas.spriteCount];
        spriteAtlas.GetSprites(icons);
        IconButton.GetComponentInChildren<Image>().sprite = icons[0];

        for (int i = 0; i < spriteAtlas.spriteCount; i++)
        {
            GameObject setButton = Instantiate(SelectIconButtonPrefab, content);
            setButton.GetComponent<Image>().sprite = icons[i];
            int ii = i;
            setButton.GetComponent<Button>().onClick.AddListener(delegate { SetIcon(ii); });
        }
        
        ScorllView.gameObject.SetActive(false);

        DesText.GetComponentInChildren<Text>().text = "User Info";
        inputField.gameObject.SetActive(false);


        //for testing only, delete after connect to home page
        //WebReq.email = "msljtacslw@gmail.com";
        //Init(WebReq.email);
    }

    //init the profile page, will be called by the parent module
    public void Init(string email)
    {
        // start the RequestProfileCoro below
        filePagePanel.GetComponent<FilePage>().Init(email);
        StartCoroutine(RequestProfileCoro(email));
        if (email != WebReq.email)
        {
            Debug.Log("other's profile");
        }
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
                Debug.Log(www.downloadHandler.text);

                ProfileResJson res = JsonUtility.FromJson<ProfileResJson>(www.downloadHandler.text);

                //change the icon, username and description
                DesText.GetComponentInChildren<Text>().text = res.description;
                usernameText.text = res.username;
                iconNumber = res.icon;
                IconButton.GetComponentInChildren<Image>().sprite = icons[res.icon];
            }
        }
    }

    public void UploadButtonPressed()
    {
        // Go to Uploads page
        Debug.Log("Pressed Upload Button in Profile Page");
        Instantiate(uploadPanel, transform.parent);
    }

    public void ChangePassword()
    {
        GameObject signUpAndLoginPanel = Instantiate(signUpAndLoginPanelPrefab, transform.parent);
    }

    public void ChangeIcon()
    {
        ScorllView.gameObject.SetActive(true);
    }

    public void SetIcon(int index)
    {
        iconNumber = index;
        IconButton.GetComponentInChildren<Image>().sprite = icons[index];
        ScorllView.gameObject.SetActive(false);
        RequestUpdateProfile();
    }


    public void ChangeDescription()
    {
        if(!inputField.gameObject.activeSelf)
        {
            inputField.text = DesText.GetComponentInChildren<Text>().text;
            inputField.gameObject.SetActive(true);
        }
        else
        {
            inputField.gameObject.SetActive(false);
            DesText.GetComponentInChildren<Text>().text = inputField.text;
            RequestUpdateProfile();
        }
        
    }

    public void ChangeUsername()
    {
        if (!inputField.gameObject.activeSelf)
        {
            inputField.text = usernameText.text;
            inputField.gameObject.SetActive(true);
        }
        else
        {
            inputField.gameObject.SetActive(false);
            usernameText.text = inputField.text;
            RequestUpdateProfile();
        }

    }

    public void GoToFavorites()
    {
        GameObject favoritespage = Instantiate(FavoritesPanelPrefab);
    }
    void RequestUpdateProfile()
    {
        StartCoroutine(RequestUpdateProfileCoro());
    }

    IEnumerator RequestUpdateProfileCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "profile/editAll", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new ProfileUpdateReqJson(WebReq.email, DesText.GetComponentInChildren<Text>().text, iconNumber, usernameText.text)));

            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", WebReq.bearerToken);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
