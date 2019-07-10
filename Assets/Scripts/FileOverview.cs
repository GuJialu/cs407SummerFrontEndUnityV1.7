using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FileOverview : MonoBehaviour
{
    public Image fileCoverImage;
    public Text authorName;
    public Text description;
    public Text downloads;
    public Text likes;
    public Text date;
    public GameObject FileDetailViewPanelPrefab;
    string key;
    public GameObject unlikeButton;
    public GameObject deleteButton;
    string infoDownloadUrl;

    public void Init(FileJson fileJson)
    {
        authorName.text = fileJson.username;
        if (fileJson.anonymous)
        {
            authorName.text = "anonymous";
        }
        downloads.text = fileJson.downloadNum.ToString();
        likes.text = fileJson.likes.ToString();
        date.text = fileJson.dateUpdated;
        infoDownloadUrl = fileJson.infoDownloadUrl;
        // Add information that sets the like or dislike button according to the state of favorites.
        key = fileJson.key;
        StartCoroutine(RequestDownloadInfoCoro());
    }

    IEnumerator RequestDownloadInfoCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(infoDownloadUrl))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                yield break;
            }
            else
            {
                Debug.Log("file info download complete!");
                byte[] data = www.downloadHandler.data;
                UpdateInfo(data);
            }
        }
    }


    public void UpdateInfo(byte[] data)
    {
        using (MemoryStream zipToOpen = new MemoryStream(data))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                ZipArchiveEntry desEntry = archive.GetEntry("des.txt");
                using (StreamReader reader = new StreamReader(desEntry.Open()))
                {
                    description.text = reader.ReadToEnd();
                }
                ZipArchiveEntry imageEntry = archive.GetEntry("workingspace.PNG");
                Texture2D Tex2D;
                Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
                using (MemoryStream ms = new MemoryStream())
                {
                    imageEntry.Open().CopyTo(ms);
                    Tex2D.LoadImage(ms.ToArray()); 
                }
                fileCoverImage.sprite = Sprite.Create(Tex2D, new Rect(0, 0, Tex2D.width, Tex2D.height), new Vector2(0, 0));
            }
        }
    }

    public void OpenDetialedPage()
    {
        GameObject fileDetailViewPanel = Instantiate(FileDetailViewPanelPrefab, transform.parent.parent);
        fileDetailViewPanel.GetComponent<FileDetailView>().init(this);
    }

    public void DeleteFile()
    {
        StartCoroutine(DeleteFileCoro());
    }
    IEnumerator DeleteFileCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "/file/likeFile", new WWWForm())) // TODO change element
        {
            // TODO finish HTTP request for file like
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new likeFavoriteReqJson(key)) // TODO find correct JSON.
                );
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
    public void LikeFile()
    {
        // Add element that disables like button.
        StartCoroutine(LikeFileCoro());
    }
    IEnumerator LikeFileCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "/file/likeFile", new WWWForm()))
        {
            // TODO finish HTTP request for file like
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new likeFavoriteReqJson(key))
                );
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
    public void UnlikeFile()
    {
        // TODO remove element from user's favorites.
        // use KEY or EMAIL AND FILENAME

        StartCoroutine(UnlikeFileCoro());
    }
    IEnumerator UnlikeFileCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl+ "/file/unlikeFile", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson( new unlikeFavoriteReqJson(key))
                );
            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                
                Destroy(gameObject);
            }
        }
    }
    public void EnableUnlike()
    {
        unlikeButton.SetActive(true);
    }
    public void EnableDelete()
    {
        deleteButton.SetActive(true);
    }
    public void DisableUnlike()
    {
        unlikeButton.SetActive(false);
    }
    public void DisableDelete()
    {
        deleteButton.SetActive(false);
    }
}

struct unlikeFavoriteReqJson
{
    public string key;
    public unlikeFavoriteReqJson(string key)
    {
        this.key = key;
    }
}
struct likeFavoriteReqJson
{
    public string key;
    public likeFavoriteReqJson(string key)
    {
        this.key = key;
    }
}