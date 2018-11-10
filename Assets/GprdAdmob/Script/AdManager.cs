using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{

    public static AdManager Instance
    {
        get;
        set;
    }
    public bool InterstitialLoaded = false;
    public bool InterstitialClosed = false;
    
    public string BannerAdId = "ca-app-pub-3940256099942544/6300978111";
    public string InterstitialAdId = "ca-app-pub-3940256099942544/1033173712";
    InterstitialAd interstitialAd;
    AdRequest request;
    private BannerView bannerView;
    public GameObject NOEU;
    public GameObject YesEu;
    public GameObject NoNO;
    public void requestAds()
    {
        if (PlayerPrefs.GetInt("NoPersonalized") == 1)
        {
            request = new AdRequest.Builder().AddExtra("npa", "1").Build();
            Debug.Log("NoPersonalized ads EU");
            NOEU.SetActive(true);
            YesEu.SetActive(false);
            NoNO.SetActive(false);

        }
        else
        {
            if (PlayerPrefs.GetInt("FirstTime") == 1)
            {
                if (PlayerPrefs.GetInt("Localized") == 1)
                {
                    //request = new AdRequest.Builder().Build();
                    request = new AdRequest.Builder().Build();
                    Debug.Log("Personalized ads EU");
                    NOEU.SetActive(false);
                    YesEu.SetActive(true);
                    NoNO.SetActive(false);
                }
            }
            else if (PlayerPrefs.GetInt("BBb") == 1)
            {
                if (PlayerPrefs.GetInt("Localized") == 0)
                {
                    NOEU.SetActive(false);
                    YesEu.SetActive(false);
                    NoNO.SetActive(true);
                    request = new AdRequest.Builder().Build();
                    Debug.Log("personalized No EU");
                }
                else
                {
                    Debug.Log("First time need select ");
                }
            }


            else if (PlayerPrefs.GetInt("NotEU") == 1)
            {
                NOEU.SetActive(false);
                YesEu.SetActive(false);
                NoNO.SetActive(true);
                Debug.Log("Personalized Not EU ");
                request = new AdRequest.Builder().Build();
            }
        }



    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        interstitialAd = new InterstitialAd(InterstitialAdId);

        Showbanner();
        requestAds();

        interstitialAd.LoadAd(request);
    }
    public void RequestBanner()
    {
        bannerView = new BannerView(BannerAdId, AdSize.Banner, AdPosition.Bottom);
        requestAds();
        bannerView.LoadAd(request);
        bannerView.Show();

    }
    public void Showbanner()
    {
        
            RequestBanner();
    }
    public void HideBanner()
    {
        bannerView.Destroy();
        bannerView.Hide();
        bannerView.Destroy();
    }

    public void ShowInterstitial()
    {
        requestAds();
        interstitialAd.LoadAd(request);
        if (interstitialAd.IsLoaded())
        {

            InterstitialLoaded = true;
            interstitialAd.Show();
        }
        else
        {

        }

        interstitialAd.OnAdClosed += InterstitialAd_onAdClosed;
    }

    private void InterstitialAd_onAdClosed(object sender, System.EventArgs e)
    {
        InterstitialLoaded = false;
        InterstitialClosed = true;
    }
    
}
