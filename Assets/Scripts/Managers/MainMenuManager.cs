using Plugins.Audio.Core;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : Singleton<MainMenuManager>
{
    public TMP_Text playerNameTxt;
    public RawImage playerAvatarImg;
    public GameObject authBtn;
    public GameObject scoreTxtObj;

    public GameObject loadingPanel;
    public TMP_Text verTxt;
    public SelectLvlBtn[] selectLvlBtns;

    public SourceAudio ost;
    private bool canvasAnimPlaying = false;

    public void DataLoaded()
    {
        loadingPanel.SetActive(false);
        YandexGames.Instance.GameInitialized();

        if (GameData.data.prevGameVersion != Application.version)
        {
            GameData.data.prevGameVersion = Application.version;
            GameData.SaveData();
        }

        for (int i = 0; i < selectLvlBtns.Length; i++)
        {
            SelectLvlBtn.LvlState lvlState = SelectLvlBtn.LvlState.Locked;
            if (GameData.data.cLevelId >= i) lvlState = SelectLvlBtn.LvlState.Unlocked;
            if (GameData.data.cLevelId > i) lvlState = SelectLvlBtn.LvlState.Completed;
            selectLvlBtns[i].Init(i, lvlState);
        }

        if (YandexGames.IsAuth) Authorized();
    }

    public void NameLoaded()
    {
        playerNameTxt.text = GameData.playerName;
    }

    public void SelectLevel(int levelId)
    {
        loadingPanel.SetActive(true);
        GameManager.LvlId = levelId;
        Invoke(nameof(LoadGameScene), 1f);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(GameManager.LvlId+1);
    }

    public void AvatarURLLoaded()
    {
        if (GameData.playerAvatar == null) StartCoroutine(nameof(DownloadPlayerAvatar), GameData.playerAvatarURL);
        else playerAvatarImg.texture = GameData.playerAvatar;
    }

    public void OtherGamesReq()
    {
        Application.OpenURL(@"https://yandex.ru/games/developer/62555");
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
        Application.targetFrameRate = 60;
        if (GameData.dataLoaded) DataLoaded();
        else if (Application.isEditor) GameData.LoadData();
        else loadingPanel.SetActive(true);

        verTxt.text = "v." + Application.version;
        if (GameData.playerName != string.Empty) NameLoaded();
        if (GameData.playerAvatarURL != string.Empty) AvatarURLLoaded();
        ost.Play("MenuOst");
    }
}
