using UnityEngine;

public class AudioUtils
{
    public const string SfxSourseTag = "SfxAudioSource";
    public static AudioSource FindSfxSource()
    {
        return GameObject.FindWithTag(SfxSourseTag).GetComponent<AudioSource>();
    }
}
