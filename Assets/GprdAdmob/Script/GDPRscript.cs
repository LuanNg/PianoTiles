using DFTGames.Localization;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GDPRscript : MonoBehaviour
{
    string code;
    
    public void Start()
    {
        PlayerPrefs.DeleteAll();
        StartCoroutine(DetectCountry());
        if (PlayerPrefs.GetInt("country") == 0)
        {
            if (Application.systemLanguage == SystemLanguage.French)
            {
                Debug.Log("This system is in French. ");
            }
            else if (Application.systemLanguage == SystemLanguage.English)
            {
                Localize.SetCurrentLanguage(SystemLanguage.English);
                LocalizeImage.SetCurrentLanguage();
                Debug.Log("This system is in English. ");
            }
            else if (Application.systemLanguage == SystemLanguage.Spanish)
            {
                Localize.SetCurrentLanguage(SystemLanguage.Spanish);
                LocalizeImage.SetCurrentLanguage();
                Debug.Log("This system is in Spanish. ");
            }
            else if (Application.systemLanguage == SystemLanguage.Italian)
            {
                Localize.SetCurrentLanguage(SystemLanguage.Italian);
                LocalizeImage.SetCurrentLanguage();
                Debug.Log("This system is in Italian. ");
            }
            else if (Application.systemLanguage == SystemLanguage.Romanian)
            {
                Localize.SetCurrentLanguage(SystemLanguage.Romanian);
                LocalizeImage.SetCurrentLanguage();
                Debug.Log("This system is in Romanian. ");
            }
            else if (Application.systemLanguage == SystemLanguage.French)
            {
                Localize.SetCurrentLanguage(SystemLanguage.French);
                LocalizeImage.SetCurrentLanguage();
                Debug.Log("This system is in French. ");
            }
            else if (Application.systemLanguage == SystemLanguage.Portuguese)
            {
                Localize.SetCurrentLanguage(SystemLanguage.Portuguese);
                LocalizeImage.SetCurrentLanguage();
                Debug.Log("This system is in Portuguese. ");
            }
            else if (Application.systemLanguage == SystemLanguage.Dutch)
            {
                Localize.SetCurrentLanguage(SystemLanguage.Dutch);
                LocalizeImage.SetCurrentLanguage();
                Debug.Log("This system is in Dutch. ");
            }
            else if (Application.systemLanguage == SystemLanguage.Arabic)
            {
                Localize.SetCurrentLanguage(SystemLanguage.Arabic);
                LocalizeImage.SetCurrentLanguage();
                Debug.Log("This system is in arabic. ");
            }
        }
    }
    public GameObject PopUp;
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public string UrlTerms;
    public void OpenURLTerms()
    {
        Application.OpenURL(UrlTerms);
    }
    public void Personalized()
    {
        PopUp.SetActive(false);
        PlayerPrefs.SetInt("country", 1);
        PlayerPrefs.SetInt("NoPersonalized", 0);
        PlayerPrefs.SetInt("FirstTime", 1);
    }
   
    public void NoPersonalized()
    {
        PopUp.SetActive(false);
        PlayerPrefs.SetInt("country", 1);
        PlayerPrefs.SetInt("NoPersonalized", 1);
        PlayerPrefs.SetInt("FirstTime", 1);
    }

    IEnumerator DetectCountry()
    {
        if (PlayerPrefs.GetInt("country") == 0) 
        {
            UnityWebRequest www = UnityWebRequest.Get("https://extreme-ip-lookup.com/json");
            www.chunkedTransfer = false;
            yield return www.Send();
            Debug.Log("pinging");
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
                Debug.Log("www.error");
            }
            else
            {
                
                if (www.isDone)
                    
                {
                    ipapi code2 = JsonUtility.FromJson<ipapi>(www.downloadHandler.text);

                    code = code2.continent;
                    Debug.Log(code2.country);

                    if (code == "Europe")
                    
                        { 
                        Debug.Log("Your country in Europe  ");
                        PlayerPrefs.SetInt("Localized", 1);
                        PopUp.SetActive(true);
                        //PlayerPrefs.SetInt("country", 1);
                    }
                    else 
                    {
                        PlayerPrefs.SetInt("NotEU", 1);
                        Debug.Log("Your country Not in Europe");

                    }
               }
            }
        }
    }
    
    public class codeCountry
    {
        public string ip;
        public string city;
        public string region;
        public string region_code;
        public string country;
        public string country_name;
        public string continent_code;
        public string in_eu;
        public string postal;
        public string latitude;
        public string longitude;
        public string timezone;
        public string utc_offset;
        public string country_calling_code;
        public string currency;
        public string languages;
        public string asn;
        public string org;
    }
}
