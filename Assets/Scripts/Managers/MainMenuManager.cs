using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenuManager : Singleton<MainMenuManager>
{
    public TMP_Text playerNameTxt;
    public RawImage playerAvatarImg;

    public GameObject loadingPanel;
    public TMP_Text verTxt;

    public void DataLoaded(bool firstTime)
    {
        loadingPanel.SetActive(false);
        if (firstTime) YandexGames.Instance.GameInitialized();

        if (GameData.data.prevGameVersion != Application.version.ToString())
        {
            GameData.data.prevGameVersion = Application.version.ToString();
            GameData.SaveData();
        }
    }

    public void NameLoaded()
    {
        playerNameTxt.text = GameData.playerName;
    }

    public void AvatarURLLoaded()
    {
        if (GameData.playerAvatar == null) DownloadPlayerAvatar(GameData.playerAvatarURL);
        else playerAvatarImg.texture = GameData.playerAvatar;
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
        if (GameData.dataLoaded) DataLoaded(false);
        else if (Application.isEditor) GameData.LoadData();
        else loadingPanel.SetActive(true);

        verTxt.text = "v." + Application.version;
        if (GameData.playerName != string.Empty) NameLoaded();
        if (GameData.playerAvatarURL != string.Empty) AvatarURLLoaded();
    }
}
