using UnityEngine;
using System.Collections;
using Achievement;
//using FullSerializer;
using System.Collections.Generic;
using System;
using System.Text;
using Mio.Utils.MessageBus;
using Mio.TileMaster;

public class AchievementHelper : MonoBehaviour {
    //private static readonly fsSerializer _serializer = new fsSerializer();
    private static AchievementHelper m_Instance;
    private static Message msgAchievementDataChanged = new Message(MessageBusType.AchievementDataChanged);
    public static AchievementHelper Instance {
        get { return m_Instance; }
    }

    AchievementManager ach = new AchievementManager();
    public AchievementManager AchievementManager {
        get { return ach; }
    }

	bool isCollect = false;
    /// <summary>
    /// Is there any changes since the last property dump?
    /// </summary>
    public bool IsDataChanged {
        get;private set;
    }

    // Use this for initialization
    void Awake() {
        m_Instance = this;
    }

    private static List<PropertyModel> listProperties = new List<PropertyModel>(25);
    private static List<AchievementModel> listAchievements = new List<AchievementModel>(50);

    public void InitializeWithCSV(string propertyData, string achievementData) {
        //CSVReader.LoadFromString(propertyData, InitPropertyDataLineRead);
        //CSVReader.LoadFromString(achievementData, InitAchievementDataLineRead);
        //DoInitialize();
        //Invoke("DoInitialize",1);
    }

    private void DoInitialize() {
        ach.Initialize(listProperties, listAchievements);
        ach.OnAchievementUnlocked -= OnAchievementUnlocked;
        ach.OnAchievementUnlocked += OnAchievementUnlocked;
        //print("Achievement system initialized");
    }

    private void OnAchievementUnlocked (Achievement.Achievement achievement) {
        if(GameManager.Instance != null) GameManager.Instance.SessionData.needAttentionAchievement = true;

        Mio.Utils.AnalyticsHelper.Instance.LogUnlockAchievement(achievement.ID);
        Mio.Utils.AnalyticsHelper.Instance.LogClamiableAchievement(achievement.DisplayName);
		Debug.Log ("OnAchievementUnlocked");
		if (achievement.Data.tag == "daily" && isCollect == true) {
			AchievementHelper.Instance.LogAchievement("finishQuest");
		}
		MessageBus.Annouce(msgAchievementDataChanged);
    }

    /// <summary>
    /// Log/Increase a property in achievement system
    /// </summary>
    public void LogAchievement(string achievementProperty, int value = 1) {
		//Debug.Log ("LogAchievement");
  //      IsDataChanged = true;
  //      ach.IncreaseAchievementProperty(achievementProperty, value);
		//if(!isCollect)
		//	isCollect = true;
        //MessageBus.Annouce(msgAchievementDataChanged);
    }

    private void InitAchievementDataLineRead(int line_index, List<string> line) {
        if (line_index > 0) {
            //Debug.LogFormat("Achievement {0}, tag {1}", line[1], line[8])
            //ID,title,description,conditions,tag,ishidden,isactive
            var achievement = CreateAchievementModel(
                line[0],//id
                line[1],//title
                line[2],//description 
                line[3],//icon
                line[4],//condition
                line[5],//reward                
                !string.IsNullOrEmpty(line[6]),//is hidden
                !string.IsNullOrEmpty(line[7]),//is active
                line.Count > 8 ? line[8] : string.Empty //tag
                );
            listAchievements.Add(achievement);
        }
    }

    private void InitPropertyDataLineRead(int line_index, List<string> line) {
        if (line_index > 0) {
            string id = line[0];
            int initValue = int.Parse(line[1]);
            string tag = null;
            if (line.Count > 2) {
                tag = line[2];
            }
            var property = CreatePropertyModel(id, initValue, tag);
            listProperties.Add(property);
        }
    }

    /// <summary>
    /// Return a string as CSV contains current value of all properties in system. For backup purpose
    /// </summary>
    public string DumpCurrentProperties() {
        //var allProperties = ach.GetAllPropertiesData();
        //if (allProperties == null) {
        //    Debug.LogWarning("All properties data return as null!");
        //    return null;
        //}

        //StringBuilder res = new StringBuilder(1000);
        ////header row
        //res.Append("ID,current value");
        //res.Append('\n');
        //for (int i = 0; i < allProperties.Count; i++) {
        //    //Debug.Log("Dumping property " + allProperties[i].ID + " value: " + allProperties[i].currentValue);
        //    res.Append(allProperties[i].ID);
        //    res.Append(',');
        //    res.Append(allProperties[i].currentValue);
        //    res.Append('\n');
        //}
        //IsDataChanged = false;
        ////Debug.Log("Dumping achievement value: " + res.ToString());
        //return res.ToString();
        return string.Empty;
    }

    public void ClaimAchievement(AchievementModel achievement, Action<List<AchievementRewardModel>> onAchievementClaimed = null) {
        if (!achievement.isClaimed) {
            //print("Claiming achievement with hash: " + achievement.GetHashCode());
            ach.MarkAchievementAsClaimed(achievement.ID);
            if(onAchievementClaimed != null) {
                onAchievementClaimed(achievement.listReward);
                Mio.Utils.AnalyticsHelper.Instance.LogClaimedAchievement(achievement.title);
            }

            MessageBus.Annouce(msgAchievementDataChanged);
        }
    }

    /// <summary>
    /// Get unlocked and claimed status of all achievement in current system. For back up purpose
    /// </summary>
    public string DumpUnlockedAndClaimedAchievements() {
        //var allAchievements = ach.GetAllAchievementsData();
        //if (allAchievements == null) {
        //    Debug.LogWarning("All achievements data return as null!");
        //    return null;
        //}

        //StringBuilder res = new StringBuilder(1000);
        ////header row
        //res.Append("ID,isunlocked,isclaimed");
        //res.Append('\n');
        //for (int i = 0; i < allAchievements.Count; i++) {
        //    var achievement = allAchievements[i];
        //    if(achievement.isUnlocked || achievement.isClaimed) {
        //        res.Append(achievement.ID);
        //        res.Append(',');
        //        res.Append(achievement.isUnlocked ? "x" : "");
        //        res.Append(',');
        //        res.Append(achievement.isClaimed ? "x" : "");
        //        res.Append('\n');
        //    }
        //}
        //return res.ToString();
        return string.Empty;
    }

    /// <summary>
    /// Restore achievement system value from a dump backup
    /// </summary>
    /// <param name="propertyDump"></param>
    /// <param name="unlockedAndClaimedAchievementDump"></param>
    public void RestoreAchievementDataFromDump(string propertyDump, string unlockedAndClaimedAchievementDump) {
        //Debug.Log("Restoring property data with string: " + propertyDump);
        //Debug.Log("Restoring achievement data with string: " + unlockedAndClaimedAchievementDump);
        //CSVReader.LoadFromString(unlockedAndClaimedAchievementDump, RestoreAchievementDumpLineRead);
        //CSVReader.LoadFromString(propertyDump, RestorePropertyDumpLineRead);
    }

    private void RestorePropertyDumpLineRead(int line_index, List<string> line) {
        if(line_index > 0) {
            //Debug.Log("Setting value for property " + line[0] + " value: " + line[1]);
            ach.SetPropertyValue(line[0], int.Parse(line[1]));
        }
    }

    private void RestoreAchievementDumpLineRead(int line_index, List<string> line) {
        if(line_index > 0) {
            if (!string.IsNullOrEmpty(line[1])) {
                ach.MarkAchievementAsUnlocked(line[0]);
            }

            if (!string.IsNullOrEmpty(line[2])) {
                ach.MarkAchievementAsClaimed(line[0]);
            }
        }
    }
    

    public void ResetAchievementsWithTag(string tag) {
        //ach.ResetAchievementsByTag(tag);
        //MessageBus.Annouce(msgAchievementDataChanged);
    }

    public List<string> GetUnlockedAchievements() {
        var allAchievements = ach.GetAllAchievementsData();
        if (allAchievements == null) {
            Debug.LogWarning("All achievements data return as null!");
            return null;
        }

        List<string> unlockedAchievements = new List<string>(200);

        for (int i = 0; i < allAchievements.Count; i++) {
            if (allAchievements[i].isUnlocked) {
                unlockedAchievements.Add(allAchievements[i].ID);
            }
        }

        return unlockedAchievements;
    }

    internal float GetAchievementProgress(string achievementID) {
        return AchievementManager.GetAchievementProgress(achievementID);
    }

    public List<AchievementModel> GetListAchievements() {
        return ach.GetAllAchievementsData();
    }

    /// <summary>
    /// Get all achievements with specified type
    /// </summary>
    public List<AchievementModel> GetListAchievements (AchievementType type) {
        string tag = string.Empty;
        switch (type) {
            case AchievementType.Once:
                tag = string.Empty;
                break;
            case AchievementType.RepeatDaily:
                tag = "daily";
                break;
        }
        return ach.GetAllAchievementsDataWithTag(tag);
    }

    public string DumpAchievements() {
        var allAchievements = ach.GetAllAchievementsData();
        StringBuilder res = new StringBuilder(1000);
        //header row
        res.Append("ID,title,description,conditions,rewards,ishidden,isactive,isunlocked,isclaimed");
        res.Append('\n');
        for (int j = 0; j < allAchievements.Count; j++) {
            var achievement = allAchievements[j];            
            res.Append(achievement.ID);
            res.Append(',');
            res.Append(achievement.title);
            res.Append(',');
            res.Append(achievement.description);
            res.Append(',');

            //serialize conditions
            res.Append("\"[");
            for (int i = 0; i < achievement.listConditions.Count; i++)/* var condition in achievement.listConditions) */{
                res.Append('[');
                //the target property
                res.Append("\"\"");
                res.Append(achievement.listConditions[i].propertyID);
                res.Append("\"\"");
                res.Append(',');
                //the expression string
                res.Append("\"\"");
                res.Append(achievement.listConditions[i].expressionString);
                res.Append("\"\"");
                res.Append(',');
                //the target value
                res.Append(achievement.listConditions[i].targetValue);
                res.Append(']');
                res.Append(',');
            }
            //remove last comma
            res.Remove(res.Length - 1, 1);
            res.Append("]\"");
            //finished conditions
            res.Append(',');

            //serialize reward list
            res.Append("\"[");
            for (int i = 0; i < achievement.listReward.Count; i++)/* var condition in achievement.listConditions) */{
                res.Append('[');
                //the target property
                res.Append(achievement.listReward[i].value);
                res.Append(',');
                //the expression string
                res.Append("\"\"");
                res.Append(achievement.listReward[i].type);
                res.Append("\"\"");
                res.Append(']');
                res.Append(',');
            }
            //remove last comma
            res.Remove(res.Length - 1, 1);
            res.Append("]\"");
            //finished reward list
            res.Append(',');

            //is hidden?
            if (achievement.isHidden) {
                res.Append('x');
            }
            res.Append(',');
            //is active?
            if (achievement.isActive) {
                res.Append('x');
            }
            res.Append(',');
            //is unlocked?
            if (achievement.isUnlocked) {
                res.Append('x');
            }
            res.Append(',');
            //is claimed?
            if (achievement.isClaimed) {
                res.Append('x');
            }
            res.Append('\n');
        }
        return res.ToString();
    }

    public static PropertyModel CreatePropertyModel(string ID, int initValue, string tag = null, int currentValue = 0) {
        PropertyModel prop = new PropertyModel();
        prop.ID = ID;
        prop.initValue = initValue;
        prop.currentValue = initValue;
        prop.tag = tag;

        return prop;
    }

    public static AchievementModel CreateAchievementModel(string ID,
                                                            string title,
                                                            string description,
                                                            string icon, string condition,
                                                            string rewardJSON,                                                            
                                                            bool isActive,
                                                            bool isHidden,
                                                            string tag) {
        //Condition conds = Deserialize(typeof(Condition), condition) as Condition;
        //fsData operators = fsJsonParser.Parse(condition);

        //List<OperatorModel> listOperators = new List<OperatorModel>(3);
        ////parse all operator from json array
        //var operatorList = operators.AsList;
        //for (int i = 0; i < operatorList.Count; i++) {
        //    var conditionParts = operatorList[i].AsList;
        //    OperatorModel om = new OperatorModel();
        //    om.propertyID = conditionParts[0].AsString;
        //    om.expressionString = conditionParts[1].AsString;
        //    om.targetValue = (int)conditionParts[2].AsInt64;
        //    listOperators.Add(om);
        //}

        //if (listOperators.Count > 0) {
        //    AchievementModel am = new AchievementModel();

        //    fsData reward = fsJsonParser.Parse(rewardJSON);
        //    var rewardList = reward.AsList;
        //    var listRewardModel = new List<AchievementRewardModel>(5);
        //    for (int i = 0; i < rewardList.Count; i++) {
        //        var rewardParts = rewardList[i].AsList;

        //        AchievementRewardModel r = new AchievementRewardModel();
        //        r.value = (int)rewardParts[0].AsInt64;
        //        r.type = rewardParts[1].AsString;

        //        listRewardModel.Add(r);
        //    }


        //    am.ID = ID;
        //    am.title = title;
        //    am.description = description;
        //    am.icon = icon;
        //    am.isActive = isActive;
        //    am.isHidden = isHidden;
        //    am.isUnlocked = false;
        //    am.listConditions = listOperators;
        //    am.listReward = listRewardModel;
        //    am.tag = tag;
        //    return am;
        //}

        return null;
    }

    public static object Deserialize(Type type, string serializedState) {
        // step 1: parse the JSON data
        //fsData data = fsJsonParser.Parse(serializedState);
        ////data.
        //// step 2: deserialize the data
        //object deserialized = null;
        //_serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

        //return deserialized;
        return null;
    }

    #region GM Command
    public static void GMIncreaseAchievementProperty(string id, int value) {
        if (m_Instance != null) {
            m_Instance.LogAchievement(id, value);
        }
        Debug.Log("Achievement increased");
    }

    public static void GMUnlockAchievement(string id) {
        if(m_Instance != null) {
            m_Instance.AchievementManager.MarkAchievementAsUnlocked(id);
        }
        Debug.Log("Achievement unlocked");
    }
    #endregion
}


