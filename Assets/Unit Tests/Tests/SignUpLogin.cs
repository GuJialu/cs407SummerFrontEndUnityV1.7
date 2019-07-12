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
            GameObject loginAndSignUpPanel = Object.Instantiate((GameObject)Resources.Load("Prefabs/LoginAndSignUp Panel"));
            loginAndSignUp = loginAndSignUpPanel.GetComponent<LoginAndSignUp>();
            Debug.Log(loginAndSignUp);
        }

        [TearDown]
        public void TearDown()
        {

        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SignUpPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            Debug.Log(loginAndSignUp);
            loginAndSignUp.signUpEmailInput.text = "testuser4@email.com";
            loginAndSignUp.signUpPasswordInput.text = "1234";
            loginAndSignUp.signUpRenterPasswordInput.text = "1234";
            loginAndSignUp.signUpUserNameInput.text = "testuser";
            loginAndSignUp.RequestSignUp();
            yield return new WaitForSeconds(2f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SignUpPasswordRenterPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            Debug.Log(loginAndSignUp);
            loginAndSignUp.signUpEmailInput.text = "testuser4@email.com";
            loginAndSignUp.signUpPasswordInput.text = "1234";
            loginAndSignUp.signUpRenterPasswordInput.text = "12342";
            loginAndSignUp.signUpUserNameInput.text = "testuser";
            loginAndSignUp.RequestSignUp();

            Assert.IsNotEmpty(loginAndSignUp.errorMessageText.text);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SignUpInvaildPasswordPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            Debug.Log(loginAndSignUp);
            loginAndSignUp.signUpEmailInput.text = "testuser4@email.com";
            loginAndSignUp.signUpPasswordInput.text = "";
            loginAndSignUp.signUpRenterPasswordInput.text = "";
            loginAndSignUp.signUpUserNameInput.text = "testuser";
            loginAndSignUp.RequestSignUp();

            Assert.IsNotEmpty(loginAndSignUp.errorMessageText.text);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SignUpInvalidEmailPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            Debug.Log(loginAndSignUp);

            loginAndSignUp.signUpPasswordInput.text = "1234";
            loginAndSignUp.signUpRenterPasswordInput.text = "1234";
            loginAndSignUp.signUpUserNameInput.text = "testuser";

            loginAndSignUp.signUpEmailInput.text = "testuser4email.com";
            loginAndSignUp.RequestSignUp();

            Assert.IsNotEmpty(loginAndSignUp.errorMessageText.text);

            loginAndSignUp.signUpEmailInput.text = "testu额@email.com";
            loginAndSignUp.RequestSignUp();

            Assert.IsNotEmpty(loginAndSignUp.errorMessageText.text);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SignUpDuplicateEmailPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            Debug.Log(loginAndSignUp);
            loginAndSignUp.signUpEmailInput.text = "testuser4@email.com";
            loginAndSignUp.signUpPasswordInput.text = "1234";
            loginAndSignUp.signUpRenterPasswordInput.text = "1234";
            loginAndSignUp.signUpUserNameInput.text = "testuser";
            loginAndSignUp.RequestSignUp();

            yield return new WaitForSeconds(2f);

            Debug.Log(loginAndSignUp);
            loginAndSignUp.signUpEmailInput.text = "testuser4@email.com";
            loginAndSignUp.signUpPasswordInput.text = "1234";
            loginAndSignUp.signUpRenterPasswordInput.text = "1234";
            loginAndSignUp.signUpUserNameInput.text = "testuser";
            loginAndSignUp.RequestSignUp();

            yield return new WaitForSeconds(2f);

            Assert.IsNotEmpty(loginAndSignUp.errorMessageText.text);

            yield return null;
        }

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

            //Assert.IsNotNull(WebReq.bearerToken);
            //Assert.IsNotNull(WebReq.email);
            //Assert.IsNull(loginAndSignUp);

            yield return null;
        }

        [UnityTest]
        public IEnumerator LoginIncorrectPasswordPasses()
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
            loginAndSignUp.loginPasswordInput.text = "12345";
            loginAndSignUp.RequestLogin();

            Assert.IsNotEmpty(loginAndSignUp.errorMessageText.text);

            yield return null;
        }
    }
}
