using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
struct DownloadReqJson
{
    public string key;

    public DownloadReqJson(string key)
    {
        this.key = key;
    }
}

[System.Serializable]
struct DownloadResJson
{
    public string status;
    public string URL;
}

public class FileDetailView : MonoBehaviour
{
    public Image fileCoverImage;
    public Text authorName;
    public Text description;
    public Text downloads;
    public Text likes;
    public Text date;

    public string DownloadKey;

    public void init(FileOverview fileOverview)
    {
        fileCoverImage.sprite = fileOverview.fileCoverImage.sprite;
        authorName.text = fileOverview.authorName.text;
        description.text = fileOverview.description.text;
        downloads.text = fileOverview.downloads.text;
        likes.text = fileOverview.likes.text;
        date.text = fileOverview.date.text;

        DownloadKey = fileOverview.key;
    }

    public void DownloadButton()
    {
        StartCoroutine(RequestDownloadCoro());
    }

    IEnumerator RequestDownloadCoro()
    {
        string DownloadURL;

        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "file/getDownloadURL", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new DownloadReqJson(DownloadKey))
                );
            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");

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
                DownloadResJson res = JsonUtility.FromJson<DownloadResJson>(www.downloadHandler.text);
                DownloadURL = res.URL;
                Debug.Log(res.status + " " + res.URL);

                using (UnityWebRequest WebReq = UnityWebRequest.Get(DownloadURL))
                {
                    byte[] fileData;

                    yield return WebReq.SendWebRequest();

                    if (WebReq.isNetworkError || WebReq.isHttpError)
                    {
                        Debug.Log(WebReq.error);
                        yield break;
                    }
                    else
                    {
                        byte[] data = WebReq.downloadHandler.data;
                        
                        try
                        {
                            Download(data);
                        }
                        catch (System.Exception e)
                        {
                            Debug.Log(e);
                            yield break;
                        }
                    }
                }
            }
        }
    }

    public void Download(byte[] data)
    {
        using (MemoryStream zipToOpen = new MemoryStream(data))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                //ZipArchiveEntry fileEntry = archive.GetEntry("modcontent.txt");
                string filePath = WebReq.objectFolderPath + "test";
                Debug.Log(filePath);    
                archive.ExtractToDirectory(filePath);

                // how to get the folder name
                // if folder is exist, then update
                // if foder is not exist, then download
            }
        }
    }
}
