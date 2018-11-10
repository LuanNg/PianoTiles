using UnityEngine;
using System.Collections;
using System;
using Mio.TileMaster;
using Mio.Utils.MessageBus;
using Mio.Utils;
public class RemoveAdsPopUp : SSController {
    [SerializeField]
    private UILabel lbPriceNoAdsItem;
    [SerializeField]
    private GameObject isPuschased;
    [SerializeField]
    private GameObject btnBuyNoAds;


    public void InitUI () {
        isPuschased.SetActive(AdHelper.Instance.IsAdsDisabled());
        btnBuyNoAds.SetActive(!AdHelper.Instance.IsAdsDisabled());
    }
    public override void OnEnable () {
        base.OnEnable();
        if (GameManager.Instance.GameConfigs.iap_prices != null)
            lbPriceNoAdsItem.text = GameManager.Instance.GameConfigs.iap_prices[(int)StorePackage.RemoveAds];
        InitUI();
        MessageBus.Instance.Subscribe(MessageBusType.PurchaseFailed, OnProductPurchasedFailed);
        MessageBus.Instance.Subscribe(MessageBusType.ProductPurchased, OnProductPurchased);
        MessageBus.Instance.Subscribe(MessageBusType.OnCompletedRestoreApple, OncompletedRestore);

    }
    public override void OnDisable () {
        base.OnDisable();
        MessageBus.Instance.Unsubscribe(MessageBusType.PurchaseFailed, OnProductPurchasedFailed);
        MessageBus.Instance.Unsubscribe(MessageBusType.ProductPurchased, OnProductPurchased);
        MessageBus.Instance.Unsubscribe(MessageBusType.OnCompletedRestoreApple, OncompletedRestore);
    }

    private void OncompletedRestore (Message mess) {
        //AdHelper.Instance.SetNoAds(IAPManager.m_StoreController.products.all[(int)StorePackage.RemoveAds].hasReceipt);
        SceneManager.Instance.SetLoadingVisible(false);
        InitUI();
        string command = "restore fail";
        if ((bool)mess.data == true)
            command = "restore completed";
        SceneManager.Instance.OpenPopup(ProjectConstants.Scenes.MessageInviteFriends,
            new MessageBoxDataModel(command,
                () => {
                    SceneManager.Instance.CloseScene();
                }));
    }

    private void OnProductPurchased (Message mess) {
        //AdHelper.Instance.SetNoAds(IAPManager.m_StoreController.products.all[(int)StorePackage.RemoveAds].hasReceipt);
        SceneManager.Instance.SetLoadingVisible(false);
        InitUI();
    }

    private void OnProductPurchasedFailed (Message mess) {
        SceneManager.Instance.SetLoadingVisible(false);
    }

    public void OnPostponeAdsClicked () {
        UnityAdsHelper.Instance.ShowRewardVideoPostponeAds();
    }

    public void BuyRemoveAdsItem () {
        SceneManager.Instance.SetLoadingVisible(true);
        IAPManager.Instance.BuyRemoveAdsPackage();
    }
    public void RestoreItem () {
#if UNITY_IOS
        SceneManager.Instance.SetLoadingVisible(true);
        IAPManager.Instance.RestorePurchases();
#endif
    }
    public void ClosePopUp () {
        SceneManager.Instance.CloseScene();
    }
}
