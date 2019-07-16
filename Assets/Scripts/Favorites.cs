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

    // public GameObject exitButton;
    public GameObject removeFavoriteButton;
    public GameObject stopRemovingButton;

    // public Text errorMessage;

    string endpoint = "profile/viewfavoritefile";

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
        Debug.Log("startReqFav");
        //not logged in
        if (WebReq.email == null)
        {
            return;
        }
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
            www.SetRequestHeader("Authorization", WebReq.bearerToken);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                FavoritesPageResJson res = JsonUtility.FromJson<FavoritesPageResJson>(www.downloadHandler.text);
                Debug.Log(JsonUtility.ToJson(res));
                CreateFavoritesOverviews(res);
            }
        }
    }


    public void EnableRemoveFromFavorites()
    {
        // TODO check if this works
        removeFavoriteButton.SetActive(false);
        stopRemovingButton.SetActive(true);
        foreach (FileOverview overview in FavoritesScrollViewContent.GetComponentsInChildren<FileOverview>())
        {
            overview.EnableUnlike();
        }

    }
    public void DisableRemoveFromFavorites()
    {
        // TODO check if this works

        removeFavoriteButton.SetActive(true);
        stopRemovingButton.SetActive(false);
        foreach (FileOverview overview in FavoritesScrollViewContent.GetComponentsInChildren<FileOverview>())
        {
            overview.DisableUnlike();
        }

    }
    public void ConfirmRemoveFromFavorites()
    {
        // TODO be called by 'RemoveFromFavorites'

        // TODO create pop up panel.
    }

    void CreateFavoritesOverviews(FavoritesPageResJson filePageResJson)
    {
        foreach (FileJson fileJson in filePageResJson.files)
        {
            GameObject fileOverviewPanel = Instantiate(fileOverviewPanelPrefab, FavoritesScrollViewContent.transform);
            FileOverview fileOverview = fileOverviewPanel.GetComponent<FileOverview>();
            fileOverview.Init(fileJson);
        }
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

struct FavoritesPageResJson
{
    public int status;
    public List<FileJson> files;
}