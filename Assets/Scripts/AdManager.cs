using UnityEngine;
using System.Collections;
using Heyzap;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    enum AdStatus
    {
        None,
        WaitingForTime,
        WaitingForDisplay,
        Showed
    }

    public enum IncentivizedAdType
    {
        None = 0,
        BiggerBase
    }


    public delegate void NoAdsEvent();

    public static event NoAdsEvent OnNoAdsPurchased;

    public bool NoAds
    {
        get
        {
            return PlayerPrefs.GetInt("NoAds", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("NoAds", value ? 1 : 0);
        }
    }


    public delegate void IncentivizedAdEvent(IncentivizedAdType adTag, bool success);

    public static event IncentivizedAdEvent OnIncentivizedAdWatched;

    private void IncentivizedAdWatched(IncentivizedAdType adTag, bool success)
    {
        if (OnIncentivizedAdWatched != null)
            OnIncentivizedAdWatched(adTag, success);
    }


    public static AdManager instance = null;

    [HideInInspector]
    public bool showAds = true;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }

    //blic Text debugText;
    void Start()
    {
#if !UNITY_EDITOR
        HeyzapAds.Start("2dea7f3a29b36270e2a12558c659cb29", HeyzapAds.FLAG_NO_OPTIONS);
        HeyzapAds.HideDebugLogs();

        HZIncentivizedAd.AdDisplayListener incentivizedListener = delegate (string adState, string adTag)
        {
            if (adState.Equals("incentivized_result_complete"))
            {
                //IncentivizedAdWatched ((IncentivizedAdType)System.Enum.Parse (typeof(IncentivizedAdType), adTag), true);
                IncentivizedAdWatched(IncentivizedAdType.BiggerBase, true);

                //onIncentivizedAdSuccess (adTag);
            }
            else if (adState.Equals("incentivized_result_incomplete"))
            {
                //IncentivizedAdWatched ((IncentivizedAdType)System.Enum.Parse (typeof(IncentivizedAdType), adTag), false);
                IncentivizedAdWatched(IncentivizedAdType.BiggerBase, false);

            }
            else if (adState.Equals("failed"))
            {
                //IncentivizedAdWatched ((IncentivizedAdType)System.Enum.Parse (typeof(IncentivizedAdType), adTag), false);

                IncentivizedAdWatched(IncentivizedAdType.BiggerBase, false);
            }


            //Debug.Log("adState : " + adState +" -- adTag : "+adTag);

        };

        HZIncentivizedAd.SetDisplayListener(incentivizedListener);

        IncentivizedAdFetch(AdManager.IncentivizedAdType.BiggerBase);
#endif

    }

    //public void PurchaseNoAds()
    //{
    //    NoAds = true;
    //    HideBanner();
    //    StopAllCoroutines();
    //    //GameAnalytics.NewDesignEvent ("NoAdsPurchased");
    //    if (OnNoAdsPurchased != null)
    //        OnNoAdsPurchased();
    //}

    public void StartCountDown()
    {
        if (!NoAds && adStatus == AdStatus.None)
            StartCoroutine(CountDownForAds());
    }

    void OnApplicationPause(bool paused)
    {
        if (!paused && adStatus != AdStatus.None)
        {
            StopAllCoroutines();
            adStatus = AdStatus.None;
            StartCountDown();
        }
    }


    public bool ShowInterstitialAd()
    {
        if (adStatus == AdStatus.WaitingForDisplay)
        {
            HZInterstitialAd.Show();
            adStatus = AdStatus.Showed;
            return true;
        }
        return false;
    }

    public void InterstitialAdFetch()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
            HZInterstitialAd.Fetch();
    }


    public void ShowIncentivizedAd(IncentivizedAdType adTag)
    {
        if (HZIncentivizedAd.IsAvailable(adTag.ToString()))
        {
            HZIncentivizedShowOptions showOptions = new HZIncentivizedShowOptions();
            showOptions.Tag = adTag.ToString();
            HZIncentivizedAd.ShowWithOptions(showOptions);
        }
        else
        {
            IncentivizedAdFetch(adTag);
        }
    }

    public void IncentivizedAdFetch(IncentivizedAdType adTag)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
            HZIncentivizedAd.Fetch(adTag.ToString());
    }

    public bool IsIncentivizedAdAvaiable(IncentivizedAdType adTag)
    {
        return HZIncentivizedAd.IsAvailable(adTag.ToString());
    }


    //public void ShowBannerAtBottom()
    //{
    //    if (!NoAds)
    //    {
    //        HZBannerShowOptions opt = new HZBannerShowOptions();
    //        opt.Position = HZBannerShowOptions.POSITION_BOTTOM;
    //        opt.Tag = "bannerTag";
    //        HZBannerAd.ShowWithOptions(opt);
    //    }
    //}

    //public void ShowBannerAtTop()
    //{
    //    if (!NoAds)
    //    {
    //        HZBannerShowOptions opt = new HZBannerShowOptions();
    //        opt.Position = HZBannerShowOptions.POSITION_TOP;
    //        opt.Tag = "bannerTag";
    //        opt.SelectedFacebookSize = HZBannerShowOptions.FacebookSize.BANNER_HEIGHT_50;
    //        opt.SelectedAdMobSize = HZBannerShowOptions.AdMobSize.BANNER;
    //        HZBannerAd.ShowWithOptions(opt);
    //    }
    //}

    public void HideBanner()
    {
        HZBannerAd.Hide();
    }

    AdStatus adStatus = AdStatus.None;

    private IEnumerator CountDownForAds()
    {

        float currTime = 0;
        adStatus = AdStatus.Showed;

        while (true)
        {
            if (adStatus == AdStatus.WaitingForTime)
            {
                //GamePhoDebug.Log("Time : "+ (Time.realtimeSinceStartup - currTime));
                if (Time.realtimeSinceStartup - currTime >= 60)
                {
                    if (HZInterstitialAd.IsAvailable())
                        adStatus = AdStatus.WaitingForDisplay;
                    else
                        InterstitialAdFetch();
                }
            }
            else if (adStatus == AdStatus.Showed)
            {
                currTime = Time.realtimeSinceStartup;
                adStatus = AdStatus.WaitingForTime;
                InterstitialAdFetch();
            }
            yield return new WaitForEndOfFrame();
        }
    }
}