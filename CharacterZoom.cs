// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public enum RotateZoomType
{
    /// <summary> Fade In effect sequence. </summary>
    SetCharacterToDefault,
    None
}
namespace Fungus
{
    /// <summary>
    /// Moves the camera to a location specified by a View object.
    /// </summary>
    [CommandInfo("Animation",
                 "Zoom Flip Character",
                 "Zoom the character on Stage. MUST BE PLACED UNDER PORTRAIT if there's portrait change or at least not too close")]
    [AddComponentMenu("")]
    public class CharacterZoom : Command
    {
        [SerializeField] RotateZoomType status = RotateZoomType.None;
        [Tooltip("Character to zoom & scale")]
        [SerializeField] protected Character character;
        [Tooltip("Scale character")]
        [SerializeField] protected Vector3 scaleCharacterUI = new Vector3(1f, 1f, 1f);
        [Tooltip("Wait until the fade has finished before executing next command")]
        [SerializeField] protected bool flipCharacter = false;
        [Tooltip("Enable scale")]
        [SerializeField] protected bool enableScale = true;
        [Tooltip("Enable move")]
        [SerializeField] protected bool enableMove = false;
        [SerializeField] protected bool enableRotate = false;
        [Tooltip("Move character")]
        [SerializeField] protected Vector3 moveCharacterUI = new Vector3(0f, 0f, 0f);
        [Tooltip("Rotate angle in floats")]
        [SerializeField] protected float rotateAngle = 0f;

        [Tooltip("Duration")]
        [SerializeField] protected float scaleMoveDuration = 0.3f;
        [Tooltip("Ease type for the scale tween.")]
        [SerializeField] protected LeanTweenType easeType;

        [Tooltip("Wait until the fade has finished before executing next command")]
        [SerializeField] protected bool waitUntilFinished = true;
        protected bool isCompleted = false;
        protected bool moveIsCompleted = false; protected bool moveIsCompleted1 = false; protected bool moveIsCompleted2 = false; protected bool moveIsCompleted3 = false; protected bool rotIsTru = false;   
        protected virtual void SetDefaultCharScale()
        {
            for (int i = character.State.holder.transform.childCount - 1; i >= 0; i--)
            {
                var a = character.State.holder.transform.GetChild(i).gameObject;
                var c = a.GetComponent<RectTransform>();

                LeanTween.scale(c, Vector3.one, scaleMoveDuration).setRecursive(true).setEase(easeType).setOnComplete(() =>
                {
                    isCompleted = false;
                });
                //Reset character rotation if any
                LeanTween.rotateAround(c, Vector3.forward, 0, scaleMoveDuration).setEaseOutQuad().setOnComplete(() =>
                {
                    c.rotation = Quaternion.identity;
                    rotIsTru = false;
                });
            }
        }
        protected virtual void SetDefaultCharPosition()
        {
            for (int i = character.State.holder.transform.childCount - 1; i >= 0; i--)
            {
                var b = character.State.holder.transform.GetChild(i).gameObject;
                var c = b.GetComponent<RectTransform>();

                float newOffsetMinnX = c.offsetMin.x;
                float newOffsetMinnY = c.offsetMin.y;

                float newOffsetMaxxX = c.offsetMax.x;
                float newOffsetMaxxY = c.offsetMax.y;

                LeanTween.value(b, newOffsetMinnX, 0f, scaleMoveDuration).setOnUpdate((float val) =>
                {
                    c.offsetMin = new Vector2(val, c.offsetMin.y);
                }).setOnComplete(() =>
                    {                        
                        moveIsCompleted = false;
                    });
                LeanTween.value(b, newOffsetMinnY, 0f, scaleMoveDuration).setOnUpdate((float val) =>
                {
                    c.offsetMin = new Vector2(c.offsetMin.x, val);
                }).setOnComplete(() =>
                    {
                        moveIsCompleted1 = false;
                    });

                LeanTween.value(b, newOffsetMaxxX, 0f, scaleMoveDuration).setOnUpdate((float val) =>
                {
                    c.offsetMax = new Vector2(val, c.offsetMax.y);
                }).setOnComplete(() =>
                    {
                        moveIsCompleted2 = false;
                    });
                LeanTween.value(b, newOffsetMaxxY, 0f, scaleMoveDuration).setOnUpdate((float val) =>
                {
                    c.offsetMax = new Vector2(c.offsetMax.x, val);
                }).setOnComplete(() =>
                    {
                        moveIsCompleted3 = false;
                    });

                StartCoroutine(FinalStop());
            }
        }
        protected virtual IEnumerator FinalStop()
        {
            while (true)
            {
                yield return null;
                if (!moveIsCompleted && !moveIsCompleted1 && !moveIsCompleted2 && !moveIsCompleted3 && !isCompleted && !rotIsTru)
                {
                    break;
                }
            }
            Stops();
        }
        protected virtual void Stops()
        {
            for (int i = character.State.holder.transform.childCount - 1; i >= 0; i--)
            {
                var b = character.State.holder.transform.GetChild(i).gameObject;
                b.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            }
            Continue();
            StopAllCoroutines();
        }
        protected virtual void Continues()
        {
            Continue();
        }

        protected virtual void ZoomRotateCharacter()
        {
            if (character && character.State.portraitImage != null)
            {
                for (int i = character.State.holder.transform.childCount - 1; i >= 0; i--)
                {
                    var b = character.State.holder.transform.GetChild(i).gameObject;
                    var c = b.GetComponent<RectTransform>();

                        if (enableScale)
                        {
                            LeanTween.scale(c, scaleCharacterUI, scaleMoveDuration).setRecursive(true).setEase(easeType).setOnComplete(() =>
                                {
                                    isCompleted = true;
                                    Continues();
                                });
                        }
                        if (enableMove)
                        {
                            LeanTween.move(c, moveCharacterUI, scaleMoveDuration).setEase(easeType).setOnComplete(() =>
                                {
                                    moveIsCompleted = true;
                                    moveIsCompleted1 = true;
                                    moveIsCompleted2 = true;
                                    moveIsCompleted3 = true;
                                    Continues();
                                    
                                });
                        }
                        if (flipCharacter)
                        {
                            LeanTween.scale(c, new Vector3(-scaleCharacterUI.x, scaleCharacterUI.y, scaleCharacterUI.z), scaleMoveDuration).setEase(easeType).setOnComplete(() =>
                                {
                                    Continues();
                                });                          
                        }
                        if(enableRotate)
                        {
                            LeanTween.rotateAround(c, Vector3.forward, rotateAngle, scaleMoveDuration).setEase(easeType).setOnComplete(() =>
                                {
                                    Continues();
                                });   
                        }
                }
                
            }
        }

        #region Public members

        public override void OnEnter()
        {
            if (status == RotateZoomType.SetCharacterToDefault)
            {
                if(character != null)
                {
                    Canvas.ForceUpdateCanvases();
                    moveIsCompleted = true;
                    moveIsCompleted1 = true;
                    moveIsCompleted2 = true;
                    moveIsCompleted3 = true;
                    isCompleted = true;
                    rotIsTru = true;

                    SetDefaultCharPosition();
                    SetDefaultCharScale();
                }

                if (!waitUntilFinished)
                {
                    Continue();
                }
            }
            else
            {
                 if (character != null)
                {
                    Canvas.ForceUpdateCanvases();
                    ZoomRotateCharacter();
                }

                if (!waitUntilFinished)
                {
                    Continue();
                }
            }
        }
        public override string GetSummary()
        {
            if (character == null)
            {
                return "Error: No character selected";
            }
            else
            {
                return character.name;
            }
        }

        public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

        #endregion
    }
}