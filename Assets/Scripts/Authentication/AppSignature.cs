using System;
using UnityEngine;

public class AppSignature
{
    public enum Fingerprint { MD5 = 0, SHA1 = 1 };

    public static string GetCertificateFingerprint(Fingerprint fingerprint)
    {
        string packageName = "com.utools.appcertificatemanager.CertificateManager";

        AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

        AndroidJavaObject certificateManager = new AndroidJavaObject(packageName, context);
        if (fingerprint == Fingerprint.MD5)
            return certificateManager.Call<string>("getCertificateMD5Fingerprint");
        else if (fingerprint == Fingerprint.SHA1)
            return certificateManager.Call<string>("getCertificateSHA1Fingerprint");
        else
            throw new ArgumentException("Invalid value. Fingerprint can be only 0 (MD5) or 1 (SHA1)");
    }
}
