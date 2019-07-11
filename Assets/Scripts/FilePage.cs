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
    public int sortingMethod;
    public int fliterType;
    public int filterTime;
    public string keyword;

    public FilePageCache(FilePage filePage)
    {
        fileOverviews = new List<GameObject>();
        foreach(Transform fileOverviewTrans in filePage.filePanel.transform)
        {
            if (fileOverviewTrans.gameObject.activeSelf)
            {
                fileOverviews.Add(fileOverviewTrans.gameObject);
            }
        }
        pageNum = filePage.currentPageNum;
        sortingMethod = filePage.sortMethodDropdown.value;
        fliterType = filePage.filterDropdown.value;
        filterTime = filePage.timeDropdown.value;
        keyword = filePage.keyword;
    }

    public bool CacheEqualto(FilePage filePage)
    {
        return
            pageNum == filePage.currentPageNum &&
            sortingMethod == filePage.sortMethodDropdown.value &&
            fliterType == filePage.filterDropdown.value &&
            filterTime == filePage.timeDropdown.value &&
            keyword == filePage.keyword;
    }
}

[System.Serializable]
struct FilePageReqJson
{
    public string authorEmail;
    public string sortingMethod;
    public FilterTypes filterType;
    public string filterTime;
    public string searchKeyword;
    public bool searchByContributor;
    public int startRank;

    public FilePageReqJson(string authorEmail, string sortingMethod, string filterType, string filterTime, string searchKeyword, bool searchByContributor, int startRank)
    {
        this.authorEmail = authorEmail;
        this.sortingMethod = sortingMethod;
        this.filterType = new FilterTypes(filterType);
        this.filterTime = filterTime;
        this.searchKeyword = searchKeyword;
        this.searchByContributor = searchByContributor;
        this.startRank = startRank;
    }
}

[System.Serializable]
public struct FilterTypes
{
    public List<string> content;

    public FilterTypes(string type)
    {
        content = new List<string>();
        if (!string.IsNullOrEmpty(type))
        {
            content.Add(type);
        }
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
    public InfoDownloadUrl infoDownloadUrl;
    public string key;
}

[System.Serializable]
public struct InfoDownloadUrl
{
    public int status;
    public string URL;
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
    public InputField keywordInput;
    public GameObject keywordPanel;
    public Toggle searchByContributorToggle;


    public int currentPageNum;
    int numFiles;
    int numFilesPerPage = 16;
    int MaxPageNum() { return numFiles / numFilesPerPage + 1;}

    int StartRank() { return numFilesPerPage * (currentPageNum - 1) + 1; }
    string email;
    public string keyword;
    public Dropdown sortMethodDropdown;
    public Dropdown filterDropdown;
    public Dropdown timeDropdown;

    Queue<FilePageCache> filePageCacheQueue;
    int cacheSize = 4;

    public void Start()
    {
        filePageCacheQueue = new Queue<FilePageCache>();
    }

    // init the file page, will be called by the parent module(profile, homepage) after instansate a file page
    public void Init(string email = null)
    {
        this.email = email;
        keyword = null;

        //Reload FilePanel
        ReloadFilePanel();
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
        RequestFiles();
    }

    public void KeywordSearch()
    {
        //keyword = keywordInput and show keyword panel
        if (string.IsNullOrEmpty(keywordInput.text))
        {
            return;
        }
        keyword = keywordInput.text;
        keywordPanel.SetActive(true);

        //Reload FilePanel
        ReloadFilePanel();
    }

    public void UndoKeywordSearch()
    {
        //keyword = null and hide keyword panel
        keyword = null;
        keywordPanel.SetActive(false);

        //Reload FilePanel
        ReloadFilePanel();
    }

    public void ReloadFilePanel()
    {
        currentPageNum = 1;
        ToPage(0);
    }

    public void RequestFiles()
    {
        //clear filepage


        foreach (Transform fileOverviewTrans in filePanel.transform)
        {
            if (fileOverviewTrans.gameObject.activeSelf)
            {
                fileOverviewTrans.gameObject.SetActive(false);
            }
        }

        if (LoadCache())
        {
            return;
        }

        StartCoroutine(RequestFilesCoro());
    }

    IEnumerator RequestFilesCoro()
    {
        //Startrank is (currentPageNum-1)*FilesPerPage, Range is files per page
        string authorEmail = email;

        string sortingMethod = null; //default is timeASC
        switch (sortMethodDropdown.value)
        {
            case 0:
                sortingMethod = "timeDESC";
                break;
            case 1:
                sortingMethod = "nameDESC";
                break;
            case 2:
                sortingMethod = "downloadsDESC";
                break;
            case 3:
                sortingMethod = "likesDESC";
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

        bool searchByContributor = searchByContributorToggle.isOn;

        int startRank = StartRank();

        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "file/listAll", new WWWForm()))
        {
            Debug.Log(JsonUtility.ToJson(new FilePageReqJson(email, sortingMethod, filterType, filterTime, searchKeyword, searchByContributor, startRank)).Replace("\"\"", "null"));
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new FilePageReqJson(email, sortingMethod, filterType, filterTime, searchKeyword, searchByContributor, startRank)).Replace("\"\"", "null")
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
                SaveCache();
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


    void SaveCache()
    {
        FilePageCache filePageCache = new FilePageCache(this);

        if (filePageCacheQueue.Count >= 4)
        {
            FilePageCache cacheToDestory = filePageCacheQueue.Dequeue();
            foreach(GameObject fileOverview in cacheToDestory.fileOverviews)
            {
                Destroy(fileOverview);
            }
        }

        filePageCacheQueue.Enqueue(filePageCache);
    }

    bool LoadCache()
    {
        //search for hit in cache
        foreach (FilePageCache filePageCache in filePageCacheQueue)
        {
            if (filePageCache.CacheEqualto(this))
            {
                Debug.Log("hit");
                foreach (GameObject fileOverview in filePageCache.fileOverviews)
                {
                    fileOverview.SetActive(true);
                }
                return true;
            }
        }
        return false;
    }

    void EnableDelete()
    {
        foreach (FileOverview overview in filePanel.GetComponentsInChildren<FileOverview>())
        {
            overview.EnableDelete();
        }
    }
    void DisableDelete()
    {
        foreach (FileOverview overview in filePanel.GetComponentsInChildren<FileOverview>())
        {
            overview.DisableDelete();
        }

    }
}
