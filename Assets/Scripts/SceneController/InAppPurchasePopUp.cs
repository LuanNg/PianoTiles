using UnityEngine;
using System.Collections;
using Mio.Utils;
using Mio.Utils.MessageBus;
using System;
//using UnityEngine.Purchasing;

namespace Mio.TileMaster {
    public class InAppPurchasePopUp : SSController {
        [Header("Current Diamond")]
        [SerializeField]
        private UILabel lbCurrentDiamond;
        [Header("Reward Diamond")]
        [SerializeField]
        private UILabel lblDiamondReward;
        [SerializeField]
        private Animator diamondpackageEffect;

        [Header("NO Ads")]
        [SerializeField]
        private UILabel priceNoAds;
        [SerializeField]
        private GameObject isPuschased;
        [SerializeField]
        private GameObject btnBuyNoAds;
        public override void OnEnableFS () {
            // a fail safe to prevent user from using too old game configs
            if (GameManager.Instance.GameConfigs.iap_values == null || GameManager.Instance.GameConfigs.iap_values.Count <= 0) {
                GameManager.Instance.GameConfigs.iap_values = new System.Collections.Generic.List<int>();
                GameManager.Instance.GameConfigs.iap_values.AddRange(new int[] { 120, 250, 800 });
                //GameManager.Instance.GameConfigs.iap_prices = new System.Collections.Generic.List<string>();

                //GameManager.Instance.GameConfigs.iap_prices.AddRange(new string[] { "$0.49","$0.99","$1.99" });

            }

            if (GameManager.Instance.GameConfigs.videoAdsReward != 0) {
                lblDiamondReward.text = GameManager.Instance.GameConfigs.videoAdsReward.ToString();
            }

            if (GameManager.Instance.GameConfigs.iap_prices != null)
                priceNoAds.text = GameManager.Instance.GameConfigs.iap_prices[(int)StorePackage.RemoveAds];
        }
        public override void OnEnable () {
            //check Remove ads
            isPuschased.SetActive(AdHelper.Instance.IsAdsDisabled());
            btnBuyNoAds.SetActive(!AdHelper.Instance.IsAdsDisabled());

            lbCurrentDiamond.text = ProfileHelper.Instance.CurrentDiamond.ToString();
            MessageBus.Instance.Subscribe(MessageBusType.DiamondChanged, OnDiamondChanged);
            MessageBus.Instance.Subscribe(MessageBusType.ProductPurchased, OnProductPurchased);
            MessageBus.Instance.Subscribe(MessageBusType.PurchaseFailed, OnProductPurchasedFailed);
            UnityAdsHelper.Instance.OnViewedAds += OnViewedAds;
            //SceneManager.Instance.SetLoadingVisible(true);
        }

        private void OnProductPurchasedFailed (Message msg) {
            SceneManager.Instance.SetLoadingVisible(false);
        }

        private void OnViewedAds () {
            diamondpackageEffect.SetTrigger("d0");
        }

        private void OnProductPurchased (Message msg) {
            SceneManager.Instance.SetLoadingVisible(false);
            StorePackage purchased = (StorePackage)msg.data;
            if (purchased >= 0 && (int)purchased < GameManager.Instance.GameConfigs.iap_values.Count) {
                ProfileHelper.Instance.CurrentDiamond += GameManager.Instance.GameConfigs.iap_values[(int)purchased];
                if (purchased == StorePackage.SmallDiamond)
                    diamondpackageEffect.SetTrigger("d1");
                else if (purchased == StorePackage.MediumDiamond)
                    diamondpackageEffect.SetTrigger("d2");
                else if (purchased == StorePackage.BigDiamond)
                    diamondpackageEffect.SetTrigger("d3");
                else if (purchased == StorePackage.RemoveAds) {
					//AdHelper.Instance.SetNoAds(IAPManager.m_StoreController.products.all[(int)StorePackage.RemoveAds].hasReceipt);
                    isPuschased.SetActive(true);
                    btnBuyNoAds.SetActive(false);
                }


                //AnalyticsHelper.Instance.LogBuyMarketItem()
            }
            //switch (purchased) {
            //    case DiamondPackage.Small:
            //        ProfileHelper.Instance.CurrentDiamond += GameManager.Instance.GameConfigs.iap_values[0];
            //        break;
            //    case DiamondPackage.Medium:
            //        break;
            //    case DiamondPackage.Big:
            //        break;
            //}
        }

        public override void OnDisable () {
            MessageBus.Instance.Unsubscribe(MessageBusType.DiamondChanged, OnDiamondChanged);
            MessageBus.Instance.Unsubscribe(MessageBusType.ProductPurchased, OnProductPurchased);
            MessageBus.Instance.Unsubscribe(MessageBusType.PurchaseFailed, OnProductPurchasedFailed);
            UnityAdsHelper.Instance.OnViewedAds -= OnViewedAds;
        }
        /// <summary>
        /// close popup when tap btn close
        /// </summary>
        public void CloseThisPopUp () {
            SceneManager.Instance.CloseScene();
        }
        /// <summary>
        /// "Get it" btn TAP
        /// </summary>
        public void ShowVideoAds () {
            //Debug.Log("show Ads for reward diamond");
            //UnityAdsHelper.Instance.ShowRewardedAd();
            AdHelper.Instance.ShowVideoRewardAds();
            //RewardVideoAds.Instance.ShowRewardedAd();
        }
        /// <summary>
        /// "BuyDiamond" btn TAP
        /// </summary>
        /// <param name="_diamondsize"></param>
        public void BuyDiamondPackage (StorePackage _diamondsize) {
            switch (_diamondsize) {
                case StorePackage.SmallDiamond:
                    Debug.Log("Buy Small Diamond package");
                    IAPManager.Instance.BuySmallDiamondPackage();
                    break;
                case StorePackage.MediumDiamond:
                    Debug.Log("Buy Medium Diamond package");
                    IAPManager.Instance.BuyMediumDiamondPackage();
                    break;
                case StorePackage.BigDiamond:
                    Debug.Log("Buy BIG Diamond package");
                    IAPManager.Instance.BuyBigDiamondPackage();
                    break;
                default:
                    Debug.Log("This should never happen. If it is, you are fucked. LOL");
                    break;
            }

            SceneManager.Instance.SetLoadingVisible(true);
        }

        public void NoAdsItemClick () {
            SceneManager.Instance.OpenPopup(ProjectConstants.Scenes.RemoveAdsPopUp);
        }

        private void OnDiamondChanged (Message obj) {
            lbCurrentDiamond.text = ProfileHelper.Instance.CurrentDiamond.ToString();
            //diamondBarEffect.Play();
        }
    }
}
