using UnityEngine;

public class RulesWindowController : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
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
