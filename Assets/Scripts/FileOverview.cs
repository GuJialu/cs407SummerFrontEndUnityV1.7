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
    public Text fileName;
    public Text authorName;
    public Text description;
    public Text downloads;
    public Text likes;
    public Text date;
    public GameObject FileDetailViewPanelPrefab;
    public GameObject profilePanelPrefab;
    public string key;
    public string email;
    public GameObject unlikeButton;
    public GameObject deleteButton;
    public GameObject likeButton;
    public Button authorProfileButton;
    string infoDownloadUrl;

    Transform canvasTrans;

    public void Init(FileJson fileJson)
    {
        authorName.text = fileJson.username;
        if (fileJson.anonymous)
        {
            authorName.text = "anonymous";
            authorProfileButton.interactable = false;
        }
        fileName.text = fileJson.fileName;
        downloads.text = fileJson.downloadNum.ToString();
        likes.text = fileJson.likes.ToString();
        string dateStr = System.DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(fileJson.dateUpdated)).UtcDateTime.ToString("MM/dd/yyyy");
        date.text = dateStr;

        infoDownloadUrl = fileJson.infoDownloadUrl.URL;
        Debug.Log(fileJson.infoDownloadUrl.status+" "+infoDownloadUrl);

        key = fileJson.key;
        email = fileJson.email;
        StartCoroutine(RequestDownloadInfoCoro());

        canvasTrans = GetComponentInParent<Canvas>().transform;
    }

    IEnumerator RequestDownloadInfoCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(infoDownloadUrl))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                //Destroy(gameObject);
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
                    if (description.text.Length > 40)
                    {
                        description.text.Substring(0, 40);
                    }
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

    public void OpenProfilePage()
    {
        GameObject profilePage = Instantiate(profilePanelPrefab, canvasTrans);
        profilePage.GetComponent<Profile>().Init(email);
    }

    public void DeleteFile()
    {
        StartCoroutine(DeleteFileCoro());
    }
    IEnumerator DeleteFileCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "file/deleteFile", new WWWForm())) // TODO change element
        {
            // TODO finish HTTP request for file like
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new DeleteFileReqJson(key)) // TODO find correct JSON.
                );

            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", WebReq.bearerToken);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
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
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "file/likeFile", new WWWForm()))
        {
            // TODO finish HTTP request for file like
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new LikeFavoriteReqJson(WebReq.email, key))
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
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl+ "file/unlikeFile", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson( new UnlikeFavoriteReqJson(WebReq.email, key))
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

    public void favoritesView()
    {
        EnableUnlike();
        DisableDelete();
    }
    public void publicView()
    {
        EnableLike();
        DisableDelete();
        DisableUnlike();
    }
    public void creatorView()
    {
        EnableDelete();
        DisableUnlike();
        DisableLike();
    }
    public void EnableLike()
    {
        likeButton.SetActive(true);

    }
    public void EnableUnlike()
    {
        unlikeButton.SetActive(true);
    }
    public void EnableDelete()
    {
        deleteButton.SetActive(true);
    }
    public void DisableLike()
    {
        likeButton.SetActive(false);
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

struct UnlikeFavoriteReqJson
{
    public string email;
    public string key;
    public UnlikeFavoriteReqJson(string email, string key)
    {
        this.email = email;
        this.key = key;
    }
}
struct LikeFavoriteReqJson
{
    public string email;
    public string key;
    public LikeFavoriteReqJson(string email, string key)
    {
        this.email = email;
        this.key = key;
    }
}

struct DeleteFileReqJson
{
    public string key;
    public DeleteFileReqJson(string key)
    {
        this.key = key;
    }
}