using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

enum SortingMethod
{
    date,
    downloads,
    likes,
}

class FilePageCache
{
    public List<GameObject> fileOverviews;
    public int pageNum;
    public SortingMethod sortingMethod;
}

[System.Serializable]
struct FilePageReqJson
{
    public string email;
    public string sortingMethod;
    public int startRank;
    public int range;
}

[System.Serializable]
struct FilePageResJson
{
    public int status;
    public string err_message;
}

public class FilePage : MonoBehaviour
{
    public GameObject fileOverviewPanelPrefab;
    public GameObject filePanel;

    int currentPageNum;
    int numFiles;
    int numFilesPerPage = 16;

    Queue<FilePageCache> filePageCacheQueue;

    // init the file page, will be called by the parent module(profile, homepage) after instansate a file page
    void Init(string email = null)
    {

    }
    
    public void ShowPage(int offset)
    {
        int pageNum = currentPageNum + offset;
        //request files sorted by selected method ranked from currentPageNum to currentPageNum+numFilesPerPage
        //Change current page
    }

    public void RequestFilePage(string email = null)
    {

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

            }
        }
    }
}
