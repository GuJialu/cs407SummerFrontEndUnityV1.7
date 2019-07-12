using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

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
    public GameObject likeButton;
    public GameObject unlikeButton;

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
                string filename = Regex.Replace(DownloadKey, @"[^0-9a-zA-Z]+", "");
                string filePath = Application.dataPath + "/StreamingAssets/DownloadedGameFiles/" + filename;
                Debug.Log(filePath);

                if (!Directory.Exists(filePath))
                {
                    archive.ExtractToDirectory(filePath);
                }
                else
                {
                    Directory.Delete(filePath);
                    archive.ExtractToDirectory(filePath);
                }
            }
        }
    }
    public void LikeFile()
    {
        // Add element that disables like button.
        likeButton.SetActive(false);
        unlikeButton.SetActive(true);
        StartCoroutine(LikeFileCoro());
    }
    IEnumerator LikeFileCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "/files/likeFile", new WWWForm()))
        {
            // TODO finish HTTP request for file like
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new likeFavoriteReqJson(DownloadKey))
                );
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                description.text = "REEEEE";
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    public void UnlikeFile()
    {
        // TODO remove element from user's favorites.
        // use KEY or EMAIL AND FILENAME
        likeButton.SetActive(true);
        unlikeButton.SetActive(false);
        StartCoroutine(UnlikeFileCoro());
    }
    IEnumerator UnlikeFileCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "/files/unlikeFile", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new unlikeFavoriteReqJson(DownloadKey))
                );
            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {

                Destroy(gameObject);
            }
        }
    }
}
