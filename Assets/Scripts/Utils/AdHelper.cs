//using Mido.Analytics;
using System;
//using Mio.Monentize;
using Mio.TileMaster;
using Mio.Utils.MessageBus;
using UnityEngine;

//using AerServ;

namespace Mio.Utils {
    public enum MopubAdsRequestStatus {
        Idle = 0,
        Waiting = 1,
        ShowAdAfterLoaded = 2,
        AdLoaded = 4,
        Failed = -1,
        Expired = -2
    }
    //public enum AdsProvider {
    //    Aerserv,
    //    AdMob,
    //    Chartboost
    //}
#pragma warning disable 162
    public class AdHelper : MonoSingleton<AdHelper> {
        [SerializeField]
        private bool adsEnabled;

        public bool isAdsEnabled {
            get { return adsEnabled; }
            set { adsEnabled = value; }
        }

        //[SerializeField]
        //private MopubAdsRequestStatus requestFullStatus = MopubAdsRequestStatus.Idle;
        ////private bool isRequestFullProcess=false;
        //private int retryCount = 0;
        //public int maxRetryPerAdsLoad = 2;
        //public string mopubInterstitialID;
        //public string mopubRewardVideoID;

        //use to prevent showing ads too much
        private float lastAdsShown = 0;
        private bool isNoAds;

        //private static Message msgRewardVideoFinished = new Message (MessageBusType.RewardVideoDiamond, true);
        //private static Message msgRewardVideoFailed = new Message (MessageBusType.RewardVideoDiamond, false);

        //private bool canShowRewardAds = false;
        //private bool needShow = false;
        //private static AdHelper instance;
        //public static AdHelper Instance {
        //    get {
        //        if (instance == null) {
        //            Debug.LogError("Adhelper instance is null, is this an error?");
        //        }

        //        return instance;
        //    }
        //}
        //// Use this for initialization
        //void Start () {
        //    if (instance == null) {
        //        instance = this;
        //        //print("here");
        //        DontDestroyOnLoad(this.gameObject);
        //    }
        //    else {
        //        if (this != instance) {
        //            Destroy(this);
        //            return;
        //        }
        //    }
        //}

        public void Initialize () {
            //		if (adsEnabled) {
            //			RequestFullAds ();
            //		}

            //		if(GameManager.Instance.GameConfigs.mopub_reward_ids != null) {
            //		    var rewardIds = GameManager.Instance.GameConfigs.mopub_reward_ids;
            //		    if(Application.platform == RuntimePlatform.Android) {
            //		        if(rewardIds.Count >= 1) {
            //		            mopubRewardVideoID = rewardIds[0];
            //		        }
            //		    }else if(Application.platform == RuntimePlatform.IPhonePlayer) {
            //		        if(rewardIds.Count >= 2) {
            //		            mopubRewardVideoID = rewardIds[1];
            //		        }
            //		    }
            //		}
            ////		mopubRewardVideoID = "2d38f4e6881341369e9fc2c2d01ddc9d";
            //AdsManager.Instance.Initialize();
            //		MoPub.initializeRewardedVideo ();

            //		RequestVideoReward ();
        }

        public void SetNoAds (bool _isNoAds) {
            adsEnabled = !_isNoAds;
        }

        public bool IsAdsDisabled () {
            return !adsEnabled;

        }

        public void ShowVideoRewardAds () {
            //AnalyticsHelper.Instance.LogRewardedvideo ("click");
            //Debug.Log ("====== Requesting reward video ads with ID: " + mopubRewardVideoID);
            //SceneManager.Instance.SetLoadingVisible ();
            //if (canShowRewardAds) {
            //	MoPub.showRewardedVideo (mopubRewardVideoID);
            //} else {
            //	needShow = true;
            //	MoPub.requestRewardedVideo (mopubRewardVideoID);
            //}
#if UNITY_ANDROID || UNITY_IPHONE
            UnityAdsHelper.Instance.ShowRewardedAd();
#endif
        }

        public void ShowVideoPostponeAds () {
#if UNITY_ANDROID || UNITY_IPHONE
            UnityAdsHelper.Instance.ShowRewardVideoPostponeAds();
#endif
        }


        void RequestVideoReward () {
            //MoPub.requestRewardedVideo (mopubRewardVideoID);
        }


        public void PrepareFullScreenAd () {
            if (adsEnabled) {
                //MoPub.requestInterstitialAd(mopubInterstitialID);
            }
        }

        public void ShowFullScreenAd () {
           // #if UNITY_ANDROID || UNITY_IPHONE
            //if (!isNoAds)//no ads = false => show ads;
            //{
            //Debug.Log("Trying to show fullscreen ads: " + adsEnabled);
            if (adsEnabled) {
                //Debug.Log("Showing fullscreen ads");
                //MoPub.showInterstitialAd(mopubInterstitialID);

                //Debug.Log(string.Format("Elapsed time: {0}, ads interval: {1}", Time.realtimeSinceStartup - lastAdsShown, GameManager.Instance.GameConfigs.adsInterval));
                long currentTime = DateTime.Now.ToEpochTime();
                if (currentTime >= ProfileHelper.Instance.TimeToResumeAds) {
                    //show ads if the time elapsed more than specified
                    if (Time.realtimeSinceStartup - lastAdsShown >= GameManager.Instance.GameConfigs.adsInterval) {
                        //CallShowFullAds();
                        // check if the config is valid, and the platform is matched or not
                        //if (GameManager.Instance.GameConfigs.priorityMopub != null
                        //                   && GameManager.Instance.GameConfigs.priorityMopub.Count >= 2
                        //                   && ((GameManager.Instance.GameConfigs.priorityMopub[0] && Application.platform == RuntimePlatform.Android))
                        //                   || (GameManager.Instance.GameConfigs.priorityMopub[1] && Application.platform == RuntimePlatform.IPhonePlayer)) {
                        //    //if matched, prioritize mopub
                        //    //AnalyticsHelper.Instance.LogFullScreenAds("call");
                        //    //CallShowFullAds ();
                        //}
                        //else {
                        //else, prioritize Unity ads
                        UnityAdsHelper.Instance.ShowInterstitialAds();
                        AdmobAdsHelper.Instance.ShowInterstitialAds();
                        //UnityAdsHelper.Instance.ShowInterstitialAds(false);
                        //}
                    }
                }
                //CallShowFullAds();
            }//end if
             //}
//#endif
        }
        //end method

        public void ResetTimeStamp () {
            lastAdsShown = Time.realtimeSinceStartup;
        }

        /// <summary>
        /// This should only be used by internal component only, don't call it yourself
        /// </summary>
        public void DisplayInterstitialAds () {
            //CallShowFullAds (false);
        }

#if UNITY_ANDROID || UNITY_IPHONE

		void OnEnable ()
		{
			// Listen to all events for illustration purposes
			//UnregisterCallbacks ();
			//RegisterCallbacks ();
		}

		void OnDisable ()
		{
			// Remove all event handlers
			//UnregisterCallbacks ();
		}

		private void RegisterCallbacks ()
		{
			//MoPubManager.onAdLoadedEvent += onAdLoadedEvent;
			//MoPubManager.onAdFailedEvent += onAdFailedEvent;
			//MoPubManager.onAdClickedEvent += onAdClickedEvent;
			//MoPubManager.onAdExpandedEvent += onAdExpandedEvent;
			//MoPubManager.onAdCollapsedEvent += onAdCollapsedEvent;

			//MoPubManager.onInterstitialLoadedEvent += onInterstitialLoadedEvent;
			//MoPubManager.onInterstitialFailedEvent += onInterstitialFailedEvent;
			//MoPubManager.onInterstitialShownEvent += onInterstitialShownEvent;
			//MoPubManager.onInterstitialClickedEvent += onInterstitialClickedEvent;
			//MoPubManager.onInterstitialDismissedEvent += onInterstitialDismissedEvent;
			//MoPubManager.onInterstitialExpiredEvent += onInterstitialExpiredEvent;

			//MoPubManager.onRewardedVideoLoadedEvent += onRewardedVideoLoadedEvent;
			//MoPubManager.onRewardedVideoFailedEvent += onRewardedVideoFailedEvent;
			//MoPubManager.onRewardedVideoExpiredEvent += onRewardedVideoExpiredEvent;
			//MoPubManager.onRewardedVideoShownEvent += onRewardedVideoShownEvent;
			//MoPubManager.onRewardedVideoFailedToPlayEvent += onRewardedVideoFailedToPlayEvent;
			//MoPubManager.onRewardedVideoReceivedRewardEvent += onRewardedVideoReceivedRewardEvent;
			//MoPubManager.onRewardedVideoClosedEvent += onRewardedVideoClosedEvent;
			//MoPubManager.onRewardedVideoLeavingApplicationEvent += onRewardedVideoLeavingApplicationEvent;
		}

		private void UnregisterCallbacks ()
		{
			//MoPubManager.onAdLoadedEvent -= onAdLoadedEvent;
			//MoPubManager.onAdFailedEvent -= onAdFailedEvent;
			//MoPubManager.onAdClickedEvent -= onAdClickedEvent;
			//MoPubManager.onAdExpandedEvent -= onAdExpandedEvent;
			//MoPubManager.onAdCollapsedEvent -= onAdCollapsedEvent;

			//MoPubManager.onInterstitialLoadedEvent -= onInterstitialLoadedEvent;
			//MoPubManager.onInterstitialFailedEvent -= onInterstitialFailedEvent;
			//MoPubManager.onInterstitialShownEvent -= onInterstitialShownEvent;
			//MoPubManager.onInterstitialClickedEvent -= onInterstitialClickedEvent;
			//MoPubManager.onInterstitialDismissedEvent -= onInterstitialDismissedEvent;
			//MoPubManager.onInterstitialExpiredEvent -= onInterstitialExpiredEvent;

			//MoPubManager.onRewardedVideoLoadedEvent -= onRewardedVideoLoadedEvent;
			//MoPubManager.onRewardedVideoFailedEvent -= onRewardedVideoFailedEvent;
			//MoPubManager.onRewardedVideoExpiredEvent -= onRewardedVideoExpiredEvent;
			//MoPubManager.onRewardedVideoShownEvent -= onRewardedVideoShownEvent;
			//MoPubManager.onRewardedVideoFailedToPlayEvent -= onRewardedVideoFailedToPlayEvent;
			//MoPubManager.onRewardedVideoReceivedRewardEvent -= onRewardedVideoReceivedRewardEvent;
			//MoPubManager.onRewardedVideoClosedEvent -= onRewardedVideoClosedEvent;
			//MoPubManager.onRewardedVideoLeavingApplicationEvent -= onRewardedVideoLeavingApplicationEvent;
		}

		/// <summary>
		/// Well, this one here is an example of what you should not code. 
		/// It's handling logic and UI tasks in the same class. 
		/// What if suddenly we want to reuse this class in other project?
		/// Should we refactor it again to comply with the logic to show UI?
		/// Bad practice
		/// </summary>
		public void ShowVideoRewardFailedDialog ()
		{
			SceneManager.Instance.SetLoadingVisible (false);
			MessageBoxDataModel msg = new MessageBoxDataModel ();
			//msg.message = "The game data on your device and our server do not look alike. Which data would you like to use?";
			//msg.messageNo = "SERVER DATA";
			//msg.messageYes = "CURRENT DATA";
			msg.message = Localization.Get ("pu_rewardloadfailed_title");
			//msg.messageNo = Localization.Get("pu_rewardloadfailed_btn_ok");
			msg.messageYes = Localization.Get ("pu_rewardloadfailed_btn_ok");
			;
			//msg.OnNoButtonClicked = delegate { };
			msg.OnYesButtonClicked = delegate {
			};

			SceneManager.Instance.OpenPopup (ProjectConstants.Scenes.MessageBoxPopup, msg);
		}

		// ham nay chi goi mot lan duy nhat
		//default retry count = 0
		public void RequestFullAds (string adId = null, int _retryCount = 0)
		{
#if UNITY_EDITOR
			return;
#endif
			//if (requestFullStatus != MopubAdsRequestStatus.AdLoaded) {
			//	if (adId != null)
			//		mopubInterstitialID = adId;
			//	retryCount = _retryCount;

			//	if (retryCount < maxRetryPerAdsLoad) {
			//		requestFullStatus = MopubAdsRequestStatus.Waiting;
			//		MoPub.requestInterstitialAd (this.mopubInterstitialID);
			//	}
			//}
		}

		// khi show ads goi ham nay -> tu dong retry quang cao neu ko thanh cong
		// sau khi show xong tu dong request cai moi.
		public void ShowFullAds (string adId = null)
		{
#if UNITY_EDITOR
			return;
#endif
			//if (adId != null)
			//	this.mopubInterstitialID = adId;

			//if (requestFullStatus == MopubAdsRequestStatus.AdLoaded) {
			//	CallShowFullAds ();
			//} else {
			//	RequestFullAdsAndShow (this.mopubInterstitialID, 1);
			//}
		}

		public void RejectShowFullAndWhenFinish ()
		{
			//if (requestFullStatus == MopubAdsRequestStatus.ShowAdAfterLoaded) {
			//	requestFullStatus = MopubAdsRequestStatus.Waiting;
			//}
		}

		private void RequestFullAdsAndShow (string adId, int _retryCount = 0)
		{
			//retryCount = _retryCount;
			//this.mopubInterstitialID = adId;
			//if (retryCount < maxRetryPerAdsLoad) {
			//	requestFullStatus = MopubAdsRequestStatus.ShowAdAfterLoaded;
			//	MoPub.requestInterstitialAd (this.mopubInterstitialID);
			//}
		}

		private void CallShowFullAds (bool callUnityAdsIfFailed = true)
		{
			//todo
#if UNITY_EDITOR
			return;
#endif
			//if (requestFullStatus == MopubAdsRequestStatus.AdLoaded) {
			//	MoPub.showInterstitialAd (this.mopubInterstitialID);
			//	requestFullStatus = MopubAdsRequestStatus.Idle;//reset sau khi xong
			//	retryCount = 0;
			//} else if (requestFullStatus == MopubAdsRequestStatus.Idle || requestFullStatus == MopubAdsRequestStatus.Expired || requestFullStatus == MopubAdsRequestStatus.Failed) {
			//	RequestFullAds ();
			//	if (callUnityAdsIfFailed) {
			//		//try calling Unity ads if failed to show mopub ads
			//		UnityAdsHelper.Instance.ShowInterstitialAds (false);
			//	}
			//}
		}


#region banner

		void onAdLoadedEvent (float height)
		{
			//Debug.Log("onAdLoadedEvent. height: " + height);
		}


		void onAdFailedEvent ()
		{
			Debug.Log ("onAdFailedEvent");
		}


		void onAdClickedEvent ()
		{
			Debug.Log ("onAdClickedEvent");
		}


		void onAdExpandedEvent ()
		{
			Debug.Log ("onAdExpandedEvent");
		}


		void onAdCollapsedEvent ()
		{
			Debug.Log ("onAdCollapsedEvent");
		}

#endregion

#region fullScreen

		//public bool IsFullScreenAdsReady ()
		//{
		//	//return (requestFullStatus == MopubAdsRequestStatus.AdLoaded
		//	//|| requestFullStatus == MopubAdsRequestStatus.Expired
		//	//|| requestFullStatus == MopubAdsRequestStatus.Failed
		//	//);
            
		//}

		void onInterstitialLoadedEvent ()
		{
			//Debug.Log("onInterstitialLoadedEvent success");

			//if (requestFullStatus == MopubAdsRequestStatus.ShowAdAfterLoaded) {
			//	requestFullStatus = MopubAdsRequestStatus.AdLoaded;
			//	CallShowFullAds ();
			//} else {
			//	requestFullStatus = MopubAdsRequestStatus.AdLoaded;
			//}

		}

		void onInterstitialFailedEvent ()
		{
			//requestFullStatus = MopubAdsRequestStatus.Failed;
			//AnalyticsHelper.Instance.LogFullScreenAds("fail");
			////Debug.Log("onInterstitialFailedEvent");
			//AutoRetry ();
		}

		private void AutoRetry ()
		{
			//++retryCount;
			//if (requestFullStatus == MopubAdsRequestStatus.AdLoaded) {
			//	return;
			//}
			//if (requestFullStatus == MopubAdsRequestStatus.ShowAdAfterLoaded) {
			//	//Debug.Log("AutoRetryShow");

			//	RequestFullAdsAndShow (this.mopubInterstitialID, retryCount);
			//} else {
			//	//Debug.Log("AutoRetry");
			//	RequestFullAds (this.mopubInterstitialID, retryCount);
			//}
		}


		void onInterstitialShownEvent ()
		{
			AnalyticsHelper.Instance.LogFullScreenAds("show");
			//Debug.Log("onInterstitialShownEvent");
		}


		void onInterstitialClickedEvent ()
		{
			AnalyticsHelper.Instance.LogFullScreenAds("click");
			//Debug.Log("onInterstitialClickedEvent");
		}


		void onInterstitialDismissedEvent ()
		{
			//requestFullStatus = MopubAdsRequestStatus.Idle;
			//lastAdsShown = Time.realtimeSinceStartup;
			////Debug.Log("onInterstitialDismissedEvent");
			//// sau khi hien quang cao xong chuan bi quang cao moi
			////RequestFullAds(mopubInterstitialID);
		}


		void onInterstitialExpiredEvent ()
		{
			//Debug.Log("onInterstitialExpiredEvent");
			//requestFullStatus = MopubAdsRequestStatus.Expired;
			//AnalyticsHelper.Instance.LogAdExpired (mopubInterstitialID);
			//AutoRetry ();
		}



#endregion

#region Reward Video

		void onRewardedVideoLoadedEvent (string adUnitId)
		{
			Debug.Log ("onRewardedVideoLoadedEvent: " + adUnitId);
			// MoPub.showRewardedVideo(adUnitId);
			//AnalyticsHelper.Instance.LogRewardedvideo ("start");
			//canShowRewardAds = true;
			//if (needShow) {
			//	MoPub.showRewardedVideo (adUnitId);
			//	needShow = false;
			//}
		}


		void onRewardedVideoFailedEvent (string adUnitId)
		{
			//Debug.Log ("onRewardedVideoFailedEvent: " + adUnitId);
			//// MessageBus.MessageBus.Annouce(msgRewardVideoFailed);
			//AnalyticsHelper.Instance.LogRewardedvideo ("fail");
			//if (needShow)
			//	ShowVideoRewardFailedDialog ();
			//canShowRewardAds = false;
			//needShow = false;
            
		}

		void onRewardedVideoExpiredEvent (string adUnitId)
		{
			//Debug.Log ("onRewardedVideoExpiredEvent: " + adUnitId);
			////MessageBus.MessageBus.Annouce(msgRewardVideoFailed);
			//canShowRewardAds = false;
			//needShow = false;
			//AnalyticsHelper.Instance.LogAdExpired (adUnitId);
		}


		void onRewardedVideoShownEvent (string adUnitId)
		{
			//Debug.Log ("onRewardedVideoShownEvent: " + adUnitId);    
			//canShowRewardAds = false;
			//needShow = false;
		}


		void onRewardedVideoFailedToPlayEvent (string adUnitId)
		{
			//Debug.Log ("onRewardedVideoFailedToPlayEvent: " + adUnitId);
			//if (needShow)
			//	ShowVideoRewardFailedDialog ();
			//canShowRewardAds = false;
			//needShow = false;
			//// MessageBus.MessageBus.Annouce(msgRewardVideoFailed);
			////    ShowVideoRewardFailedDialog();
		}


		//void onRewardedVideoReceivedRewardEvent (MoPubManager.RewardedVideoData rewardedVideoData)
		//{
		//	//Debug.Log ("onRewardedVideoReceivedRewardEvent: " + rewardedVideoData);
		//	//canShowRewardAds = false;
		//	//needShow = false;
		//	//AnalyticsHelper.Instance.LogRewardedvideo ("finish");
		//	//MessageBus.MessageBus.Annouce (msgRewardVideoFinished);
		//	//AchievementHelper.Instance.LogAchievement ("watchVideoAds");
		//}


		void onRewardedVideoClosedEvent (string adUnitId)
		{
			//Debug.Log ("onRewardedVideoClosedEvent: " + adUnitId);
			//canShowRewardAds = false;
			//needShow = false;
			//SceneManager.Instance.SetLoadingVisible (false);
			//RequestVideoReward ();
		}


		void onRewardedVideoLeavingApplicationEvent (string adUnitId)
		{
			//Debug.Log ("onRewardedVideoLeavingApplicationEvent: " + adUnitId);
		}

#endregion

#endif

    }
    //end class
}
//end namespace