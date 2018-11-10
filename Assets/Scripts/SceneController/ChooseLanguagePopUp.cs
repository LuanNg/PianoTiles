using UnityEngine;
using System.Collections;
using Mio.Utils.MessageBus;
using Mio.TileMaster;
using Mio.Utils;

public class ChooseLanguagePopUp : SSController {
    public ChooseLanguageItemView[] listLanguage;

    public override void OnEnable () {
        base.OnEnable();
        MessageBus.Instance.Subscribe(MessageBusType.LanguageChanged, OnLanguageChanged);
        string currLanguage = Localization.language;
        SelectLanguageController(currLanguage);
    }

    public override void OnDisable () {
        base.OnDisable();
        MessageBus.Instance.Unsubscribe(MessageBusType.LanguageChanged, OnLanguageChanged);
    }

    private void OnLanguageChanged (Message msg) {
        string language = (string)msg.data;
        GameManager.Instance.SetupGameFont(language);
        SelectLanguageController(language);
        AnalyticsHelper.Instance.LogClickItem("Change language " + language);
    }

    public void SelectLanguageController (string currLanguage) {
        for (int i = 0; i < listLanguage.Length; i++) {
            listLanguage[i].InitUI(currLanguage);
        }
    }

    public void ClosePopUp () {
        SceneManager.Instance.CloseScene();
    }
}
