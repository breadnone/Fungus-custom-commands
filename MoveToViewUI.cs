// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public enum scaleCharLeanTweenType
{
    /// <summary> Fade In effect sequence. </summary>
    SetCharacterScaleToDefault,
    None
}
namespace Fungus
{
    /// <summary>
    /// Moves the camera to a location specified by a View object.
    /// </summary>
    [CommandInfo("Camera",
                 "Move To View UI",
                 "Moves the camera to a location specified by a View object.")]
    [AddComponentMenu("")]
    public class MoveToViewUI : Command
    {
        [Tooltip("Time for move effect to complete")]
        [SerializeField] protected float duration = 1;

        [Tooltip("View to transition to when move is complete")]
        [SerializeField] protected View targetView;
        public virtual View TargetView { get { return targetView; } }

        [Tooltip("Wait until the fade has finished before executing next command")]
        [SerializeField] protected bool waitUntilFinished = true;

        [Tooltip("Camera to use for the pan. Will use main camera if set to none.")]
        [SerializeField] protected Camera targetCamera;

        [SerializeField] protected LeanTweenType orthoSizeTweenType = LeanTweenType.easeInOutQuad;
        [SerializeField] protected LeanTweenType posTweenType = LeanTweenType.easeInOutQuad;
        [SerializeField] protected LeanTweenType rotTweenType = LeanTweenType.easeInOutQuad;
        [Tooltip("Optional Canvas setup, if the canvas is ScreenSpace")]
        [SerializeField] protected Canvas canvas;
        [Tooltip("This may cause fickers on Enable/Disable during runtime. This change your canvas to World Space permanently")]
        [SerializeField] protected bool forceToCanvasWorldSpace = false;
        [Tooltip("Screen Resolution")]
        [SerializeField] protected Vector2 screenResolution = new Vector2(1920, 1080);
        [SerializeField] scaleCharLeanTweenType status = scaleCharLeanTweenType.None;
        [Tooltip("Character to zoom & scale")]
        [SerializeField] protected Character character;

        [Tooltip("Scale character")]
        [SerializeField] protected Vector2 scaleCharacterUI = new Vector2(1f, 1f);
        [Tooltip("Enable scale")]
        [SerializeField] protected bool enableScale = true;
        [Tooltip("Enable move")]
        [SerializeField] protected bool enableMove = false;
        [Tooltip("Move character")]
        [SerializeField] protected Vector2 moveCharacterUI = new Vector3(1f, 1f, 1f);
        [Tooltip("Duration")]
        [SerializeField] protected float scaleMoveDuration = 0.5f;
        [Tooltip("Ease type for the scale tween.")]
        [SerializeField] protected LeanTweenType easeType;
        private static List<GameObject> cacheChar = new List<GameObject>();
        private static bool canvasIsWorldSpace = false;
        protected bool isCompleted = false; protected bool moveIsCompleted = false; protected bool moveIsCompleted1 = false;
        protected bool moveIsCompleted2 = false; protected bool moveIsCompleted3 = false;
        protected virtual void AcquireCamera()
        {
            if (targetCamera != null)
            {
                return;
            }

            targetCamera = Camera.main;
            if (targetCamera == null)
            {
                targetCamera = GameObject.FindObjectOfType<Camera>();
            }
        }

        protected virtual void SetDefaultCharScale()
        {            
            if (character != null)
            {
                for (int i = 0; i < cacheChar.Count; i++)
                {
                    Canvas.ForceUpdateCanvases();
                    var b = cacheChar[i];
                    var c = b.GetComponent<RectTransform>();

                    LeanTween.scale(c, Vector3.one, 0.5f).setRecursive(true).setEase(easeType).setOnComplete(() =>
                    {
                        isCompleted = false;
                        //SetToDefaultCanvasSpace();
                    });
                }
            }
        }
        protected virtual void SetDefaultCharPosition()
        {
            if (character != null)
            {
                Canvas.ForceUpdateCanvases();
                for (int i = 0; i < cacheChar.Count; i++)
                {

                    var b = cacheChar[i];
                    var c = b.GetComponent<RectTransform>();
                    
                    float newOffsetMinnX = c.offsetMin.x;
                    float newOffsetMinnY = c.offsetMin.y;

                    float newOffsetMaxxX = c.offsetMax.x;
                    float newOffsetMaxxY = c.offsetMax.y;
                    
                    LeanTween.value(b, newOffsetMinnX, 0f, 0.5f).setOnUpdate((float val) =>
                    {
                        c.offsetMin = new Vector2(val, c.offsetMin.y);
                    }).setOnComplete(() =>
                        {
                            moveIsCompleted = false;
                        });
                    LeanTween.value(b, newOffsetMinnY, 0f, 0.5f).setOnUpdate((float val) =>
                    {
                        c.offsetMin = new Vector2(c.offsetMin.x, val);
                    }).setOnComplete(() =>
                        {
                            moveIsCompleted1 = false;
                        });

                    LeanTween.value(b, newOffsetMaxxX, 0f, 0.5f).setOnUpdate((float val) =>
                    {
                        c.offsetMax = new Vector2(val, c.offsetMax.y);
                    }).setOnComplete(() =>
                        {
                            moveIsCompleted2 = false;
                        });
                    LeanTween.value(b, newOffsetMaxxY, 0f, 0.5f).setOnUpdate((float val) =>
                    {
                        c.offsetMax = new Vector2(c.offsetMax.x, val);
                    }).setOnComplete(() =>
                        {
                            moveIsCompleted3 = false;
                        });

                    StartCoroutine(FinalStop());
                }
            }
        }
        protected virtual IEnumerator FinalStop()
        {
            while(true)
            {
                yield return null;
                if(!moveIsCompleted && !moveIsCompleted1 && !moveIsCompleted2 && !moveIsCompleted3)
                {
                    cacheChar = new List<GameObject>();
                    yield break;
                }
            }
        }

        protected virtual void ScaleMoveCharacter()
        {
            if (character && character.State.portraitImage != null)
            {
                if (character != null)
                {
                    for (int i = character.State.holder.transform.childCount - 1; i >= 0; i--)
                    {
                        var b = character.State.holder.transform.GetChild(i).gameObject;
                        cacheChar.Add(b);
                    }
                }
                if(cacheChar != null)
                {
                    var holdersS = character.State.holder.GetComponent<RectTransform>();

                    if(forceToCanvasWorldSpace)
                    {
                        SetCanvasToWorldSpace();
                    }

                    if (enableScale)
                    {
                        for (int i = 0; i < cacheChar.Count; i++)
                        {
                            var b = cacheChar[i];
                            var c = b.GetComponent<RectTransform>();

                            LeanTween.scale(c, scaleCharacterUI, scaleMoveDuration).setRecursive(true).setEase(easeType).setOnComplete(() =>
                            {
                                isCompleted = true;
                            });
                        }
                    }
                    if (enableMove)
                    {
                        for (int i = 0; i < cacheChar.Count; i++)
                        {
                            var b = cacheChar[i];
                            var c = b.GetComponent<RectTransform>();

                            LeanTween.move(c, moveCharacterUI, scaleMoveDuration).setEase(easeType).setOnComplete(() =>
                            {
                                moveIsCompleted = true;
                                moveIsCompleted1 = true;
                                moveIsCompleted2 = true;
                                moveIsCompleted3 = true;
                            });
                        }
                    }
                    //OnCompleted();
                }
            }
        }

        protected IEnumerator OnCompleted()
        {
            while (true)
            {
                yield return null;
                if (enableScale == false && enableMove == true && moveIsCompleted == true)
                {
                    break;
                }
                if (enableScale == true && enableMove == false && isCompleted == true)
                {
                    break;
                }
                if (enableScale == true && enableMove == true && moveIsCompleted == true && isCompleted == true)
                {
                    break;
                }
            }
            //SetToDefaultCanvasSpace();
        }

        protected virtual IEnumerator SetToDefaultCanvasSpace()
        {
            if (canvas != null)
            {
                if (targetCamera != null)
                {
                    yield return new WaitForSeconds(1f);
                    Canvas.ForceUpdateCanvases();
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = targetCamera;
                    var canvy = canvas.GetComponent<CanvasScaler>();
                    canvy.referenceResolution = screenResolution;
                    canvy.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    canvy.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    canvasIsWorldSpace = false;
                }
            }
        }

        protected void SetCanvasToWorldSpace()
        {
            Canvas.ForceUpdateCanvases();
            if (canvas != null)
            {

                if (targetCamera != null)
                {
                    canvasIsWorldSpace = false;
                    canvas.renderMode = RenderMode.WorldSpace;
                    canvas.worldCamera = targetCamera;
                }

                /*
                                    if(targetCamera != null)
                                    {
                                        canvasIsWorldSpace = true;

                                        canvas.renderMode = RenderMode.ScreenSpaceCamera;
                                        canvas.worldCamera = targetCamera;

                                        var canvy = canvas.GetComponent<CanvasScaler>();
                                        canvy.referenceResolution = screenResolution;
                                        canvy.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                                        canvy.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                                    }
                                    */

            }
        }

        public virtual void Start()
        {
            AcquireCamera();
        }

        #region Public members

        public override void OnEnter()
        {
            if (status == scaleCharLeanTweenType.SetCharacterScaleToDefault)
            {

                //SetToDefaultCanvasSpace();
                SetDefaultCharPosition();
                SetDefaultCharScale();
                AcquireCamera();

                if (targetCamera == null ||
                    targetView == null)
                {
                    Continue();
                    return;
                }

                var cameraManager = FungusManager.Instance.CameraManager;

                Vector3 targetPosition = targetView.transform.position;
                Quaternion targetRotation = targetView.transform.rotation;
                float targetSize = targetView.ViewSize;

                cameraManager.PanToPosition(targetCamera, targetPosition, targetRotation, targetSize, duration, delegate
                {
                    SayDialog sayDialog = SayDialog.GetSayDialog();
                    sayDialog.GetComponent<DialogInput>().enabled = false;
                    if (waitUntilFinished)
                    {                        
                        Continue();
                        sayDialog.GetComponent<DialogInput>().enabled = true;
                    }
                }, orthoSizeTweenType, posTweenType, rotTweenType);

                if (!waitUntilFinished)
                {
                    Continue();
                }
            }
            else
            {
                //Create new list on first run
                cacheChar = new List<GameObject>();
                //Cache the character to a list
                if (character != null)
                {
                    for (int i = character.State.holder.transform.childCount - 1; i >= 0; i--)
                    {
                        var b = character.State.holder.transform.GetChild(i).gameObject;
                        cacheChar.Add(b);
                    }
                }

                if (character != null)
                {
                    ScaleMoveCharacter();
                }

                AcquireCamera();
                if (targetCamera == null ||
                    targetView == null)
                {
                    Continue();
                    return;
                }

                var cameraManager = FungusManager.Instance.CameraManager;

                Vector3 targetPosition = targetView.transform.position;
                Quaternion targetRotation = targetView.transform.rotation;
                float targetSize = targetView.ViewSize;

                cameraManager.PanToPosition(targetCamera, targetPosition, targetRotation, targetSize, duration, delegate
                {
                    if (waitUntilFinished)
                    {
                        Continue();
                    }
                }, orthoSizeTweenType, posTweenType, rotTweenType);

                if (!waitUntilFinished)
                {
                    Continue();
                }
            }
        }

        public override void OnStopExecuting()
        {
            var cameraManager = FungusManager.Instance.CameraManager;

            cameraManager.Stop();
        }

        public override string GetSummary()
        {
            if (targetView == null)
            {
                return "Error: No view selected";
            }
            else
            {
                return targetView.name;
            }
        }

        public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

        #endregion
    }
}