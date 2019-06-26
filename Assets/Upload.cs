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
    public bool anonymous;
    public string fileName;
    public UploadReqJson(bool anonymous, string fileName)
    {
        this.anonymous = anonymous;
        this.fileName = fileName;
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

    // public GameObject fileName;
    // Start is called before the first frame update

    void Start()
    {

        // GameObject b= Instantiate(localFileButtonPrefab, FileScrollViewContent.transform);
        // b.GetComponentInChildren<Text>().text = "example";
        // var myButton = (GameObject)Instantiate(localFileButtonPrefab)
        // Button button = myButton.getCompo
        // git need to be fixed
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
                StartCoroutine(RequestUploadCoro(WebReq.email, anonymousToggle.isOn, uploadTitleTextBox.text));
                Debug.Log("HEEEEEEEEEEEEE");
            }

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

    IEnumerator RequestUploadCoro(string email, bool anonymous, string fileName)
    {
        string uploadUrl;
        string infoUploadUrl;

        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "account/tobeimplent", new WWWForm())) // TODO fix this web request
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new UploadReqJson(anonymous, fileName)));

            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                yield break;
            }
            else
            {
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
            string filePath = WebReq.objectFolderPath + fileName;
            string fileInfoPath = filePath + "info";
            string tempZipPath = Application.dataPath + fileName + ".zip";

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

