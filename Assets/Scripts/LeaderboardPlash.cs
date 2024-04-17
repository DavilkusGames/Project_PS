using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPlash : MonoBehaviour
{
    public TMP_Text nicknameTxt;
    public TMP_Text scoreTxt;
    public Color kingColor;

    public void Init(string name, int score, bool isKing)
    {
        nicknameTxt.text = name;
        scoreTxt.text = score.ToString("000000");
        if (isKing) GetComponent<Image>().color = kingColor;
    }
}
