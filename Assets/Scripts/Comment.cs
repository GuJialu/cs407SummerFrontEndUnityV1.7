using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Comment : MonoBehaviour
{
    public Text authorNameAndTimeText;
    public Text commentText;
    public Image iconImage;
    public SpriteAtlas spriteAtlas;
    public AuthorButton authorButton;

    public void Init(CommentJson commentJson)
    {
        string authorName = commentJson.username;
        string dateStr = System.DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(commentJson.dateUpdated)).UtcDateTime.ToString("MM/dd/yyyy");
        authorNameAndTimeText.text = authorName + "        " + dateStr;
        commentText.text = commentJson.comment;
        authorButton.email = commentJson.email;
        RequestIcon(commentJson.email);
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
}
