using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Networking;
using System.IO.Compression;

struct UploadReqJson
{
    //json structure
    public int anonymous;
    public string filename;
    public string type;

    public UploadReqJson(int anonymous, string filename, string type)
    {
        this.anonymous = anonymous;
        this.filename = filename;
        this.type = type;
    }
}

struct UploadResJson
{
    public string status;
    public string uploadUrl;
    public string infoUploadUrl;
}

public class Upload : MonoBehaviour
{
    public GameObject localFileButtonPrefab;
    public GameObject FileScrollViewContent;

    public InputField uploadTitleTextBox;
    public InputField folderNameTextBox;
    public Toggle anonymousToggle;
    public GameObject uploadButton;
    public GameObject cancelButton;
    public Text errorMessage;
    public Dropdown fileTypeDropdown;

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

                button.onClick.AddListener(delegate { FolderSelected(currDirectory.Name); });
                // Hello Moto
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
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

    public void Uploadfiles()
    {
        string objectFolderPath = Application.dataPath + "/StreamingAssets/LocalGameFiles/";

        if (String.IsNullOrWhiteSpace(folderNameTextBox.text) || String.IsNullOrWhiteSpace(uploadTitleTextBox.text)) // if an input is blank
        {
            errorMessage.text = "an input field is blank or is white space. please put valid inputs";
        }
        else
        {
            // Else check if the folder specified in the folderName textbox exists
            if (!Directory.Exists(objectFolderPath + folderNameTextBox.text)) // if it does not exist
            {
                // then failure
                Debug.Log("noooooooope");
                errorMessage.text = "Could not find the given folder. Please check your input";

            }
            else
            {
                //
                errorMessage.text = "";
                StartCoroutine(RequestUploadCoro());
                Debug.Log("HEEEEEEEEEEEEE");
            }

        }
    }

    IEnumerator RequestUploadCoro()
    {
        string uploadUrl;
        string infoUploadUrl;

        string filename = uploadTitleTextBox.text;
        string folderName = folderNameTextBox.text;
        int anonymous = anonymousToggle.isOn ? 1 : 0; ;


        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "file/getUploadURL", new WWWForm())) // TODO fix this web request
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new UploadReqJson(anonymous, filename, fileTypeDropdown.captionText.text)));
            Debug.Log(JsonUtility.ToJson(new UploadReqJson(anonymous, filename, fileTypeDropdown.captionText.text)));

            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", WebReq.bearerToken);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
                yield break;
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                UploadResJson res = JsonUtility.FromJson<UploadResJson>(www.downloadHandler.text);
                errorMessage.text = "success";
                uploadUrl = res.uploadUrl;
                infoUploadUrl = res.infoUploadUrl;

                Debug.Log(res.uploadUrl);
                Debug.Log(res.infoUploadUrl);
            }
        }

        byte[] fileData;
        byte[] infoData;        

        try
        {
            string filePath = WebReq.objectFolderPath + folderName;
            string fileInfoPath = filePath + "/info";
            string tempZipPath = Application.dataPath + "/" + filename + ".zip";

            ZipFile.CreateFromDirectory(filePath, tempZipPath);
            fileData = File.ReadAllBytes(tempZipPath);
            File.Delete(tempZipPath);

            ZipFile.CreateFromDirectory(fileInfoPath, tempZipPath);
            infoData = File.ReadAllBytes(tempZipPath);
            File.Delete(tempZipPath);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            yield break;
        }

        using(UnityWebRequest www = UnityWebRequest.Put(uploadUrl, fileData))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                yield break;
            }
            else
            {
                Debug.Log("file Upload complete!");
            }
        }

        using (UnityWebRequest www = UnityWebRequest.Put(infoUploadUrl, infoData))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                yield break;
            }
            else
            {
                Debug.Log("file info Upload complete!");
            }
        }
    }
}

