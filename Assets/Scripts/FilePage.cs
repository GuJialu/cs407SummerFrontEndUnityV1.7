﻿using System.Collections;
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
    public int rateFrom;
    public int rateTo;

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
        rateFrom = filePage.rateFromDropdown.value;
        rateTo = filePage.rateToDropdown.value + 1;
    }

    public bool CacheEqualto(FilePage filePage)
    {
        return
            pageNum == filePage.currentPageNum &&
            sortingMethod == filePage.sortMethodDropdown.value &&
            fliterType == filePage.filterDropdown.value &&
            filterTime == filePage.timeDropdown.value &&
            keyword == filePage.keyword &&
            rateFrom == filePage.rateFromDropdown.value &&
            rateTo == filePage.rateToDropdown.value + 1;
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
    public int filterRateFrom;
    public int filterRateTo;

    public FilePageReqJson(string authorEmail, string sortingMethod, string filterType, string filterTime, string searchKeyword, bool searchByContributor, int startRank, int filterRateFrom, int filterRateTo)
    {
        this.authorEmail = authorEmail;
        this.sortingMethod = sortingMethod;
        this.filterType = new FilterTypes(filterType);
        this.filterTime = filterTime;
        this.searchKeyword = searchKeyword;
        this.searchByContributor = searchByContributor;
        this.startRank = startRank;
        this.filterRateFrom = filterRateFrom;
        this.filterRateTo = filterRateTo;
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
    public float rate;
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
    public int total_files;
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
    int MaxPageNum() { return (int)System.Math.Ceiling((double)numFiles / numFilesPerPage);}

    int StartRank() { return numFilesPerPage * (currentPageNum - 1) + 1; }
    string email;
    public string keyword;
    public Dropdown sortMethodDropdown;
    public Dropdown filterDropdown;
    public Dropdown timeDropdown;
    public Dropdown rateFromDropdown;
    public Dropdown rateToDropdown;

    Queue<FilePageCache> filePageCacheQueue;
    int cacheSize = 4;

    // init the file page, will be called by the parent module(profile, homepage) after instansate a file page
    public void Init(string email = null)
    {
        filePageCacheQueue = new Queue<FilePageCache>();

        this.email = email;
        keyword = null;
        rateFromDropdown.value = 0;
        rateToDropdown.value = 4;
        rateFromDropdown.onValueChanged.AddListener(delegate { ReloadFilePanel(); });
        rateToDropdown.onValueChanged.AddListener(delegate { ReloadFilePanel(); });

        //Reload FilePanel
        ReloadFilePanel();
    }
    
    public void ToPage(int offset)
    {
        int pageNum = currentPageNum + offset;

        if (pageNum <= 0 || (pageNum>MaxPageNum()&&MaxPageNum()!=0))
        {
            //do nothing
            return;
        }

        currentPageNum = pageNum;
        UpdateIndex();

        RequestFiles();
    }

    void UpdateIndex()
    {
        int i = 0;
        foreach (Text t in indexs)
        {
            int indexNum = currentPageNum - 2 + i;
            if (indexNum <= 0 || indexNum > MaxPageNum())
            {
                t.text = "-";
            }
            else
            {
                t.text = indexNum.ToString();
            }
            ++i;
        }
    }

    public void ToLastPage()
    {
        currentPageNum = MaxPageNum();
        Debug.Log(currentPageNum);
        ToPage(0);
    }

    public void ToFirstPage()
    {
        currentPageNum = 1;
        Debug.Log(currentPageNum);
        ToPage(0);
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
            case 4:
                sortingMethod = "rateDESC";
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
            Debug.Log(JsonUtility.ToJson(new FilePageReqJson(email, sortingMethod, filterType, filterTime, searchKeyword, searchByContributor, startRank, rateFromDropdown.value, rateToDropdown.value + 1)).Replace("\"\"", "null"));
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new FilePageReqJson(email, sortingMethod, filterType, filterTime, searchKeyword, searchByContributor, startRank, rateFromDropdown.value, rateToDropdown.value + 1)).Replace("\"\"", "null")
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
                numFiles = res.total_files;
                CreateFileOverviews(res);
                SaveCache();

                UpdateIndex();
            }
        }
    }

    void CreateFileOverviews(FilePageResJson filePageResJson)
    {
        foreach(FileJson fileJson in filePageResJson.file_list)
        {
            if(email!=null && !email.Equals(WebReq.email) && fileJson.anonymous)
            {
                continue;
            }
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
                filePageCache.fileOverviews.RemoveAll(x => x == null);

                foreach (GameObject fileOverview in filePageCache.fileOverviews)
                {
                    fileOverview.SetActive(true);
                }
                return true;
            }
        }
        return false;
    }

    public void ClearCache()
    {
        while (filePageCacheQueue.Count > 0)
        {
            FilePageCache cacheToDestory = filePageCacheQueue.Dequeue();
            foreach (GameObject fileOverview in cacheToDestory.fileOverviews)
            {
                if (fileOverview != null)
                {
                    Destroy(fileOverview);
                }
            }
        }
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
