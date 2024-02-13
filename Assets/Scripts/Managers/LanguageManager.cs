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
        public List<string> translatedStrs;
    }

    public List<TranslatedText> translatedTexts;

    private void Start()
    {
        //foreach (var tTxt in translatedTexts) tTxt.translatedStrs.Insert(0, tTxt.txt.text);
    }
}
