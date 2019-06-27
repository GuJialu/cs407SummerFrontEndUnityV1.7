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
        public IEnumerator LoginPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            loginAndSignUp.signUpEmailInput.text = "testuser4@email.com";
            loginAndSignUp.signUpPasswordInput.text = "1234";
            loginAndSignUp.signUpRenterPasswordInput.text = "1234";
            loginAndSignUp.signUpUserNameInput.text = "testuser";
            loginAndSignUp.RequestSignUp();
            yield return new WaitForSeconds(2f);

            loginAndSignUp.loginEmailInput.text = "testuser4@email.com";
            loginAndSignUp.loginPasswordInput.text = "1234";
            loginAndSignUp.RequestLogin();

            yield return new WaitForSeconds(2f);

            Assert.IsNotNull(WebReq.bearerToken);
            Assert.IsNotNull(WebReq.email);
            //Assert.IsNull(loginAndSignUp);

            yield return null;
        }

        [UnityTest]
        public IEnumerator ProfileUsernameChangePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            // test case change user name from testuser4 to testuser5
            profile.usernameText.text = "testuser5";
            profile.ChangeUsername();

            yield return new WaitForSeconds(2f);

            Assert.IsNotEmpty("profile change username error");

            yield return null;
        }

        [UnityTest]
        public IEnumerator ProfileUserDescriptionChangePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            // test case change user name from testuser4 to testuser5
            profile.inputField.text = "description change test";
            profile.ChangeDescription();

            yield return new WaitForSeconds(2f);

            Assert.IsNotEmpty("profile change description error");

            yield return null;
        }

        [UnityTest]
        public IEnumerator ProfileUserIconChangePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            // test case change user name from testuser4 to testuser5
            profile.ChangeIcon();
            profile.SetIcon(3);

            yield return new WaitForSeconds(2f);

            Assert.IsNotEmpty("profile change icon error");

            yield return null;
        }

        [UnityTest]
        public IEnumerator ProfileUserPasswordChangePasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            // test case change user name from testuser4 to testuser5
            profile.ChangePassword();

            yield return new WaitForSeconds(2f);

            Assert.IsNotEmpty("profile change password error");

            yield return null;
        }
    }
}
