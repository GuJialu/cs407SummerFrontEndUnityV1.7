using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FileOverViewTest
    {
        FileOverview fileOverview;

        [SetUp]
        public void Setup()
        {
            GameObject o = new GameObject("cavasObj");
            o.AddComponent<Canvas>();
            GameObject fileOverviewPanel = Object.Instantiate((GameObject)Resources.Load("Prefabs/File overview Panel"), o.transform);
            fileOverview = fileOverviewPanel.GetComponent<FileOverview>();
            Debug.Log(fileOverview);

            FileJson fileJson = new FileJson();
            fileJson.anonymous = false;
            fileJson.email = "FilePageTest@email.com";
            fileJson.dateUpdated = "1561736000534";
            fileJson.fileName = "";
            fileJson.infoDownloadUrl = new InfoDownloadUrl();
            fileJson.infoDownloadUrl.status = 200;
            fileJson.infoDownloadUrl.URL = "";
            fileJson.key = "FilePageTest@email.com|SpangeBob";

            fileOverview.Init(fileJson);
        }

        [UnityTest]
        public IEnumerator FileOverViewPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        [UnityTest]
        public IEnumerator FileOverViewOpenAuthorProfilePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            fileOverview.OpenProfilePage();

            yield return null;
        }
    }
}
