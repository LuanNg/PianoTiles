using Mio.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mio.Monentize
{
    public class AdsHelper : MonoSingleton<AdsHelper>
    {
        [Header("UnityAds sections")]
        public bool useUnityAds;
        public UnityAdsHelper unityads;

        [Header("Admob sections")]
        public bool useAdmob;
        public AdmobAdsHelper admob;
    }
}