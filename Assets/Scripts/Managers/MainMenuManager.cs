using Plugins.Audio.Core;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenuManager : Singleton<MainMenuManager>
{
    public TMP_Text playerNameTxt;
    public RawImage playerAvatarImg;
    public GameObject authBtn;
    public GameObject scoreTxtObj;

    public GameObject loadingPanel;
    public TMP_Text verTxt;

    public SourceAudio ost;

    public void DataLoaded()
    {
        loadingPanel.SetActive(false);
        YandexGames.Instance.GameInitialized();

        if (GameData.data.prevGameVersion != Application.version)
        {
            GameData.data.prevGameVersion = Application.version;
            GameData.SaveData();
        }

        if (YandexGames.IsAuth) Authorized();
    }

    public void NameLoaded()
    {
        playerNameTxt.text = GameData.playerName;
    }

    public void AvatarURLLoaded()
    {
        if (GameData.playerAvatar == null) StartCoroutine(nameof(DownloadPlayerAvatar), GameData.playerAvatarURL);
        else playerAvatarImg.texture = GameData.playerAvatar;
    }

    public void AuthRequestBtn()
    {
        YandexGames.Instance.AuthBtnRequest();
    }

    public void Authorized()
    {
        authBtn.SetActive(false);
        scoreTxtObj.SetActive(true);
        LanguageManager.Instance.SetAdditionalText(6, ' ' + GameData.data.score.ToString());
    }

    public void PromoActive()
    {
        // PROMO ACTIVE!
    }

    private IEnumerator DownloadPlayerAvatar(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
            Debug.Log(request.error);
        else
        {
            GameData.playerAvatar = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Debug.Log("Player avatar downloaded");
            playerAvatarImg.texture = GameData.playerAvatar;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 30;
        if (GameData.dataLoaded) DataLoaded();
        else if (Application.isEditor) GameData.LoadData();
        else loadingPanel.SetActive(true);

        verTxt.text = "v." + Application.version;
        ost.Play("Missing");
        if (GameData.playerName != string.Empty) NameLoaded();
        if (GameData.playerAvatarURL != string.Empty) AvatarURLLoaded();
    }
}
