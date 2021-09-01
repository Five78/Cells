using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private bool _playAgain;
    private bool _onPlay = false;

    private AudioSource _source;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_onPlay) return;

        if (_source == null)
            _source = AudioUtils.FindSfxSource();

        _source.PlayOneShot(_clip);
        if(!_playAgain)
            _onPlay = true;
    }
}
