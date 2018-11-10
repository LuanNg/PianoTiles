using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mio.Utils
{
    public class AdmobAdsHelper : MonoBehaviour
    {
        public bool isEnabled = false;
        [Header("iOS")]
        public string appIDiOS;
        public string idInterstitialIOs;
        [Header("Android")]
        public string appIDAndroid;        
        public string idInterstitialAndroid;

        [Header("Config")]
        public bool autoRequestAdsOnInit = true;
        public float delayToRetryRequestAds = 10;

        private string appID;
        private string adsID;

        private bool isInterstitialAdsReady = false;

        private InterstitialAd interstitial;
        private WaitForSeconds wait;

        private static AdmobAdsHelper instance;
        public static AdmobAdsHelper Instance {
            get {
                if (instance == null)
                {
                    Debug.LogError("UnityAdhelper instance is null, is this an error?");
                }

                return instance;
            }
        }

        void Start()
        {
            if (instance == null)
            {
                instance = this;
                //print("here");
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                if (this != instance)
                {
                    Destroy(this);
                    return;
                }
            }
        }

        public void Initialize()
        {
            if (!isEnabled) return;
#if UNITY_ANDROID
            adsID = idInterstitialAndroid;
            appID = appIDAndroid;
#elif UNITY_IPHONE
            adsID = idInterstitialIOs;
            appID = appIDiOS;
#else
            appID = adsID = "unexpected_platform";
#endif
            wait = new WaitForSeconds(delayToRetryRequestAds);

            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(appID);
            InitInterstitialAds();
        }

        void InitInterstitialAds()
        {
            if (!isEnabled) return;
            // Initialize an InterstitialAd.
            interstitial = new InterstitialAd(adsID);
            // Called when an ad request has successfully loaded.
            interstitial.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is shown.
            interstitial.OnAdOpening += HandleOnAdOpened;
            // Called when the ad is closed.
            interstitial.OnAdClosed += HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

            //auto request ads on init;
            if (autoRequestAdsOnInit)
            {
                RequestInterstitialAds();
            }
        }

        private void HandleOnAdLoaded(object sender, EventArgs e)
        {
            isInterstitialAdsReady = true;
        }

        private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            isInterstitialAdsReady = false;
            StartCoroutine(C_RequestInterstitialAds());
        }

        private void HandleOnAdOpened(object sender, EventArgs e)
        {
            //track ads view here
        }

        private void HandleOnAdClosed(object sender, EventArgs e)
        {
            RequestInterstitialAds();
            //track close ads here
        }

        private void HandleOnAdLeavingApplication(object sender, EventArgs e)
        {
            //track Click ads here
        }

        private IEnumerator C_RequestInterstitialAds()
        {
            yield return wait;
            RequestInterstitialAds();
        }

        public void RequestInterstitialAds()
        {
            if (!isEnabled) return;
            AdRequest adRequest;
            adRequest = new AdRequest.Builder().Build();
            interstitial.LoadAd(adRequest);
        }

        public void ShowInterstitialAds()
        {
            if (!isEnabled) return;
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }
        }
    }
}