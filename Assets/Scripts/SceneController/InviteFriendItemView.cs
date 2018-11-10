using UnityEngine;
using System.Collections;
using Mio.Utils.MessageBus;
//using Facebook.Unity;
using MovementEffects;
using System.Collections.Generic;
namespace Mio.TileMaster
{
    public class InviteFriendItemView : MonoBehaviour
    {
        [SerializeField]
        private UIToggle toggle;
        [SerializeField]
        private UILabel friendName;
        [SerializeField]
        private UI2DSprite spriteAvatar;
        [SerializeField]
        private Sprite defaultAvatar;
        private bool onSelected;

        //public string idfriend;

        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                //lbindex.text = index.ToString();
            }
        }

        private InvitableFriend model;
        public InvitableFriend Model { get { return model; } set { model = value; RefreshItemView(); } }

        private void RefreshItemView()
        {
            //Debug.Log(model.id);
            friendName.text = model.name;
            //Debug.Log(model.isSelected);
            toggle.value = model.isSelected;
            //lbindex.text = index.ToString();
            this.Index = model.index;
            ResetAvatar(model);

        }

        private void ResetAvatar(InvitableFriend model)
        {
            if (model.avatar != null)
                spriteAvatar.sprite2D = model.avatar;
            else 
            {
                spriteAvatar.sprite2D = defaultAvatar;
                Timing.RunCoroutine(IEDownloadAvatar(model));
            }
        }

        void Start() {
          //  toggle.activeSprite.cachedGameObject.SetActive(false);
            onSelected = toggle.startsActive;
        }
        public void TouchInCheckboxAction()
        {
            onSelected = toggle.value;
            if(model != null)
                model.isSelected = onSelected;
            MessageBus.Annouce(new Message(MessageBusType.OnSelectedOrUnselectedInviteFriend, model));
          //  toggle.activeSprite.cachedGameObject.SetActive(onSelected);
        }

        private IEnumerator<float> IEDownloadAvatar(InvitableFriend _modelFriend)
        {
            yield return Timing.WaitForSeconds(1f);
            if (_modelFriend.index == this.Index) 
            {
                DownloadFacebookAvatar(_modelFriend);
            }
        }

        private void DownloadFacebookAvatar(InvitableFriend modelForGetAvatar)
        {
            if (string.IsNullOrEmpty(modelForGetAvatar.picture.data.url))
                return;
            string linkAvatar = modelForGetAvatar.picture.data.url;
            AssetDownloader.Instance.DownloadAndCacheAsset(linkAvatar, 0, null, null,
                (WWW www) =>
                {
                    Rect rec = new Rect(0, 0, www.texture.width, www.texture.height);
                    Sprite.Create(www.texture, rec, new Vector2(0, 0), 1);
                    Sprite sprite = Sprite.Create(www.texture, rec, new Vector2(0, 0), .01f);
                    //if (pModel != null)
                    //    pModel.avatar = spriteAvatar;
                    //avatar.sprite2D = spriteAvatar;
                    modelForGetAvatar.avatar = sprite;
                    if (modelForGetAvatar.index == this.Index) 
                    {
                        spriteAvatar.sprite2D = sprite;
                    }
                });


        }

    }
}
