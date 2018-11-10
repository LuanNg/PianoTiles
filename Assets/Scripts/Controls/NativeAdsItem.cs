using UnityEngine;
using AudienceNetwork;
using System.Collections;
using System;
using Mio.Utils.MessageBus;
using Mio.Utils;
using UnityEngine.UI;
using MovementEffects;
using System.Collections.Generic;
using Mio.TileMaster;

public class NativeAdsItem : MonoBehaviour {
    //the camera on which the native ads will be shown
    public Camera impressionCamera;
    private static WaitForSeconds waitDownloadImage = new WaitForSeconds(0.25f);
    public NativeAd nativeAd;

    public UI2DSprite iconImage;

    public UIButton btnCall2Action;
    public UILabel txtCall2Action;
    public UILabel txtTitle;
    public UILabel txtSubTitle;
    public UILabel txtSocialContext;
    public Sprite m_sprite;

    public GameObject cachedGameObject;
    [SerializeField]
    private string url_iOS;
    [SerializeField]
    private string url_Android;
    private string url;

    //public bool isTest = false;
    //void Update() {
    //    if (isTest) {
    //        isTest = false;
    //        //AdSettings.AddTestDevice("066d8be5209aa3a0");
    //        //LoadNativeAds("439221796278657_445175152349988");
    //    }
    //}

    void Awake () {
        cachedGameObject = gameObject;
#if UNITY_ANDROID
        url = url_Android;
#elif UNITY_IOS
        url = url_iOS;
#endif
    }

    void OnEnable () {
        //LoadCrossPromotion ();
    }

    public void LoadCrossPromotion () {
        //if (isLoaded)
        //	return;
        //CrossPromotion c = GameManager.Instance.crossItem;
        //txtTitle.text = c.name;
        //txtSubTitle.text = Localization.Get(c.sub);
        //txtSocialContext.text = Localization.Get(c.socical);
        //txtCall2Action.text = Localization.Get(c.textCallBtn);
        //c.LoadIcon ((Sprite sprite) =>
        //	{
        //		if(m_sprite == null){
        //			iconImage.sprite2D = sprite;
        //		}
        //	});
    }
    public bool isLoaded;
#pragma warning disable 162
    public void LoadNativeAds (string placementID) {
#if UNITY_ANDROID || UNITY_IPHONE
        if (impressionCamera == null) {
            Debug.LogWarning("Impression camera for native ads is null, no native ads will be created");
            return;
        }

#if UNITY_EDITOR
        //#if UNITY_ANDROID
        //        //only run on device
        //        
        //#elif UNITY_IOS

        //#endif
        //#else 
        return;
#endif

        //AdSettings.AddTestDevice("4b55fbfb8fb5b2f91ef8ac3a2d44fd9e");
        // Create a native ad request with a unique placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        NativeAd nativeAd = new AudienceNetwork.NativeAd(placementID);
        this.nativeAd = nativeAd;
        //print("Loading native ads with ID " + placementID);
        nativeAd.RegisterGameObjectForImpression(gameObject, new UIButton[] { btnCall2Action }, impressionCamera);
        nativeAd.NativeAdDidLoad = OnNativeAdLoaded;
        nativeAd.NativeAdDidFailWithError = OnNativeAdLoadFailed;
        nativeAd.NativeAdWillLogImpression = OnNativeAdWillLogImpression;
        nativeAd.NativeAdDidClick = OnNativeAdClicked;
        nativeAd.LoadAd();


        //// Set delegates to get notified on changes or when the user interacts with the ad.
        //nativeAd.NativeAdDidLoad = (delegate () {
        //    print("+++++++++======Native ad loaded.");
        //    Debug.Log("Loading images...");
        //    // Use helper methods to load images from native ad URLs
        //    StartCoroutine(nativeAd.LoadIconImage(nativeAd.IconImageURL));
        //    StartCoroutine(nativeAd.LoadCoverImage(nativeAd.CoverImageURL));
        //    Debug.Log("Images loaded.");
        //    txtTitle.text = nativeAd.Title;
        //    txtSocialContext.text = nativeAd.SocialContext;
        //    txtCall2Action.text = nativeAd.CallToAction;
        //});
        //nativeAd.NativeAdDidFailWithError = (delegate (string error) {
        //    print("++++++=====Native ad failed to load with error: " + error);
        //});
        //nativeAd.NativeAdWillLogImpression = (delegate () {
        //    print("++++++++=====Native ad logged impression.");
        //});
        //nativeAd.NativeAdDidClick = (delegate () {
        //    print("+++++++=====Native ad clicked.");
        //});


        Debug.Log("=====+++++++LOADING NATIVE ADS with ID " + placementID);
#endif
    }

    //  void Start() {
    ////Loom.Instance.CheckInitial ();
    //      //gameObject.SetActive(false);
    //  }

    private void OnNativeAdClicked () {
        //TODO: do wthyw
        //Debug.LogError("Native ads clicked " + nativeAd.Title);
        AnalyticsHelper.Instance.LogClickItem("Native Ad");
    }

    private void OnNativeAdWillLogImpression () {
        //TODO: ????
    }

    int failCount = 0;
    private void OnNativeAdLoadFailed (string error) {
        //Loom.Instance.QueueOnMainThread(() => {
        Debug.Log("NATIVE ADS: Load native ads failed with error: " + (++failCount).ToString() + error);
        //txtTitle.text = "Ads loaded fail";
        MessageBus.Annouce(new Message(MessageBusType.NativeAdItemFailedToLoad, gameObject));
        //reload ads after some delay if failed
        //StartCoroutine(ReloadAd());
        //});
    }

    public void OnInhouseNativeAdClicked () {
        Application.OpenURL(url);
        GameManager.Instance.ResetCrossPromotion();
        //Invoke ("LoadCrossPromotion",2f);
        AchievementHelper.Instance.LogAchievement("checkNewGame");

    }

    int loadCount = 0;
    private void OnNativeAdLoaded () {
        Debug.Log("NATIVE ADS: Ad Loaded " + (++loadCount).ToString());
        //Loom.Instance.QueueOnMainThread (() => {			
        //Debug.LogWarning ("NATIVE ADS: Ad Loaded");
        if (nativeAd != null) {
            btnCall2Action.onClick.Clear();
            nativeAd.RegisterGameObjectForImpression(gameObject, new UIButton[] { btnCall2Action }, impressionCamera);
            //gameObject.SetActive(true);
            if (txtTitle != null) {
                txtTitle.text = nativeAd.Title;
            }
            if (txtSocialContext != null) {
                if (!string.IsNullOrEmpty(nativeAd.SocialContext)) {
                    txtSocialContext.text = nativeAd.SocialContext;
                }
                else {
                    txtSocialContext.gameObject.SetActive(false);
                }
            }
            if (txtCall2Action != null) {
                txtCall2Action.text = nativeAd.CallToAction;
            }
            if (txtSubTitle != null) {
                txtSubTitle.text = nativeAd.Subtitle;
            }
            if (iconImage != null) {
                StartCoroutine(C_LoadIconImage());
            }
            isLoaded = true;

            MessageBus.Annouce(new Message(MessageBusType.NativeAdItemLoaded, gameObject));
        }
        //});
    }

    private IEnumerator C_LoadIconImage () {
        StartCoroutine(nativeAd.LoadIconImage(nativeAd.IconImageURL));
        while (nativeAd.IconImage == null) {
            yield return waitDownloadImage;
        }
        //print("Icon image loaded. Showing it to the audience");
        iconImage.sprite2D = m_sprite = nativeAd.IconImage;
    }

    private IEnumerator LoadCoverImage () {
        //StartCoroutine(nativeAd.LoadCoverImage(nativeAd.CoverImageURL));
        //while (nativeAd.CoverImage == null) {
        yield return waitDownloadImage;
        //}
        //print("Cover image loaded");
        //coverImage.sprite = nativeAd.CoverImage;
    }

    private IEnumerator ReloadAd () {
        print("NATIVE ADS: Waiting before reload native ads");
        yield return Timing.WaitForSeconds(5);
        print("Reloading native ads...");
        //nativeAd.lo
    }
}
