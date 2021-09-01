using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesWindowController : MonoBehaviour
{
    private Animator _animator;
    private GameSession _session;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _session = FindObjectOfType<GameSession>();
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
