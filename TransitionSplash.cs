// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;
using UnityEngine.Serialization;
namespace Fungus
{
    public enum transitionSplash
    {
        /// <summary> Fade In effect sequence. </summary>
        VerticalLoop,
        /// <summary> Fade In effect sequence. </summary>
        VerticalSqueeze,
        /// <summary> Fade In effect sequence. </summary>
        VerticalSqueeze2,
        /// <summary> Stop All </summary>
        Disable
    }

    /// <summary>
    /// Tween sequence
    /// </summary>
    [CommandInfo("Animation",
                 "Screen Transition",
                 "Collection of transition effects")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class TransitionSplash : Command
    {
        [Tooltip("Wait until the tween has finished before executing the next command")]
        [HideInInspector] protected bool waitUntilFinished = true;
        [Tooltip("Enable")]
        [SerializeField] public transitionSplash splashSelect;
        [Tooltip("Text")]
        [SerializeField] public String txtAdd = "Aya's Loop";
        [Tooltip("Text Size")]
        [SerializeField] public int txtSize = 33;
        [Tooltip("Sorting Order")]
        [SerializeField] public int sortOrder = 10;
        [SerializeField] public bool disableText = false;
        [Tooltip("Name of a label in this block to jump to")]
        [SerializeField] protected StringData _targetLabel = new StringData("");
        [SerializeField] protected GameObject UpperMenu;
        protected bool stillTweening = false;
        private TextMeshProUGUI textMesh;
        private Vector3 posxy;
        private Vector2 posxyzLeft;
        private IEnumerator coroutine;
        private Canvas nCanvas;
        private RawImage sqLine;        
        Color32 color0 = new Color32(139, 191, 229, 255); Color32 color1 = new Color32(229, 187, 139, 255);
        Color32 color2 = new Color32(139, 165, 229, 255); Color32 color3 = new Color32(203, 229, 139, 255);
        Color32 color4 = new Color32(139, 195, 229, 255); Color32 color5 = new Color32(229, 206, 139, 255);
        Color32 color6 = new Color32(229, 139, 221, 255);
        protected void sequenceMove()
        {
            coroutine = SequenceOfLines(0.2f);
            StartCoroutine(coroutine);
        }
        protected void squeezeLines()
        {
            coroutine = VerticalSqueeze(0.2f);
            StartCoroutine(coroutine);
        }
         protected void squeezeLines2()
        {
            coroutine = VerticalSqueeze2(0.2f);
            StartCoroutine(coroutine);
        }
        protected void crCanvas()
        {
            Canvas myCanvas;
            CanvasScaler myCanvasScaler;
            GraphicRaycaster myGraphicRaycaster;
            GameObject myGO = new GameObject("stvphtwod");
            myGO.AddComponent<Canvas>();
            myCanvas = myGO.GetComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //myCanvas.worldCamera = mainCamera;
            myCanvas.pixelPerfect = false;
            myCanvas.sortingOrder = sortOrder;
            myGO.AddComponent<CanvasScaler>();
            myCanvasScaler = myGO.GetComponent<CanvasScaler>();
            myCanvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
            myCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            myCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            myCanvasScaler.matchWidthOrHeight = 0.5f;
            myGO.AddComponent<GraphicRaycaster>();
            myGraphicRaycaster = myGO.GetComponent<GraphicRaycaster>();
            myGraphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.All;
            nCanvas = myCanvas.GetComponent<Canvas>();
        }
        protected void crRawImage()
        {
            RawImage myRawImage;
            GameObject myGO = new GameObject("seqlinessqrschild");
            myGO.AddComponent<RawImage>();
            myRawImage = myGO.GetComponent<RawImage>();
            myRawImage.color = Color.yellow;
            myRawImage.raycastTarget = true;
            myRawImage.rectTransform.sizeDelta = new Vector2(Screen.width / 6, 2);
            sqLine = myRawImage.GetComponent<RawImage>();
            myRawImage.transform.SetParent(nCanvas.transform, false);
        }
        protected void crTxt()
        {
            TextMeshProUGUI myTxt;
            GameObject myGO = new GameObject("seqlinessqrschildtxtt");
            myGO.AddComponent<TextMeshProUGUI>();
            myTxt = myGO.GetComponent<TextMeshProUGUI>();
            myTxt.color = Color.white;
            myTxt.text = txtAdd;
            myTxt.fontSize = txtSize;
            myTxt.alignment = TextAlignmentOptions.Center;
            myTxt.fontStyle = FontStyles.Bold;
            myTxt.rectTransform.sizeDelta = new Vector2(0, Screen.height);
            myTxt.raycastTarget = true;
            myTxt.transform.SetParent(nCanvas.transform, false);
            myTxt.transform.SetAsLastSibling();
            textMesh = myTxt.GetComponent<TextMeshProUGUI>();
            posxyzLeft = new Vector2(Screen.width / 6 * 0, 0);
            myTxt.transform.position = posxyzLeft + new Vector2(Screen.width / 6 * 1, 0);
        }
        public void createObjects()
        {
            if(UpperMenu != null)
            {            
                UpperMenu.SetActive(false);
            }
            crCanvas();
            crRawImage();
            if(disableText == false && txtAdd != null)
            {
                crTxt();
            }
        }
        protected IEnumerator SequenceOfLines(float seqMove)
        {
            createObjects();
            
            sqLine.enabled = false;
            posxy = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
            RawImage blob0 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob0.enabled = false;
            blob0.name = sqLine.name + "_sequentiallines_";
            blob0.color = color0;
            blob0.transform.SetParent(nCanvas.transform, false);
            blob0.transform.SetAsFirstSibling();
            blob0.transform.position = posxy + new Vector3(Screen.width / 6 * 0, 0, 0);

            if(!disableText && txtAdd != null)
            {
                LeanTween.move(textMesh.gameObject, new Vector3(0, Screen.height, 0), 3);
                LeanTween.scale(textMesh.rectTransform, new Vector3(-1, 1f, 1f), 0.3f).setEaseInOutQuad().setLoopPingPong(-1);
                LeanTween.value(textMesh.gameObject, 1f, 0f, 2f).setOnUpdate((float val) =>
                        {
                            TextMeshProUGUI sr = textMesh.gameObject.GetComponent<TextMeshProUGUI>();
                            Color newColor = sr.color;
                            newColor.a = val;
                            sr.color = newColor;
                        });
            }
            //1st sequence of 6
            LeanTween.scale(blob0.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setOnUpdate((float val) =>
                {
                    blob0.enabled = true;
                });
            //2nd sequence of 6
            RawImage blob1 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob1.enabled = false;
            blob1.color = color1;
            blob1.name = sqLine.name + "_sequentiallines_";
            blob1.transform.SetParent(nCanvas.transform, false);
            blob1.transform.SetAsFirstSibling();
            blob1.transform.position = posxy + new Vector3(Screen.width / 6 * 1, 0, 0);
            LeanTween.scale(blob1.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.1f).setOnUpdate((float val) =>
                {
                    blob1.enabled = true;
                });
            //2nd sequence of 6
            RawImage blob2 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob2.enabled = false;
            blob2.color = color2;
            blob2.name = sqLine.name + "_sequentiallines_";
            blob2.transform.SetParent(nCanvas.transform, false);
            blob2.transform.SetAsFirstSibling();
            blob2.transform.position = posxy + new Vector3(Screen.width / 6 * 2, 0, 0);
            LeanTween.scale(blob2.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.2f).setOnUpdate((float val) =>
                {
                    blob2.enabled = true;
                });
            //3rd sequence of 6
            RawImage blob3 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob3.enabled = false;
            blob3.color = color3;
            blob3.name = sqLine.name + "_sequentiallines_";
            blob3.transform.SetParent(nCanvas.transform, false);
            blob3.transform.SetAsFirstSibling();
            blob3.transform.position = posxy + new Vector3(Screen.width / 6 * 3, 0, 0);
            LeanTween.scale(blob3.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.3f).setOnUpdate((float val) =>
                {
                    blob3.enabled = true;
                });
            //4th sequence of 6
            RawImage blob4 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob4.enabled = false;
            blob4.color = color4;
            blob4.name = sqLine.name + "_sequentiallines_";
            blob4.transform.SetParent(nCanvas.transform, false);
            blob4.transform.SetAsFirstSibling();
            blob4.transform.position = posxy + new Vector3(Screen.width / 6 * 4, 0, 0);
            LeanTween.scale(blob4.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.4f).setOnUpdate((float val) =>
                {
                    blob4.enabled = true;
                });
            //5th sequence of 6
            RawImage blob5 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob5.enabled = false;
            blob5.color = color5;
            blob5.name = sqLine.name + "_sequentiallines_";
            blob5.transform.SetParent(nCanvas.transform, false);
            blob5.transform.SetAsFirstSibling();
            blob5.transform.position = posxy + new Vector3(Screen.width / 6 * 5, 0, 0);
            LeanTween.scale(blob5.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.5f).setOnUpdate((float val) =>
                {
                    blob5.enabled = true;
                });
            //6th sequence of 6
            RawImage blob6 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob6.enabled = false;
            blob6.color = color6;
            blob6.name = sqLine.name + "_sequentiallines_";
            blob6.transform.SetParent(nCanvas.transform, false);
            blob6.transform.SetAsFirstSibling();
            blob6.transform.position = posxy + new Vector3(Screen.width / 6 * 6, 0, 0);
            LeanTween.scale(blob6.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.6f).setOnComplete(
                () =>
                {
                    stillTweening = true;
                    }).setOnUpdate((float val) =>
                {
                    blob6.enabled = true;
                });                 
            //Make sure all tweens are done before doing the next sequence of tweens
            while (!stillTweening)
            {
                yield return null;
            }
            if (waitUntilFinished)
            {
                Continue();
            }
            if(_targetLabel != null)
            {
                jumpLbl();
            }
            LeanTween.scale(blob0.rectTransform, new Vector3(1f, 1f, 1f), 0.2f).setEaseInOutQuad().setOnComplete(
            () =>
            {
                blob0.enabled = false;
                GameObject.Destroy(blob0);
            });
            LeanTween.scale(blob1.rectTransform, new Vector3(1f, 1f, 1f), 0.2f).setEaseInOutQuad().setDelay(0.1f).setOnComplete(
            () =>
            {
                blob1.enabled = false;
                GameObject.Destroy(blob1);
            });
            LeanTween.scale(blob2.rectTransform, new Vector3(1f, 1f, 1f), 0.2f).setEaseInOutQuad().setDelay(0.2f).setOnComplete(
            () =>
            {
                blob2.enabled = false;
                GameObject.Destroy(blob2);
            });
            LeanTween.scale(blob3.rectTransform, new Vector3(1f, 1f, 1f), 0.2f).setEaseInOutQuad().setDelay(0.3f).setOnComplete(
            () =>
            {
                blob3.enabled = false;
                GameObject.Destroy(blob3);
            });
            LeanTween.scale(blob4.rectTransform, new Vector3(1f, 1f, 1f), 0.2f).setEaseInOutQuad().setDelay(0.4f).setOnComplete(
            () =>
            {
                blob4.enabled = false;
                GameObject.Destroy(blob4);
            });
            LeanTween.scale(blob5.rectTransform, new Vector3(1f, 1f, 1f), 0.2f).setEaseInOutQuad().setDelay(0.5f).setOnComplete(
            () =>
            {
                blob5.enabled = false;
                GameObject.Destroy(blob5);
            });
            LeanTween.scale(blob6.rectTransform, new Vector3(1f, 1f, 1f), 0.2f).setEaseInOutQuad().setDelay(0.6f).setOnComplete(
            () =>
            {
                blob6.enabled = false;
                stillTweening = false;
                GameObject.Destroy(blob6);
                if(!disableText && txtAdd != null)
                {
                    LeanTween.cancel(textMesh.gameObject, true);
                    GameObject.Destroy(textMesh);
                }
            });
            //Make sure all tweens are done before destroying the Canvas
            while (stillTweening)
            {
                yield return null;
            }
            //Wait for the next frame and destroy canvas
            yield return null;
            if(UpperMenu != null)
            {    
                UpperMenu.SetActive(true);
            }
            GameObject.Destroy(nCanvas.gameObject);
        }
        protected IEnumerator VerticalSqueeze(float seqMove)
        {
            createObjects();
            sqLine.enabled = false;
            posxy = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
            //1st sequence of 6
            RawImage blob0 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob0.enabled = false;
            blob0.color = color0;
            blob0.name = sqLine.name + "_sequentiallines_";
            blob0.transform.SetParent(nCanvas.transform, false);
            blob0.transform.SetAsFirstSibling();
            blob0.transform.position = posxy + new Vector3(Screen.width / 6 * 0, 0, 0);
            //Moving text
            if(!disableText && txtAdd != null)
            {
                LeanTween.move(textMesh.gameObject, new Vector3(Screen.width / 6 * 1, Screen.height, 0), 3);
                LeanTween.value(textMesh.gameObject, 1f, 0f, 2f).setOnUpdate((float val) =>
                        {
                            TextMeshProUGUI sr = textMesh.gameObject.GetComponent<TextMeshProUGUI>();
                            Color newColor = sr.color;
                            newColor.a = val;
                            sr.color = newColor;
                        });
            }
            LeanTween.scale(blob0.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setOnUpdate((float val) =>
                {
                    blob0.enabled = true;
                });
            //2nd sequence of 6
            RawImage blob1 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob1.enabled = false;
            blob1.color = color1;
            blob1.name = sqLine.name + "_sequentiallines_";
            blob1.transform.SetParent(nCanvas.transform, false);
            blob1.transform.SetAsFirstSibling();
            blob1.transform.position = posxy + new Vector3(Screen.width / 6 * 1, 0, 0);
            LeanTween.scale(blob1.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.1f).setOnUpdate((float val) =>
                {
                    blob1.enabled = true;
                });
            //2nd sequence of 6
            RawImage blob2 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob2.enabled = false;
            blob2.color = color2;
            blob2.name = sqLine.name + "_sequentiallines_";
            blob2.transform.SetParent(nCanvas.transform, false);
            blob2.transform.SetAsFirstSibling();
            blob2.transform.position = posxy + new Vector3(Screen.width / 6 * 2, 0, 0);
            LeanTween.scale(blob2.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.2f).setOnUpdate((float val) =>
                {
                    blob2.enabled = true;
                });
            //3rd sequence of 6
            RawImage blob3 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob3.enabled = false;
            blob3.color = color3;
            blob3.name = sqLine.name + "_sequentiallines_";
            blob3.transform.SetParent(nCanvas.transform, false);
            blob3.transform.SetAsFirstSibling();
            blob3.transform.position = posxy + new Vector3(Screen.width / 6 * 3, 0, 0);
            LeanTween.scale(blob3.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.3f).setOnUpdate((float val) =>
                {
                    blob3.enabled = true;
                });
            //4th sequence of 6
            RawImage blob4 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob4.enabled = false;
            blob4.color = color4;
            blob4.name = sqLine.name + "_sequentiallines_";
            blob4.transform.SetParent(nCanvas.transform, false);
            blob4.transform.SetAsFirstSibling();
            blob4.transform.position = posxy + new Vector3(Screen.width / 6 * 4, 0, 0);
            LeanTween.scale(blob4.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.4f).setOnUpdate((float val) =>
                {
                    blob4.enabled = true;
                });
            //5th sequence of 6
            RawImage blob5 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob5.enabled = false;
            blob5.color = color5;
            blob5.name = sqLine.name + "_sequentiallines_";
            blob5.transform.SetParent(nCanvas.transform, false);
            blob5.transform.SetAsFirstSibling();
            blob5.transform.position = posxy + new Vector3(Screen.width / 6 * 5, 0, 0);
            LeanTween.scale(blob5.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.5f).setOnUpdate((float val) =>
                {
                    blob5.enabled = true;
                });
            //5th sequence of 6
            RawImage blob6 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob6.enabled = false;
            blob6.color = color6;
            blob6.name = sqLine.name + "_sequentiallines_";
            blob6.transform.SetParent(nCanvas.transform, false);
            blob6.transform.SetAsFirstSibling();
            blob6.transform.position = posxy + new Vector3(Screen.width / 6 * 6, 0, 0);
            LeanTween.scale(blob6.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.6f).setOnComplete(
                () =>
                {
                    stillTweening = true;
                }).setOnUpdate((float val) =>
                {
                    blob6.enabled = true;
                });
            //Make sure all tweens are done before doing the next sequence of tweens
            while (!stillTweening)
            {
                yield return null;
            }

            if (waitUntilFinished)
            {
                Continue();
            }
            LeanTween.scale(blob0.rectTransform, new Vector3(-1f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setOnComplete(
                            () =>
                            {
                                blob0.enabled = false;
                                GameObject.Destroy(blob0);
                            });
            LeanTween.scale(blob1.rectTransform, new Vector3(-1f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setDelay(0.1f).setOnComplete(
                            () =>
                            {
                                blob1.enabled = false;
                                GameObject.Destroy(blob1);
                            });
            LeanTween.scale(blob2.rectTransform, new Vector3(-1f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setDelay(0.2f).setOnComplete(
                            () =>
                            {
                                blob2.enabled = false;
                                GameObject.Destroy(blob2);
                            });
            LeanTween.scale(blob3.rectTransform, new Vector3(-1f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setDelay(0.3f).setOnComplete(
                            () =>
                            {
                                blob3.enabled = false;
                                GameObject.Destroy(blob3);
                            });
            LeanTween.scale(blob4.rectTransform, new Vector3(-1f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setDelay(0.4f).setOnComplete(
                            () =>
                            {
                                blob4.enabled = false;
                                GameObject.Destroy(blob4);
                            });
            LeanTween.scale(blob5.rectTransform, new Vector3(-1f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setDelay(0.5f).setOnComplete(
                            () =>
                            {
                                blob5.enabled = false;
                                GameObject.Destroy(blob5);
                            });
            LeanTween.scale(blob6.rectTransform, new Vector3(-1f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setDelay(0.6f).setOnComplete(
                            () =>
                            {
                                blob6.enabled = false;
                                stillTweening = false;
                                
                                GameObject.Destroy(blob6);
                                if(!disableText && txtAdd != null)
                                {
                                    LeanTween.cancel(textMesh.gameObject, true);
                                    GameObject.Destroy(textMesh);
                                }
                            });
            //Make sure all tweens are done before destroying the Canvas
            while (stillTweening)
            {
                yield return null;
            }
            //Wait for the next frame and destroy canvas
            yield return null;
            if(UpperMenu != null)
            {    
                UpperMenu.SetActive(true);
            }
            GameObject.Destroy(nCanvas.gameObject);
        }
        protected IEnumerator VerticalSqueeze2(float seqMove)
        {
            createObjects();
            sqLine.enabled = false;
            posxy = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
            //1st sequence of 6
            RawImage blob0 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob0.enabled = false;
            blob0.color = color0;
            blob0.name = sqLine.name + "_sequentiallines_";
            blob0.transform.SetParent(nCanvas.transform, false);
            blob0.transform.SetAsFirstSibling();
            blob0.transform.position = posxy + new Vector3(Screen.width / 6 * 0, 0, 0);
            if(!disableText && txtAdd != null)
            {
                LeanTween.move(textMesh.gameObject, new Vector3(Screen.width / 6 * 1, Screen.height, 0), 3);
                LeanTween.value(textMesh.gameObject, 1f, 0f, 2f).setOnUpdate((float val) =>
                    {
                        TextMeshProUGUI sr = textMesh.gameObject.GetComponent<TextMeshProUGUI>();
                        Color newColor = sr.color;
                        newColor.a = val;
                        sr.color = newColor;
                    });
            }
            LeanTween.scale(blob0.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setOnUpdate((float val) =>
                {
                    blob0.enabled = true;
                });
            //2nd sequence of 6
            RawImage blob1 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob1.enabled = false;
            blob1.color = color1;
            blob1.name = sqLine.name + "_sequentiallines_";
            blob1.transform.SetParent(nCanvas.transform, false);
            blob1.transform.SetAsFirstSibling();
            blob1.transform.position = posxy + new Vector3(Screen.width / 6 * 1, 0, 0);
            LeanTween.scale(blob1.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.1f).setOnUpdate((float val) =>
                {
                    blob1.enabled = true;
                });
            //2nd sequence of 6
            RawImage blob2 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob2.enabled = false;
            blob2.color = color2;
            blob2.name = sqLine.name + "_sequentiallines_";
            blob2.transform.SetParent(nCanvas.transform, false);
            blob2.transform.SetAsFirstSibling();
            blob2.transform.position = posxy + new Vector3(Screen.width / 6 * 2, 0, 0);
            LeanTween.scale(blob2.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.2f).setOnUpdate((float val) =>
                {
                    blob2.enabled = true;
                });
            //3rd sequence of 6
            RawImage blob3 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob3.enabled = false;
            blob3.color = color3;
            blob3.name = sqLine.name + "_sequentiallines_";
            blob3.transform.SetParent(nCanvas.transform, false);
            blob3.transform.SetAsFirstSibling();
            blob3.transform.position = posxy + new Vector3(Screen.width / 6 * 3, 0, 0);
            LeanTween.scale(blob3.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.3f).setOnUpdate((float val) =>
                {
                    blob3.enabled = true;
                });
            //4th sequence of 6
            RawImage blob4 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob4.enabled = false;
            blob4.color = color4;
            blob4.name = sqLine.name + "_sequentiallines_";
            blob4.transform.SetParent(nCanvas.transform, false);
            blob4.transform.SetAsFirstSibling();
            blob4.transform.position = posxy + new Vector3(Screen.width / 6 * 4, 0, 0);
            LeanTween.scale(blob4.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.4f).setOnUpdate((float val) =>
                {
                    blob4.enabled = true;
                });
            //5th sequence of 6
            RawImage blob5 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob5.enabled = false;
            blob5.color = color5;
            blob5.name = sqLine.name + "_sequentiallines_";
            blob5.transform.SetParent(nCanvas.transform, false);
            blob5.transform.SetAsFirstSibling();
            blob5.transform.position = posxy + new Vector3(Screen.width / 6 * 5, 0, 0);
            LeanTween.scale(blob5.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.5f).setOnUpdate((float val) =>
                {
                    blob5.enabled = true;
                });
            //5th sequence of 6
            RawImage blob6 = Instantiate(sqLine, transform.position, Quaternion.identity) as RawImage;
            blob6.enabled = false;
            blob6.color = color6;
            blob6.name = sqLine.name + "_sequentiallines_";
            blob6.transform.SetParent(nCanvas.transform, false);
            blob6.transform.SetAsFirstSibling();
            blob6.transform.position = posxy + new Vector3(Screen.width / 6 * 6, 0, 0);
            LeanTween.scale(blob6.rectTransform, new Vector3(1f, Screen.height, 1f), 0.3f).setEaseInOutQuad().setDelay(0.6f).setOnComplete(
                () =>
                {
                    stillTweening = true;
                    }).setOnUpdate((float val) =>
                {
                    blob6.enabled = true;
                });
            //Make sure all tweens are done before doing the next sequence of tweens
            while (!stillTweening)
            {
                yield return null;
            }
            if (waitUntilFinished)
            {
                Continue();
            }
            LeanTween.scale(blob0.rectTransform, new Vector3(0f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    blob0.enabled = false;
                    GameObject.Destroy(blob0);
                });
            LeanTween.scale(blob1.rectTransform, new Vector3(0f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    blob1.enabled = false;
                    GameObject.Destroy(blob1);
                });
            LeanTween.scale(blob2.rectTransform, new Vector3(0f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    blob2.enabled = false;
                    GameObject.Destroy(blob2);
                });
            LeanTween.scale(blob3.rectTransform, new Vector3(0f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    blob3.enabled = false;
                    GameObject.Destroy(blob3);
                });
            LeanTween.scale(blob4.rectTransform, new Vector3(0f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    blob4.enabled = false;
                    GameObject.Destroy(blob4);
                });
            LeanTween.scale(blob5.rectTransform, new Vector3(0f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    blob5.enabled = false;
                    GameObject.Destroy(blob5);
                });
            LeanTween.scale(blob6.rectTransform, new Vector3(0f, Screen.height, 1f), 0.2f).setEaseInOutQuad().setOnComplete(
                () =>
                {
                    blob6.enabled = false;
                    stillTweening = false;                    
                    GameObject.Destroy(blob6);
                    if(!disableText && txtAdd != null)
                    {
                        LeanTween.cancel(textMesh.gameObject, true);
                        GameObject.Destroy(textMesh);
                    }
                });
            //Make sure all tweens are done before destroying the Canvas
            while (stillTweening)
            {
                yield return null;
            }
            //Wait for the next frame and destroy canvas
            yield return null;
            if(UpperMenu != null)
            {    
                UpperMenu.SetActive(true);
            }
            GameObject.Destroy(nCanvas.gameObject);
        }
        protected void jumpLbl()
        {
            if (_targetLabel.Value == "")
            {
                Continue();
                return;
            }
            var commandList = ParentBlock.CommandList;
            for (int i = 0; i < commandList.Count; i++)
            {
                var command = commandList[i];
                Label label = command as Label;
                if (label != null && label.Key == _targetLabel.Value)
                {
                    Continue(label.CommandIndex + 1);
                    return;
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

            switch (splashSelect)
            {
                case (transitionSplash.Disable):
                    Continue();
                    break;
                case (transitionSplash.VerticalLoop):
                    sequenceMove();
                    break;
                case (transitionSplash.VerticalSqueeze):
                    squeezeLines();
                    break;
                case (transitionSplash.VerticalSqueeze2):
                    squeezeLines2();
                    break;
            }
            if (!waitUntilFinished)
            {
                Continue();
            }
        }
        public override bool HasReference(Variable variable)
        {
            return _targetLabel.stringRef == variable ||
                base.HasReference(variable);
        }

        #region Backwards compatibility
        [HideInInspector] [FormerlySerializedAs("targetLabel")] public Label targetLabelOLD;

        protected virtual void OnEnable()
        {
            if (targetLabelOLD != null)
            {
                _targetLabel.Value = targetLabelOLD.Key;
                targetLabelOLD = null;
            }
        }
        #endregion
        public override void OnCommandAdded(Block parentBlock)
        {
            splashSelect = transitionSplash.Disable;
        }
        #endregion
    }
}
