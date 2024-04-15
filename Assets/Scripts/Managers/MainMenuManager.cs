using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    public GameObject loadingPanel;

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

    public void PromoActive()
    {

    }

    private void Start()
    {
        Application.targetFrameRate = 30;
        if (GameData.dataLoaded) DataLoaded(false);
        else if (Application.isEditor) GameData.LoadData();
        else loadingPanel.SetActive(true);
    }
}
