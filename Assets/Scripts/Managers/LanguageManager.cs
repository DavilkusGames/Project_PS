using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageManager : Singleton<LanguageManager>
{
    [System.Serializable]
    public class TranslatedText
    {
        public TMP_Text txt;
        public string rusStr;
        public string engStr;

        public string additionalTxt = string.Empty;
    }

    public List<TranslatedText> translatedTexts;
    private static bool IsRus = false;

    private void Start()
    {
        SetLang(IsRus);
    }

    public void SetLang(bool isRus)
    {
        IsRus = isRus;

        for (int i = 0; i < translatedTexts.Count; i++)
        {
            translatedTexts[i].txt.text = (IsRus ? translatedTexts[i].rusStr : translatedTexts[i].engStr) +
                translatedTexts[i].additionalTxt;
        }
    }

    public void SetText(int id, string rusTxt, string engTxt)
    {
        translatedTexts[id].rusStr = rusTxt;
        translatedTexts[id].engStr = engTxt;
        translatedTexts[id].txt.text = (IsRus ? translatedTexts[id].rusStr : translatedTexts[id].engStr) +
            translatedTexts[id].additionalTxt;
    }

    public void SetAdditionalText(int id, string txt)
    {
        translatedTexts[id].additionalTxt = txt;
        translatedTexts[id].txt.text = (IsRus ? translatedTexts[id].rusStr : translatedTexts[id].engStr) + 
            translatedTexts[id].additionalTxt;
    }
}
