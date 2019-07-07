using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum SortingMethod
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
    public string filterType;
    public string filterTime;
    public string searchKeyword;
    public int startRank;

    public FilePageReqJson(string authorEmail, string sortingMethod, string filterType, string filterTime, string searchKeyword, int startRank)
    {
        this.authorEmail = authorEmail;
        this.sortingMethod = sortingMethod;
        this.filterType = filterType;
        this.filterTime = filterTime;
        this.searchKeyword = searchKeyword;
        this.startRank = startRank;
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
    public Text[] indexs;


    int currentPageNum;
    int numFiles;
    int numFilesPerPage = 16;
    int MaxPageNum() { return numFiles / numFilesPerPage + 1;}

    int StartRank() { return numFilesPerPage * (currentPageNum - 1) + 1; }
    string email;
    string keyword;
    public Dropdown sortMethodDropdown;
    public Dropdown filterDropdown;
    public Dropdown timeDropdown;

    Queue<FilePageCache> filePageCacheQueue;

    public void Start()
    {
        Init();
    }

    // init the file page, will be called by the parent module(profile, homepage) after instansate a file page
    public void Init(string email = null)
    {
        this.email = email;
        keyword = null;
        currentPageNum = 1;
        ToPage(0);
        RequestFiles();
    }
    
    public void ToPage(int offset)
    {
        int pageNum = currentPageNum + offset;

        if (pageNum <= 0/*&&pageNum>MaxPageNum()/*/)
        {
            //do nothing
            return;
        }

        currentPageNum = pageNum;
        int i = 0;
        foreach(Text t in indexs)
        {
            int indexNum = currentPageNum - 2 + i;
            if (indexNum <= 0)
            {
                t.text = "-";
            }
            else
            {
                t.text = indexNum.ToString();
            }
            ++i;
        }
        //RequestFiles();
    }

    public void KeywordSearch()
    {
        //keyword = keywordInput and show keyword panel
        //RequestFiles();
    }

    public void UndoKeywordSearch()
    {
        //keyword = null and hide keyword panel
        //RequestFiles();
    }

    public void RequestFiles()
    {
        //cache old and search new in cache
        StartCoroutine(RequestFilesCoro());
    }

    IEnumerator RequestFilesCoro()
    {
        //Startrank is (currentPageNum-1)*FilesPerPage, Range is files per page
        string authorEmail = email;

        string sortingMethod = null; //default is null
        switch (sortMethodDropdown.value)
        {
            case 1:
                sortingMethod = "timeASC";
                break;
            case 2:
                sortingMethod = "nameASC";
                break;
            case 3:
                sortingMethod = "downloads";
                break;
            case 4:
                sortingMethod = "likes";
                break;
        }

        string filterType = null; //default is null
        switch (filterDropdown.value)
        {
            case 1:
                filterType = "Visual Mods";
                break;
            case 2:
                filterType = "UI Mods";
                break;
            case 3:
                filterType = "Game logic Mods";
                break;
        }

        string filterTime = null; //default is null
        switch (timeDropdown.value)
        {
            case 1:
                filterTime = "oneday";
                break;
            case 2:
                filterTime = "threemonths";
                break;
            case 3:
                filterTime = "oneyear";
                break;
        }

        string searchKeyword = keyword;

        int startRank = StartRank();

        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "file/listAll", new WWWForm()))
        {
            Debug.Log(sortingMethod);
            Debug.Log(JsonUtility.ToJson(new FilePageReqJson(email, sortingMethod, filterType, filterTime, searchKeyword, startRank)));
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new FilePageReqJson(email, sortingMethod, filterType, filterTime, searchKeyword, startRank))
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
