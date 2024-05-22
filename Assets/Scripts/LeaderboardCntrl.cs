using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LeaderboardEntry
{
    public string name;
    public int score;
}

public class LeaderboardCntrl : MonoBehaviour
{
    public GameObject lbPlashPrefab;
    public Transform scrollViewParent;
    public Button refreshBtn;
    public GameObject loadingIcon;

    public LeaderboardEntry[] lbEntries;

    private List<GameObject> lbList = new List<GameObject>();
    private bool isLoaded = false;

    public void LoadLeaderboard()
    {
        if (isLoaded) return;
        isLoaded = true;
        Debug.Log("Leaderboard Loading...");
        loadingIcon.SetActive(true);
        Invoke(nameof(LoadLB), 1.5f);
    }

    public void RefreshLeaderboard()
    {
        if (!isLoaded) return;
        if (lbList.Count > 0)
        {
            for (int i = 0; i < lbList.Count; i++)
            {
                Destroy(lbList[i]);
                lbList[i] = null;
            }
        }

        isLoaded = false;
        refreshBtn.interactable = false;
        LoadLeaderboard();
    }

    public void LBLoaded()
    {
        loadingIcon.SetActive(false);
        refreshBtn.interactable = true;
        for (int i = 0; i < lbEntries.Length; i++)
        {
            GameObject lbPlash = Instantiate(lbPlashPrefab);
            lbPlash.transform.SetParent(scrollViewParent);
            lbPlash.transform.localPosition = Vector3.zero;
            lbPlash.transform.localScale = Vector3.one;
            lbPlash.GetComponent<LeaderboardPlash>().Init(lbEntries[i].name, lbEntries[i].score, (i == 0));
            lbList.Add(lbPlash);
        }
    }

    private void LoadLB()
    {
        LBLoaded();
        //YandexGames.Instance.GetLeaderboardRequest();
    }
}
