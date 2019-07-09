using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameConfig.LoadMods(WebReq.objectFolderPath);
        //GameConfig.LoadMods(WebReq.downloadFolderPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
