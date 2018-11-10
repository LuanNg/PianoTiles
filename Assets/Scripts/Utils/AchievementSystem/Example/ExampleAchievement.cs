using UnityEngine;
using System.Collections;
using Achievement;
using System.Collections.Generic;
using System;

public class ExampleAchievement : MonoBehaviour {   
    public AchievementBanner achbanner;

    [SerializeField]
    private TextAsset propertyCSV;
    [SerializeField]
    private TextAsset achievementCSV;

	void Start () {
        AchievementHelper.Instance.InitializeWithCSV(propertyCSV.text, achievementCSV.text);
        AchievementHelper.Instance.AchievementManager.OnAchievementUnlocked -= OnAchievementUnlocked;
        AchievementHelper.Instance.AchievementManager.OnAchievementUnlocked += OnAchievementUnlocked;
    }

    private void OnAchievementUnlocked(Achievement.Achievement ach) {
        achbanner.ShowAchievement(ach.DisplayName, ach.Description);
    }

    public void LeftButtonClicked() {
        AchievementHelper.Instance.AchievementManager.IncreaseAchievementProperty("LeftButtonClicked", 1);
    }


    public void FinishSong() {
        AchievementHelper.Instance.AchievementManager.IncreaseAchievementProperty("songfinished");
    }

    public void BuySong() {
        AchievementHelper.Instance.AchievementManager.IncreaseAchievementProperty("songowned");
    }

    public void Got5Star() {
        AchievementHelper.Instance.AchievementManager.IncreaseAchievementProperty("song5star");
    }

    public void DumpProperties() {
        Debug.Log("Dumping properties...");
        //Debug.Log(AchievementHelper.Instance.DumpProperties());
    }

    public void DumpAchievements() {
        Debug.Log("Dumping achievements...");
        //Debug.Log(AchievementHelper.Instance.DumpAchievements());
    }


    // Update is called once per frame
 //   void Update () {
 //       var achs = ach.CheckAllAchievements();
 //       foreach(var achievement in achs) {
 //           Debug.Log("Achievement unlocked: " + achievement.DisplayName);
 //       }
	//}
}
