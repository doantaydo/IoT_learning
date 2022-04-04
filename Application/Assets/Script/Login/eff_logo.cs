using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class eff_logo : MonoBehaviour
{
    float start_A;
    SpriteRenderer renderer;
    public GameObject inputZone;
    private CanvasGroup _canvasLayer;
    private Tween twenFade;

    private bool device_status = false;
    void Start()
    {
        start_A = 0f;
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(1f, 1f, 1f, start_A);
        _canvasLayer = inputZone.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start_A < 1f) {
            start_A += 0.01f;
            renderer.color = new Color(1f, 1f, 1f, start_A);
            if (start_A > 0.9f) {
                inputZone.SetActive(true);
                FadeIn(_canvasLayer, 0.25f);
            }
            
        }
    }

    void Fade(CanvasGroup _canvas, float endValue, float duration, TweenCallback onFinish)
    {
        if (twenFade != null)
        {
            twenFade.Kill(false);
        }

        twenFade = _canvas.DOFade(endValue, duration);
        twenFade.onComplete += onFinish;
    }

    void FadeIn(CanvasGroup _canvas, float duration)
    {
        Fade(_canvas, 1f, duration, () =>
        {
            _canvas.interactable = true;
            _canvas.blocksRaycasts = true;
        });
    }
}
