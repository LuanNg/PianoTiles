using UnityEngine;
using System.Collections;
using Mio.Utils;
using System;
using System.Collections.Generic;
using Mio.Utils.MessageBus;
using MovementEffects;

namespace Mio.TileMaster
{
#pragma warning disable 162
    public class InviteFriendController : SSController
    {

        private string afterPagingToken;
        //[SerializeField]
        //private int friendLimitPerQuery = 500;
        //private int limitQueryTime = 50; //maximum query time
        //private float queryTimeout = 17f;
        public List<InvitableFriend> listInvitableFriend;


        Dictionary<string, InvitableFriend> dicInvitableFriend = new Dictionary<string, InvitableFriend>();
        private Dictionary<string, InvitableFriend> dicInvitedFriend = new Dictionary<string, InvitableFriend>();
        //private Dictionary<GameObject, InviteFriendItemView> currentItemDic;
        private List<InvitableFriend> listFriendsID = new List<InvitableFriend>();
        // Use this for initialization

        [Header("UI")]
        [SerializeField]
        private UIInput inputSearch;
        [SerializeField]
        private UIWrapContent itemScrollList;
        //[SerializeField]
        ////[SerializeField]
        ////private UIGrid itemGrid;
        [SerializeField]
        public int minIndexItem = -49;
        [SerializeField]
        public int maxIndexItem = 0;
        [SerializeField]
        public int maxItemInScroll = 8;
        [SerializeField]
        public int maxItemSize;

        public SpringPanel spring;


        //refresh content when search change;
        private void RefreshContent()
        {
            
            //itemScrollList.onInitializeItem -= OnSongItemNeedInitialized;
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < itemScrollList.transform.childCount; i++)
            {
                pos.y = -maxItemSize * i;
                itemScrollList.transform.GetChild(i).localPosition = pos;
            }
            spring.target = Vector3.zero;
            spring.enabled = true;
        }

        //private IEnumerator<float> IEWaitForRefreshContent(int numberContent)
        //{
        //    RefreshContent();
        //    yield return Timing.WaitForSeconds(0.5f);
        //    OnProgressContentWithListFriendsSize(numberContent);
        //}

        public void SelectAllContent()
        {
            for (int i = 0; i < listFriendsID.Count; i++)
            {
                listFriendsID[i].isSelected = true;

            }
            OnProgressContentWithListFriendsSize(listFriendsID.Count);
        }

        public void UnselectAllContent()
        {
            for (int i = 0; i < listFriendsID.Count; i++)
            {
                listFriendsID[i].isSelected = false;
            }
            OnProgressContentWithListFriendsSize(listFriendsID.Count);
        }

        public override void OnEnableFS()
        {
            //itemScrollList.onInitializeItem += OnSongItemNeedInitialized;
            itemScrollList.minIndex = minIndexItem;
            itemScrollList.maxIndex = maxIndexItem;
            maxItemSize = itemScrollList.itemSize;
            //currentItemDic = new Dictionary<GameObject, InviteFriendItemView>(8);
            listFriendsID = new List<InvitableFriend>();
        }

        private void OnSongItemNeedInitialized(GameObject go, int wrapIndex, int realIndex)
        {
            //Debug.Log("  rewrew"+wrapIndex);
            //Debug.Log(realIndex);
            if (realIndex < 0)
                realIndex = -realIndex;
            UpdateItemWhenSearch(itemScrollList.transform.GetChild(wrapIndex).gameObject, realIndex);
        }

        public override void OnEnable()
        {
            //MessageBus.Instance.Subscribe(MessageBusType.OnSelectedOrUnselectedInviteFriend, OnSelectedOrUnselectedInvateFriend);
            //if (FacebookManager.Instance.IsLogin())
            //{
            //    Debug.Log("GEt list friends started");
            //    GetInvitedFriendList();
            //    GetInvitableFriendList(OnSuccedGetListFriend);
            //}
            //AnalyticsHelper.Instance.LogOpenScene(ProjectConstants.SceneNames.GetSceneName(ProjectConstants.Scenes.InviteFriends));
            //ShowOrHideListFriendItem(false);
        }

        private void OnProgressContentWithListFriendsSize(int itemNum)
        {
            //Debug.Log(itemNum);
            //itemScrollList.WrapContent();
            //itemGrid.Reposition();
            itemScrollList.minIndex = -(listFriendsID.Count - 1);
            RefreshContent();
            for (int i = 0; i < itemScrollList.transform.childCount; i++)
            {
                if (i < itemNum)
                {
                    //Debug.Log("ABC");
                    UpdateItemWhenSearch(itemScrollList.transform.GetChild(i).gameObject, i);
                    itemScrollList.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    itemScrollList.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            //itemScrollList.onInitializeItem += OnSongItemNeedInitialized;
        }

        private void UpdateItemWhenSearch(GameObject go, int indexItem)
        {
            if (indexItem < listFriendsID.Count && indexItem >= 0)
            {
                InviteFriendItemView inFriend = go.GetComponent<InviteFriendItemView>();
                inFriend.Model = listFriendsID[indexItem];
//                inFriend.Index = indexItem + 1;
            }
        }

        public void CloseScene()
        {
            SceneManager.Instance.CloseScene();
        }

        public override void OnDisable()
        {
            MessageBus.Instance.Unsubscribe(MessageBusType.OnSelectedOrUnselectedInviteFriend, OnSelectedOrUnselectedInvateFriend);
        }

        private void InitUI(int itemNumber)
        {
            //itemScrollList.onInitializeItem -= OnSongItemNeedInitialized;
            itemScrollList.gameObject.SetActive(true);
            //itemScrollList.WrapContent();
            OnProgressContentWithListFriendsSize(itemNumber);
        }

        private void OnSelectedOrUnselectedInvateFriend(Message msg)
        {
            InvitableFriend friendModel = (InvitableFriend)(msg.data);
            if (friendModel != null)
            {
                for (int i = 0; i < listFriendsID.Count; i++)
                {
                    if (listFriendsID[i].id == friendModel.id)
                    {
                        listFriendsID[i].isSelected = friendModel.isSelected;
                    }
                }
            }
        }

        private List<InvitableFriend> tempListFriendsInSearch;
        public void OnSearchChange()
        {
            string searchText = inputSearch.value.ToLower();
            tempListFriendsInSearch = new List<InvitableFriend>();
            if (listFriendsID == null)
                listFriendsID = new List<InvitableFriend>();
            else
                listFriendsID.Clear();

            foreach (KeyValuePair<string, InvitableFriend> pair in dicInvitableFriend)
            {
                if (pair.Value.name.ToLower().Contains(searchText))
                {
                    tempListFriendsInSearch.Add(pair.Value);
                }
            }
            listFriendsID.AddRange(tempListFriendsInSearch);
            tempListFriendsInSearch.Clear();
            if (listFriendsID.Count == 0)
                itemScrollList.gameObject.SetActive(false);
            else
            {
                itemScrollList.gameObject.SetActive(false);

                InitUI(listFriendsID.Count);
            }
        }

        //get list friends from local
        private void GetInvitedFriendList()
        {
            if (FileUtilities.IsFileExist(GameConsts.USER_INVITED_FRIENDS))
            {
                //  Debug.Log(dicInvitedFriend.Count);
                dicInvitedFriend = FileUtilities.DeserializeObjectFromFile<Dictionary<string, InvitableFriend>>(FileUtilities.GetWritablePath(GameConsts.USER_INVITED_FRIENDS), "sowhatthehellareyouwaitingfor", true);
                //if (dicInvitableFriend == null)
                //{
                //    dicInvitableFriend = new Dictionary<string, InvitableFriend>();
                //}
            }
            //Debug.Log(dicInvitedFriend.Count);
        }

        //save list friends to local
        private void SaveInvatedFriendsList(Dictionary<string, InvitableFriend> dic_friends_save)
        {
            FileUtilities.SerializeObjectToFile<Dictionary<string, InvitableFriend>>(dic_friends_save, FileUtilities.GetWritablePath(GameConsts.USER_INVITED_FRIENDS), "sowhatthehellareyouwaitingfor", true);
        }

        //using Facebook API get list friends
        void GetInvitableFriendList(Action<bool> result = null)
        {
            //if (listFriendsID.Count > 0)
            //{
            //    result(false);
            //    OnProgressContentWithListFriendsSize(listFriendsID.Count);
            //    return;
            //}
            //itemScrollList.gameObject.SetActive(false);
            //FacebookManager.Instance.GetInvitatbleFriend(afterPagingToken, friendLimitPerQuery, res =>
            //{
            //    if (res != null)
            //    {
            //        string json = res.RawResult;
            //        fsData jsonData = fsJsonParser.Parse(json);
            //        AllInvitableFriend query = new AllInvitableFriend();
            //        fsResult r = FileUtilities.JSONSerializer.TryDeserialize(jsonData, ref query);
            //        //= Pathfinding.Serialization.JsonFx.JsonReader.Deserialize(json, typeof(AllInvitableFriend)) as AllInvitableFriend;
            //        if (r.Failed)
            //        {
            //            Debug.LogError("Failed to Parse Invitable friend list");
            //        }

            //        for (int i = 0; i < query.data.Count; i++)
            //        {
            //            //listFriend.Add(query.data[i].id);
            //            InvitableFriend fr = new InvitableFriend();
            //            fr.id = query.data[i].id;
            //            fr.name = query.data[i].name;
            //            fr.picture.data.url = query.data[i].picture.data.url;
            //            fr.index = i + 1;
            //            //Debug.Log("Key: " + fr.name + ":" + fr.id + "linkAvata: " + fr.picture.data.url);

            //            if (dicInvitedFriend.ContainsKey(fr.name) == false)
            //            {
            //                dicInvitableFriend[fr.name] = fr;//new InvitableFriend(query.data[i].id, query.data[i].name);
            //                //dicInvitableFriendLocal[fr.name] = fr;
            //            }
            //        }

            //        if (query.paging != null)
            //        {
            //            afterPagingToken = query.paging.cursors.after;

            //        }
            //        else
            //        {
            //            Debug.LogError("Null");
            //            result(false);
            //        }
            //        result(true);
            //    }
            //    else
            //    {
            //        Debug.LogError("res null");
            //    }
            //});
        }

        /// <summary>
        /// sussedGetListFriend
        /// </summary>
        /// <param name="result"></param>
        private void OnSuccedGetListFriend(bool result)
        {
            if (result)
            {
                if (dicInvitableFriend.Count <= 0)
                {
                    Debug.Log("Get list friends again");
                    GetInvitableFriendList(OnSuccedGetListFriend);
                    return;
                }
                listFriendsID = new List<InvitableFriend>();
                listFriendsID.AddRange(dicInvitableFriend.Values);
                //InitUI(listFriendsID.Count);
                itemScrollList.gameObject.SetActive(true);
                itemScrollList.minIndex = -(listFriendsID.Count - 1);
                OnProgressContentWithListFriendsSize(listFriendsID.Count);
                itemScrollList.onInitializeItem -= OnSongItemNeedInitialized;
                itemScrollList.onInitializeItem += OnSongItemNeedInitialized;
            }
        }


        //send btn tap
        public void OnInviteClicked()
        {
//            // get id of friends selected and save to list
//            List<string> listFriendSelected = new List<string>();
//            foreach (InvitableFriend f in listFriendsID)
//            {
//                if (f.isSelected)
//                {
//                    listFriendSelected.Add(f.id);
//                }
//            }
//            if (listFriendSelected.Count <= 0)
//            {
//                //Debug.Log("no a friend select");
//                SceneManager.Instance.OpenPopup(ProjectConstants.Scenes.MessageInviteFriends, "No a friend select");
//                return;
//            }

//            // call Facebook function to invite friends using list above
//            if (listFriendSelected.Count > 0)
//            {
//#if UNITY_EDITOR
//                OnInviteSuccess();
//                return;
//#endif

//                Debug.Log("Invite number of friends: " + listFriendSelected.Count);
//                FacebookManager.Instance.SendInviteBeta((callback =>
//                {
//                    if (callback != null)
//                    {
//                        if (callback.To != null)
//                        {
//                            foreach (string s in callback.To)
//                            {
//                                Debug.LogWarning("Invite successful: " + s);
//                            }
//                            OnInviteSuccess();
//                        }
//                        else
//                        {
//                            Debug.LogError("callback.To is null");
//                            //OnInviteFail("You must select at least one friend to invite");
//                        }
//                    }
//                    else
//                    {
//                        Debug.LogError("callback is null");
//                        OnInviteFail();
//                    }
//                }), listFriendSelected);
//            }
//            else
//            {
//                OnInviteFail("You must select at least one friend to invite");
//            }
        }



        private void OnInviteSuccess()
        {
            AnalyticsHelper.Instance.LogClickItem("Invited friend");
            //Debug.Log("xong");
            //SceneManager.Instance.OpenPopup(ProjectConstants.Scenes.MessageInviteFriends, "Invite successful");
            dicInvitedFriend = new Dictionary<string, InvitableFriend>();
            for (int i = 0; i < listFriendsID.Count; i++)
            {
                if (listFriendsID[i].isSelected && dicInvitableFriend.ContainsKey(listFriendsID[i].name))
                {
                    dicInvitedFriend.Add(listFriendsID[i].name, listFriendsID[i]);
                    dicInvitableFriend.Remove(listFriendsID[i].name);
                }
            }
            listFriendsID = new List<InvitableFriend>(dicInvitableFriend.Values);
            SaveInvatedFriendsList(dicInvitedFriend);
            GetInvitableFriendList(OnSuccedGetListFriend);

        }
        void OnInviteFail(string errorMessage = "An error occurred, please try again later!")
        {
            Debug.Log(errorMessage);
            //SceneManager.Instance.OpenPopup(ProjectConstants.Scenes.MessageInviteFriends, errorMessage);
        }
    }



    public class AllInvitableFriend
    {
        public List<InvitableFriend> data = new List<InvitableFriend>();
        public FacebookPaging paging;

    }
    public class PictureField
    {
        public class DataPicture
        {
            public bool is_silhouette;
            public string url;
            public DataPicture()
            {
                is_silhouette = false;
                url = "";
            }
        }
        public DataPicture data;
        public PictureField() 
        {
            data = new DataPicture();
        }

    }

    public class InvitableFriend
    {
        public int index;
        public string id;
        public string name;

        //[FileHelpers.FieldIgnored]
        public bool isSelected;

        //[FileHelpers.FieldIgnored]
        public bool isMatchedSearch;

        public PictureField picture = new PictureField();

        public Sprite avatar;
    }
    public class FacebookPaging
    {
        public class FacebookCursors
        {
            public string before; //paging before token
            public string after; // paging after token

            public FacebookCursors()
            {
                before = "";
                after = "";
            }
        }

        public FacebookCursors cursors;
        public string next; // string url query next page
        public string previous; // string url query previous page

        public FacebookPaging()
        {
            cursors = new FacebookCursors();
            next = "";
            previous = "";
        }

    }
}
