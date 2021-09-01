using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsWindowController : MonoBehaviour
{
    [SerializeField] private AudioSettingsWindget _music;
    [SerializeField] private AudioSettingsWindget _sfx;

    private Animator _animator;
    private GameSession _session;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _session = GameSession.Instance;
    }

    private void Start()
    {
        _music.SetModel(GameSettings.I.Music);
        _sfx.SetModel(GameSettings.I.Sfx);
    }

    public void Close()
    {
        _animator.SetTrigger("close");
    }

    public void OnCloseAnimatorComplete()
    {
        Destroy(gameObject);
    }
}
