using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AdminFilePageTest : MonoBehaviour
    {
        LoginAndSignUp loginAndSignUp;
        AdminFilePage adminFilePage;

        [SetUp]
        public void Setup()
        {
            GameObject loginAndSignUpPanel = Object.Instantiate((GameObject)Resources.Load("Prefabs/LoginAndSignUp Panel"));
            loginAndSignUp = loginAndSignUpPanel.GetComponent<LoginAndSignUp>();
            Debug.Log(loginAndSignUp);

            GameObject o = new GameObject("cavasObj");
            o.AddComponent<Canvas>();
            GameObject adminFilePagePanel = Object.Instantiate((GameObject)Resources.Load("Prefabs/Administration File Page Panel"), o.transform);
            adminFilePage = adminFilePagePanel.GetComponent<AdminFilePage>();
            Debug.Log(adminFilePage);
        }
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator AdminFilePagePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            loginAndSignUp.loginEmailInput.text = "FilePageTest@email.com";
            loginAndSignUp.loginPasswordInput.text = "12";
            loginAndSignUp.RequestLogin();

            yield return new WaitForSeconds(5f);

            adminFilePage.Init(WebReq.email);

            yield return new WaitForSeconds(5f);

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

            yield return new WaitForSeconds(5f);

            adminFilePage.Init(WebReq.email);
            yield return new WaitForSeconds(5f);
            Assert.IsTrue(adminFilePage.filePanel.transform.childCount > 0);

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

            yield return new WaitForSeconds(5f);

            adminFilePage.Init(WebReq.email);
            yield return new WaitForSeconds(5f);

            FileOverview fileOverview = adminFilePage.filePanel.transform.GetChild(5).GetComponent<FileOverview>();
            Assert.IsTrue(fileOverview.authorName.text.Equals("anonymous"));

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePageNoFilePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            loginAndSignUp.loginEmailInput.text = "nofiletest@email.com";
            loginAndSignUp.loginPasswordInput.text = "1234";
            loginAndSignUp.RequestLogin();

            yield return new WaitForSeconds(5f);

            Debug.Log(WebReq.email);
            adminFilePage.Init(WebReq.email);
            yield return new WaitForSeconds(5f);

            Assert.IsTrue(adminFilePage.filePanel.transform.childCount == 0);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePagePublicPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            adminFilePage.Init();
            yield return new WaitForSeconds(5f);

            Assert.IsTrue(adminFilePage.filePanel.transform.childCount > 0);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePageSortByDatePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            adminFilePage.Init();
            yield return new WaitForSeconds(5f);

            Assert.IsTrue(adminFilePage.filePanel.transform.childCount > 0);

            adminFilePage.sortMethodDropdown.value = 0;
            adminFilePage.ReloadFilePanel();

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePageSortByDownloadsPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            adminFilePage.Init();
            yield return new WaitForSeconds(5f);

            Assert.IsTrue(adminFilePage.filePanel.transform.childCount > 0);

            adminFilePage.sortMethodDropdown.value = 1;
            adminFilePage.ReloadFilePanel();

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePageSortByLikesPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            adminFilePage.Init();
            yield return new WaitForSeconds(5f);

            Assert.IsTrue(adminFilePage.filePanel.transform.childCount > 0);

            adminFilePage.sortMethodDropdown.value = 2;
            adminFilePage.ReloadFilePanel();
            yield return new WaitForSeconds(5f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePageFilterByVisualModPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            adminFilePage.Init();
            yield return new WaitForSeconds(5f);

            Assert.IsTrue(adminFilePage.filePanel.transform.childCount > 0);
            adminFilePage.filterDropdown.value = 1;
            adminFilePage.ReloadFilePanel();
            yield return new WaitForSeconds(5f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePageFilterByUIModPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            adminFilePage.Init();
            yield return new WaitForSeconds(5f);

            Assert.IsTrue(adminFilePage.filePanel.transform.childCount > 0);
            adminFilePage.filterDropdown.value = 5;
            adminFilePage.ReloadFilePanel();
            yield return new WaitForSeconds(5f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePageFilterByGameLogicModPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            adminFilePage.Init();
            yield return new WaitForSeconds(5f);

            Assert.IsTrue(adminFilePage.filePanel.transform.childCount > 0);
            adminFilePage.filterDropdown.value = 3;
            adminFilePage.ReloadFilePanel();
            yield return new WaitForSeconds(5f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePageFilterTimePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            adminFilePage.Init();
            yield return new WaitForSeconds(5f);

            Assert.IsTrue(adminFilePage.filePanel.transform.childCount > 0);
            adminFilePage.timeDropdown.value = 1;
            adminFilePage.ReloadFilePanel();
            yield return new WaitForSeconds(5f);

            adminFilePage.timeDropdown.value = 5;
            adminFilePage.ReloadFilePanel();
            yield return new WaitForSeconds(5f);

            adminFilePage.timeDropdown.value = 3;
            adminFilePage.ReloadFilePanel();
            yield return new WaitForSeconds(5f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePageSearchPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            adminFilePage.Init();
            yield return new WaitForSeconds(5f);

            adminFilePage.keywordInput.text = "Jia";
            adminFilePage.searchByContributorToggle.isOn = false;
            adminFilePage.KeywordSearch();
            yield return new WaitForSeconds(5f);
            Assert.IsTrue(adminFilePage.keywordPanel.activeSelf);

            adminFilePage.UndoKeywordSearch();
            yield return new WaitForSeconds(5f);

            adminFilePage.searchByContributorToggle.isOn = true;
            adminFilePage.KeywordSearch();
            yield return new WaitForSeconds(5f);
            Assert.IsTrue(adminFilePage.keywordPanel.activeSelf);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePageDeleteFile()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            loginAndSignUp.loginEmailInput.text = "testuser001@email.com";
            loginAndSignUp.loginPasswordInput.text = "1234";
            loginAndSignUp.RequestLogin();
            yield return new WaitForSeconds(5f);

            adminFilePage.Init(WebReq.email);
            yield return new WaitForSeconds(5f);

            foreach (Transform fileOverviewTran in adminFilePage.filePanel.transform)
            {
                fileOverviewTran.GetComponent<FileOverview>().EnableDelete();
            }
            foreach (Transform fileOverviewTran in adminFilePage.filePanel.transform)
            {
                fileOverviewTran.GetComponent<FileOverview>().DisableDelete();
            }
            yield return new WaitForSeconds(5f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AdminFilePageDeleteComment()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            loginAndSignUp.loginEmailInput.text = "testuser001@email.com";
            loginAndSignUp.loginPasswordInput.text = "1234";
            loginAndSignUp.RequestLogin();
            yield return new WaitForSeconds(5f);

            adminFilePage.Init(WebReq.email);
            yield return new WaitForSeconds(5f);
            Assert.IsTrue(WebReq.isAdmin);

            yield return null;
        }
    }
}