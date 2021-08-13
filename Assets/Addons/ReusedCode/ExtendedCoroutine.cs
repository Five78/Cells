using System;
using System.Collections;
using UnityEngine;

public class ExtendedCoroutine
{
    public MonoBehaviour Owner { get; private set; }
    public Coroutine Coroutine { get; private set; }

    public bool IsProcessing => Coroutine != null;

    public Func<IEnumerator> Routine { get; private set; }

    public event Action OnFinished;

    public ExtendedCoroutine(MonoBehaviour owner, Func<IEnumerator> routine)
    {
        Owner = owner;
        Routine = routine;
    }

    public void Start()
    {
        Stop();

        Coroutine = Owner.StartCoroutine(Process());
    }

    public void Stop()
    {
        if (IsProcessing)
        {
            Owner.StopCoroutine(Coroutine);

            Coroutine = null;
        }
    }

    private IEnumerator Process()
    {
        yield return Routine.Invoke();

        Coroutine = null;

        OnFinished?.Invoke();
    }
}
