using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SortingMethod
{
    date,
    downloads,
    likes,
}

public class FilePage : MonoBehaviour
{
    int currentPageNum;
    int numFiles;
    int numFilesPerPage = 16;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void ShowPage(int offset)
    {
        int pageNum = currentPageNum + offset;
        //request files sorted by selected method ranked from currentPageNum to currentPageNum+numFilesPerPage
        //Change current page
    }
}
