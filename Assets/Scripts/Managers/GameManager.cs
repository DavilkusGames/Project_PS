
using Plugins.Audio.Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int scoreForLvl = 500;
    public TMP_Text levelLabelTxt;
    public Animator canvasAnim;
    public SourceAudio ost;
    public BlackPanel blackPanel;
    public TMP_Text plusScoreTxt;

    public static int LvlId = 0;

    private bool pauseState = false;
    private bool levelCompleted = false;

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

    public void LevelCompleted()
    {
        if (GameData.data.cLevelId == LvlId) GameData.data.cLevelId++;
        else scoreForLvl = 0;
        GameData.data.score += scoreForLvl;
        plusScoreTxt.text = "+ " + scoreForLvl.ToString();
        LanguageManager.Instance.SetAdditionalText(8, ' ' + GameData.data.score.ToString("000000"));
        canvasAnim.Play("LevelCompletedAnim");
        GameData.SaveData();
    }

    public void BlocksPanelState(bool state)
    {
        canvasAnim.Play(state ? "ShowBlocks" : "HideBlocks");
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        blackPanel.FadeIn(LoadMainMenu, 2f);
    }

    public void RetryLevel()
    {
        blackPanel.FadeIn(LoadRetry, 2f);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadRetry()
    {
        SceneManager.LoadScene(LvlId + 1);
    }

    public void SetPauseState(bool state)
    {
        if (levelCompleted) return;
        pauseState = state;
        canvasAnim.Play(pauseState ? "PauseAnim" : "UnpauseAnim");
        Time.timeScale = state ? 0f : 1f;
    }
}
