
using Plugins.Audio.Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public TMP_Text levelLabelTxt;
    public Animator canvasAnim;
    public SourceAudio ost;
    public BlackPanel blackPanel;

    public static int LvlId = 0;

    private bool pauseState = false;

    private void Start()
    {
        Application.targetFrameRate = 60;
        levelLabelTxt.text = LvlId.ToString("0000");
        blackPanel.FadeOut(null, 2f);
        ost.Play("GameOst");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SetPauseState(!pauseState);
    }

    public void ToMainMenu()
    {
        blackPanel.FadeIn(LoadMainMenu, 2f);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetPauseState(bool state)
    {
        pauseState = state;
        canvasAnim.Play(pauseState ? "PauseAnim" : "UnpauseAnim");
    }
}
