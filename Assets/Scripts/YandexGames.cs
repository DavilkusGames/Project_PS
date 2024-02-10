using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexGames : Singleton<YandexGames>
{
    public delegate void FullscreenAdCallback();
    public delegate void RewardedAdCallback(bool isRewarded);

    #region JS Functions import
    [DllImport("__Internal")] private static extern bool SDKInit();
    [DllImport("__Internal")] private static extern bool PlayerInit();
    [DllImport("__Internal")] private static extern bool AuthCheck();
    [DllImport("__Internal")] private static extern void GameReady();
    [DllImport("__Internal")] private static extern string GetLang();
    [DllImport("__Internal")] private static extern bool IsMobilePlatform();
    [DllImport("__Internal")] private static extern void ShowFullscreenAd();
    [DllImport("__Internal")] private static extern void ShowRewardedAd();
    [DllImport("__Internal")] private static extern void SaveToLb(int score);
    [DllImport("__Internal")] private static extern void SaveCloudData(string data);
    [DllImport("__Internal")] private static extern void LoadCloudData();
    #endregion

    public static bool IsInit { get; private set; }
    public static bool IsRus { get; private set; }
    public static bool IsAuth { get; private set; }
    public static bool IsMobile { get; private set; }

    public LanguageManager languageManager;

    private static string[] RusLangDomens = { "ru", "be", "kk", "uk", "uz" };
    private FullscreenAdCallback fullscreenAdCallback;
    private RewardedAdCallback rewardedAdCallback;
    private float prevAdShowTime = 0f;
    private bool isRewarded = false;

    private void Start()
    {
        if (!Application.isEditor) StartCoroutine(nameof(SDKInit));
    }

    public void DataSaved()
    {

    }

    public void DataLoaded(string data)
    {

    }

    public void FullscreenAdClosed()
    {

    }

    public void Rewarded()
    {

    }

    public void RewardedAdClosed()
    {

    }

    private IEnumerator SDKInitProgress()
    {
        yield return new WaitForSeconds(0.5f);
        while (!SDKInit()) yield return new WaitForSeconds(0.2f);
        IsInit = true;
        prevAdShowTime = Time.time;
        IsRus = RusLangDomens.Contains(GetLang());
        IsMobile = IsMobilePlatform();
        Debug.Log("IsRus: " + IsRus.ToString());
        // TRANSLATE

        while (!PlayerInit()) yield return new WaitForSeconds(0.2f);
        IsAuth = AuthCheck();
        Debug.Log("IsAuth: " + IsAuth.ToString());

        //GameData.LoadData();
    }
}
