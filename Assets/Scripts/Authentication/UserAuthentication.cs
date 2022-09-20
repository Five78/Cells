using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class UserAuthentication : MonoBehaviour
{   
    private FirebaseUser _user;

    private void Awake()
    {
        string sha1 = AppSignature.GetCertificateFingerprint(AppSignature.Fingerprint.SHA1);

        try
        {
            SignInToFirebase("kfrwirk@mail.com", sha1);
        } catch (FirebaseException)
        {
            FirebaseDatabase.DefaultInstance.GoOffline();
            Application.Quit();
        }
    }

    private void SignInToFirebase(string email, string password)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled");
                return;
            }

            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            _user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                _user.DisplayName, _user.UserId);
        });
    }
}
