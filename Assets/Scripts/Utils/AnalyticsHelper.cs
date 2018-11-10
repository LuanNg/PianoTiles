using UnityEngine;
using System.Collections.Generic;
//using Fabric.Answers;
//using Facebook.Unity;
using UnityEngine.Analytics;
using System;

namespace Mio.Utils {
    public class AnalyticsHelper : MonoBehaviour {
        [Header("Which analytics service to use?")]
        public bool useGoogleAnalytics = true;
        public bool useFabric = true;
        public bool useFacebook = true;
        public bool useUnityAnalytics = true;

        //[Header("Google Analytic Object")]
        //[SerializeField]
        //private GoogleAnalyticsV4 gaService;

        //private bool gaExisted = false;
        //private string iosGAID = string.Empty;
        //private string androidGAID = string.Empty;
        //private string otherGAID = string.Empty;

        private static Dictionary<string, object> parameters = new Dictionary<string, object>();
        private static AnalyticsHelper instance;
        public static AnalyticsHelper Instance {
            get {
                if (instance == null) {
                    instance = GameObject.FindObjectOfType<AnalyticsHelper>();
                    if (instance == null) {
                        //Debug.logWarning("Analytic helper instance is null");
                    }
                }
                return instance;
            }
        }

        public const string EVENT_START_GAME = "Start Level";
        public const string EVENT_LEVEL_FISNIHED = "Level Finished";
        public const string EVENT_BUY_INGAME_ITEM = "Buy Ingame Item";
        public const string EVENT_AD_EXPIRED = "AD Expired";
        public const string EVENT_ABTEST = "AB TEST";
        public const string EVENT_ITEM_CLICKED = "Click";
        public const string EVENT_DAILYREWARD_CLAIMED = "Daily reward";
        public const string EVENT_ACHIEVEMENT_CLAIMED = "Claimed";
        public const string EVENT_ACHIEVEMENT_UNLOCKED = "Unlocked";

        public const string EVENTTYPE_TRANSACTION = "Transaction";
        public const string EVENTTYPE_NAVIGATION = "Navigation";
        public const string EVENTTYPE_INTERACTION = "Interaction";

        //new event

        public const string EVENT_PREMIUM_SONG_PURCHASE = "Premium song purchase";
        public const string EVENT_SONG_START = "Song start";
        public const string EVENT_FB_LOGIN = "FB login";
        public const string EVENT_REWARDED_VIDEO = "Rewarded video ";
        public const string EVENT_ADS_FULL = "Ads full ";
        public const string EVENT_GAME_STARTED = "game started";
        public const string EVENT_INAPP_PURCHASE = "Inapp purchase";

        public const string EVENT_ACHIEVEMENT_CLAIMED2 = "Achievement claimed";
        public const string EVENT_ACHIEVEMENT_CLAIMABLE = "Achievement claimable";

        //online Event
        public const string EVENT_ONLINE_OPEN = "Online Open Online";
        public const string EVENT_ONLINE_LEAVEROOM = "Online LeaveRoom";
        public const string EVENT_ONLINE_PLAYCLICK = "Online Play Click";
        public const string EVENT_ONLINE_WIN = "Online Win";
        public const string EVENT_ONLINE_LOSE = "Online lose";
        public const string EVENT_ONLINE_CONNECTERROR = "Online OnConnectionError";
        public const string EVENT_ONLINE_DISCONECT = "Online OnServerDisconnect";
        public const string EVENT_ONLINE_ROOMCREATE = "Online Room";
        public const string EVENT_ONLINE_START = "Online start ";
        public const string EVENT_ONLINE_REVENGE_CLICK = "Online revenge click";
        public const string EVENT_ONLINE_REVENGE_JOINED = "Online revenge joined";
        public const string EVENT_ONLINE_REPLAY_ROOM = "Online revenge room create";


        // Use this for initialization
        void Awake () {
            if (instance == null && instance != this) {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else {
                Destroy(this);
                return;
            }
        }


        public void SetGAID (string androidID, string iosID, string otherID, bool alsoUseGA = true) {
            //androidGAID = androidID;
            //iosGAID = iosID;
            //otherGAID = otherID;
            //this.useGoogleAnalytics = alsoUseGA;
        }

        //public void SetFabricID(string appID, string )

        /// <summary>
        /// Start logging session
        /// </summary>
        public void Initialize () {
            //gaService = GoogleAnalyticsV4.instance;
            //if (gaService == null) {
            //    //Debug.logWarning("Can't instantiate google analytic service");
            //    return;
            //}

            //if ((gaService != null) && useGoogleAnalytics) {
            //    if (!(string.IsNullOrEmpty(androidGAID))) {
            //        gaService.androidTrackingCode = androidGAID;
            //        gaService.otherTrackingCode = androidGAID;
            //        //print("other tracking code " + gaService.otherTrackingCode);
            //    }

            //    if (!string.IsNullOrEmpty(iosGAID)) {
            //        gaService.IOSTrackingCode = iosGAID;
            //        gaService.otherTrackingCode = iosGAID;
            //    }

            //    if (!string.IsNullOrEmpty(otherGAID)) {
            //        gaService.otherTrackingCode = otherGAID;
            //    }

            //    gaService.bundleIdentifier = Application.bundleIdentifier;
            //    gaService.bundleVersion = Application.version;
            //    gaService.productName = Application.productName;


            //    gaService.InitializeTracker();

            //    gaExisted = true;
            //} else {
            //    useGoogleAnalytics = false;
            //}            
        }



        /// <summary>
        /// Log event when player open the game
        /// </summary>
        public void LogGameOpened (int numStart) {
            //if ((gaService != null) && useGoogleAnalytics) {
            //    gaService.LogEvent(EVENTTYPE_NAVIGATION, "Open game", "Game started", numStart);
            //}

            //if (useFabric) {
            //    Dictionary<string, object> parameters = new Dictionary<string, object>();
            //    parameters.Add("numstart", numStart);
            //    Answers.LogCustom("Game Started", parameters);
            //}
        }

        public void LogABTest (string eventType, string eventValue) {
            //if ((gaService != null) && useGoogleAnalytics) {
            //    gaService.LogEvent(EVENT_ABTEST, eventType, eventValue, 1);
            //}

            parameters.Clear();
            parameters.Add(eventType, eventValue);

            //if (useFabric) {
            //    Answers.LogCustom(EVENT_ABTEST, parameters);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_ABTEST, 1, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_ABTEST, parameters);
            }
        }


        /// <summary>
        /// Log event when user start playing a level
        /// </summary>
		public void LogLevelStarted (string levelName) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    gaService.LogEvent("Progress", "Level Started", "Level " + levelName + " started", 1);
            //}

            //if (useFabric) {
            //    Answers.LogLevelStart(levelName);
            //}

            parameters.Clear();
            parameters.Add("Song name", levelName);

            //if (useFacebook) {                
            //    FB.LogAppEvent(EVENT_START_GAME, 1, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_START_GAME, parameters);
            }
        }

        /// <summary>
        /// Log event and measurement when player finished a level
        /// </summary>
		public void LogLevelFinished (string levelName, int score, int numStar, int crown) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    gaService.LogEvent("Progress", "Level completed", "Level " + levelName + " completed", 1);
            //    gaService.LogEvent("Progress", "Level finished with star", "Level " + levelName + " finished", numStar);
            //    gaService.LogEvent("Progress", "Level finished with score", "Level " + levelName + " finished", score);
            //    gaService.LogEvent("Progress", "Level finished with crown", "Level " + levelName + " finished", crown);
            //}

            parameters.Clear();
            parameters.Add("stars", numStar);
            parameters.Add("crowns", crown);

            //if (useFabric) {                
            //    Answers.LogLevelEnd(levelName, score, null, parameters);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_LEVEL_FISNIHED, 1, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_LEVEL_FISNIHED, parameters);
            }
        }

        /// <summary>
        /// Log event when user share on facebook
        /// </summary>
        public void LogShareSocial (int currentLevel) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    gaService.LogEvent("Social", "Shared on facebook", "Level " + currentLevel + " is shared on facebook", 1);
            //}
        }

        public void LogOpenScene (string sceneName) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_NAVIGATION, "Open scene", "Opening scene " + sceneName, 1);
            //}

            //if (useFabric) {
            //    Answers.LogContentView(sceneName, "Scene");
            //}

            parameters.Clear();
            parameters.Add("Scene name", sceneName);

            //if (useFacebook && FB.IsInitialized) {                
            //    FB.LogAppEvent(AppEventName.ViewedContent, 1, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent("Open Scene", parameters);
            }

            //Debug.log("LogOpenScene "+ sceneName);
        }

        public void LogClaimDailyReward (string index) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_INTERACTION, "Daily reward", "Claiming daily reward of " + index, 1);
            //}

            parameters.Clear();
            parameters.Add("Day", index);

            //if (useFabric) {
            //    Answers.LogCustom(EVENT_DAILYREWARD_CLAIMED, parameters);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_DAILYREWARD_CLAIMED, 1, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_DAILYREWARD_CLAIMED, parameters);
            }

            //Debug.log(EVENT_DAILYREWARD_CLAIMED + " Day "+index);
        }

        public void LogClickItem (string itemID) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_INTERACTION, "View Item", "Viewing Item " + itemID, 1);
            //}

            parameters.Clear();
            parameters.Add("item", itemID);

            //if (useFabric) {                
            //    Answers.LogCustom(EVENT_ITEM_CLICKED, parameters);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_ITEM_CLICKED, 1, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_ITEM_CLICKED, parameters);
            }
        }

        public void LogClaimAchievement (string achievementID) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_INTERACTION, "Achievement Claim", "Claim " + achievementID, 1);
            //}

            parameters.Clear();
            parameters.Add("achievement", achievementID);

            //if (useFabric) {
            //    Answers.LogCustom(EVENT_ACHIEVEMENT_CLAIMED, parameters);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_ACHIEVEMENT_CLAIMED, 1, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_ACHIEVEMENT_CLAIMED, parameters);
            }
        }

        public void LogUnlockAchievement (string achievementID) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_INTERACTION, "Achievement Unlocked", "Unlocked " + achievementID, 1);
            //}

            parameters.Clear();
            parameters.Add("achievement", achievementID);

            //if (useFabric) {
            //    Answers.LogCustom(EVENT_ACHIEVEMENT_UNLOCKED, parameters);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_ACHIEVEMENT_UNLOCKED, 1, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_ACHIEVEMENT_UNLOCKED, parameters);
            }
        }

        public void LogBuyMarketItem (string itemID, string itemName, string itemType) {

            LogBuyInAppPurchase(itemName);

            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent("Transactions", "Buy Item", "Bought Item " + itemID, 1);
            //}            

            parameters.Clear();
            parameters.Add("Item ID", itemID);
            parameters.Add("Item name", itemName);
            parameters.Add("Item Type", itemType);

            //if (useFacebook) {                
            //    FB.LogPurchase(0, "", parameters);
            //}

            //if (useFabric) {
            //    //Answers.LogPurchase(null, null, true, itemName, itemType, itemID);
            //    Answers.LogCustom("Purchase", parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.Transaction(itemID, 0, "USD");
            }
        }

        public void LogBuyInGameItem (string itemType, int quantity, int price) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_TRANSACTION, EVENT_BUY_INGAME_ITEM, "Bought Item " + itemType, quantity);
            //}

            parameters.Clear();
            parameters.Add("Item Type", itemType);
            parameters.Add("Quantity", quantity);
            parameters.Add("Price", price);

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_BUY_INGAME_ITEM, 1, parameters);
            //}

            //if (useFabric) {
            //    //Answers.LogPurchase(null, null, true, itemName, itemType, itemID);
            //    Answers.LogCustom(EVENT_BUY_INGAME_ITEM, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_BUY_INGAME_ITEM, parameters);
            }
        }

        public void LogAdExpired (string adID) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent("Misc", EVENT_AD_EXPIRED, "Ad expired " + adID, 1);
            //}

            parameters.Clear();
            parameters.Add(EVENT_AD_EXPIRED, adID);

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_AD_EXPIRED, 1, parameters);
            //}

            //if (useFabric) {
            //    //Answers.LogPurchase(null, null, true, itemName, itemType, itemID);
            //    Answers.LogCustom(EVENT_AD_EXPIRED, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_AD_EXPIRED, parameters);
            }
        }

        public void LogRewardedvideo (string state) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_INTERACTION, EVENT_REWARDED_VIDEO + state, EVENT_REWARDED_VIDEO + state, 1);
            //}


            //if (useFabric) {
            //    Answers.LogCustom(EVENT_REWARDED_VIDEO + state);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_REWARDED_VIDEO + state);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_REWARDED_VIDEO + state);
            }

            //Debug.log(EVENT_REWARDED_VIDEO + state);
        }


        public void LogFullScreenAds (string state) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_INTERACTION, EVENT_ADS_FULL + state, EVENT_ADS_FULL + state, 1);
            //}


            //if (useFabric) {
            //    Answers.LogCustom(EVENT_ADS_FULL + state);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_ADS_FULL + state);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_ADS_FULL + state);
            }

            //Debug.log(EVENT_REWARDED_VIDEO + state);
        }

        public void LogFBLogin () {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_INTERACTION, EVENT_FB_LOGIN, "FB login", 1);
            //}


            //if (useFabric) {
            //    Answers.LogCustom(EVENT_FB_LOGIN);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_FB_LOGIN);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_FB_LOGIN);
            }

            //Debug.log("lOG "+EVENT_FB_LOGIN);
        }

        public void LogSongstart (string songName) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    gaService.LogEvent("Progress", EVENT_SONG_START, "Song_name: " + songName, 1);
            //}

            //if (useFabric) {
            //    Answers.LogLevelStart("Song_name: " + songName);
            //}

            parameters.Clear();
            parameters.Add("Song_name", songName);

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_SONG_START, 1, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_SONG_START, parameters);
            }

            //Debug.log("Song start " + songName);
        }

        public void LogBuyPremiumSong (string songName) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_TRANSACTION, EVENT_PREMIUM_SONG_PURCHASE, "song_name: " + songName, 1);
            //}

            parameters.Clear();
            parameters.Add("song_name", songName);

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_PREMIUM_SONG_PURCHASE, 1, parameters);
            //}

            //if (useFabric) {
            //    //Answers.LogPurchase(null, null, true, itemName, itemType, itemID);
            //    Answers.LogCustom(EVENT_PREMIUM_SONG_PURCHASE, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_PREMIUM_SONG_PURCHASE, parameters);
            }
        }

        public void LogGameStarted () {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_INTERACTION, EVENT_GAME_STARTED, EVENT_GAME_STARTED, 1);
            //}


            //if (useFabric) {
            //    Answers.LogCustom(EVENT_GAME_STARTED);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_GAME_STARTED);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_GAME_STARTED);
            }

            //Debug.log("lOG " + EVENT_GAME_STARTED);
        }

        public void LogBuyInAppPurchase (string itemName) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_TRANSACTION, EVENT_INAPP_PURCHASE, "name_of_the_item: " + itemName, 1);
            //}

            parameters.Clear();
            parameters.Add("name_of_the_item", itemName);


            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_INAPP_PURCHASE, 1, parameters);
            //}

            //if (useFabric) {
            //    //Answers.LogPurchase(null, null, true, itemName, itemType, itemID);
            //    Answers.LogCustom(EVENT_INAPP_PURCHASE, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_INAPP_PURCHASE, parameters);
            }
        }

        public void LogClaimedAchievement (string achievementName) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_INTERACTION, EVENT_ACHIEVEMENT_CLAIMED2, "Name of the achievement " + achievementName, 1);
            //}

            parameters.Clear();
            parameters.Add("Name of the achievement", achievementName);

            //if (useFabric) {
            //    Answers.LogCustom(EVENT_ACHIEVEMENT_CLAIMED2, parameters);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_ACHIEVEMENT_CLAIMED2, 1, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_ACHIEVEMENT_CLAIMED2, parameters);
            }

            //Debug.log("LogClaimedAchievement "+ achievementName);
        }

        public void LogClamiableAchievement (string achievementName) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_INTERACTION, EVENT_ACHIEVEMENT_CLAIMABLE, "Name of the achievement " + achievementName, 1);
            //}

            parameters.Clear();
            parameters.Add("Name of the achievement", achievementName);

            //if (useFabric) {
            //    Answers.LogCustom(EVENT_ACHIEVEMENT_CLAIMABLE, parameters);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(EVENT_ACHIEVEMENT_CLAIMABLE, 1, parameters);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(EVENT_ACHIEVEMENT_CLAIMABLE, parameters);
            }

            //Debug.log("LogClamiableAchievement " + achievementName);
        }

        ///// <summary>
        ///// Log event when user clicked on a button
        ///// </summary>
        //public void LogButtonClicked(string buttonName) {
        //    if ((gaExisted) && useGoogleAnalytics) {
        //        gaService.LogEvent("Interaction", "Button clicked", "Button " + buttonName + " clicked", 1);
        //    }
        //}

        public void LogOnline (string logEvent) {
            //if ((gaExisted) && useGoogleAnalytics) {
            //    //                print("Logging event open scene");
            //    gaService.LogEvent(EVENTTYPE_INTERACTION, logEvent, logEvent, 1);
            //}


            //if (useFabric) {
            //    Answers.LogCustom(logEvent);
            //}

            //if (useFacebook) {
            //    FB.LogAppEvent(logEvent);
            //}

            if (useUnityAnalytics) {
                UnityEngine.Analytics.Analytics.CustomEvent(logEvent);
            }
        }
    }
}