// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Fungus
{
    public enum cartoonType2
    {
        /// <summary> Fade In effect sequence. </summary>
        Enable,
        /// <summary> Stop All </summary>
        Disable
    }
    /// <summary>
    /// Tween sequence
    /// </summary>
    [CommandInfo("Animation",
                 "Cartoon Effects2",
                 "Sequence of tween animations. For Overlay mode/object to be Always-On-Top, drag and drop your Camera to inspector")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public partial class CartoonEffects : Command
    {
        [Tooltip("Wait until the tween has finished before executing the next command")]
        [HideInInspector] protected bool waitUntilFinished = true;
        [Tooltip("Where the effect spawns on screen")]
        [HideInInspector] public Vector3 effectPosition;
        [Tooltip("Enable")]
        [SerializeField] public cartoonType2 enable;
        [Tooltip("GameObject/Bitmaps")]
        [SerializeField] public Image mainSprite;
        [Tooltip("The shape of the easing curve applied to the animation")]
        [SerializeField] protected LeanTweenType mainEaseType = LeanTweenType.punch;
        [Tooltip("Scales sprite")]
        [SerializeField] public float mainSpriteScale = 1f;
        [Tooltip("Disable sprite")]
        [SerializeField] public bool disableSprite = false;
        [Tooltip("The type of loop to apply once the animation has completed")]
        [SerializeField]
        protected LeanTweenType mainLoopType = LeanTweenType.clamp;
        [Tooltip("Tween rotation")]
        [SerializeField] public float rotationAngle = 30f;
        [Tooltip("Tween rotation")]
        [SerializeField] public bool rotationLoop = false;
        [Tooltip("particles")]
        [SerializeField] public Image secondarySprite;
        [Tooltip("Scales secondary sprite size")]
        [SerializeField] public float secondaryScale = 0.4f;
        [Tooltip("Tween rotation")]
        [SerializeField] public float secondaryRotation = 30f;
        [Tooltip("Tween rotation")]
        [SerializeField] public bool secondaryLoop = false;
        [Tooltip("particles")]
        [SerializeField] public Image particleSprite;
        [Tooltip("The shape of the easing curve applied to the animation")]
        [SerializeField] protected LeanTweenType pEaseType = LeanTweenType.easeInOutCubic;
        [Tooltip("Tween x pattern")]
        [SerializeField] public bool xPattern = false;
        [Tooltip("Tween + pattern")]
        [SerializeField] public bool plusPattern = false;
        [Tooltip("Tween duration")]
        [SerializeField] public float tweenDurations = 0.4f;
        [Tooltip("Fade duration")]
        [SerializeField] public float fadeDurations = 0.4f;
        [Tooltip("Particle tween rotation")]
        [SerializeField] public float rotateParticles = 0f;
        [Tooltip("Scales particle size")]
        [SerializeField] public float particleScale = 0.4f;
        [Tooltip("Disable particle sprites")]
        [SerializeField] public bool disableParticle = false;
        [Tooltip("Enable continuous fading while tweening to particles")]
        [SerializeField] public bool enableAlpha = false;
        [Header("Location on where the effect appears on screen. The camera option is OPTIONAL!")]
        [Tooltip("Where the effects should be appeared on screen, x position")]
        [SerializeField] public float posX = 200f;
        [Tooltip("Where the effects should be appeared on screen, y position")]
        [SerializeField] public float posY = 200f;
        [Space(6)]
        [Tooltip("Sorting order")]
        [SerializeField] public int sortingOrder = 0;
        [Space(4)]
        [Tooltip("Screen Space Camera mode, if empty Overlay mode is used")]
        [SerializeField] public Camera optionalMainCamera;
        [Tooltip("Store them in a List and make them re-usable")]
        [SerializeField] public List<Image> partcList = new List<Image>();
        private IEnumerator coroutine;
        private Canvas nCanvas;
        private int objIndexNum;
        private int midObjIndexNum;
        private int lastIndexNum;
        int onLast = 0;
        int onMid = 8;
        int onTop = 16;
        protected void saveWait()
        {
            coroutine = StopAnim(0.1f);
            //Start all delayed calls
            StartCoroutine(coroutine);
        }

        protected void crCanvas()
        {
            //Programmatically create an empty Canvas.
            Canvas myCanvas;
            CanvasScaler myCanvasScaler;
            GraphicRaycaster myGraphicRaycaster;
            GameObject myGO = new GameObject("stvphtwod");
            myGO.AddComponent<Canvas>();
            myCanvas = myGO.GetComponent<Canvas>();
            if (optionalMainCamera != null)
            {
                myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                myCanvas.worldCamera = optionalMainCamera;
            }
            else
            {
                myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            myCanvas.pixelPerfect = true;
            myCanvas.sortingOrder = sortingOrder;
            myGO.AddComponent<CanvasScaler>();
            myCanvasScaler = myGO.GetComponent<CanvasScaler>();
            myCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            myCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            myCanvasScaler.matchWidthOrHeight = 1f;
            myGO.AddComponent<GraphicRaycaster>();
            myGraphicRaycaster = myGO.GetComponent<GraphicRaycaster>();
            myGraphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.All;
            nCanvas = myCanvas.GetComponent<Canvas>();
        }
        //Main method
        protected void Splash(int partcNum)
        {   //If Canvas exist, us that, otherwise generate new one
            if (nCanvas == null)
            {
                crCanvas();
            }

            midObjIndexNum = onLast;
            objIndexNum = onTop;
            effectPosition = new Vector3(posX, posY, 1f);
            var spunPoint = effectPosition;

            //Initial pivot location for object spawn
            if (!disableParticle)
            {
                particleSprite.enabled = true;
            }

            if (mainSprite != null && disableSprite == false)
            {
                Image blobmain = Instantiate(mainSprite, spunPoint, Quaternion.identity) as Image;
                blobmain.name = "ssplashpopp-main";
                blobmain.transform.SetParent(nCanvas.transform, false);
                blobmain.transform.SetAsLastSibling();                
                blobmain.transform.localScale = new Vector3(mainSpriteScale, mainSpriteScale, 1f);
                LeanTween.scale(blobmain.rectTransform, Vector3.zero, tweenDurations).setEase(mainEaseType).setLoopType(mainLoopType);
                blobmain.enabled = true;
                if (rotationLoop)
                {
                    LeanTween.rotateAroundLocal(blobmain.rectTransform, Vector3.forward, rotationAngle, 0.3f).setLoopPingPong(-1);
                }
            }
            if (particleSprite != null && disableParticle == false)
            {
                for (int i = 0; i < partcNum; i++)
                {
                    Image blob0 = Instantiate(particleSprite, spunPoint, Quaternion.identity) as Image;
                    blob0.name = "ssplashpopp" + (i + 1);
                    blob0.transform.SetParent(nCanvas.transform, false);
                    blob0.transform.SetSiblingIndex(midObjIndexNum++);
                    blob0.transform.localScale = new Vector3(particleScale, particleScale, 1f);
                    partcList.Add(blob0);

                    if (disableParticle == true && particleSprite != null)
                    {
                        blob0.enabled = false;

                    }
                    if (disableParticle == false && particleSprite != null)
                    {
                        blob0.enabled = true;
                    }
                }

                //Set max particles & get from the List
                int maxParticles = 7;
                for (int w = maxParticles; w < partcList.Count; w++)
                {
                    foreach (Image partcb in partcList)
                    {
                        LeanTween.scale(partcb.rectTransform, new Vector3(0f, 0f, 0f), tweenDurations).setEaseInQuad().setLoopClamp();
                        if (enableAlpha == true && disableParticle == false && particleSprite != null)
                        {
                            LeanTween.alpha(partcb.rectTransform, 0f, fadeDurations).setEase(LeanTweenType.linear).setLoopClamp();
                        }

                        if (rotateParticles > 0 && disableParticle == false && particleSprite != null)
                        {
                            LeanTween.rotateAroundLocal(partcb.rectTransform, Vector3.forward, rotationAngle, 0.3f).setLoopPingPong(-1);
                        }
                    }
                    if (plusPattern)
                    {
                        LeanTween.moveLocal(partcList[0].gameObject, spunPoint + new Vector3(160, 0f, 0f), tweenDurations).setLoopClamp().setEase(pEaseType);
                        LeanTween.moveLocal(partcList[4].gameObject, spunPoint + new Vector3(0f, -180f, 0f), tweenDurations).setLoopClamp().setEase(pEaseType);
                        LeanTween.moveLocal(partcList[5].gameObject, spunPoint + new Vector3(-160f, 0f, 0f), tweenDurations).setLoopClamp().setEase(pEaseType);
                        LeanTween.moveLocal(partcList[2].gameObject, spunPoint + new Vector3(0f, 180f, 0f), tweenDurations).setLoopClamp().setEase(pEaseType);
                    }

                    if (xPattern)
                    {
                        LeanTween.moveLocal(partcList[1].gameObject, spunPoint + new Vector3(130f, 120f, 0f), tweenDurations).setLoopClamp().setEase(pEaseType);
                        LeanTween.moveLocal(partcList[3].gameObject, spunPoint + new Vector3(130f, -120f, 0f), tweenDurations).setLoopClamp().setEase(pEaseType);
                        LeanTween.moveLocal(partcList[6].gameObject, spunPoint + new Vector3(-130f, -120f, 0f), tweenDurations).setLoopClamp().setEase(pEaseType);
                        LeanTween.moveLocal(partcList[7].gameObject, spunPoint + new Vector3(-130f, 120f, 0f), tweenDurations).setLoopClamp().setEase(pEaseType);
                    }
                }

            }

            if (waitUntilFinished)
            {
                Continue();
            }
        }

        //Destroy all instantiated objects
        protected IEnumerator StopAnim(float searchNdestroy)
        {
            if (partcList != null)
            {
                if (particleSprite != null)
                {
                    particleSprite.enabled = false;
                }

                if (mainSprite != null)
                {
                    particleSprite.enabled = false;
                }
                var imgclones = Resources.FindObjectsOfTypeAll<Image>();
                foreach (var bg in imgclones)
                {
                    if (bg.name.StartsWith("ssplashpopp"))
                    {
                        LeanTween.scale(bg.gameObject, new Vector3(-1f, 0f, 0f), 0.2f).setDestroyOnComplete(true);
                        partcList.Clear();
                        partcList.TrimExcess();
                    }
                }
                yield return new WaitForSeconds(searchNdestroy);
                var canvcont = Resources.FindObjectsOfTypeAll<Canvas>().Where(obj => obj.name == "stvphtwod");
                foreach (Canvas canvs in canvcont)
                {
                    if (canvs != null)
                    {
                        GameObject.Destroy(canvs.gameObject);
                    }
                }

                Debug.Log("Item =" + partcList.Count);
                Debug.Log("Element =" + partcList.Capacity);

                if (waitUntilFinished)
                {
                    Continue();
                }
            }
        }
        #region Public members
        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }
        public override void OnEnter()
        {
            Canvas.ForceUpdateCanvases();
            //RunForestRun();
            if (mainSprite != null && disableSprite == false)
            {
                mainSprite.enabled = false;
            }
            if (particleSprite != null)
            {
                particleSprite.enabled = false;
            }

            switch (enable)
            {
                case (cartoonType2.Enable):
                    Splash(8);
                    SecondaryEf(2);
                    break;
                case (cartoonType2.Disable):
                    saveWait();
                    break;
            }

            if (!waitUntilFinished)
            {
                Continue();
            }
        }
        public override void OnCommandAdded(Block parentBlock)
        {
            enable = cartoonType2.Disable;
        }
        #endregion
    }
}