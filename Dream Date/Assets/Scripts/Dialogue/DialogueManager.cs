using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyExcelGenerated;

namespace DiceGirl.Dialogues
{
    public class DialogueManager : MonoBehaviour
    {
        public Image bgImg;
        public Text nameTxt, contentTxt;

        public Transform portraitTsf;

        public float typingInterval = .05f;

        [SerializeField]
        List<Dialogue> dialogueList = new List<Dialogue>();

        Animator anim;

        int curIndex;

        bool isVisible;

        DialoguePortrait lastPortrait;

        System.Action endAction;

        Dialogue curDialogue
        {
            get
            {
                try
                {
                    return dialogueList[curIndex];
                }
                catch
                {
                    return null;
                }
            }
        }

        string curName { get { return string.IsNullOrEmpty(curDialogue.nameID) ? "" : ConfigManager.Instance.GetLocalizationValueByKey(curDialogue.nameID); } }
        string curContent { get { return string.IsNullOrEmpty(curDialogue.contentID) ? "" : ConfigManager.Instance.GetLocalizationValueByKey(curDialogue.contentID); } }

        bool isTyping;
        Coroutine typeCoro;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                OnSwitchUIVisible();
        }
        public void StartDialogue(string id, System.Action endAction = null)
        {
            var dialogueList = ConfigManager.Instance.GetDialogueListByID(id);

            if (dialogueList == null || dialogueList.Count == 0) return;


            this.endAction = endAction;

            OnOpenDialogueUI();

            this.dialogueList = dialogueList;
            isVisible = true;
            curIndex = 0;
            UpdateCurDialogue();


        }
        public void EndDialogue()
        {
            endAction?.Invoke();

            foreach (Transform tsf in portraitTsf)
                Destroy(tsf.gameObject);

            OnCloseDialogueUI();
        }

        void UpdateCurDialogue()
        {
            if (curDialogue == null)
            {
                EndDialogue();
                return;
                
            }

            UpdateName();
            UpdatePortrait();
            UpdateBg();

            if (typeCoro != null) StopCoroutine(typeCoro);
            typeCoro = StartCoroutine(IType());

        }

        void UpdateName()
        {
            nameTxt.text = curName;
            nameTxt.gameObject.SetActive(!string.IsNullOrEmpty(nameTxt.text));
        }

        void UpdatePortrait()
        {
            var portrait = ConfigManager.Instance.GetDialoguePortrait(curDialogue.portraitName); 

            if (lastPortrait != portrait)
            {
                lastPortrait = portrait;

                foreach (Transform tsf in portraitTsf)
                    Destroy(tsf.gameObject);

                if (!string.IsNullOrEmpty(curDialogue.portraitName))
                    Instantiate(portrait, portraitTsf);
            }

            //动画状态
            if (!string.IsNullOrEmpty(curDialogue.portraitAnimationID))
            {
                portrait.SetAnimationID(curDialogue.portraitAnimationID);
            }


        }

        void UpdateBg()
        {
            var bg = ConfigManager.Instance.GetBackground(curDialogue.bgName);

            bgImg.gameObject.SetActive(bg != null);
            bgImg.sprite = bg;
        }

        void ContinueDialogue()
        {
            if (isTyping)
            {
                if (typeCoro != null) StopCoroutine(typeCoro);

                isTyping = false;
                nameTxt.text = curName;
                contentTxt.text = curContent;
                return;
            }

            curIndex++;
            UpdateCurDialogue();
        }

        IEnumerator IType()
        {
            isTyping = true;

            contentTxt.text = "";

            string curContent = this.curContent;

            var intervalTime = new WaitForSeconds(typingInterval);

            for (int i = 0; i < curContent.Length; i++)
            {
                contentTxt.text += curContent[i];
                yield return intervalTime;
            }

            isTyping = false;
        }

        public void SetUIVisible(bool visibled)
        {
            isVisible = visibled;

            nameTxt.gameObject.SetActive(visibled && !string.IsNullOrEmpty(nameTxt.text));
            contentTxt.gameObject.SetActive(visibled);
        }

        public void OnOpenDialogueUI()
        {
            gameObject.SetActive(true);
            anim.enabled = true;
            anim.SetBool("enable", true);
        }
        public void OnCloseDialogueUI()
        {
            anim.enabled = true;
            anim.SetBool("enable", false);
        }

        public void OnDisableAnimator()
        {
            anim.enabled = false;
        }
        public void OnClose()
        {
            gameObject.SetActive(false);
        }

        public void OnContinueBtn()
        {
            ContinueDialogue();
        }

        public void OnSwitchUIVisible()
        {
            SetUIVisible(!isVisible);
        }

    }
}
