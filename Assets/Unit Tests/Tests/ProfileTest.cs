using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ProfileTest
    {
        LoginAndSignUp loginAndSignUp;
        Profile profile;

        [SetUp]
        public void Setup()
        {
            GameObject loginAndSignUpPanel = Object.Instantiate((GameObject)Resources.Load("Prefabs/LoginAndSignUp Panel"));
            loginAndSignUp = loginAndSignUpPanel.GetComponent<LoginAndSignUp>();
            Debug.Log(loginAndSignUp);

            GameObject profilePanel = Object.Instantiate((GameObject)Resources.Load("Prefabs/Profile Panel"));
            profile = profilePanel.GetComponent<Profile>();
            Debug.Log(profile);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ProfilePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
