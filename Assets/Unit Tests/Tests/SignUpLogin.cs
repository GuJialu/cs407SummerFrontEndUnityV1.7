using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SignUpLogin
    {
        LoginAndSignUp loginAndSignUp;

        [SetUp]
        public void Setup()
        {
            GameObject loginAndSignUpPanel = (GameObject)Resources.Load("Prefabs/LoginAndSignUp Panel");
            loginAndSignUp = loginAndSignUpPanel.GetComponent<LoginAndSignUp>();
            Debug.Log(loginAndSignUp);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(loginAndSignUp.gameObject);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SignUpLoginWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            Debug.Log(loginAndSignUp);

            yield return null;
        }

        [UnityTest]
        public IEnumerator LoginWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            Debug.Log(loginAndSignUp);

            yield return null;
        }
    }
}
