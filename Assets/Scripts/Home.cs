using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.U2D;

public class Home : MonoBehaviour
{
    public GameObject iconButton;
    public GameObject loginButton;
    public GameObject logoutButton;

    public SpriteAtlas spriteAtlas;

    public GameObject loginSignUpPanelPrefab;
    public GameObject profilePanelPrefab;

    Sprite originalIconSprite;

    // Start is called before the first frame update
    void Start()
    {
        WorkShopEvents.loginEvent.AddListener(OnLogin);
        iconButton.GetComponent<Button>().enabled = false;
        originalIconSprite = iconButton.GetComponentInChildren<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenProfilePanel()
    {
        Instantiate(profilePanelPrefab, transform.parent).GetComponent<Profile>().Init(WebReq.email);
    }

    public void OpenLoginSignUpPanel()
    {
        Instantiate(loginSignUpPanelPrefab, transform.parent);
    }

    public void Logout()
    {
        WebReq.bearerToken = null;
        WebReq.email = null;
        logoutButton.SetActive(false);
        loginButton.SetActive(true);
        iconButton.GetComponentInChildren<Image>().sprite = originalIconSprite;
    }

    public void ReturnToMainSence()
    {

    }

    public void OnLogin(string email)
    {
        StartCoroutine(RequestProfileIconCoro(email));
        logoutButton.SetActive(true);
        loginButton.SetActive(false);
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
                
                Sprite[] icons = new Sprite[spriteAtlas.spriteCount];
                spriteAtlas.GetSprites(icons);
                iconButton.GetComponentInChildren<Image>().sprite = icons[res.icon];
                iconButton.GetComponent<Button>().enabled = true;
            }
        }
    }
}
