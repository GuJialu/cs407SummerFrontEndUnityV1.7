using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
