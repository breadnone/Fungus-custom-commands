// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
namespace Fungus
{
    public enum clicChar
    {
        Enable,
        Disable
    }
    /// <summary>
    /// Tween sequence
    /// </summary>
    [CommandInfo("Sprite",
                 "Clickable Character",
                 "Adds ability for character to be clickable with the use of buttons. Yes, you heard me right :)")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class ClickableCharacter : Command
    {
        [Tooltip("Enable")]
        [SerializeField] public clicChar status;
        [SerializeField] protected Character character;
        [SerializeField] protected Button rectButton0;
        [SerializeField] protected GameObject ifRectButton0Clicked;
        [SerializeField] protected Button rectButton1;
        [SerializeField] protected GameObject ifRectButton1Clicked;
        [SerializeField] protected Button rectButton2;
        [SerializeField] protected GameObject ifRectButton2Clicked;
        [SerializeField] protected GameObject canvas;
        [SerializeField] protected bool characterHolderAsParent = false;
        [SerializeField] protected bool enableDebugLog = false;
        [SerializeField] protected float upperRegionHeight = 2f;
        [SerializeField] protected float upperRegionOffset = -3f;
        [SerializeField] protected float centerRegionHeight = 2f;
        [SerializeField] protected float centerRegionOffset = -1.5f;
        [SerializeField] protected float lowerRegionHeight = 2f;
        [SerializeField] protected float lowerRegionOffset = 0f;
        [Header("REFERENCE RESOLUTION")]
        [SerializeField] protected float ResWidth = 1920f;
        [SerializeField] protected float ResHeight = 1080f;
        protected Canvas nCanvas;
        public static bool actives = false;
        WaitForSeconds waiting = new WaitForSeconds(0.2f);
        WaitForSeconds waitDestroy = new WaitForSeconds(0.2f);

        protected void crCanvas()
        {
            //Instantiate default canvas as parent
            Canvas myCanvas;
            CanvasScaler myCanvasScaler;
            CanvasGroup myCanvasGroup;
            GraphicRaycaster myGraphicRaycaster;
            GameObject myGO = new GameObject("stvphtwod-fclickable" + "_" + character.name);
            myGO.AddComponent<Canvas>();
            myCanvas = myGO.GetComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //myCanvas.worldCamera = mainCamera;
            myCanvas.pixelPerfect = false;
            myCanvas.sortingOrder = 99;
            myGO.AddComponent<CanvasScaler>();
            myCanvasScaler = myGO.GetComponent<CanvasScaler>();
            myCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            myCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            myCanvasScaler.matchWidthOrHeight = 0.5f;
            myCanvasScaler.referenceResolution = new Vector2(ResWidth, ResHeight);
            myGO.AddComponent<GraphicRaycaster>();
            myGraphicRaycaster = myGO.GetComponent<GraphicRaycaster>();
            myGraphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.All;

            myGO.AddComponent<CanvasGroup>();
            myCanvasGroup = myGO.GetComponent<CanvasGroup>();
            myCanvasGroup.interactable = true;
            myCanvasGroup.blocksRaycasts = true;
            myCanvasGroup.ignoreParentGroups = true;

            nCanvas = myCanvas.GetComponent<Canvas>();
        }

        protected void ClickableCharacters()
        {
            if(canvas != null)
            {
                if (rectButton0 && rectButton1 && rectButton2 != null)
                {
                    if(character.State.portraitImage == null)
                    {
                        Debug.Log("Portrait has not been spwaned yet!");
                        return;
                    }
                    canvas.SetActive(true);
                    /*
                    if(ifRectButton0Clicked && ifRectButton1Clicked && ifRectButton2Clicked != null)
                    {
                        ifRectButton0Clicked.SetActive(false);
                        ifRectButton1Clicked.SetActive(false);
                        ifRectButton2Clicked.SetActive(false);
                    }
                    */
                    crCanvas();
                    Continue();

                    //The maths
                    var charSizX = character.State.portraitImage.rectTransform.rect.x;
                    var charSizY = character.State.portraitImage.rectTransform.rect.y / 3;
                    var charPosX = character.State.portraitImage.transform.position.x;
                    var charPosY = character.State.portraitImage.transform.position.y;
                    var charPos = new Vector2(charPosX, charPosY);
                    var scaleSpwn = new Vector2(charSizX, charSizY / 3);
                    var spwn = new Vector2(charPosX, charPosY);

                    //Set parent
                    rectButton0.transform.SetParent(nCanvas.transform, false);
                    rectButton1.transform.SetParent(nCanvas.transform, false);
                    rectButton2.transform.SetParent(nCanvas.transform, false);
                    
                    if(characterHolderAsParent)
                    {
                        //This option is for others with weird setups
                        nCanvas.transform.SetParent(character.State.holder, false);
                    }

                    //getting the rectTransform value of the character sprites during runtime is necessary, incase the original was modified
                    var getRectX = character.State.portraitImage.rectTransform.rect.x;

                    rectButton0.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(charSizX, getRectX / lowerRegionHeight);
                    rectButton1.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(charSizX, getRectX / centerRegionHeight);
                    rectButton2.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(charSizX, getRectX / upperRegionHeight);

                    //Always pivoting to first button
                    rectButton0.transform.position = new Vector2(character.State.portraitImage.transform.position.x, getRectX / 3 * lowerRegionOffset);
                    rectButton1.transform.position = new Vector2(character.State.portraitImage.transform.position.x, rectButton0.transform.position.y + getRectX / 3 * centerRegionOffset);
                    rectButton2.transform.position = new Vector2(character.State.portraitImage.transform.position.x, rectButton0.transform.position.y + getRectX / 3 * upperRegionOffset);

                    //Add listeners
                    rectButton0.onClick.AddListener(() => buttonCallBack(rectButton0));
                    rectButton1.onClick.AddListener(() => buttonCallBack(rectButton1));
                    rectButton2.onClick.AddListener(() => buttonCallBack(rectButton2));
                }
                else if(rectButton0 && rectButton1 && rectButton2 != null)
                {
                    if(status == clicChar.Enable)
                    {
                        Continue();
                        return;
                    }
                }
            }
        }
        //Button callbakcs
        private void buttonCallBack(Button buttonPressed)
        {
            if (buttonPressed == rectButton0)
            {
                StartCoroutine(GetClickedCharacter0());
            }

            if (buttonPressed == rectButton1)
            {
                StartCoroutine(GetClickedCharacter1());
            }

            if (buttonPressed == rectButton2)
            {
                StartCoroutine(GetClickedCharacter2());
            }
        }

        public IEnumerator DisableClickable()
        {
            var dObjects = Resources.FindObjectsOfTypeAll<Canvas>().Where(obj => obj.name == "stvphtwod-fclickable" + "_" + character.name);

            rectButton0.transform.SetParent(canvas.transform, false);
            rectButton1.transform.SetParent(canvas.transform, false);
            rectButton2.transform.SetParent(canvas.transform, false);
            if(canvas != null)
            {
                canvas.SetActive(false);
            }
            yield return waitDestroy;
            foreach (Canvas clone in dObjects)
            {
                GameObject.Destroy(clone.gameObject);
            }
        }
        //Lower Region
        public IEnumerator GetClickedCharacter0()
        {
            //prevent click spams
            if(enableDebugLog)
            {
                Debug.Log("Bottom region was clicked!");
            }
            yield return waiting;
            if(ifRectButton0Clicked != null)
            {
                ifRectButton0Clicked.SetActive(true);
            }
            this.StopAllCoroutines();
        }
        //Center Region
        public IEnumerator GetClickedCharacter1()
        {
            //prevent click spams
            if(enableDebugLog)
            {
                Debug.Log("Center region was clicked!");
            }
            yield return waiting;
            if(ifRectButton1Clicked != null)
            {
                ifRectButton1Clicked.SetActive(true);
            }
        }
        //Upper Region
        public IEnumerator GetClickedCharacter2()
        {
            //prevent click spams
            if(enableDebugLog)
            {
                Debug.Log("Upper region was clicked!");
            }
            yield return waiting;
            if(ifRectButton2Clicked != null)
            {
                ifRectButton2Clicked.SetActive(true);
            }
        }

        #region Public members
        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }

        public override string GetSummary()
        {
            string noCol = "";
            string noBtn0 = "";
            string noBtn1 = "";
            string noBtn2 = "";
            string noCan = "";
            if (character == null)
            {
                noCol = "Error: No Character is selected";
            }
            if (rectButton0 == null && status == clicChar.Enable)
            {
                noBtn0 = "Error: null Button0";
            }
            if (rectButton1 == null && status == clicChar.Enable)
            {
                noBtn1 = "Error: null Button1";
            }
            if (rectButton2 == null && status == clicChar.Enable)
            {
                noBtn2 = "Error: null Button2";
            }
            return noCol + ":" + noBtn0 + ":" + noBtn1 + ":" + noBtn2  + ":" + noCan;
        }
        public override void OnEnter()
        {
            //Force 1st frame update
            Canvas.ForceUpdateCanvases();

            switch (status)
            {
                case (clicChar.Disable):
                    StartCoroutine(DisableClickable());
                    actives = false;
                    break;
                case (clicChar.Enable):
                    if (rectButton0 && rectButton1 && rectButton2 != null)
                    {
                        if(canvas != null)
                        {
                            ClickableCharacters();
                            actives = true;
                        }
                    }
                    break;
            }
            if(status == clicChar.Disable)
            {
                Continue();
            }
        }

        public override void OnCommandAdded(Block parentBlock)
        {
            status = clicChar.Disable;
        }
        #endregion
    }
}
