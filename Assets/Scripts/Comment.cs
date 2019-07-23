using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using UnityEngine.UI;

[System.Serializable]
struct LikeCommentReqJson
{
    public int comment_id;
    public string email;

    public LikeCommentReqJson(int comment_id, string email)
    {
        this.comment_id = comment_id;
        this.email = email;
    }
}

enum LikeOption
{
    likeComment,
    unlikeComment,
    dislikeComment,
    undislikeComment,
}

public class Comment : MonoBehaviour
{
    public Text authorNameAndTimeText;
    public Text commentText;
    public Image iconImage;
    public SpriteAtlas spriteAtlas;
    public AuthorButton authorButton;
    public int commentID;

    public GameObject likeButton;
    public GameObject dislikeButton;
    public GameObject unlikeButton;
    public GameObject undislikeButton;

    public Text likeNumText;
    public Text dislikeNumText;

    public GameObject loginSignUpPanelPrefab;

    Transform canvasTrans;

    public void Init(CommentJson commentJson)
    {
        string authorName = commentJson.username;
        string dateStr = System.DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(commentJson.dateUpdated)).UtcDateTime.ToString("MM/dd/yyyy");
        authorNameAndTimeText.text = authorName + "        " + dateStr;
        commentText.text = commentJson.comment;
        authorButton.email = commentJson.email;
        commentID = commentJson.comment_id;
        likeNumText.text = commentJson.like.ToString();
        dislikeNumText.text = commentJson.dislike.ToString();

        if (commentJson.liked)
        {
            likeButton.SetActive(false);
            unlikeButton.SetActive(true);
        }
        if (commentJson.disliked)
        {
            dislikeButton.SetActive(false);
            undislikeButton.SetActive(true);
        }

        RequestIcon(commentJson.email);

        canvasTrans = transform.parent.parent;
    }

    void RequestIcon(string email)
    {
        StartCoroutine(RequestIconCoro(email));
    }

    IEnumerator RequestIconCoro(string email)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "profile/viewAll", new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new ProfileReqJson(email)));

            www.uploadHandler = new UploadHandlerRaw(ReqJson);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                ProfileResJson res = JsonUtility.FromJson<ProfileResJson>(www.downloadHandler.text);

                //change the icon
                Sprite[] icons = new Sprite[spriteAtlas.spriteCount];
                spriteAtlas.GetSprites(icons);
                iconImage.sprite = icons[res.icon];
            }
        }
    }

    public void Like()
    {
        if (WebReq.email == null)
        {
            Instantiate(loginSignUpPanelPrefab, canvasTrans);
            return;
        }
        if (undislikeButton.activeSelf)
        {
            UnDislike();
        }
        likeButton.SetActive(false);
        unlikeButton.SetActive(true);
        int.TryParse(likeNumText.text, out int i);
        likeNumText.text = (i + 1).ToString();
        StartCoroutine(RequestLikeCommentsCoro(LikeOption.likeComment));
    }

    public void Dislike()
    {
        if (WebReq.email == null)
        {
            Instantiate(loginSignUpPanelPrefab, canvasTrans);
            return;
        }
        if (unlikeButton.activeSelf)
        {
            UnLike();
        }
        dislikeButton.SetActive(false);
        undislikeButton.SetActive(true);
        int.TryParse(dislikeNumText.text, out int i);
        dislikeNumText.text = (i + 1).ToString();
        StartCoroutine(RequestLikeCommentsCoro(LikeOption.likeComment));
    }

    public void UnLike()
    {
        if (WebReq.email == null)
        {
            Instantiate(loginSignUpPanelPrefab, canvasTrans);
            return;
        }
        likeButton.SetActive(true);
        unlikeButton.SetActive(false);
        int.TryParse(likeNumText.text, out int i);
        likeNumText.text = (i - 1).ToString();
        StartCoroutine(RequestLikeCommentsCoro(LikeOption.unlikeComment));
    }

    public void UnDislike()
    {
        if (WebReq.email == null)
        {
            Instantiate(loginSignUpPanelPrefab, canvasTrans);
            return;
        }
        dislikeButton.SetActive(true);
        undislikeButton.SetActive(false);
        int.TryParse(dislikeNumText.text, out int i);
        dislikeNumText.text = (i - 1).ToString();
        StartCoroutine(RequestLikeCommentsCoro(LikeOption.undislikeComment));
    }

    IEnumerator RequestLikeCommentsCoro(LikeOption likeOption)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebReq.serverUrl + "comment/"+likeOption.ToString(), new WWWForm()))
        {
            byte[] ReqJson = System.Text.Encoding.UTF8.GetBytes(
                JsonUtility.ToJson(new LikeCommentReqJson(commentID, WebReq.email))
                );
            Debug.Log(JsonUtility.ToJson(new LikeCommentReqJson(commentID, WebReq.email)));
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
                //EnableLikeButtons(true);
            }
        }
    }

    /*
    void EnableLikeButtons(bool b)
    {
        if (b)
        {
            likeButton.interactable = true;
            dislikeButton.interactable = true;
        }
        else
        {
            likeButton.interactable = false;
            dislikeButton.interactable = false;
        }
    }
    */
}
