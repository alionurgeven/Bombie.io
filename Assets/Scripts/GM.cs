using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour {

    public static GM Instance;
    public GameObject Player;
    public GameObject MainMenu, HUD, StartButton, StartBiggerButton;
    public InputField NameField;
    public TextMesh PlayerName;

    [SerializeField]
    private int MAX_NUM_NATURALPIECES = 150;

	// Use this for initialization
	private void Start () {
        SpawnNaturalPieces();
        SpawnIdleAI();

        if (PlayerPrefs.GetInt("StartBiggerReady") == 1)
        {
            StartBiggerButton.SetActive(true);
        }
        else
        {
            StartBiggerButton.SetActive(false);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        NameField.text = PlayerPrefs.GetString("PlayerName");
    }
    

    private Vector3 MAX_MAP_BOUNDARY = new Vector3(50f,50f,0);
    private Vector3 spawnPosition;
    private float x, y;
    private void SpawnNaturalPieces()
    {
        for (int i = 0; i < 150; i++)
        {
            x = Random.Range(-MAX_MAP_BOUNDARY.x, MAX_MAP_BOUNDARY.x);
            y = Random.Range(-MAX_MAP_BOUNDARY.y, MAX_MAP_BOUNDARY.y);
            spawnPosition = Vector3.right * x + Vector3.up * y;
            PoolManager.Instance.Spawn("NaturalPieces", spawnPosition,Quaternion.identity);
        }
    }
    private void SpawnIdleAI()
    {
        for (int i = 0; i < 100; i++)
        {
            x = Random.Range(-MAX_MAP_BOUNDARY.x, MAX_MAP_BOUNDARY.x);
            y = Random.Range(-MAX_MAP_BOUNDARY.y, MAX_MAP_BOUNDARY.y);
            spawnPosition = Vector3.right * x + Vector3.up * y;
            PoolManager.Instance.Spawn("IdleAI", spawnPosition, Quaternion.identity);
        }
    }

    public void StartGame()
    {
        sessionStarted = true;
        startedBigger = false;
        Player.SetActive(true);
        MainMenu.SetActive(false);
        HUD.SetActive(true);
        x = Random.Range(-MAX_MAP_BOUNDARY.x, MAX_MAP_BOUNDARY.x);
        y = Random.Range(-MAX_MAP_BOUNDARY.y, MAX_MAP_BOUNDARY.y);
        spawnPosition = Vector3.right * x + Vector3.up * y;
        Player.transform.position = spawnPosition;
        PlayerPrefs.SetString("PlayerName", NameField.text);
        PlayerName.text = NameField.text;
    }

    public bool startedBigger;
    public void StartBigger()
    {
        sessionStarted = true;
        startedBigger = true;
        Player.SetActive(true);
        MainMenu.SetActive(false);
        HUD.SetActive(true);
        x = Random.Range(-MAX_MAP_BOUNDARY.x, MAX_MAP_BOUNDARY.x);
        y = Random.Range(-MAX_MAP_BOUNDARY.y, MAX_MAP_BOUNDARY.y);
        spawnPosition = Vector3.right * x + Vector3.up * y;
        Player.transform.position = spawnPosition;
        PlayerPrefs.SetString("PlayerName", NameField.text);
        PlayerName.text = NameField.text;
    }
    
    public void EndGame()
    {
        Player.SetActive(false);
        MainMenu.SetActive(true);
        HUD.SetActive(false);
        //TODO - ALi send session time to analytics
        Debug.Log("Session Time: " + sessionTime);
        sessionTime = 0;
        sessionStarted = false;
        if (Player.GetComponent<Player>().GetScore() >= 150)
        {
            PlayerPrefs.SetInt("StartBiggerReady" , 1);
        }
    }

    private bool sessionStarted;
    private float sessionTime;
    private void Update()
    {
        if (sessionStarted)
        {
            sessionTime += Time.deltaTime;
        }
    }
    bool facebookPageOppening = false, twitterOppening = false;
    bool started = false;

    IEnumerator OpenFacebookPageRoutine()
    {
        facebookPageOppening = true;
        yield return new WaitForSeconds(1);

#if UNITY_EDITOR
        Application.OpenURL("https://www.facebook.com/bombie.io/");
#endif
#if UNITY_ANDROID
        Application.OpenURL("fb://page/466902510356051");
#elif UNITY_IOS
		Application.OpenURL ("fb://profile/1870447899866581");
#endif

        facebookPageOppening = false;
    }

    IEnumerator OpenTwitterPageRoutine()
    {
        twitterOppening = true;
        Application.OpenURL("twitter://user?screen_name=io_bombie");

        yield return new WaitForSeconds(1);
        Application.OpenURL("https://www.twitter.com/io_bombie/");

        twitterOppening = false;
    }

    public void OpenFacebookPage()
    {
        if (!facebookPageOppening)
            StartCoroutine(OpenFacebookPageRoutine());
    }

    public void OpenTwitterPage()
    {
        if (!twitterOppening)
            StartCoroutine(OpenTwitterPageRoutine());
    }
    void OnEnable()
    {
        AdManager.OnIncentivizedAdWatched += AdManager_OnIncentivizedAdWatched;
    }
    void OnDisable()
    {
        AdManager.OnIncentivizedAdWatched -= AdManager_OnIncentivizedAdWatched;
    }
    private void AdManager_OnIncentivizedAdWatched(AdManager.IncentivizedAdType adTag, bool success)
    {
        if (success)
        {
            StartBigger();
            PlayerPrefs.SetInt("StartBiggerReady", 0);
        }
        else
        {
            StartGame();
        }
    }
    public AdManager.IncentivizedAdType CurrentRewardedMode
    {
        get
        {
            return (AdManager.IncentivizedAdType)PlayerPrefs.GetInt("CurrentRewardedMode", (int)AdManager.IncentivizedAdType.None);
        }
        set
        {
            PlayerPrefs.SetInt("CurrentRewardedMode", (int)value);
        }
    }
    public void ShowIncentivizedAd()
    {
        CurrentRewardedMode = AdManager.IncentivizedAdType.BiggerBase;
        AdManager.instance.ShowIncentivizedAd(AdManager.IncentivizedAdType.BiggerBase);
    }
}
