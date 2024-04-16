
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public TMP_Text levelLabelTxt;
    public Animator canvasAnim;

    public static int LvlId = 0;

    private bool pauseState = false;

    private void Start()
    {
        Application.targetFrameRate = 60;
        levelLabelTxt.text = LvlId.ToString("0000");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SetPauseState(!pauseState);
    }

    public void SetPauseState(bool state)
    {
        pauseState = state;
        canvasAnim.Play(pauseState ? "PauseAnim" : "UnpauseAnim");
    }
}
