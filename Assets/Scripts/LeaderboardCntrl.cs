using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardCntrl : MonoBehaviour
{
    public GameObject lbPlashPrefab;
    public Transform scrollViewParent;
    public Button refreshBtn;
    public GameObject loadingIcon;

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

        refreshBtn.interactable = false;
        LoadLeaderboard();
    }

    public void LBLoaded()
    {
        loadingIcon.SetActive(false);
        refreshBtn.interactable = true;
        for (int i = 0; i < 5; i++)
        {
            GameObject lbPlash = Instantiate(lbPlashPrefab);
            lbPlash.transform.SetParent(scrollViewParent);
            lbPlash.transform.localPosition = Vector3.zero;
            lbPlash.transform.localScale = Vector3.one;
            lbPlash.GetComponent<LeaderboardPlash>().Init("TEST", 31151, (i == 0));
            lbList.Add(lbPlash);
        }
    }

    private void LoadLB()
    {
        LBLoaded();
    }
}
