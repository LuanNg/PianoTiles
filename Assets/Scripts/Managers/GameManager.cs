using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Mio.Utils;
using Mio.Utils.MessageBus;
using Mio.TileMaster;
//using Facebook.Unity;
//using Facebook.MiniJSON;
//using Parse;
//using Parse.Utilities;
//using UnityParseHelpers;

namespace Mio.TileMaster {
    /// <summary>
    /// Responsible for managing the game's state
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Const variables
        private const string GAME_CONFIG_FILE = "config";
        private const string STORE_DATA_FILE = "store";
        private const string LOCALIZATION_DATA_FILE = "Localization";
        private const string GAME_VERSION_DATA_FILE = "version";
        private const string ACHIEVEMENT_FILE = "achievement";
        private const string ACHIEVEMENT_PROPERTY_FILE = "achievementProperties";
        private const string GAME_STATE_DATA_FILE = "gf0gf9h8";
        private const string USER_DATA_FILE = "lkh456";
        private const string ACHIEVEMENT_DATA_FILE = "544jk6kh7";
        private const string ACHIEVEMENT_DATA_DUMP_FILE = "k5h46kj56";
        private const string ACHIEVEMENT_PROPERTY_DATA_FILE = "kjh34jh5";
        private const string ACHIEVEMENT_PROPERTY_DATA_DUMP_FILE = "654gfh324dg";
        private static readonly string ENCRYPTION_KEY = string.Empty;//"jk65kj8hf290g93";
        //private static readonly string TEMPORARY_KEY_TRANSITION_USER_DATA_LOCATION_112 = "TRANSITIONINGUSERDATALOCATIONFORVER112";
        private const string FONT_RESOURCE_DEFAULT = "Fonts/fontLatin";
        private const string FONT_RESOURCE_JAPANESE = "Fonts/fontJapanese";
        private const string FONT_RESOURCE_KOREAN = "Fonts/fontKorean";
        private const string FONT_RESOURCE_CHINESE = "Fonts/fontChinese";
        #endregion

        private static Message msgGameDataChanged = new Message(MessageBusType.GameDataChanged);

        #region Public properties
        public bool printDebug = true;

        [Header("For multi-language support")]
        public UIFont referenceFontAtlas;
        [HideInInspector]
        public UIFont defaultFont;
        [HideInInspector]
        public UIFont japaneseFont;
        [HideInInspector]
        public UIFont koreanFont;

        //[Header("dic friend in a song")]
        //public Dictionary<string, List<PlayerRankingModel>> dic_player_rank;
        //private List<string> m_listFriends;
        //[Header("Remote console")]
        //[Tooltip("If enable, user can browse logs and send send command to this game over TCP/IP")]
        //[SerializeField]
        //private bool enableRemoteConsole;
        //public bool EnableRemoteConsole {
        //    get { return enableRemoteConsole; }
        //    private set { enableRemoteConsole = value; }
        //}
        //public CUDLR.Server remoteConsoleServer;

        [Header("Game's configuration URL")]
        [SerializeField]
        private string configURL;
        private GameConfigModel configs;
        public GameConfigModel GameConfigs {
            get { return configs; }
            private set {
                configs = value;
            }
        }

        private SessionDataModel sessionData;
        public SessionDataModel SessionData {
            get { return sessionData; }
            set { sessionData = value; }
        }

        //Store data        
        private StoreDataModel storeData;
        public StoreDataModel StoreData {
            get { return storeData; }
            set {
                storeData = value;
            }
        }


        [Header("Game's versions' data URL")]
        [SerializeField]
        private string gameVersionURL;
        private GameVersionModel versionsData;
        public GameVersionModel VersionsData {
            get { return versionsData; }
            private set { versionsData = value; }
        }
		public AllCrossPromotion crossPromotion{ get; set;}
		public CrossPromotion crossItem{ get; set;}
        private string deviceID;
        private bool deviceIDFetched = false;
        public string DeviceID {
            get {
                if (deviceIDFetched) {
                    return deviceID;
                }else {
                    deviceIDFetched = true;
#if UNITY_ANDROID
                    //deviceID = SystemInfo.deviceUniqueIdentifier;
                    AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
                    AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
                    deviceID = secure.CallStatic<string>("getString", contentResolver, "android_id");
#else
                    deviceID = SystemInfo.deviceUniqueIdentifier;
#endif
                    return deviceID;
                }
            }
        }

        private GameDataModel gameData;
        public GameDataModel GameData {
            get { return gameData; }
            set {
                gameData = value;
            }
        }

        public byte[] LocalizationData { get; internal set; }

        //private bool diamondChanged = false;

#endregion

        private string storedataWritePath, gamestateWritePath, gameconfigWritePath, gameversionWritePath, userdataWritePath,
            achievementPropertiesWritePath, achievementDataWritePath, achievementPropertiesDumpWritePath, achievementDataDumpWritePath;


        //the csv text of achievement properties and data of achievements
        private string rawPropertiesData = string.Empty, rawAchievementData = string.Empty;
        //the csv text of achievement record data
        private string rawAchievementDump, rawPropertyDump;

        void Awake ()
        {
            //generate save, load path to reduce string generation
            storedataWritePath = FileUtilities.GetWritablePath(STORE_DATA_FILE);
            gameconfigWritePath = FileUtilities.GetWritablePath(GAME_CONFIG_FILE);
            gamestateWritePath = FileUtilities.GetWritablePath(GAME_STATE_DATA_FILE);
            gameversionWritePath = FileUtilities.GetWritablePath(GAME_VERSION_DATA_FILE);
            userdataWritePath = FileUtilities.GetWritablePath(USER_DATA_FILE);
            achievementDataWritePath = FileUtilities.GetWritablePath(ACHIEVEMENT_DATA_FILE);
            achievementDataDumpWritePath = FileUtilities.GetWritablePath(ACHIEVEMENT_DATA_DUMP_FILE);
            achievementPropertiesWritePath = FileUtilities.GetWritablePath(ACHIEVEMENT_PROPERTY_DATA_FILE);
            achievementPropertiesDumpWritePath = FileUtilities.GetWritablePath(ACHIEVEMENT_PROPERTY_DATA_DUMP_FILE);

            Application.targetFrameRate = 60;
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
            //if (enableRemoteConsole) {
            //    remoteConsoleServer.StartServer();
            //}
        }

		public void ResetCrossPromotion()
        {
			//if (crossPromotion == null)
			//	return;
			//if (crossPromotion.data.Count == 0)
			//	return;
			//crossPromotion.Remove (crossItem);
			//string prefs = PlayerPrefs.GetString (GameConsts.CROSSPROMOTION,"");
			//prefs += "-" + crossItem.name;
			//PlayerPrefs.SetString (GameConsts.CROSSPROMOTION,prefs);
			//PlayerPrefs.Save ();
			//if (crossPromotion.data.Count > 0) {
			//	crossItem = crossPromotion.data [UnityEngine.Random.Range (0, crossPromotion.data.Count)];
			//} else {
			//	crossItem.Apply (GameConfigs.crossDefault);
			//}
		}

        void OnApplicationPause (bool state)
        {
            if (ProfileHelper.Instance.IsUserDataInitialized)
            {
                SaveUserData();
                ProfileHelper.Instance.PushUserData(true);
            }

            //if (state) {
            //    Screen.sleepTimeout = Screen.;
            //}
        }

        public void Initialize ()
        {
            //ParseUser.LogOut();
            MessageBus.Instance.Subscribe(MessageBusType.UserDataChanged, OnUserDataChanged);
            MessageBus.Instance.Subscribe(MessageBusType.UserDataConflictSolved, OnUserDataConflictSolved);
            MessageBus.Instance.Subscribe(MessageBusType.GameDataChanged, OnGameDataChanged);
            //MessageBus.Instance.Subscribe(MessageBusType.AchievementDataChanged, OnAchievementDataChanged);
            MessageBus.Instance.Subscribe(MessageBusType.OnUserDataInitialized, OnSuceedFB);
            MessageBus.Instance.Subscribe(MessageBusType.RewardVideoDiamond, OnVideoRewardEvent);

            //well, this code is a hack, to reduce number of drawcall by 1 :v
            //remember, 1 camera = 1 drawcall
            var goSolidCam = GameObject.Find("SolidCamera");
            if (goSolidCam != null) {
                var solidCam = goSolidCam.GetComponent<Camera>();
                if (solidCam != null) {
                    solidCam.clearFlags = CameraClearFlags.Nothing;
                }
            }
            //FacebookManager.Instance.onSuccedInitFB += OnSuceedFB;
            //MessageBus.Instance.Subscribe(MessageBusType.DiamondChanged, OnDiamondBalanceChanged);
        }

        

        public void SetupGameFont (string language)
        {
            if ((language.Contains("Vietnamese")) || (language.Contains("English")) || (language.Contains("Russian"))) {
                if (defaultFont == null) {
                    var font = Resources.Load<UIFont>(FONT_RESOURCE_DEFAULT) as UIFont;
                    if (font == null) {
                        Debug.LogError("Can't load default font from resource. No text will be shown");
                        return;
                    }

                    defaultFont = font;
                }

                referenceFontAtlas.replacement = defaultFont;
            }
            else if (language.Contains("Japanese"))
            {
                if (japaneseFont == null)
                {
                    var font = Resources.Load<UIFont>(FONT_RESOURCE_JAPANESE) as UIFont;
                    if (font == null) {
                        Debug.LogError("Can't load Japanese font from resource. No text will be shown");
                        return;
                    }

                    japaneseFont = font;
                }

                referenceFontAtlas.replacement = japaneseFont;
            }
            else if (language.Contains("Korean"))
            {
                if (koreanFont == null)
                {
                    var font = Resources.Load<UIFont>(FONT_RESOURCE_KOREAN) as UIFont;
                    if (font == null)
                    {
                        Debug.LogError("Can't load Korean font from resource. No text will be shown");
                        return;
                    }

                    koreanFont = font;
                }

                referenceFontAtlas.replacement = koreanFont;
            }
        }

        private void OnSuceedFB (Message obj)
        {
            //if (ParseUser.CurrentUser != null) {
            //    Debug.Log("Querying level ranking data...");
            //    RankingManager.Instance.InitializeFriendData();
            //}
            //else {
            //    Debug.Log("get level ranking fail");
            //}
        }


#region Message Handlers
        private void OnDiamondBalanceChanged (Message obj) {
            //diamondChanged = true;
        }

        private void OnUserDataConflictSolved (Message obj) {
            string property = ProfileHelper.Instance.AchievementPropertiesCSV;
            string achievement = ProfileHelper.Instance.AchievementUnlockedAndClaimedCSV;
            if (!string.IsNullOrEmpty(property) && !string.IsNullOrEmpty(achievement)) {
                rawAchievementDump = achievement;
                rawPropertyDump = property;
                AchievementHelper.Instance.RestoreAchievementDataFromDump(rawPropertyDump, rawAchievementDump);
            }
            else {
                Debug.LogWarning("Achievement data is null or empty when trying to solve user data conflict. Skipping...");
            }
        }

        private void OnAchievementDataChanged (Message obj) {
            if (AchievementHelper.Instance.IsDataChanged) {
                SaveAchievementValue();
                //SaveAchievementData();
            }
        }

        private void OnVideoRewardEvent (Message msg) {
            if(msg.data == null) {
                Debug.LogWarning("Null data in video reward event, skip processing");
                return;
            }

            SceneManager.Instance.SetLoadingVisible(false);

            bool success = (bool)msg.data;
            Debug.Log("Video reward view: " + success);
            if (success) {
                Debug.Log("============Video reward value: " + GameConfigs.videoAdsReward);
                ProfileHelper.Instance.CurrentDiamond += GameConfigs.videoAdsReward;
            }else {
                
            }
        }

        private void OnGameDataChanged (Message obj) {
            SaveGameData();
        }

        private void OnUserDataChanged (Message obj) {
            //print("Saving user data");
            SaveUserData(false);
        }
#endregion

#region Save / Load game's data
        public void SaveStoreData () {
            FileUtilities.SerializeObjectToFile<StoreDataModel>(storeData, storedataWritePath, ENCRYPTION_KEY, true);
        }

        public void LoadStoreDataFromLocal () {
            //storeData = FileUtilities.DeserializeObjectFromFile<StoreDataModel>(storedataWritePath, ENCRYPTION_KEY, true);
            //if (storeData == null) {
            //    storeData = new StoreDataModel();
            //}
            var t = Resources.Load<TextAsset>(STORE_DATA_FILE);
            if (t == null) {
                storeData = new StoreDataModel();
            }
            else {
                //Debug.Log(t.text);
                //storeData = FileUtilities.DeserializeObjectFromText<StoreDataModel>(t.text);
                string csv = t.text;
                //fsData jsonData = fsJsonParser.Parse(json);
                StoreDataModel currentStoreData = new StoreDataModel();
                //then de-serialize it for easier access                    
                bool success = ParseStoreData(csv, ref currentStoreData);

                if (!success) {
                    //if fail to de-serialize, we're doomed
                    Debug.LogWarning("Could not de-serialize store data");
                }
                else {
                    //store data has been parsed successfully, 
                    storeData = currentStoreData;
                    storeData.version = VersionsData.store;
                    //SaveStoreData();
                    //Debug.Log("New game store data loaded, version: " + currentStoreData.version);
                }
            }


        }

        public void SaveGameConfigData () {
            FileUtilities.SerializeObjectToFile<GameConfigModel>(configs, gameconfigWritePath, ENCRYPTION_KEY, true);
        }

        public void LoadGameConfigDataFromLocal () {
            //configs = FileUtilities.DeserializeObjectFromFile<GameConfigModel>(gameconfigWritePath, ENCRYPTION_KEY, true);
            //if (configs == null) {
            //    configs = new GameConfigModel();
            //}
            var t = Resources.Load<TextAsset>(GAME_CONFIG_FILE);
            if(t == null) {
                configs = new GameConfigModel();
            }
            else {
                configs = FileUtilities.DeserializeObjectFromText<GameConfigModel>(t.text);
            }
        }

        /// <summary>
        /// Save current game's state into local storage. Such as played level, 
        /// </summary>
        public void SaveGameData () {
            //this.Print("Saving game data...");
            FileUtilities.SerializeObjectToFile<GameDataModel>(gameData, gamestateWritePath, ENCRYPTION_KEY, true);
        }

        public void LoadLocalGameData () {
            //this.Print("Reading local game data ...");
            gameData = FileUtilities.DeserializeObjectFromFile<GameDataModel>(gamestateWritePath, ENCRYPTION_KEY, true);
            if (gameData == null) {
                this.Print("No local game data found, creating new one...");
                gameData = new GameDataModel();
                gameData.numStart = 0;
            }
            GameData.numStart += 1;
            
            //#if !UNITY_EDITOR
            //            //this.Print("Last song ID: " + GameData.lastPlaySongID);
            //            //PlayerPrefs.SetInt(TEMPORARY_KEY_TRANSITION_USER_DATA_LOCATION_112, 112);
            //            if(UnityEngine.Random.Range(0,100) <= 15) {
            //                GameConfigs.mopub_android = "3eb2af31da8641a8a5b4afcd022c305a";
            //                GameConfigs.mopub_ios = "22772838eb7347328010db0a8dfec45d";
            //            }
            //            //SaveGameData();
            //#endif

        }

        public void StoreLastPlayLevel () {
            //this.Print("Storing last level: " + SessionData.currentLevel.songData.storeID);
            SessionData.lastLevel = SessionData.currentLevel;
            GameData.lastPlaySongID = SessionData.currentLevel.songData.storeID;
            MessageBus.Annouce(msgGameDataChanged);
            //this.Print("Last song ID: " + GameData.lastPlaySongID);
        }

        private void SaveGameVersionsData () {
            //this.Print("Saving game versions' data...");
            FileUtilities.SerializeObjectToFile(versionsData, gameversionWritePath, ENCRYPTION_KEY, true);
        }

        internal void LoadLocalGameVersions () {
            var t = Resources.Load<TextAsset>(GAME_VERSION_DATA_FILE);
            if(t != null) {
                versionsData = FileUtilities.DeserializeObjectFromText<GameVersionModel>(t.text);
            }
            else {
                versionsData = new GameVersionModel();
            }
            //this.Print("Reading local game versions' data ...");
            //versionsData = FileUtilities.DeserializeObjectFromFile<GameVersionModel>(gameversionWritePath, ENCRYPTION_KEY, true);
        }


        public void SaveUserData (bool alsoSaveToParse = false) {
            //print("Saving user's data...");
            ProfileHelper.Instance.IncreaseDataVersion();
            //ProfileHelper.Instance.SaveLocalUserData();
            string json = ProfileHelper.Instance.SerializedUserData();
            //print("Saving user data " + json);

//#if !NETFX_CORE
            FileUtilities.SaveFileWithPassword(json, userdataWritePath, ENCRYPTION_KEY, true);
//#else
//            PlayerPrefs.SetString("ptnw", json);
//            PlayerPrefs.Save();
//#endif
        }

        internal void LoadLocalUserData () {
            //this.Print("Reading local user's data ...");
            //if (PlayerPrefs.HasKey(TEMPORARY_KEY_TRANSITION_USER_DATA_LOCATION_112)) {
            //    //PlayerPrefs.SetInt(TEMPORARY_KEY_TRANSITION_USER_DATA_LOCATION_112, 112);
            //    ProfileHelper.Instance.LoadLocalUserData();
            //    //print("here");
            //    PlayerPrefs.DeleteKey(TEMPORARY_KEY_TRANSITION_USER_DATA_LOCATION_112);
            //}
            //else {
            string json;
//#if !NETFX_CORE
            json = FileUtilities.LoadFileWithPassword(userdataWritePath, ENCRYPTION_KEY, true);
//#else
//            json = PlayerPrefs.GetString("ptnw", "");
//#endif
            //print("Loading user data " + json);
            ProfileHelper.Instance.DeserializeUserData(json);
                //ProfileHelper.Instance.LoadLocalUserData();
            //}
        }

        /// <summary>
        /// Save current achievement model to hard disk
        /// </summary>
        private void SaveAchievementData () {
            if (!string.IsNullOrEmpty(rawPropertiesData)) {
                FileUtilities.SaveFileWithPassword(rawPropertiesData, achievementPropertiesWritePath, ENCRYPTION_KEY, true);
            }

            if (!string.IsNullOrEmpty(rawAchievementData)) {
                FileUtilities.SaveFileWithPassword(rawAchievementData, achievementDataWritePath, ENCRYPTION_KEY, true);
            }
        }

        /// <summary>
        /// Save current value of achievement system into hard disk
        /// </summary>
        public void SaveAchievementValue () {
            rawPropertyDump = AchievementHelper.Instance.DumpCurrentProperties();
            if (!string.IsNullOrEmpty(rawPropertyDump)) {
                ProfileHelper.Instance.AchievementPropertiesCSV = rawPropertyDump;
                //Debug.Log("saving property dump: " + rawPropertyDump);
                FileUtilities.SaveFileWithPassword(rawPropertyDump, achievementPropertiesDumpWritePath, ENCRYPTION_KEY, true);
            }

            rawAchievementDump = AchievementHelper.Instance.DumpUnlockedAndClaimedAchievements();
            if (!string.IsNullOrEmpty(rawAchievementDump)) {
                ProfileHelper.Instance.AchievementUnlockedAndClaimedCSV = rawAchievementDump;
                //rawAchievementDump = achievement;
                FileUtilities.SaveFileWithPassword(rawAchievementDump, achievementDataDumpWritePath, ENCRYPTION_KEY, true);
            }
        }

        internal void LoadLocalAchievementData () {
            rawPropertiesData = FileUtilities.LoadFileWithPassword(achievementPropertiesWritePath, ENCRYPTION_KEY, true);
            rawAchievementData = FileUtilities.LoadFileWithPassword(achievementDataWritePath, ENCRYPTION_KEY, true);
            LoadLocalAchievementValue();
        }

        private void LoadLocalAchievementValue () {
            //Debug.Log("Loading property dump from path: " + achievementPropertiesDumpWritePath);
            rawPropertyDump = FileUtilities.LoadFileWithPassword(achievementPropertiesDumpWritePath, ENCRYPTION_KEY, true);
            //Debug.Log("Raw property dump: " + rawPropertyDump);
            rawAchievementDump = FileUtilities.LoadFileWithPassword(achievementDataDumpWritePath, ENCRYPTION_KEY, true);
        }

#endregion

#region Download data from internet
        internal void RefreshLocalizationData (Action onComplete = null, Action<string> onError = null) {
            //bool needDownload = false;
            TextAsset asset = Resources.Load<TextAsset>("Localization");
            if (asset != null) {
                LocalizationData = asset.bytes;
            }
            else {
                Debug.LogWarning("Could not load localization data");
                //needDownload = true;
            }
            Helpers.Callback(onComplete);
            //AssetDownloader.Instance.DownloadAndCacheAsset(GameConfigs.localizationURL, VersionsData.localization,
            //        (progress) => {
            //            //TODO: Report download process here
            //        },
            //        (errorMessage) => {
            //            if (needDownload) {
            //                Helpers.CallbackWithValue(onError, "Error trying to download game localization data \n " + errorMessage);
            //            }
            //            else {
            //                Debug.Log("Failed to download game localization data from the Internet, using local one");
            //                Helpers.Callback(onComplete);
            //            }
            //        },
            //        (downloadedData) => {
            //            //Debug.Log("Successfully download game localization from the Internet");

            //            //try to parse latest config data
            //            LocalizationData = downloadedData.bytes;
            //            //Debug.Log("Downloaded localization data " + csv);
            //            //LocalizationData = csv;
            //            Helpers.Callback(onComplete);
            //        });
        }

        internal void RefreshAchievementData (Action onComplete = null, Action<string> onError = null) {
            //for testing purpose
            //Helpers.Callback(onComplete);
            //return;
            //Debug.Log("Refreshing achievement data");
            bool needDownload = false;
            if (string.IsNullOrEmpty(rawAchievementData) || string.IsNullOrEmpty(rawPropertiesData)) {
                needDownload = true;
            }
            else {
                AchievementHelper.Instance.InitializeWithCSV(rawPropertiesData, rawAchievementData);
                Helpers.Callback(onComplete);
                return;
            }

            if (needDownload) {
                var ap = Resources.Load<TextAsset>(ACHIEVEMENT_PROPERTY_FILE);
                var ad = Resources.Load<TextAsset>(ACHIEVEMENT_FILE);
                if(ap == null || ad == null) {
                    Debug.LogWarning("Can't read achievement data from resource =.=");
                }

                AchievementHelper.Instance.InitializeWithCSV(ap.text, ad.text);
                SaveAchievementData();
                Helpers.Callback(onComplete);
            }

            //if (Application.internetReachability == NetworkReachability.NotReachable) {
            //    //but the game require new data to be downloaded
            //    if (needDownload) {
            //        Helpers.CallbackWithValue(onError, "Fail to initialize game achievement properties");
            //    }
            //    else {
            //        AchievementHelper.Instance.InitializeWithCSV(rawPropertiesData, rawAchievementData);
            //        //if (!string.IsNullOrEmpty(rawPropertyDump) && !string.IsNullOrEmpty(rawAchievementDump)) {
            //        //    AchievementHelper.Instance.RestoreAchievementDataFromDump(rawPropertyDump, rawAchievementDump);
            //        //}
            //        //there is no connection can be made, should report as error
            //        Helpers.Callback(onComplete);
            //        this.Print("Can not connect to the Internet, using local achievement properties");
            //    }
            //}
            //else {
            //    AssetDownloader.Instance.DownloadAndCacheAsset(configs.achievementPropertiesURL, VersionsData.achievement,
            //        (progress) => {
            //        },
            //        (errorMessage) => {
            //            if (needDownload) {
            //                Helpers.CallbackWithValue(onError, "Failed to download Achievement properties with error: \n" + errorMessage);
            //            }
            //            else {
            //                print("Failed to download new achievement properties from the Internet, using local one");
            //                AchievementHelper.Instance.InitializeWithCSV(rawPropertiesData, rawAchievementData);
            //                //if (!string.IsNullOrEmpty(rawPropertyDump) && !string.IsNullOrEmpty(rawAchievementDump)) {
            //                //    AchievementHelper.Instance.RestoreAchievementDataFromDump(rawPropertyDump, rawAchievementDump);
            //                //}
            //                Helpers.Callback(onComplete);
            //            }
            //        },
            //        (www) => {
            //            //Debug.Log("Downloaded achievement properties " + www.text);
            //            string properties = www.text;
            //            if (!string.IsNullOrEmpty(properties)) {
            //                //Download Achievement data
            //                AssetDownloader.Instance.DownloadAndCacheAsset(configs.achievementURL, VersionsData.achievement,
            //                    (progress) => {
            //                    },
            //                    (errorMessage) => {
            //                        if (needDownload) {
            //                            Helpers.CallbackWithValue(onError, "Failed to download Achievement data with error: \n" + errorMessage);
            //                        }
            //                        else {
            //                            print("Failed to download new achievement data from the Internet, using local one");
            //                            AchievementHelper.Instance.InitializeWithCSV(rawPropertiesData, rawAchievementData);
            //                            Helpers.Callback(onComplete);
            //                        }
            //                    },
            //                    (achievementData) => {
            //                        //Debug.Log("Downloaded achievement data " + achievementData.text);
            //                        string data = achievementData.text;
            //                        if (!string.IsNullOrEmpty(data)) {
            //                            rawPropertiesData = properties;
            //                            rawAchievementData = data;

            //                            AchievementHelper.Instance.InitializeWithCSV(rawPropertiesData, rawAchievementData);
            //                            //TODO: populate unlocked achievements here
            //                            SaveAchievementData();
            //                            //if (!string.IsNullOrEmpty(rawPropertyDump) && !string.IsNullOrEmpty(rawAchievementDump)) {
            //                            //    AchievementHelper.Instance.RestoreAchievementDataFromDump(rawPropertyDump, rawAchievementDump);
            //                            //}
            //                            //print("Calling back to start game");
            //                            Helpers.Callback(onComplete);
            //                        }
            //                    }
            //                    );
            //            }
            //        }
            //        );


            //}
        }

        internal void RefreshGameVersionsData (Action onComplete = null, Action<string> onError = null) {
            bool needDownload = false;
            GameVersionModel gameVersions = null;
            if (VersionsData == null) {
                needDownload = true;
            }

            if (Application.internetReachability == NetworkReachability.NotReachable) {
                //but the game require new data to be downloaded
                if (needDownload) {
                    Helpers.CallbackWithValue(onError, "Fail to initial game version file");
                }
                else {
                    //there is no changes to the game config, inform listener about that
                    Helpers.Callback(onComplete);
                    this.Print("Can not connect to the Internet, using local game version data");
                }
            }
            else {
                AssetDownloader.Instance.DownloadAsset(gameVersionURL,
                    (progress) => {
                        //TODO: Report download process here
                    },
                    (errorMessage) => {
                        if (needDownload) {
                            Helpers.CallbackWithValue(onError, "Error trying to download game versions data \n " + errorMessage);
                        }
                        else {
                            this.Print("Failed to download game versions data from the Internet, using local one");
                            if (onComplete != null) {
                                Helpers.Callback(onComplete);
                            }
                        }
                    },
                    (downloadedData) => {
                        this.Print("Successfully download game versions from the internet");

                        //try to parse latest config data
                        string json = downloadedData.text;
                        gameVersions = JsonUtility.FromJson<GameVersionModel>(json);
                        if (gameVersions == null) {
                            //if fail to deserialize, we're doomed
                            this.Print("Could not de-serialize game version data downloaded from server");
                            //message.text = "Failed to parse game config data. Please make sure your device has stable connection to the Internet";
                            if (needDownload) {
                                Helpers.CallbackWithValue(onError, "Error trying to parse game version data \n " );
                            }
                            else {
                                this.Print("Failed to download game version data from the Internet, using local game version");
                                Helpers.Callback(onComplete);
                            }
                        }
                        else {
                            //we made it!!!!
                            VersionsData = gameVersions;
                            SaveGameVersionsData();
                            Helpers.Callback(onComplete);
                        }

                        //fsData jsonData = fsJsonParser.Parse(json);

                        ////then deserialize it for easier acccess                    
                        //fsResult result = FileUtilities.JSONSerializer.TryDeserialize(jsonData, ref gameVersions);

                        //if (result.Failed) {
                        //    //if fail to deserialize, we're doomed
                        //    this.Print("Could not de-serialize game version data downloaded from server");
                        //    //message.text = "Failed to parse game config data. Please make sure your device has stable connection to the Internet";
                        //    if (needDownload) {
                        //        Helpers.CallbackWithValue(onError, "Error trying to parse game version data \n " + result.AsException.ToString());
                        //    }
                        //    else {
                        //        this.Print("Failed to download game version data from the Internet, using local game version");
                        //        Helpers.Callback(onComplete);
                        //    }
                        //}
                        //else {
                        //    //we made it!!!!
                        //    VersionsData = gameVersions;
                        //    SaveGameVersionsData();
                        //    Helpers.Callback(onComplete);
                        //}
                    });
            }
        }


        /// <summary>
        /// Refresh latest game config data from server
        /// </summary>
        /// <param name="onComplete">Callback when the data has been loaded. True if there is any changes in game config</param>
        /// <param name="onError">Callback when there is error getting the file. With detail error message</param>
        public void RefreshGameConfigData (Action<bool> onComplete = null, Action<string> onError = null) {
            bool needDownload = false;
            GameConfigModel gameConfig = null;
            if (GameConfigs == null || GameConfigs.configVersion == 0) {
                needDownload = true;
            }

            if (needDownload || (GameConfigs.configVersion != VersionsData.config)) {
                if (Application.internetReachability == NetworkReachability.NotReachable) {
                    //but the game require new data to be downloaded
                    if (needDownload) {
                        Helpers.CallbackWithValue(onError, "Fail to load configuration file");
                    }
                    else {
                        //there is no changes to the game config, inform listener about that
                        Helpers.CallbackWithValue(onComplete, false);
                        this.Print("Can not connect to the Internet, using local game config");
                    }
                }
                else {
                    AssetDownloader.Instance.DownloadAndCacheAsset(configURL, VersionsData.config,
                        (progress) => {
                            //TODO: Report download process here
                        },
                        (errorMessage) => {
                            if (needDownload) {
                                Helpers.CallbackWithValue(onError, "Error trying to download config data \n " + errorMessage);
                            }
                            else {
                                this.Print("Failed to download game data from the Internet, using local game config");
                                if (onComplete != null) {
                                    Helpers.CallbackWithValue(onComplete, false);
                                }
                            }
                        },
                        (downloadedData) => {
                            this.Print("Successfully download game config");

                            //try to parse latest config data
                            string json = downloadedData.text;

                            gameConfig = JsonUtility.FromJson<GameConfigModel>(json);
                            if (gameConfig == null) {
                                //if fail to deserialize, we're doomed
                                this.Print("Could not de-serialize game data downloaded from server");
                                //message.text = "Failed to parse game config data. Please make sure your device has stable connection to the Internet";
                                if (needDownload) {
                                    Helpers.CallbackWithValue(onError, "Error trying to parse config data \n ");
                                }
                                else {
                                    this.Print("Failed to download game data from the Internet, using local game config");
                                    Helpers.CallbackWithValue(onComplete, false);
                                }
                            }
                            else {
                                //save new config version
                                GameConfigs = gameConfig;
                                GameConfigs.configVersion = VersionsData.config;
                                SaveGameConfigData();
                                this.Print("New game config data loaded, version: " + gameConfig.configVersion);
                                Helpers.CallbackWithValue(onComplete, true);
                            }

                            //fsData jsonData = fsJsonParser.Parse(json);

                            ////then deserialize it for easier acccess                    
                            //fsResult result = FileUtilities.JSONSerializer.TryDeserialize<GameConfigModel>(jsonData, ref gameConfig);

                            //if (result.Failed) {
                            //    //if fail to deserialize, we're doomed
                            //    this.Print("Could not de-serialize game data downloaded from server");
                            //    //message.text = "Failed to parse game config data. Please make sure your device has stable connection to the Internet";
                            //    if (needDownload) {
                            //        Helpers.CallbackWithValue(onError, "Error trying to parse config data \n " + result.AsException.ToString());
                            //    }
                            //    else {
                            //        this.Print("Failed to download game data from the Internet, using local game config");
                            //        Helpers.CallbackWithValue(onComplete, false);
                            //    }
                            //}
                            //else {
                            //    //save new config version
                            //    GameConfigs = gameConfig;
                            //    GameConfigs.configVersion = VersionsData.config;
                            //    SaveGameConfigData();
                            //    this.Print("New game config data loaded, version: " + gameConfig.configVersion);
                            //    Helpers.CallbackWithValue(onComplete, true);
                            //}
                        });
                }
            }
            else {
                this.Print("No changed in game config version, skipping updates...");
                Helpers.CallbackWithValue(onComplete, false);
            }
        }

        /// <summary>
        /// Refresh latest store data from server
        /// </summary>
        /// <param name="onComplete">Callback when the data has been loaded. True if there is any changes in store data</param>
        /// <param name="onError">Callback when there is error getting the file. With detail error message</param>
        public void RefreshStoreData (Action<bool> onComplete = null, Action<string> onError = null) {
            if (configs == null) {
                Helpers.CallbackWithValue(onError, "Config is null");
                return;
            }

            bool needDownload = false;
            var currentStoreData = storeData;
            if (currentStoreData == null || currentStoreData.listAllSongs == null || currentStoreData.listAllSongs.Count <= 0) {
                needDownload = true;
            }

            //this.Print("Prepare to download store data, need download: " + needDownload);
            if (needDownload || (storeData.version != VersionsData.store)) {
                if (Application.internetReachability == NetworkReachability.NotReachable) {
                    //but the game require new data to be downloaded
                    if (needDownload) {
                        Helpers.CallbackWithValue(onError, "Fail to load store data file");
                    }
                    else {
                        //there is no changes to the game config, inform listener about that
                        Helpers.CallbackWithValue(onComplete, false);
                        this.Print("Can not connect to the Internet, using local game store data");
                    }
                }
                else {
                    ////if version of current store data and latest data from server is not the same, or current store data can not be read
                    //if ((needDownload) || (configs.storeDataVersion != currentStoreData.version)) {
                    AssetDownloader.Instance.DownloadAndCacheAsset(configs.storeDataURL, VersionsData.store,
                        (progress) => {
                            //TODO: Report download process here
                        },
                        (errorMessage) => {
                            if (needDownload) {
                                Helpers.CallbackWithValue(onError, "Error trying to download store data \n " + errorMessage);
                            }
                            else {
                                Debug.Log("Failed to download game data from the Internet, using local game store data");
                                Helpers.CallbackWithValue(onComplete, false);
                            }
                        },
                        (downloadedData) => {
                            Debug.Log("Successfully download store data");

                            //try to parse latest config data
                            string csv = downloadedData.text;
                            //fsData jsonData = fsJsonParser.Parse(json);

                            //then de-serialize it for easier access                    
                            bool success = ParseStoreData(csv, ref currentStoreData);

                            if (!success) {
                                //if fail to de-serialize, we're doomed
                                Debug.Log("Could not de-serialize game data downloaded from server");
                                //message.text = "Failed to parse game config data. Please make sure your device has stable connection to the Internet";
                                if (needDownload) {
                                    Helpers.CallbackWithValue(onError, "Error trying to parse store data");
                                }
                                else {
                                    Debug.Log("Failed to download game data from the Internet, using local store data");
                                    Helpers.CallbackWithValue(onComplete, false);
                                }
                            }
                            else {
                                //store data has been parsed successfully, 
                                storeData = currentStoreData;
                                storeData.version = VersionsData.store;
                                SaveStoreData();
                                Debug.Log("New game store data loaded, version: " + currentStoreData.version);
                                Helpers.CallbackWithValue(onComplete, true);

                            }
                        });
                    //}
                    //else {
                    //    this.Print("There is no changes to store data, using local one");
                    //    //if local store data can be read, and has the same version with the one on server, no need to download again
                    //    Helpers.CallbackWithValue(onComplete, false);
                    //}
                }
            }
            else {
                this.Print("No changes to store data, skipping updates...");
                Helpers.CallbackWithValue(onComplete, false);
            }
        }

        private bool ParseStoreData (string csv, ref StoreDataModel storeData) {
            if (string.IsNullOrEmpty(csv)) {
                return false;
            }

            bool res = true;
            //StoreDataModel storeData = new StoreDataModel();
            var songs = new List<SongDataModel>(100);
            var listHot = new List<SongDataModel>(10);
            var listNew = new List<SongDataModel>(10);
            Dictionary<string, int> columnName = new Dictionary<string, int>(10);
            CSVReader.LoadFromString(
                csv,
                //LineReader
                (lineIndex, content) => {
                    if (lineIndex == 0) {
                        //parse header line, then, when parsing rows, instead of calling content[0], we can use content[columnName["ID"]]
                        //for the sake of easy understanding
                        for (int i = 0; i < content.Count; i++) {
                            columnName.Add(content[i], i);
                        }
                        //print("Header column has " + content.Count + " columns");
                    }
                    else {
                        //Debug.LogError(csv);
                        //print(string.Format("Checking content row {0}, num column {1}", lineIndex, content.Count));
                        if (content.Count == columnName.Count) {
                            //Column's name: ID,name,author,songURL,fileVersion,storeID,price,starsToUnlock,maxBPM,tickPerTile,speedModifier,speedPerStar,type
                            SongDataModel song = new SongDataModel();
                            song.ID = int.Parse(content[columnName["ID"]]);
                            song.name = content[columnName["name"]];
                            song.author = content[columnName["author"]];
                            song.songURL = content[columnName["songURL"]];
                            song.version = int.Parse(content[columnName["fileVersion"]]);
                            song.storeID = content[columnName["storeID"]];
                            song.pricePrimary = int.Parse(content[columnName["price"]]);
                            song.starsToUnlock = int.Parse(content[columnName["starsToUnlock"]]);
                            song.maxBPM = int.Parse(content[columnName["maxBPM"]]);
                            song.tickPerTile = int.Parse(content[columnName["tickPerTile"]]);
                            song.speedModifier = float.Parse(content[columnName["speedModifier"]]);
                            if (!string.IsNullOrEmpty(content[columnName["speedPerStar"]].ToString())) {
                                //Debug.Log(content[columnName["speedPerStar"]].ToString());
                                //List<float> aa = new List<float>();
                                //aa.Add(1f);
                                //aa.Add(1.2f);
                                //Debug.

                                //fsData speedJson = fsJsonParser.Parse(content[columnName["speedPerStar"]]);
                                //fsData speedJson = fsJsonParser.Parse(content[columnName["speedPerStar"]]);
                                ////  var a = speedJson.AsList;
                                //var speed = speedJson.AsList;
                                ////Debug.Log(speed.Count);
                                //List<float> listSpeed = new List<float>();
                                //for (int i = 0; i < speed.Count; i++) {
                                //    //print("Casting " + speed[i]);
                                //    listSpeed.Add((float)(speed[i].AsDouble));
                                //}
                                //if (listSpeed.Count > 0) {
                                //    //Debug.Log("can setup speed");
                                //    song.speedPerStar = listSpeed;
                                //}
                                //else {
                                //    //Debug.Log("no setup speed");
                                //    song.speedPerStar = null;
                                //}
                            }
                            //song.speedPerStar = content.GetField<List<float>>( "HeaderName" );
                            song.type = (SongItemType)(int.Parse(content[columnName["type"]]));

                            if (song.type == SongItemType.Hot) {
                                listHot.Add(song);
                            }
                            else if (song.type == SongItemType.New) {
                                listNew.Add(song);
                            }

                            //print("Add song " + song.name);
                            songs.Add(song);
                        }
                    }
                }
            );

            storeData.listAllSongs = songs;
            storeData.listHotSongs = listHot;
            storeData.listNewSongs = listNew;
            return res;
        }

        //private void LineReader(int line_index, List<string> line) {
        //    print("Current line: " + line_index);
        //    for(int i = 0; i < line.Count; i++) {
        //        print("--" + line[i]);
        //    }
        //}
#endregion


        //public void PushCurrentHighscore (PlayerRankingModel pModel) {
        //    if (ParseUser.CurrentUser == null)
        //        return;
        //    //Loom.Instance.CheckInitial();
        //    //if (ParseUser.CurrentUser != null) {// da dang nhap facebook

        //    fsData data = null;
        //    string json = "";

        //    var result = FileUtilities.JSONSerializer.TrySerialize<PlayerRankingModel>(pModel, out data);
        //    if (result.Failed) {
        //        Debug.LogError("Can't serialize PlayerRankingModel, skipping");
        //        return;
        //    }

        //    json = data.ToString();
        //    //Debug.LogError ("json:"+json);
        //    if (!string.IsNullOrEmpty(json)) {
        //        ParseQuery<ParseObject> query = ParseObject.GetQuery("LevelRanking").WhereEqualTo("user_id", pModel.user_id);
        //        query.Select("user_id");
        //        query.Select("content");
        //        query.Limit(1);
        //        query.FindAsync().ContinueWith(t => {
        //            Loom.Instance.QueueOnMainThread(() => {
        //                if (t.IsFaulted) {
        //                    Debug.LogError("Loi Query Parse save ranking user");
        //                    return;
        //                }
        //                List<ParseObject> listParse = new List<ParseObject>();

        //                if (t.IsCompleted) {
        //                    IEnumerable<ParseObject> results = t.Result;
        //                    IEnumerator<ParseObject> iterator = results.GetEnumerator();

        //                    while (iterator.MoveNext()) {
        //                        ParseObject currentObj = iterator.Current;
        //                        if (currentObj != null) {
        //                            listParse.Add(currentObj);
        //                        }
        //                    }

        //                    ParseObject obj = null;
        //                    if (listParse.Count > 0) {
        //                        obj = listParse[0];
        //                    }

        //                    List<PlayerRankingModel> listplayer;
        //                    if (obj == null)// khong co -> ghi du lieu moi len parse
        //                    {
        //                        Debug.Log("insert du lieu:");
        //                        // khong co du lieu ghi moi vao
        //                        ParseObject rankLevel = new ParseObject("LevelRanking");
        //                        rankLevel["user_id"] = pModel.user_id;
        //                        listplayer = new List<PlayerRankingModel>();
        //                        listplayer.Add(pModel);
        //                        fsData dataPlayer = null;
        //                        //string json = "";
        //                        FileUtilities.JSONSerializer.TrySerialize<List<PlayerRankingModel>>(listplayer, out dataPlayer);
        //                        rankLevel["content"] = dataPlayer.ToString();
        //                        rankLevel.SaveAsync();
        //                    }
        //                    else {

        //                        Debug.Log("update du lieu:");
        //                        ParseObject rankLevel = obj;
        //                        //string contentOld = "";
        //                        string oldName = "";
        //                        if (rankLevel.ContainsKey("content")) {
        //                            //rankLevel.TryGetValue("content", out contentOld);
        //                            string jsonUserRanking = (string)rankLevel["content"];
        //                            listplayer = new List<PlayerRankingModel>();
        //                            fsData jsonData = fsJsonParser.Parse(jsonUserRanking);
        //                            fsResult dresult = FileUtilities.JSONSerializer.TryDeserialize<List<PlayerRankingModel>>(jsonData, ref listplayer);
        //                            if (dresult.Failed) {
        //                                Debug.LogError("Failed to parse user ranking");
        //                            }

        //                            bool isExitedSongLevelRanking = false;
        //                            for (int i = 0; i < listplayer.Count; i++) {
        //                                if (listplayer[i].song_id.Equals(pModel.song_id)) {
        //                                    listplayer[i] = pModel;
        //                                    isExitedSongLevelRanking = true;
        //                                    break;
        //                                }
        //                            }
        //                            if (!isExitedSongLevelRanking)
        //                                listplayer.Add(pModel);
        //                            fsData dataPlayer = null;
        //                            //string json = "";
        //                            FileUtilities.JSONSerializer.TrySerialize<List<PlayerRankingModel>>(listplayer, out dataPlayer);
        //                            rankLevel["content"] = dataPlayer.ToString();

        //                        }
        //                        else {
        //                            Debug.LogWarning("khong co content parse");
        //                        }


        //                        if (rankLevel.ContainsKey("user_name"))
        //                            rankLevel.TryGetValue("user_name", out oldName);
        //                        if (oldName.Contains((string)ParseUser.CurrentUser[GameConsts.PARSE_DATA_COLLUM_USERNAME])) {
        //                            rankLevel["user_name"] = ParseUser.CurrentUser[GameConsts.PARSE_DATA_COLLUM_USERNAME];
        //                        }

        //                        else {
        //                            //Debug.LogWarning("khong co username parse");
        //                            rankLevel["user_name"] = ParseUser.CurrentUser[GameConsts.PARSE_DATA_COLLUM_USERNAME];
        //                        }

        //                        rankLevel.SaveAsync();
        //                    }
        //                }
        //            });
        //        });

        //    }
        //    // }
        //}

        //public void InitializeFriendData () {
        //    //Debug.Log(AccessToken.CurrentAccessToken.UserId);
        //    if (AccessToken.CurrentAccessToken == null) return;

        //    FB.API("me/friends?fields=id,name", HttpMethod.GET, resGetInfo => {
        //        //			Debug.Log("Friends:"+resGetInfo.RawResult);
        //        //  List<string> listFriend = new List<string>();
        //        if (!resGetInfo.Cancelled && string.IsNullOrEmpty(resGetInfo.Error)) {
        //            //Debug.LogError("Exception qk:");
        //            try {
        //                m_listFriends = new List<string>();
        //                string json = resGetInfo.RawResult;
        //                fsData jsonData = fsJsonParser.Parse(json);
        //                //FBFriendQuery query = Pathfinding.Serialization.JsonFx.JsonReader.Deserialize(json, typeof(FBFriendQuery)) as FBFriendQuery;
        //                FBFriendQuery query = new FBFriendQuery();
        //                fsResult r = FileUtilities.JSONSerializer.TryDeserialize<FBFriendQuery>(jsonData, ref query);
        //                if (!r.Failed) {
        //                    for (int i = 0; i < query.data.Count; i++) {
        //                        m_listFriends.Add(query.data[i].id);
        //                    }
        //                }
        //            }
        //            catch (System.Exception ex) {
        //                Debug.LogError("Exception query friend facebook:" + ex.Message);
        //            }
        //        }
        //        else {
        //            Debug.LogError("Error:" + resGetInfo.Error.ToString());
        //        }
        //        QueryLevelSongInParse();
        //    });

        //    //OneSignalPushManager.PushNotificationToAllFriend();

        //}

        //private void QueryLevelSongInParse () {
        //    //Debug.LogError("Go Parse");
        //    if (m_listFriends == null)
        //        m_listFriends = new List<string>();
        //    m_listFriends.Add(AccessToken.CurrentAccessToken.UserId);
        //    Loom.Instance.CheckInitial();// check initial
        //    var query = ParseObject.GetQuery("LevelRanking").WhereContainedIn("user_id", m_listFriends).Limit(10);
        //    query.Select("user_id");
        //    query.Select("content");
        //    query.FindAsync().ContinueWith(t => {
        //        if (t.IsFaulted) {
        //            Debug.LogError("Error: " + t.Result.ToString());
        //            Loom.Instance.QueueOnMainThread(() => {
        //                //InitData(false);
        //            });
        //        }
        //        else {
        //            if (t.IsCompleted) {
        //                Loom.Instance.QueueOnMainThread(() => {// xu ly trong main thread
        //                    //Debug.Log("completed get levelranking in Parse");
        //                    IEnumerable<ParseObject> results = t.Result;
        //                    IEnumerator<ParseObject> iterator = results.GetEnumerator();
        //                    List<PlayerRankingModel> listplayer;

        //                    dic_player_rank = new Dictionary<string, List<PlayerRankingModel>>();
        //                    while (iterator.MoveNext()) {
        //                        listplayer = new List<PlayerRankingModel>();
        //                        //Debug.Log("next");
        //                        ParseObject currentObj = iterator.Current;
        //                        //string user_id = (string)currentObj["user_id"];
        //                        if (currentObj.ContainsKey("content")) {
        //                            string json = (string)currentObj["content"];

        //                            fsData jsonData = fsJsonParser.Parse(json);
        //                            fsResult result = FileUtilities.JSONSerializer.TryDeserialize<List<PlayerRankingModel>>(jsonData, ref listplayer);
        //                            //Debug.Log(json);
        //                            if (!result.Failed) {
        //                                //listplayer.Sort(new SortInSong());

        //                                for (int i = 0; i < listplayer.Count; i++) {
        //                                    if (!dic_player_rank.ContainsKey(listplayer[i].song_id))
        //                                        dic_player_rank[listplayer[i].song_id] = new List<PlayerRankingModel>();
        //                                    dic_player_rank[listplayer[i].song_id].Add(listplayer[i]);
        //                                }
        //                            }
        //                        }
        //                        else {
        //                            Debug.LogError("Data receive not contain <content>");
        //                        }
        //                    }
        //                    if (dic_player_rank != null) {
        //                        foreach (KeyValuePair<string, List<PlayerRankingModel>> pair in dic_player_rank) {
        //                            //Debug.Log("aaaa" + pair.Key);
        //                            pair.Value.Sort(new PlayerRankingComparer());
        //                        }
        //                        MessageBus.Annouce(new Message(MessageBusType.CompletedLevelRanking));
        //                    }
        //                    //InitData(true);
        //                });
        //            }
        //        }
        //    });

        //}

        //public int GetIndexLevelRankingCurrentUser (string song_id) {
        //    if (string.IsNullOrEmpty(song_id))
        //        return 0;
        //    else {
        //        int index = 0;
        //        if (dic_player_rank != null) {
        //            List<PlayerRankingModel> pModel;

        //            dic_player_rank.TryGetValue(song_id, out pModel);
        //            if (pModel != null && pModel.Count > 0) {
        //                for (int i = 0; i < pModel.Count; i++) {
        //                    if (pModel[i].user_id == AccessToken.CurrentAccessToken.UserId) {
        //                        index = i + 1;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        return index;
        //    }
        //}

        //public List<PlayerRankingModel> GetListFriendsPlayInSong (string songId) {
        //    List<PlayerRankingModel> listFriends = null;
        //    if (dic_player_rank != null) {
        //        if (dic_player_rank.ContainsKey(songId))
        //            listFriends = dic_player_rank[songId];
        //    }
        //    return listFriends;

        //}

        ///// <summary>
        ///// Update local current ranking for this user after playing any level/song
        ///// </summary>
        ///// <param name="pModel"></param>
        //public void UpdateLevelRanking (PlayerRankingModel pModel) {
        //    bool isNewPlayThisSong = true;

        //    if (dic_player_rank == null) dic_player_rank = new Dictionary<string, List<PlayerRankingModel>>();

        //    List<PlayerRankingModel> listpModel;

        //    //try to get level ranking for specified song
        //    if (dic_player_rank.TryGetValue(pModel.song_id, out listpModel)) {
        //        //if success, browse all level ranking record for that song
        //        for (int i = 0; i < listpModel.Count; i++) {
        //            //and update record of this user
        //            if (listpModel[i].user_id == AccessToken.CurrentAccessToken.UserId) {
        //                isNewPlayThisSong = false;
        //                if (listpModel[i].score < pModel.score)
        //                    listpModel[i] = pModel;

        //                break;
        //            }
        //        }
        //        //if there is no record for this user, add a new one
        //        if (isNewPlayThisSong) listpModel.Add(pModel);

        //        //finally, rank them
        //        listpModel.Sort(new PlayerRankingComparer());
        //    }
        //    else {
        //        //if there is no ranking for this song, create a new one
        //        listpModel = new List<PlayerRankingModel>();
        //        listpModel.Add(pModel);
        //        dic_player_rank[pModel.song_id] = listpModel;
        //    }

        //    //Debug.Log("refect : " + listpModel);
        //    //if (listpModel == null) {
        //    //    //Debug.Log("refacr null : ");
        //    //    listpModel = new List<PlayerRankingModel>();
        //    //    listpModel.Add(pModel);
        //    //    dic_player_rank[pModel.song_id] = listpModel;
        //    //}
        //    //else {
        //    //    for (int i = 0; i < listpModel.Count; i++) {
        //    //        if (listpModel[i].user_id == AccessToken.CurrentAccessToken.UserId ) {
        //    //            //Debug.Log("add: " + pModel.user_name);
        //    //            isNewPlayThisSong = false;
        //    //            if(listpModel[i].score < pModel.score)
        //    //                listpModel[i] = pModel;
        //    //        }
        //    //    }
        //    //    if (isNewPlayThisSong)
        //    //        listpModel.Add(pModel);
        //    //    listpModel.Sort(new SortInSong());
        //    //}
        //    //MessageBus.Annouce(new Message(MessageBusType.CompletedLevelRanking));
        //}

		//private GAME_PLAY_MODE playmode=GAME_PLAY_MODE.NORMAL;
		//public GAME_PLAY_MODE PlayMode{
		//	set{ 
		//		playmode = value;
		//	}
		//	get{ 
		//		return playmode;
		//	}
		//}
		//public void SetPlayMode(GAME_PLAY_MODE mode){
		//	PlayMode = mode;
		//}
    }
}