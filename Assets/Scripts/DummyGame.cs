using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DummyGame : MonoBehaviour
{
    public Texture2D spongeBob;
    public Texture2D hotdog;
    public Texture2D city;
    public InputField modNameInput;
    public InputField desInput;
    public Dropdown coverDropdown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMod()
    {
        string modPath = WebReq.objectFolderPath + modNameInput.text;
        string infoPath = modPath + "/info";

        if (Directory.Exists(infoPath))
        {
            Debug.Log("That path exists already.");
            return;
        }
        Directory.CreateDirectory(infoPath);

        string contentPath = modPath + "/content.txt";
        string desPath = infoPath + "/des.txt";
        string coverPath = infoPath + "/workingspace.PNG";

        using (StreamWriter outputFile = File.CreateText(desPath))
        {
            outputFile.Write(desInput.text);
        }
        using (StreamWriter outputFile = File.CreateText(contentPath))
        {
            outputFile.Write("dummy mod content");
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
    }
}
