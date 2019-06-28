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

    string infoDownloadUrl;

    public void Init(FileJson fileJson)
    {
        authorName.text = fileJson.username;
        downloads.text = fileJson.downloadNum.ToString();
        likes.text = fileJson.likes.ToString();
        date.text = fileJson.dateUpdated;
        infoDownloadUrl = fileJson.infoDownloadUrl;
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
}

