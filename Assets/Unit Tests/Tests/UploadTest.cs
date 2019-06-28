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
        public IEnumerator UploadPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
        [UnityTest]
        public IEnumerator RequestUploadPasses()
        {
            // TODO check if request upload succeeds

            yield return null;
        }
        [UnityTest]
        public IEnumerator emptyFolderCheckPasses()
        {
            // TODO check if debug message is displayed on empty folder name
            upload.folderNameTextBox.text = "";
            upload.uploadTitleTextBox.text = "Nonempty";
            upload.Uploadfiles();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(upload.errorMessage.Equals("an input field is blank or is white space. please put valid inputs"));

            yield return null;
        }
        [UnityTest]
        public IEnumerator emptyUploadNameCheckPasses()
        {
            // TODO check if debug message is displayed on empty upload name
            upload.folderNameTextBox.text = "Nonempty";
            upload.uploadTitleTextBox.text = "";
            upload.Uploadfiles();
            yield return new WaitForSeconds(2f);

            Assert.IsTrue(upload.errorMessage.Equals("an input field is blank or is white space. please put valid inputs"));
            yield return null;
        }

        [UnityTest]
        public IEnumerator buttonLoadCheckPasses()
        {
            // TODO check if buttons for folder have loaded
            yield return null;
        }
        public IEnumerator buttonListenerWorkPasses()
        {
            // TODO check if button listener changes the input text
            yield return null;
        }

        public IEnumerator cancelButtonPasses()
        {
            // TODO check if cancel button destroys panel
            yield return null;
        }
        public IEnumerator toggleAnonymousPasses()
        {
            // TODO check if anonymous toggle affects upload.
            yield return null;
        }
    }
}
