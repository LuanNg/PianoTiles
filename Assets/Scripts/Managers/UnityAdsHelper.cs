using UnityEngine;
#if UNITY_ANDROID || UNITY_IPHONE
using UnityEngine.Advertisements;
#endif
using System;
using Mio.TileMaster;

namespace Mio.Utils {
#pragma warning disable 414
    public class UnityAdsHelper : MonoBehaviour {
        public bool isEnabled = false;
        public string rewardVideoID = "rewardedVideo";
        public string interstitialZoneID = "video";
        private string rewardIDPostponeAds = "rewardVideoPostponeAds";
        public string iosID = "1073252";
        public string androidID = "1073253";
        private int diamondsReward = 10;
        private static UnityAdsHelper instance;
        public Action OnViewedAds;
        public static UnityAdsHelper Instance {
            get {
                if (instance == null) {
                    Debug.LogError("UnityAdhelper instance is null, is this an error?");
                }

                return instance;
            }
        }

        // Use this for initialization
        void Start () {
            if (instance == null) {
                instance = this;
                //print("here");
                DontDestroyOnLoad(this.gameObject);
            }
            else {
                if (this != instance) {
                    Destroy(this);
                    return;
                }
            }
        }

        public void Initialize () {
            if (!isEnabled) return;
            if (GameManager.Instance.GameConfigs.videoAdsReward != 0)
                diamondsReward = GameManager.Instance.GameConfigs.videoAdsReward;

            string id = androidID;
#if UNITY_IOS
            id = iosID;
            Advertisement.Initialize(id);
#endif
        }

        public void ShowRewardedAd () {
            if (!isEnabled) return;
#if UNITY_ANDROID || UNITY_IPHONE
            //if (Advertisement.IsReady(rewardVideoID)) {
            //    Advertisement.Show(rewardVideoID, new ShowOptions {
            //        resultCallback = HandleShowResultRewardDiamond
            //    });
            //}
#endif
        }

        public void ShowRewardVideoPostponeAds () {
            if (!isEnabled) return;
#if UNITY_ANDROID || UNITY_IPHONE
            //if (Advertisement.IsReady(rewardIDPostponeAds)) {
            //    Advertisement.Show(rewardIDPostponeAds, new ShowOptions {
            //        resultCallback = HandleShowResultPostponeAds
            //    });
            //}
#endif
        }

        public void ShowInterstitialAds (bool callMopubIfAdNotAvailable = true) {
            if (!isEnabled) return;
#if UNITY_ANDROID || UNITY_IPHONE
            //if (Advertisement.IsReady(interstitialZoneID)) {
            //    Advertisement.Show(interstitialZoneID);
            //    //AdHelper.Instance.ResetTimeStamp();
            //}
            //else {
            //    if (callMopubIfAdNotAvailable) {
            //        AdHelper.Instance.DisplayInterstitialAds();
            //    }
            //}
#endif
        }
#if UNITY_ANDROID || UNITY_IPHONE
        private void HandleShowResultRewardDiamond (ShowResult result) {
//            switch (result) {
//                case ShowResult.Finished:
//                    //Debug.Log("The ad was successfully shown.");
//                    RewardDiamond();
//					AchievementHelper.Instance.LogAchievement("watchVideoAds");
//                    break;
//                case ShowResult.Skipped:
//                    Debug.Log("The ad was skipped before reaching the end.");
//                    break;
//                case ShowResult.Failed:
//                    Debug.LogError("The ad failed to be shown.");
//                    break;
//            }
        }

        private void HandleShowResultPostponeAds (ShowResult result) {
            switch (result) {
                case ShowResult.Finished:
                    //Debug.Log("The ad was successfully shown.");
                    PostponeAds();
                    AchievementHelper.Instance.LogAchievement("watchVideoAds");
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    break;
            }
        }
#endif
        private void PostponeAds () {
            ProfileHelper.Instance.TimeToResumeAds = DateTime.Now.AddMinutes(10).ToEpochTime();
        }

        private void RewardDiamond () {
            ProfileHelper.Instance.CurrentDiamond += diamondsReward;
            if (OnViewedAds != null)
                OnViewedAds();
        }
    }
}

