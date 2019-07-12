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
        }

        [UnityTest]
        public IEnumerator FileOverViewPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
