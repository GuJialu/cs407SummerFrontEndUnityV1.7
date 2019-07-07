using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Networking;
using System.IO.Compression;

public class Favorites : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject FavoritesScrollViewContent;
    public GameObject fileOverviewPanelPrefab;

    public GameObject exitButton;
    public GameObject removeFavoriteButton;


    public Text errorMessage;

    public string endpoint = "profile/viewfavoritefile";

    void Start()
    {
        // Access database for favorites

        // foreach ( var element in request.json )
        // add element to content
        // add listener
        RequestFavorites();
    }

    public void RequestFavorites()
    {
        StartCoroutine(RequestFavoritesCoro());
    }
    IEnumerator RequestFavoritesCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + endpoint, new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new FavoritesPageReqJson(WebReq.email))
                );

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

                FilePageResJson res = JsonUtility.FromJson<FilePageResJson>(www.downloadHandler.text);
                Debug.Log(JsonUtility.ToJson(res));
                CreateFavoritesOverviews(res);
            }
        }
    }
    public void ElementSelected(string name)
    {
        // TODO, call database for more info, redirect user or pop up with more information.
    }

    public void RemoveFromFavorites()
    {
        // TODO, call database to remove favorites from list. Reload page.
    }

    public void ConfirmRemoveFromFavorites()
    {
        // TODO be called by 'RemoveFromFavorites'

        // TODO create pop up panel.
    }

    void CreateFavoritesOverviews(FilePageResJson filePageResJson)
    {
        foreach (FileJson fileJson in filePageResJson.file_list)
        {
            GameObject fileOverviewPanel = Instantiate(fileOverviewPanelPrefab, FavoritesScrollViewContent.transform);
            FileOverview fileOverview = fileOverviewPanel.GetComponent<FileOverview>();
            fileOverview.Init(fileJson);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
struct FavoritesPageReqJson
{
    public string email;

    public FavoritesPageReqJson(string email)
    {
        this.email = email;
    }
}
