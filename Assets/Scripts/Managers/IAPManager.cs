using UnityEngine;
//using UnityEngine.Purchasing;
using System;
using Mio.Utils.MessageBus;
using Mio.Utils;

namespace Mio.TileMaster {

    public enum StorePackage {
        SmallDiamond,
        MediumDiamond,
        BigDiamond,
        RemoveAds
    }

    public class IAPManager : MonoSingleton<IAPManager> /*,IStoreListener*/ {
        //public static IStoreController m_StoreController;
        //// Reference to the Purchasing system.
        //private static IExtensionProvider m_StoreExtensionProvider;

        // Product identifiers for all products capable of being purchased: 
        // "convenience" general identifiers for use with Purchasing, 
        // and their store-specific identifier counterparts 
        // for use with and outside of Unity Purchasing. 
        // Define store-specific identifiers also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

        public const string DIAMOND_PACKAGE_TIER_1 = "tier1DiamondPackage";
        public const string DIAMOND_PACKAGE_TIER_2 = "tier2DiamondPackage";
        public const string DIAMOND_PACKAGE_TIER_3 = "tier3DiamondPackage";
        public const string REMOVE_ADS = "removeAds";

        public bool IsInitialized () {
            // Only say we are initialized if both the Purchasing references are set.
            //return m_StoreController != null && m_StoreExtensionProvider != null;
            return false;
        }

        public void Initialize () {
            // If we have already connected to Purchasing ...
            if (IsInitialized()) {
                // ... we are done here.
                return;
            }

            // Create a builder, first passing in a suite of Unity provided stores.
   //         var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

   //         // Add a product to sell by its identifier, associating the general identifier with its store-specific identifiers.            
   //         builder.AddProduct(DIAMOND_PACKAGE_TIER_1, ProductType.Consumable, new IDs() { { kProductNameAppleDiamondPackageTier1, AppleAppStore.Name }, { kProductNamePlayStoreDiamondPackageTier1, GooglePlay.Name } });
   //         builder.AddProduct(DIAMOND_PACKAGE_TIER_2, ProductType.Consumable, new IDs() { { kProductNameAppleDiamondPackageTier2, AppleAppStore.Name }, { kProductNamePlayStoreDiamondPackageTier2, GooglePlay.Name } });
   //         builder.AddProduct(DIAMOND_PACKAGE_TIER_3, ProductType.Consumable, new IDs() { { kProductNameAppleDiamondPackageTier3, AppleAppStore.Name }, { kProductNamePlayStoreDiamondPackageTier3, GooglePlay.Name } });
   //         builder.AddProduct(REMOVE_ADS, ProductType.NonConsumable, new IDs() { { kProductNameAppleRemoveAds, AppleAppStore.Name }, { kProductNamePlayStoreRemoveAds, GooglePlay.Name } });
   //         //Debug.Log("init remove ads item");

   //         // Kick off the remainder of the set-up with an asynchronous call, 
   //         // passing the configuration and this class' instance. 
   //         // Expect a response either in OnInitialized or OnInitializeFailed.
			//UnityPurchasing.Initialize(this,builder);
            // show or hide ads


        }

        /// <summary>
        /// Restore purchases previously made by this customer. 
        /// Some platforms automatically restore purchases. 
        /// Apple currently requires explicit purchase restoration for IAP.
        /// </summary>
		public void RestorePurchases (bool autoRestore = false) {
            // If Purchasing has not yet been set up ...
    //        if (!IsInitialized()) {
    //            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
    //            Debug.Log("RestorePurchases FAIL. Not initialized.");
				//SceneManager.Instance.SetLoadingVisible(false);
    //            return;
    //        }

    //        // If we are running on an Apple device ... 
    //        if (Application.platform == RuntimePlatform.IPhonePlayer ||
    //            Application.platform == RuntimePlatform.OSXPlayer) {
    //            // ... begin restoring purchases
    //            Debug.Log("RestorePurchases started ...");

    //            // Fetch the Apple store-specific subsystem.
    //            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
    //            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
    //            apple.RestoreTransactions((result) => {
    //                // The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
    //                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
				//	if(!autoRestore){
				//		MessageBus.Annouce(new Message(MessageBusType.OnCompletedRestoreApple, result));

				//	}
				//	if(result)
				//	{
				//		PlayerPrefs.SetInt (VIP_LOCAL,1);
				//	}
    //            });
    //        }
    //        // Otherwise ...
    //        else {
    //            // We are not running on an Apple device. No work is necessary to restore purchases.
    //            Debug.Log("RestorePurchases FAIL. Not supported on this platform.");
				//SceneManager.Instance.SetLoadingVisible(false);
    //        }
        }

        public void BuySmallDiamondPackage () {
            BuyProductID(DIAMOND_PACKAGE_TIER_1);
        }

        public void BuyMediumDiamondPackage () {
            BuyProductID(DIAMOND_PACKAGE_TIER_2);
        }

        public void BuyBigDiamondPackage () {
            BuyProductID(DIAMOND_PACKAGE_TIER_3);
        }
        public void BuyRemoveAdsPackage () {
            BuyProductID(REMOVE_ADS);
        }


        void BuyProductID (string productId) {
            // If the stores throw an unexpected exception, use try..catch to protect logic here.
            //try {
            //    // If Purchasing has been initialized ...
            //    if (IsInitialized()) {
            //        // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
            //        Product product = m_StoreController.products.WithID(productId);

            //        // If the look up found a product for this device's store and that product is ready to be sold ... 
            //        if (product != null && product.availableToPurchase) {
            //            Debug.Log(string.Format("Purchasing product asynchronously: '{0}'", product.definition.id));
            //            // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
            //            m_StoreController.InitiatePurchase(product);
            //        }
            //        // Otherwise ...
            //        else {
            //            // ... report the product look-up failure situation  
            //            Debug.Log(string.Format("BuyProductID: FAIL. Not purchasing {0}, either is not found or is not available for purchase", productId));
            //        }
            //    }
            //    // Otherwise ...
            //    else {
            //        // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initialization.
            //        Debug.Log("BuyProductID FAIL. Not initialized.");
            //    }
            //}
            //// Complete the unexpected exception handling ...
            //catch (Exception e) {
            //    // ... by reporting any unexpected exception for later diagnosis.
            //    Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
            //}
        }

        private void GetIAPInfor () {
            // set price for ItemHitBuilder store location
            //if (m_StoreController != null) {
            //    AdHelper.Instance.SetNoAds(m_StoreController.products.all[(int)StorePackage.RemoveAds].hasReceipt);
            //    //Debug.Log("get list store started");
            //    //string currencyCode = m_StoreController.products.WithID(DIAMOND_PACKAGE_TIER_1).metadata.isoCurrencyCode;
            //    GameManager.Instance.GameConfigs.iap_prices = new System.Collections.Generic.List<string>();

            //    GameManager.Instance.GameConfigs.iap_prices.Insert(0, m_StoreController.products.WithID(DIAMOND_PACKAGE_TIER_1).metadata.localizedPriceString);
            //    GameManager.Instance.GameConfigs.iap_prices.Insert(1, m_StoreController.products.WithID(DIAMOND_PACKAGE_TIER_2).metadata.localizedPriceString);
            //    GameManager.Instance.GameConfigs.iap_prices.Insert(2, m_StoreController.products.WithID(DIAMOND_PACKAGE_TIER_3).metadata.localizedPriceString);
            //    GameManager.Instance.GameConfigs.iap_prices.Insert(3, m_StoreController.products.WithID(REMOVE_ADS).metadata.localizedPriceString);

            //    //Debug.Log("get list store completed");
            //    //foreach (var product in IAPManager.m_StoreController.products.all)
            //    //{
            //    //    //Debug.Log(product.metadata.localizedPriceString);
            //    //    GameManager.Instance.GameConfigs.iap_prices.Add(product.metadata.localizedPriceString);

            //    //}
            //}
        }

        #region Implement IStore Listener
    //    public void OnInitialized (IStoreController controller, IExtensionProvider extensions) {
    //        // Purchasing has succeeded initializing. Collect our Purchasing references.
    //        //Debug.Log("OnInitialized: PASS");

    //        // Overall Purchasing system, configured with products for this application.
    //        m_StoreController = controller;
    //        // Store specific subsystem, for accessing device-specific store features.
    //        m_StoreExtensionProvider = extensions;

    //        GetIAPInfor();
    //    }

    //    public void OnInitializeFailed (InitializationFailureReason error) {
    //        // Purchasing set-up has not succeeded. Check error for reason.
    //        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    //        MessageBus.Annouce(new Message(MessageBusType.IAPManagerInitializeFailed, error.ToString()));
    //    }

    //    public void OnPurchaseFailed (Product p, PurchaseFailureReason pfr) {
    //        // A product purchase attempt did not succeed. Check failureReason for more detail.
    //        string failedMessage = string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", p.definition.storeSpecificId, pfr);
    //        Debug.Log(failedMessage);
    //        MessageBus.Annouce(new Message(MessageBusType.PurchaseFailed, failedMessage));
    //    }

    //    public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs args) {
    //        // A consumable product has been purchased by this user.
    //        string productPurchased = args.purchasedProduct.definition.id;
    //        if (String.Equals(productPurchased, DIAMOND_PACKAGE_TIER_1, StringComparison.Ordinal)) {
    //            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", productPurchased));
    //            AnalyticsHelper.Instance.LogBuyMarketItem(DIAMOND_PACKAGE_TIER_1, "Diamond Package", "Small Diamond Package");
    //            MessageBus.Annouce(new Message(MessageBusType.ProductPurchased, StorePackage.SmallDiamond));
    //        }
    //        else if (String.Equals(productPurchased, DIAMOND_PACKAGE_TIER_2, StringComparison.Ordinal)) {
    //            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", productPurchased));
    //            AnalyticsHelper.Instance.LogBuyMarketItem(DIAMOND_PACKAGE_TIER_2, "Diamond Package", "Diamond Package");
    //            MessageBus.Annouce(new Message(MessageBusType.ProductPurchased, StorePackage.MediumDiamond));
    //        }
    //        else if (String.Equals(productPurchased, DIAMOND_PACKAGE_TIER_3, StringComparison.Ordinal)) {
    //            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", productPurchased));
    //            AnalyticsHelper.Instance.LogBuyMarketItem(DIAMOND_PACKAGE_TIER_3, "Diamond Package", "Big Diamond Package");
    //            MessageBus.Annouce(new Message(MessageBusType.ProductPurchased, StorePackage.BigDiamond));
    //        }
    //        else if (String.Equals(productPurchased, REMOVE_ADS, StringComparison.Ordinal)) {
    //            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", productPurchased));
    //            AnalyticsHelper.Instance.LogBuyMarketItem(REMOVE_ADS, "removeads Package", "removeAds Package");
    //            MessageBus.Annouce(new Message(MessageBusType.ProductPurchased, StorePackage.RemoveAds));
				//PlayerPrefs.SetInt (VIP_LOCAL,1);
    //        }
    //        else {
    //            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", productPurchased));
    //            MessageBus.Annouce(new Message(MessageBusType.PurchaseFailed, productPurchased));
    //        }
    //        // Return a flag indicating wither this product has completely been received, 
    //        //or if the application needs to be reminded of this purchase at next app launch. 
    //        //Is useful when saving purchased products to the cloud, and when that save is delayed.
    //        return PurchaseProcessingResult.Complete;
    //    }
        #endregion
    }
}