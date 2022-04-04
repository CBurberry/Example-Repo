using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TranslationState 
{
    Ready,
    Busy
};

public class ToggleTranslation : MonoBehaviour
{
    public bool IsShowing => gameObject.activeSelf;

    [BoxGroup("Transitions")]
    [SerializeField] Vector3 offScreenPosition = Vector3.zero;
    [BoxGroup("Transitions")]
    [SerializeField] Vector3 visiblePosition = Vector3.zero;
    [BoxGroup("Transitions")]
    [SerializeField] float showDuration = 0.5f;
    [BoxGroup("Transitions")]
    [SerializeField] float hideDuration = 0.5f;

    TranslationState state;

    private void Start()
    {
        state = TranslationState.Ready;
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

    public IEnumerator Show()
    {
        if (state == TranslationState.Busy)
        {
            yield break;
        }

        state = TranslationState.Busy;
        //Entry transition tween
        yield return transform.DOMove(visiblePosition, showDuration).WaitForCompletion();
        state = TranslationState.Ready;
    }


    public IEnumerator Hide()
    {
        if (state == TranslationState.Busy)
        {
            yield break;
        }

        state = TranslationState.Busy;

        //Exit transition tween
        yield return transform.DOMove(offScreenPosition, hideDuration).WaitForCompletion();
        gameObject.SetActive(false);
        state = TranslationState.Ready;
    }
}
