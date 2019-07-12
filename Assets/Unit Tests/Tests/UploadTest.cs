using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UploadTest
    {
        LoginAndSignUp loginAndSignUp;
        Upload upload;

        [SetUp]
        public void Setup()
        {
            GameObject loginAndSignUpPanel = Object.Instantiate((GameObject)Resources.Load("Prefabs/LoginAndSignUp Panel"));
            loginAndSignUp = loginAndSignUpPanel.GetComponent<LoginAndSignUp>();
            Debug.Log(loginAndSignUp);

            GameObject uploadPanel = Object.Instantiate((GameObject)Resources.Load("Prefabs/Upload Panel"));
            upload = uploadPanel.GetComponent<Upload>();
            Debug.Log(upload);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.

        [UnityTest]
        public IEnumerator RequestUploadPasses()
        {
            // TODO check if request upload succeeds
            upload.folderNameTextBox.text = "mod1";
            upload.uploadTitleTextBox.text = "Nonempty";
            WebReq.bearerToken = null;
            upload.Uploadfiles();

            Assert.IsTrue(upload.errorMessage.text.Equals(""));

            yield return null;
        }
        [UnityTest]
        public IEnumerator EmptyFolderCheckPasses()
        {
            upload.folderNameTextBox.text = "";
            upload.uploadTitleTextBox.text = "Nonempty";
            upload.Uploadfiles();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(upload.errorMessage.text.Equals("an input field is blank or is white space. please put valid inputs"));

            yield return null;
        }
        [UnityTest]
        public IEnumerator EmptyUploadNameCheckPasses()
        {
            upload.folderNameTextBox.text = "Nonempty";
            upload.uploadTitleTextBox.text = "";
            upload.Uploadfiles();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(upload.errorMessage.text.Equals("an input field is blank or is white space. please put valid inputs"));

            yield return null;
        }
        [UnityTest]
        public IEnumerator UploadPass()
        {
            loginAndSignUp.loginEmailInput.text = "testuser4@email.com";
            loginAndSignUp.loginPasswordInput.text = "1234";
            loginAndSignUp.RequestLogin();

            yield return new WaitForSeconds(2f);

            upload.folderNameTextBox.text = "mod5";
            upload.uploadTitleTextBox.text = "UploadTest";
            upload.Uploadfiles();
            yield return new WaitForSeconds(2f);

            //Assert.IsTrue(upload.overwriteConfirmPanel.activeSelf);
            yield return null;
        }
        [UnityTest]
        public IEnumerator UploadOverwritePass()
        {
            loginAndSignUp.loginEmailInput.text = "testuser4@email.com";
            loginAndSignUp.loginPasswordInput.text = "1234";
            loginAndSignUp.RequestLogin();

            yield return new WaitForSeconds(2f);

            upload.folderNameTextBox.text = "mod5";
            upload.uploadTitleTextBox.text = "UploadTest";
            upload.Uploadfiles();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(upload.overwriteConfirmPanel.activeSelf);

            upload.RequestOverwriteUpload();
            yield return new WaitForSeconds(2f);

            yield return null;
        }
        [UnityTest]
        public IEnumerator UploadOverwriteInfoOnlyPass()
        {
            loginAndSignUp.loginEmailInput.text = "testuser4@email.com";
            loginAndSignUp.loginPasswordInput.text = "1234";
            loginAndSignUp.RequestLogin();

            yield return new WaitForSeconds(2f);

            upload.folderNameTextBox.text = "mod5";
            upload.uploadTitleTextBox.text = "UploadTest";
            upload.Uploadfiles();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(upload.overwriteConfirmPanel.activeSelf);

            upload.RequestOverwriteUploadInfoOnly();
            yield return new WaitForSeconds(2f);

            yield return null;
        }
    }
}
