using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DetailedFileOverviewTests
    {
        FileDetailView detail;

        [SetUp]
        public void Setup()
        {
            GameObject detailPanel = Object.Instantiate((GameObject)Resources.Load("Prefabs/Detail Panel"));
            detail = detailPanel.GetComponent<FileDetailView>();
            Debug.Log(detail);
        }

        [UnityTest]
        public IEnumerator InfoDisplayPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            Debug.Log(detail);
            Texture2D tex;
            Sprite testSprite;

            tex = new Texture2D(128, 128);
            testSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

            detail.fileCoverImage.sprite = testSprite;
            detail.authorName.text = "test user";
            detail.description.text = "test description";
            detail.downloads.text = "0";
            detail.likes.text = "0";
            detail.date.text = "dd/mm/yy";

            detail.DownloadKey = "FilePageTest@email.com|SpongeBob";

            yield return new WaitForSeconds(2f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator DownloadPasses()
        {
            detail.DownloadButton();
            yield return new WaitForSeconds(2f);

            yield return null;
        }
    }
}