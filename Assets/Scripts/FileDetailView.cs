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

[System.Serializable]
struct CommentsReqJson
{
    public string key;

    public CommentsReqJson(string key)
    {
        this.key = key;
    }
}

[System.Serializable]
struct CommentsResJson
{
    public string status;
    public List<CommentJson> comments;
}

[System.Serializable]
public struct CommentJson
{
    public int comment_id;
    public string email;
    public string username;
    public string comment;
    public int like;
    public int dislike;
    public string dateUpdated;
    public bool liked;
    public bool disliked;
}

[System.Serializable]
struct AddCommentReqJson
{
    public string key;
    public string comment;

    public AddCommentReqJson(string key, string comment)
    {
        this.key = key;
        this.comment = comment;
    }
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

    //public Toggle likeToggle;
    public GameObject LikeButton;
    public GameObject UnlikeButton;
    public GameObject loginSignUpPanelPrefab;

    public GameObject commentPrefab;
    public Transform commentScrollContentTrans;

    public InputField commentInput;

    public void init(FileOverview fileOverview)
    {
        fileCoverImage.sprite = fileOverview.fileCoverImage.sprite;
        authorName.text = fileOverview.authorName.text;
        description.text = fileOverview.description.text;
        downloads.text = fileOverview.downloads.text;
        likes.text = fileOverview.likes.text;
        date.text = fileOverview.date.text;
        DownloadKey = fileOverview.key;

        WorkShopEvents.loginEvent.AddListener(RequestComments);

        RequestComments();
        //likeToggle.isOn = false;
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

    public void Like()
    {
        if (WebReq.email == null)
        {
            Instantiate(loginSignUpPanelPrefab, transform.parent);
            return;
        }

        StartCoroutine(LikeFileCoro());
        LikeButton.SetActive(false);
        UnlikeButton.SetActive(true);
    }

    public void UnLike()
    {
        if (WebReq.email == null)
        {
            Instantiate(loginSignUpPanelPrefab, transform.parent);
            return;
        }

        StartCoroutine(LikeFileCoro());
        LikeButton.SetActive(true);
        UnlikeButton.SetActive(false);
    }

    IEnumerator LikeFileCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "file/likeFile", new WWWForm()))
        {
            // TODO finish HTTP request for file like
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new LikeFavoriteReqJson(WebReq.email, DownloadKey))
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

            }
        }
    }

    IEnumerator UnlikeFileCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "file/unlikeFile", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new UnlikeFavoriteReqJson(WebReq.email, DownloadKey))
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

            }
        }
    }

    public void RequestComments()
    {
        StartCoroutine(RequestCommentsCoro());
    }

    IEnumerator RequestCommentsCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "comment/showComment", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new CommentsReqJson(DownloadKey))
                );
            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");

            if (WebReq.email != null)
            {
                www.SetRequestHeader("Authorization", WebReq.bearerToken);
            }

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                CommentsResJson res = JsonUtility.FromJson<CommentsResJson>(www.downloadHandler.text);
                CreateComments(res);
            }
        }
    }

    void CreateComments(CommentsResJson res)
    {
        foreach(Transform transform in commentScrollContentTrans)
        {
            if (transform.GetComponent<Comment>() != null)
            {
                Destroy(transform.gameObject);
            }
        }

        foreach(CommentJson commentJson in res.comments)
        {
            GameObject comment = Instantiate(commentPrefab, commentScrollContentTrans);
            comment.GetComponent<Comment>().Init(commentJson);
        }
    }

    public void AddComment()
    {
        if (WebReq.email == null)
        {
            Instantiate(loginSignUpPanelPrefab, transform.parent);
            return;
        }

        //submit
        StartCoroutine(RequestAddCommentsCoro());
    }

    IEnumerator RequestAddCommentsCoro()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "comment/addComment", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new AddCommentReqJson(DownloadKey, commentInput.text))
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
                //refreash
                RequestComments();
            }
        }
    }
}
