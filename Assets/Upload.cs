using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upload : MonoBehaviour
{
    public GameObject localFileButtonPrefab;
    public GameObject FileScrollViewContent;

    public GameObject fileName;
    // Start is called before the first frame update
    void Start()
    {
        GameObject b= Instantiate(localFileButtonPrefab, FileScrollViewContent.transform);
        b.GetComponentInChildren<Text>().text = "example";

        // for each folder within the requested directory (provided in WebReq.cs) create a new button.
        // Clicking the button, will fill the two text fields in the panel.
        // https://forum.unity.com/threads/how-to-assign-onclick-for-ui-button-generated-in-runtime.358974/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUploadPanel()
    {
        
    }
}
