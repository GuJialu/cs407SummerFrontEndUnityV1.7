using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Upload : MonoBehaviour
{
    public GameObject localFileButtonPrefab;
    public GameObject FileScrollViewContent;

    public InputField uploadTitleTextBox;
    public InputField folderNameTextBox;
    public GameObject uploadButton;
    public GameObject cancelButton;
    public Text errorMessage;
    // public GameObject fileName;
    // Start is called before the first frame update
    void Start()
    {

        // GameObject b= Instantiate(localFileButtonPrefab, FileScrollViewContent.transform);
        // b.GetComponentInChildren<Text>().text = "example";
        // var myButton = (GameObject)Instantiate(localFileButtonPrefab)
        // Button button = myButton.getCompo
        // for each folder within the requested directory (provided in WebReq.cs) create a new button.
        // Clicking the button, will fill the two text fields in the panel.
        // https://forum.unity.com/threads/how-to-assign-onclick-for-ui-button-generated-in-runtime.358974/

        // GameObject b = Instantiate(localFileButtonPrefab);
        // var button = GetComponent<UnityEngine.UI.Button>();
        // button.onClick.AddListener(() => FileSelected());
        string filePath = WebReq.objectFolderPath;
        DirectoryInfo d = new DirectoryInfo(filePath);
        // string[] files = Directory.GetFiles(filePath);
        try
        {
            foreach (var currDirectory in d.GetDirectories())
            {
                Debug.Log(currDirectory.Name);
                GameObject fileButton = Instantiate(localFileButtonPrefab, FileScrollViewContent.transform);
                fileButton.GetComponentInChildren<Text>().text = currDirectory.Name;
                var button = fileButton.GetComponent<Button>();
                
                button.onClick.AddListener(delegate{FolderSelected(currDirectory.Name);});
                // Hello Moto
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Uploadfiles()
    {
        string objectFolderPath = Application.dataPath + "/StreamingAssets/LocalGameFiles/";
        string folderName = folderNameTextBox.text;
        string uploadName = uploadTitleTextBox.text;
        if (String.IsNullOrWhiteSpace(folderName) || String.IsNullOrWhiteSpace(uploadName))
        {

        }
    }
    public void FolderSelected(string name)
    {
        // given button title, get upload name from uploadtitlbox

        // then after that, upload the file with the corrosponding information

        // call a method to send data
        folderNameTextBox.text = name;
        Debug.Log(name);
    }
    
    void SendDataToServer(string uploadName)
    {
        
    }
    public void ShowUploadPanel()
    {
        
    }

    
}
