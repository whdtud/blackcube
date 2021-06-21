using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;

public class FadeInOutSystem : MonoBehaviour {

	public Image Image;

    public void FadeIn(float smooth = 1f, bool pause = false, Action callback = null)
    {
        gameObject.SetActive(true);

        StopAllCoroutines();

        StartCoroutine(Co_FadeIn(smooth, pause, callback));
    }

    public void FadeOut(float destAlpha, float smooth, bool pause = false, Action callback = null)
    {
        StopAllCoroutines();

        gameObject.SetActive(true);

        StartCoroutine(Co_FadeOut(destAlpha, smooth, pause, callback));
    }

    public void End()
    {
        StopAllCoroutines();

        gameObject.SetActive(false);
    }

    private IEnumerator Co_FadeIn(float smooth = 1f, bool pause = false, Action callback = null)
    {
        float alpha = Image.color.a;

        while (alpha > 0f)
        {
            alpha = Mathf.Max(alpha - Time.unscaledDeltaTime * smooth, 0f);
            Image.color = new Color(0f, 0f, 0f, alpha);

            if (pause)
                Time.timeScale = 1f - alpha;

            yield return null;
        }

        callback?.Invoke();

        gameObject.SetActive(false);
    }

    private IEnumerator Co_FadeOut(float destAlpha, float smooth, bool pause = false, Action callback = null)
    {
        float alpha = 0f;

        while (alpha < destAlpha)
        {
            alpha = Mathf.Min(alpha + Time.unscaledDeltaTime * smooth, destAlpha);
            Image.color = new Color(0f, 0f, 0f, alpha);
            
            if (pause)
                Time.timeScale = 1f - (alpha / destAlpha);

            yield return null;
        }

        callback?.Invoke();
    }
}
