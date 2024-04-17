using UnityEngine;
using UnityEngine.UI;

public class BlackPanel : MonoBehaviour
{
    public delegate void ActionCallback();

    private float fadeSpeed = 1f;
    private Image img;
    private float startAlpha = 0f;
    private float targetAlpha = 0f;
    private float curAlpha = 0f;
    private float progress = 0f;
    private ActionCallback callback = null;

    private void Awake() { img = GetComponent<Image>(); }

    private void Update()
    {
        if (curAlpha != targetAlpha)
        {
            progress += fadeSpeed * Time.deltaTime;
            if (progress >= 1f)
            {
                progress = 1f;
                if (callback != null)
                {
                    callback();
                    callback = null;
                }
            }

            curAlpha = Mathf.Lerp(startAlpha, targetAlpha, progress);
            img.color = new Color(0f, 0f, 0f, curAlpha);
        }
    }

    public void FadeIn(ActionCallback callback, float fadeSpeed = 1f)
    {
        this.fadeSpeed = fadeSpeed;

        startAlpha = 0f;
        targetAlpha = 1f;
        curAlpha = startAlpha;
        progress = 0f;
        SetUIBlock(true);
        this.callback = callback;
    }

    public void FadeOut(ActionCallback callback, float fadeSpeed = 1f)
    {
        this.fadeSpeed = fadeSpeed;

        startAlpha = 1f;
        targetAlpha = 0f;
        curAlpha = startAlpha;
        progress = 0f;
        SetUIBlock(false);
        this.callback = callback;
    }

    private void SetUIBlock(bool isBlocking)
    {
        img.raycastTarget = isBlocking;
    }
}
