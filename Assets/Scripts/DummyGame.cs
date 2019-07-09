using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO.Compression;

public class DummyGame : MonoBehaviour
{
    public Texture2D spongeBob;
    public Texture2D hotdog;
    public Texture2D city;
    public InputField modNameInput;
    public InputField desInput;
    public InputField modContentInput;
    public Dropdown coverDropdown;

    public Text errorMessage;

    public GameObject FileScrollView;
    public GameObject localFileButtonPrefab;
    public GameObject FileScrollViewContent;
    public GameObject LoadButton;
    public GameObject LoadModButton;
    public GameObject CancelLoadButton;

    // Start is called before the first frame update
    void Start()
    {
        LoadMod();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        FileScrollView.SetActive(true);
        LoadButton.SetActive(false);
        LoadModButton.SetActive(true);
        CancelLoadButton.SetActive(true);
    }

    public void Cancel()
    {
        FileScrollView.SetActive(false);
        LoadButton.SetActive(true);
        LoadModButton.SetActive(false);
        CancelLoadButton.SetActive(false);
    }

    public void LoadMod()
    {
        string filePath = WebReq.objectFolderPath;
        DirectoryInfo d = new DirectoryInfo(filePath);

        foreach (var currDirectory in d.GetDirectories())
        {
            Debug.Log(currDirectory.Name);
            GameObject fileButton = Instantiate(localFileButtonPrefab, FileScrollViewContent.transform);
            fileButton.GetComponentInChildren<Text>().text = currDirectory.Name;
            var button = fileButton.GetComponent<Button>();
            button.onClick.AddListener(delegate { FolderSelected(currDirectory.Name); });
        }
    }

    public void FolderSelected(string name)
    {
        modNameInput.text = name;
        Debug.Log(name);
    }

    public void LoadModInfo()
    {
        if (String.IsNullOrWhiteSpace(modNameInput.text))
        {
            errorMessage.text = "an input field is blank or is white space. please put valid inputs";
            return;
        }

        //Assets / StreamingAssets / LocalGameFiles / mod1 / info / des.txt
        //string modPath = "Assets/StreamingAssets/LocalGameFiles/" + folderNameTextBox.text;
        string modPath = WebReq.objectFolderPath + modNameInput.text;
        string infoPath = modPath + "/info";
        string contentPath = modPath + "/modcontent.txt";
        string desPath = infoPath + "/des.txt";
        string coverPath = infoPath + "/workingspace.PNG";

        if (Directory.Exists(modPath))
        {
            StreamReader reader = new StreamReader(desPath);
            desInput.text = reader.ReadToEnd();
            reader.Close();
            reader = new StreamReader(contentPath);
            modContentInput.text = reader.ReadToEnd();
            reader.Close();
        }
        else
        {
            Debug.Log("No such file directory");
        }

        Cancel();
    }

    public void SaveMod()
    {
        if (String.IsNullOrWhiteSpace(modNameInput.text))
        {
            errorMessage.text = "an input field is blank or is white space. please put valid inputs";
            return;
        }

        string modPath = WebReq.objectFolderPath + modNameInput.text;
        string infoPath = modPath + "/info";

        if (!Directory.Exists(infoPath))
        {
            Directory.CreateDirectory(infoPath);
        }

        string contentPath = modPath + "/modcontent.txt";
        string desPath = infoPath + "/des.txt";
        string coverPath = infoPath + "/workingspace.PNG";

        if (File.Exists(desPath))
        {
            File.Delete(desPath);
            Debug.Log("des file delete");
        }
        if (File.Exists(coverPath))
        {
            File.Delete(coverPath);
            Debug.Log("cover file delete");
        }
        if (File.Exists(contentPath))
        {
            File.Delete(contentPath);
            Debug.Log("content file delete");
        }

        using (StreamWriter outputFile = File.CreateText(desPath))
        {
            outputFile.Write(desInput.text);
        }
        using (StreamWriter outputFile = File.CreateText(contentPath))
        {
            outputFile.Write(modContentInput.text);
        }

        Texture2D cover = spongeBob;
        switch (coverDropdown.value)
        {
            case 0:
                cover = spongeBob;
                break;
            case 1:
                cover = hotdog;
                break;
            case 2:
                cover = city;
                break;
        }
        File.WriteAllBytes(coverPath, cover.EncodeToPNG());

        
        foreach(Transform fileButtonTrans in FileScrollViewContent.transform)
        {
            Destroy(fileButtonTrans.gameObject);
        }
        
        LoadMod();
    }
}
