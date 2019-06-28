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
    public string authorEmail;
    public string sortingMethod;
    public int startRank;
    public int range;

    public FilePageReqJson(string authorEmail, string sortingMethod, int startRank, int range)
    {
        this.authorEmail = authorEmail;
        this.sortingMethod = sortingMethod;
        this.startRank = startRank;
        this.range = range;
    }
}

[System.Serializable]
public struct FileJson
{
    public string email;
    public string username;
    public string fileName;
    public string type;
    public string dateUpdated;
    public int downloadNum;
    public int likes;
    public bool anonymous;
    public string infoDownloadUrl;
}

[System.Serializable]
struct FilePageResJson
{
    public int status;
    public List<FileJson> file_list;
}

public class FilePage : MonoBehaviour
{
    public GameObject fileOverviewPanelPrefab;
    public GameObject filePanel;

    string email;
    int currentPageNum;
    int numFiles;
    int numFilesPerPage = 16;
    int MaxPageNum() { return numFiles / numFilesPerPage + 1;}


    Queue<FilePageCache> filePageCacheQueue;

    public void Start()
    {
        //Init("msljtacslw@gmail.com");
    }

    // init the file page, will be called by the parent module(profile, homepage) after instansate a file page
    public void Init(string email = null)
    {
        this.email = email;
        currentPageNum = 1;
        RequestFiles();
    }
    
    public void ToPage(int offset)
    {
        int pageNum = currentPageNum + offset;


        if (pageNum > 0/*&&pageNum<MaxPageNum()/*/)
        {
            //do nothing
            return;
        }

        currentPageNum = pageNum;

    }

    public void RequestFiles()
    {
        StartCoroutine(RequestFilesCoro());
    }

    IEnumerator RequestFilesCoro()
    {
        //Startrank is (currentPageNum-1)*FilesPerPage, Range is files per page

        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "file/listAll", new WWWForm()))
        {
            Debug.Log(JsonUtility.ToJson(new FilePageReqJson(email, "timeASC", currentPageNum - 1, numFilesPerPage)));
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new FilePageReqJson(email, "timeASC", currentPageNum-1, numFilesPerPage))
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
                CreateFileOverviews(res);
            }
        }
    }

    void CreateFileOverviews(FilePageResJson filePageResJson)
    {
        foreach(FileJson fileJson in filePageResJson.file_list)
        {
            GameObject fileOverviewPanel = Instantiate(fileOverviewPanelPrefab, filePanel.transform);
            FileOverview fileOverview = fileOverviewPanel.GetComponent<FileOverview>();
            fileOverview.Init(fileJson);            
        }
    }
}
