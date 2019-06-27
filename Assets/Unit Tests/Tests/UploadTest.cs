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
    }
}
