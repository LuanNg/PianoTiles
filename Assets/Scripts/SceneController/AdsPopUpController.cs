using UnityEngine;
using System.Collections;
using Mio.Utils;

namespace Mio.TileMaster {
    public class AdsPopUpController : SSController {
        public UILabel lbMessage;
        public UITexture image;
        private string imageUrl;
        private string linkUrl;
        private AdsPopUpModel adsModel;

        public override void OnSet(object data) {
            if(data != null) {
                adsModel = (AdsPopUpModel)data;
                lbMessage.text = adsModel.message;
                imageUrl = adsModel.imageUrl;
                linkUrl = adsModel.linkUrl;
                GetAdsImage();
            }
            AnalyticsHelper.Instance.LogOpenScene("Fail-safe ads");
        }

        private void GetAdsImage() {
            StartCoroutine(RunDownload(imageUrl));
        }

        private IEnumerator RunDownload(string url) {
            using(WWW fileLoader = new WWW(url)) {
                yield return fileLoader;
                if(fileLoader.isDone && fileLoader != null) {
                    Texture2D textureAvatar = fileLoader.texture;
                    image.mainTexture = textureAvatar;
                }
            }
        }

        public void OnAdsClicked() {
            AnalyticsHelper.Instance.LogClickItem("click-fail-safe-ads");
            Application.OpenURL(linkUrl);
        }

        public void OnCloseClicked() {
            AnalyticsHelper.Instance.LogClickItem("close-fail-safe-ads");
            SSSceneManager.Instance.Close();
        }
    }
}