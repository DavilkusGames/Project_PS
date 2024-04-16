using TMPro;
using UnityEngine;

public class SelectLvlBtn : MonoBehaviour
{
    public enum LvlState { Locked, Unlocked, Completed };

    public TMP_Text idTxt;
    public GameObject lockedMark;
    public GameObject completedMark;

    private int id = 0;
    private LvlState state;

    public void Init(int id, LvlState state)
    {
        this.id = id;
        this.state = state;
        completedMark.SetActive(state == LvlState.Completed);
        lockedMark.SetActive(state == LvlState.Locked);
        idTxt.text = id.ToString("0000");
    }

    public void Btn_Click()
    {
        if (state == LvlState.Locked) return;
        MainMenuManager.Instance.SelectLevel(id);
    }
}
