using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Threading.Tasks;
using Firebase.Extensions;
public class FirebaseAuthentication : MonoBehaviour
{
    public Canvas LoginPage;
    public Canvas SignUp;
    public Canvas SelectionRoom;

    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField nickname;

    public TMP_InputField login_email;
    public TMP_InputField login_password;

    private FirebaseAuth auth;
    private DatabaseReference databaseReference;

    public TextMeshProUGUI errorText;
    public TextMeshProUGUI errorText_SignUp;
    public TextMeshProUGUI introText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI Emailused;

    bool signInCompleted = false;
    bool signUpCompleted = false;
    bool errorEnable = false;
    bool showIntroText = false;
    bool showFeedbackText = false;
    bool showEmailUsed = false;

    List<string> existingEmails = new List<string>();

    string nicknamefound;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Update()
    {
        if (showIntroText)
        {
            introText.text = "Welcome, " + nicknamefound + "!";
            introText.gameObject.SetActive(true);
            // Debug.Log(introText.text);
        }

        if (signInCompleted)
        {
            LoginPage.gameObject.SetActive(false);
            SelectionRoom.gameObject.SetActive(true);
            signInCompleted = false; // Reset the flag

            // Get and display user data
            GetUserData();
        }

        if (signUpCompleted)
        {
            SignUp.gameObject.SetActive(false);
            SelectionRoom.gameObject.SetActive(true);
            signUpCompleted = false; // Reset the flag
        }

        if (errorEnable)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Invalid email or password. Please try again."; // Set the error text
            errorEnable = false;
        }

        if (showFeedbackText)
        {
            feedbackText.gameObject.SetActive(true);
            showFeedbackText = false;
        }

        if (showEmailUsed)
        {
            Emailused.gameObject.SetActive(true);
            showEmailUsed = false;
        }
    }

    private void GetUserData()
    {
        if (auth.CurrentUser != null)
        {
            string userId = auth.CurrentUser.UserId;
            //Debug.Log("this is the userId: " + userId);

            // Reference to the "users" node in the database
            databaseReference.Child("users").Child(userId).GetValueAsync().ContinueWith(task =>
            {
                Debug.Log("i am in dataserefernce");
                if (task.IsCompleted)
                {
                    //Debug.Log("i am in task checker");
                    DataSnapshot snapshot = task.Result;
                    nicknamefound = snapshot.Child("nickname").Value.ToString();
                    showIntroText = true;
                }
            });
        }
    }

    public void SignIn()
    {
        // Check if email or password fields are empty
        if (string.IsNullOrEmpty(login_email.text) || string.IsNullOrEmpty(login_password.text))
        {
            // Set the input fields' color to red
            login_email.image.color = Color.red;
            login_password.image.color = Color.red;

            // Enable the TextMeshProUGUI component and set the error text
            if (errorText != null)
            {
                errorText.gameObject.SetActive(true);
                errorText.text = "Please enter email and password."; // Set the error text
            }

            return;
        }

        // Clear the red color if previous errors were displayed
        login_email.image.color = Color.white;
        login_password.image.color = Color.white;

        auth.SignInWithEmailAndPasswordAsync(login_email.text, login_password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                // Handle authentication failures
                if (errorText != null)
                {
                    Debug.Log("Setting error text: Invalid email or password. Please try again.");
                    errorText.gameObject.SetActive(true);
                    errorText.text = "Invalid email or password. Please try again."; // Set the error text
                }

                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Authentication succeeded
            Firebase.Auth.FirebaseUser user = task.Result;

            Debug.Log("Firebase user signed in successfully: " + user.UserId);

            // Set the flag to indicate that sign-in is completed
            signInCompleted = true;
        });
    }

    public void SignUpAccount()
    {
        // Check if any of the required fields are empty
        if (string.IsNullOrEmpty(email.text) || string.IsNullOrEmpty(password.text) || string.IsNullOrEmpty(nickname.text))
        {
            // Set the input fields' color to red
            email.image.color = Color.red;
            password.image.color = Color.red;
            nickname.image.color = Color.red;

            // Enable the TextMeshProUGUI component and set the error text
            if (errorText_SignUp != null)
            {
                Emailused.gameObject.SetActive(false);
                errorText_SignUp.gameObject.SetActive(true);
                errorText_SignUp.text = "Please enter all the fields"; // Set the error text
            }

            return;
        }

        // Clear the red color if previous errors were displayed
        email.image.color = Color.white;
        password.image.color = Color.white;
        nickname.image.color = Color.white;

        // Attempt to create the user with the provided credentials
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("Email is already in use by another account.");
                // Check if the error is due to email already in use
                // Enable emailUsed flag
                showEmailUsed = true;
                return;
            }

            // Firebase user has been created successfully.
            Firebase.Auth.FirebaseUser user = task.Result;
            Debug.Log("Firebase user created successfully: " + user.UserId);

            // Save user data to the Realtime Database
            SaveUserData(user.UserId, email.text, nickname.text);

            // Set the flag to indicate that sign-up is completed
            signUpCompleted = true;
            GetUserData();
            showIntroText = true;
        });
    }

    void SaveUserData(string userId, string email, string nickname)
    {
        // Create a User object with the required data
        User user = new User(email, nickname);

        // Convert the User object to JSON
        string json = JsonUtility.ToJson(user);

        Debug.Log("working adding to database " + userId);
        // Save the user data to the Realtime Database
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
        Debug.Log("DONE working adding to database");
    }

    public void OnResetPasswordClick()
    {
        // Call Firebase method to send a password reset email
        auth.SendPasswordResetEmailAsync(login_email.text).ContinueWith(task =>
        {

            if (task.IsCanceled)
            {
                Debug.LogError("Password reset email send canceled.");
                feedbackText.text = "Password reset email sending canceled.";
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("Error sending password reset email: " + task.Exception);
                feedbackText.text = "Error sending password reset email. Check your email address.";
                return;
            }

            Debug.Log("Password reset email sent successfully.");
            feedbackText.text = "Password reset email sent. Check your email for further instructions.";
            showFeedbackText = true;
        });
    }

    // User class to represent user data
    [System.Serializable]
    public class User
    {
        public string email;
        public string nickname;

        public User(string email, string nickname)
        {
            this.email = email;
            this.nickname = nickname;
        }
    }
}