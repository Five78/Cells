using UnityEngine;

public class StickSound : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    private bool _onPlay = false;

    private AudioSource _source;

    public void OnPointerClick()
    {
        if (_onPlay) return;

        if (_source == null)
            _source = AudioUtils.FindSfxSource();

        _source.PlayOneShot(_clip);

        _onPlay = true;
    }
}
