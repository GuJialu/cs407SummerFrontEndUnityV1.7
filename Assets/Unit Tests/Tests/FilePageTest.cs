using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FilePageTest
    {
        LoginAndSignUp loginAndSignUp;
        FilePage filePage;

        [SetUp]
        public void Setup()
        {
            GameObject loginAndSignUpPanel = Object.Instantiate((GameObject)Resources.Load("Prefabs/LoginAndSignUp Panel"));
            loginAndSignUp = loginAndSignUpPanel.GetComponent<LoginAndSignUp>();
            Debug.Log(loginAndSignUp);

            GameObject o = new GameObject("cavasObj");
            o.AddComponent<Canvas>();
            GameObject filePagePanel = Object.Instantiate((GameObject)Resources.Load("Prefabs/File Page Panel"), o.transform);
            filePage = filePagePanel.GetComponent<FilePage>();
            Debug.Log(filePage);
        }
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator FilePagePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            loginAndSignUp.loginEmailInput.text = "FilePageTest@email.com";
            loginAndSignUp.loginPasswordInput.text = "12";
            loginAndSignUp.RequestLogin();

            yield return new WaitForSeconds(2f);

            filePage.Init(WebReq.email);

            yield return new WaitForSeconds(2f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator ViewFileUploadedPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            loginAndSignUp.loginEmailInput.text = "FilePageTest@email.com";
            loginAndSignUp.loginPasswordInput.text = "12";
            loginAndSignUp.RequestLogin();

            yield return new WaitForSeconds(2f);

            filePage.Init(WebReq.email);
            yield return new WaitForSeconds(2f);
            Assert.IsTrue(filePage.filePanel.transform.childCount > 0);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AnonmyousAuthorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            loginAndSignUp.loginEmailInput.text = "FilePageTest@email.com";
            loginAndSignUp.loginPasswordInput.text = "12";
            loginAndSignUp.RequestLogin();

            yield return new WaitForSeconds(2f);

            filePage.Init(WebReq.email);
            yield return new WaitForSeconds(2f);

            FileOverview fileOverview = filePage.filePanel.transform.GetChild(2).GetComponent<FileOverview>();
            Assert.IsTrue(fileOverview.authorName.text.Equals("anonymous"));

            yield return null;
        }

        [UnityTest]
        public IEnumerator FilePageNoFilePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            loginAndSignUp.loginEmailInput.text = "nofiletest@email.com";
            loginAndSignUp.loginPasswordInput.text = "1234";
            loginAndSignUp.RequestLogin();

            yield return new WaitForSeconds(2f);

            Debug.Log(WebReq.email);
            filePage.Init(WebReq.email);
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(filePage.filePanel.transform.childCount == 0);

            yield return null;
        }

        [UnityTest]
        public IEnumerator FilePagePublicPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            filePage.Init();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(filePage.filePanel.transform.childCount > 0);

            yield return null;
        }

        [UnityTest]
        public IEnumerator FilePageSortByDatePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            filePage.Init();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(filePage.filePanel.transform.childCount > 0);

            filePage.sortMethodDropdown.value = 0;
            filePage.ReloadFilePanel();

            yield return null;
        }

        [UnityTest]
        public IEnumerator FilePageSortByDownloadsPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            filePage.Init();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(filePage.filePanel.transform.childCount > 0);

            filePage.sortMethodDropdown.value = 1;
            filePage.ReloadFilePanel();

            yield return null;
        }

        [UnityTest]
        public IEnumerator FilePageSortByLikesPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            filePage.Init();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(filePage.filePanel.transform.childCount > 0);

            filePage.sortMethodDropdown.value = 2;
            filePage.ReloadFilePanel();
            yield return new WaitForSeconds(2f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator FilePageFilterByVisualModPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            filePage.Init();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(filePage.filePanel.transform.childCount > 0);
            filePage.filterDropdown.value = 1;
            filePage.ReloadFilePanel();
            yield return new WaitForSeconds(2f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator FilePageFilterByUIModPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            filePage.Init();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(filePage.filePanel.transform.childCount > 0);
            filePage.filterDropdown.value = 2;
            filePage.ReloadFilePanel();
            yield return new WaitForSeconds(2f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator FilePageFilterByGameLogicModPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            filePage.Init();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(filePage.filePanel.transform.childCount > 0);
            filePage.filterDropdown.value = 3;
            filePage.ReloadFilePanel();
            yield return new WaitForSeconds(2f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator FilePageFilterTimePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            filePage.Init();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(filePage.filePanel.transform.childCount > 0);
            filePage.timeDropdown.value= 1;
            filePage.ReloadFilePanel();
            yield return new WaitForSeconds(2f);

            filePage.timeDropdown.value = 2;
            filePage.ReloadFilePanel();
            yield return new WaitForSeconds(2f);

            filePage.timeDropdown.value = 3;
            filePage.ReloadFilePanel();
            yield return new WaitForSeconds(2f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator FilePageSearchPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            filePage.Init();
            yield return new WaitForSeconds(2f);

            filePage.keywordInput.text = "Jia";
            filePage.searchByContributorToggle.isOn = false;
            filePage.KeywordSearch();
            yield return new WaitForSeconds(2f);
            Assert.IsTrue(filePage.keywordPanel.activeSelf);

            filePage.UndoKeywordSearch();
            yield return new WaitForSeconds(2f);

            filePage.searchByContributorToggle.isOn = true;
            filePage.KeywordSearch();
            yield return new WaitForSeconds(2f);
            Assert.IsTrue(filePage.keywordPanel.activeSelf);

            yield return null;
        }
    }
}
