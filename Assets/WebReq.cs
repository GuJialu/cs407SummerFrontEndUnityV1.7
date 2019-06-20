﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;

public static class WebReq
{
    static string objectFolderPath = Application.dataPath + "/StreamingAssets/LocalGameFiles/";
    static string tempZipFolderPath = Application.dataPath + "/StreamingAssets/tempZips/";
    static string storageUrl;
    static string serverUrl;
    static string bearerToken;

    static IEnumerator SignUp(string email, string password, string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", email);
        form.AddField("password", password);
        using (UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", form))
        {

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    static IEnumerator Upload(string filePath)
    {
        //First request Upload to get upload url

        //Second upload file to upload url
        yield return null;
    }

    static IEnumerator ResquestUpload(string objectName)
    {
        UnityWebRequest www = UnityWebRequest.Get(serverUrl + "/reqUpload");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    static IEnumerator UploadFile(string url, string fileName)
    {
        byte[] myData;
        string objectPath = objectFolderPath + fileName;
        string tempZipPath = tempZipFolderPath + fileName + ".zip";

        try
        {
            ZipFile.CreateFromDirectory(objectPath, tempZipPath);
            myData = File.ReadAllBytes(tempZipPath);
            File.Delete(tempZipPath);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            yield break;
        }

        UnityWebRequest www = UnityWebRequest.Put(url, myData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
    }

}
