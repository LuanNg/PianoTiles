using UnityEngine;
using System.Collections;
using Mio.Utils.MessageBus;
using Mio.Utils;

namespace Mio.TileMaster {
    public class DiamondIAPItem : MonoBehaviour {
        [Header("Choose Diamond Package")]
        [SerializeField]
        private StorePackage diamondPackage;

        [Header("Current Diamond")]
        [SerializeField]
        private InAppPurchasePopUp IPAControl;

        [Header("UI")]
        [SerializeField]
        private UILabel lblQuantityDiamond;
        [SerializeField]
        private UILabel lblAmount;

        //[Header("Values")]
        //[SerializeField]
        //private int quantityDiamond;
        //[SerializeField]
        //private float amount;

        /// <summary>
        /// "BuyDiamond" btn TAP
        /// </summary>
        public void BuyDiamond () {
            IPAControl.BuyDiamondPackage(diamondPackage);
            AnalyticsHelper.Instance.LogClickItem("Try to buy diamond package " + diamondPackage);
        }

        //void Start () {
        //    MessageBus.Instance.Subscribe(MessageBusType.CompletedIAPLocalization, InitUI);
        //}

        void OnEnable () {
            InitUI();
        }

        private void InitUI () {
            if (GameManager.Instance.GameConfigs.iap_values != null && GameManager.Instance.GameConfigs.iap_values.Count > (int)diamondPackage) {
                lblQuantityDiamond.text = "x" + GameManager.Instance.GameConfigs.iap_values[(int)diamondPackage];
            }
            else {
                gameObject.SetActive(false);
                return;
            }

            if (GameManager.Instance.GameConfigs.iap_prices != null && GameManager.Instance.GameConfigs.iap_prices.Count > (int)diamondPackage) {
                lblAmount.text = GameManager.Instance.GameConfigs.iap_prices[(int)diamondPackage];
            }
        }
    }
}
