using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

public enum UIState 
{
    Ready,
    Busy
};

public class PatientChart : MonoBehaviour
{
    public bool IsShowing => gameObject.activeSelf;

    [BoxGroup("Transitions")]
    [SerializeField] Vector2 offScreenPosition = new Vector3(0f, -800f);
    [BoxGroup("Transitions")]
    [SerializeField] Vector2 visiblePosition = new Vector3(0f, 20f);
    [BoxGroup("Transitions")]
    [SerializeField] float showDuration = 0.5f;
    [BoxGroup("Transitions")]
    [SerializeField] float hideDuration = 0.5f;

    UIState state;

    private void Start()
    {
        state = UIState.Ready;
    }

    public void Toggle() 
    {
        if (IsShowing)
        {
            StartCoroutine(Hide());
        }
        else 
        {
            gameObject.SetActive(true);
            StartCoroutine(Show());
        }
    }

    private IEnumerator Show()
    {
        if (state == UIState.Busy) 
        {
            yield break;
        }

        state = UIState.Busy;
        //Entry transition tween
        yield return transform.DOMove(visiblePosition, showDuration).WaitForCompletion();
        state = UIState.Ready;
    }


    private IEnumerator Hide() 
    {
        if (state == UIState.Busy)
        {
            yield break;
        }

        state = UIState.Busy;

        //Exit transition tween
        yield return transform.DOMove(offScreenPosition, hideDuration).WaitForCompletion();
        gameObject.SetActive(false);
        state = UIState.Ready;
    }
}
