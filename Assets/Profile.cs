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

public class Profile : MonoBehaviour
{
    private int iconNumber;

    public Sprite[] icons;
    public GameObject IconButton;
    public GameObject DesText;
    public SpriteAtlas spriteAtlas;
    public InputField InputField;
    public Transform content;
    public GameObject SelectIconButtonPrefab;
    public GameObject ScorllView;

    //public Button IconButton;

    // Start is called before the first frame update
    void Start()
    {
        //LoadUserInfo(); don't use the start(), the Info needs email which will be provided by the parent module
        icons = new Sprite[spriteAtlas.spriteCount];
        spriteAtlas.GetSprites(icons);
        IconButton.GetComponentInChildren<Image>().sprite = icons[0];

        for (int i = 0; i < spriteAtlas.spriteCount; i++)
        {
            GameObject setButton = Instantiate(SelectIconButtonPrefab, content);
            setButton.GetComponent<Image>().sprite = icons[i];
            Debug.Log("in:" + i);
            int ii = i;
            setButton.GetComponent<Button>().onClick.AddListener(delegate { SetIcon(ii); });
        }
        
        ScorllView.gameObject.SetActive(false);

        DesText.GetComponentInChildren<Text>().text = "User Info";
        InputField.enabled = false;
        InputField.gameObject.SetActive(false);
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
                res.description = DesText.GetComponentInChildren<Text>().text;
                res.icon = iconNumber;            
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
        ScorllView.gameObject.SetActive(true);
    }

    public void SetIcon(int index)
    {
        iconNumber = index;
        IconButton.GetComponentInChildren<Image>().sprite = icons[index];
        ScorllView.gameObject.SetActive(false);
    }

    IEnumerator ChangeIconEnum()
    {
        //send webrequest;
        //change you icon locally
        yield return null;
    }

    public void ChangeDescription()
    {
        if(!InputField.enabled)
        {
            InputField.enabled = true;
            InputField.gameObject.SetActive(true);
        }
        else
        {
            InputField.enabled = false;
            InputField.gameObject.SetActive(false);

            DesText.GetComponentInChildren<Text>().text = InputField.text;
        }
        
    }
}
