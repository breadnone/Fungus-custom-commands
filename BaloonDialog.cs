// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
namespace Fungus
{
    public enum transiType2
    {
        /// <summary> Fade In effect sequence. </summary>
        FadeIn,
        /// <summary> Punch effect sequence. </summary>
        Punch,
        /// <summary> Stop All </summary>
        None
    }

    /// <summary>
    /// Pauses the game/by setting timescale to 0.
    /// </summary>
    [CommandInfo("Animation",
                 "Baloon Dialog",
                 "Sequence of baloon dialogs. Text & Image components MUST be placed in the same parent canvas. Preferably a freshly created canvas. To stop the animation, you must copy the exact settings and it's effect type set to NONE")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class BaloonDialog : Command
    {
        [Tooltip("Wait until the tween has finished before executing the next command")]        
        public transiType2 effectType;
        [Tooltip("GameObject/Bitmaps")]
        [SerializeField] public GameObject balloonSprite;
        [SerializeField] public float blnPositionX = 0f;
        [SerializeField] public float blnPositionY = 0f;
        [Space(4)]
        [Tooltip("Parent Canvas")]
        [SerializeField] public GameObject canvas;
        [Space(4)]
        [Tooltip("Text")]
        [SerializeField] public GameObject textComponent;
        [SerializeField] protected bool rotateAround = false;
        [SerializeField] public float txtX = 0f;
        [SerializeField] public float txtY = 0f;
        [Space(4)]

        [Tooltip("Small bubble dialog Scale")]
        [SerializeField] public float sDialogWidth = 0.3f;
        [Tooltip("Small bubble dialog Scale")]
        [SerializeField] public float sDialogHeight = 0.3f;
        [Tooltip("Medium/middle bubble dialog X position")]
        [SerializeField] public float mDialogX = 35f;
        [Tooltip("Medium/middle bubble dialog Y position")]
        [SerializeField] public float mDialogY = 35f;
        [Tooltip("Medium/middle bubble dialog Scale")]
        [SerializeField] public float mDialogWidth = 0.5f;
        [Tooltip("Medium/middle bubble dialog Scale")]
        [SerializeField] public float mDialogHeight = 0.5f;
        [Tooltip("Big/Main bubble dialog X position")]
        [SerializeField] public float bDialogX = 120f;
        [Tooltip("Big/Main bubble dialog Y position")]
        [SerializeField] public float bDialogY = 120f;
        [Tooltip("Big/Main bubble dialog Scale")]
        [SerializeField] public float bDialogWidth = 2.0f;
        [Tooltip("Big/Main bubble dialog Scale")]
        [SerializeField] public float bDialogHeight = 2.0f;
        [Space(4)]
        [Tooltip("Stop at index, based on how many times it gets triggered e.g button presses/clicks etc")]
        [SerializeField] public int stopAtIndex = 0;
        [Tooltip("if you have a custom button to trigger the progression of the story add it here so the stop at index could work")]
        [SerializeField] protected KeyCode optionalButtonTrigger;
        [SerializeField] protected bool waitUntilFinished = false;
        private static int indexNum = 0;
        private bool isRunning = false;
        private Vector3 posxy;

        private IEnumerator coroutine;
        protected virtual void OnTweenComplete()
        {
            Continue();
        }
        protected void FadeIns()
        {
            coroutine = SlowBaloon(0.3f);
            StartCoroutine(coroutine);
        }
        protected void Punchy()
        {
            coroutine = PunchBaloon(0.3f);
            StartCoroutine(coroutine);
        }
        protected void saveWait()
        {
            coroutine = StopAnim(0.2f);
            StartCoroutine(coroutine);
        }

        void Update()
        {
            if(isRunning)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(optionalButtonTrigger))
                {                
                    indexNum++;
                    //Debug.Log("Left click." + indexNum);

                    if(indexNum == stopAtIndex)
                    {
                        saveWait();
                    }
                }
            }
        }

        //Slow appearing baloon
        protected IEnumerator SlowBaloon(float waitSlowBaloon)
        {
            if (canvas != null && canvas.activeInHierarchy == false)
            {
                canvas.SetActive(true);
            }

            if (balloonSprite != null)
            {
                posxy = new Vector3(blnPositionX, blnPositionY, 1);
                var spwn = posxy;
                GameObject blob0 = Instantiate(balloonSprite, spwn, Quaternion.identity) as GameObject;
                blob0.SetActive(false);
                blob0.name = balloonSprite.name + "_baloonpop";
                blob0.transform.SetParent(canvas.transform, false);
                blob0.transform.SetAsFirstSibling();
                blob0.transform.localScale = new Vector3(sDialogWidth, sDialogHeight, 0f);
                LeanTween.value(blob0.gameObject, 0f, 1f, 0.3f).setOnUpdate((float val) =>
                {
                    Image sr = blob0.GetComponent<Image>();
                    Color newColor = sr.color;
                    newColor.a = val;
                    sr.color = newColor;
                    blob0.SetActive(true);
                });

                yield return new WaitForSeconds(0.3f);

                GameObject blob1 = Instantiate(balloonSprite, spwn + new Vector3(mDialogX, mDialogY, 0f), Quaternion.identity) as GameObject;
                blob1.SetActive(false);
                blob1.name = balloonSprite.name + "_baloonpop";
                blob1.transform.SetParent(canvas.transform, false);
                blob1.transform.SetAsFirstSibling();
                blob1.transform.localScale = new Vector3(mDialogWidth, mDialogHeight, 0f);
                LeanTween.value(blob1.gameObject, 0f, 1f, 1.3f).setOnUpdate((float val) =>
                {
                    Image sr = blob1.GetComponent<Image>();
                    Color newColor = sr.color;
                    newColor.a = val;
                    sr.color = newColor;
                    blob1.SetActive(true);
                });

                yield return new WaitForSeconds(0.3f);

                GameObject blob2 = Instantiate(balloonSprite, spwn + new Vector3(bDialogX, bDialogY, 0), Quaternion.identity) as GameObject;
                blob2.SetActive(false);
                blob2.name = balloonSprite.name + "_baloonpop";
                blob2.transform.SetParent(canvas.transform, false);
                blob2.transform.SetAsFirstSibling();
                blob2.transform.localScale = new Vector3(bDialogWidth, bDialogHeight, 0f);
                LeanTween.value(blob2.gameObject, 0f, 1f, 1.3f).setOnUpdate((float val) =>
                {
                    Image sr = blob2.GetComponent<Image>();
                    Color newColor = sr.color;
                    newColor.a = val;
                    sr.color = newColor;
                    blob2.SetActive(true);
                });
              
                    textComponent.SetActive(false);                
                    textComponent.transform.localPosition = blob2.transform.localPosition + new Vector3(txtX, txtY, 0);
                    textComponent.transform.SetAsLastSibling();
                    LeanTween.value(textComponent.gameObject, 0f, 1f, 1f).setOnUpdate((float val) =>
                    {
                        Text sr = textComponent.gameObject.GetComponent<Text>();
                        Color newColor = sr.color;
                        newColor.a = val;
                        sr.color = newColor;
                        textComponent.SetActive(true);
                    });

                    if(rotateAround)
                    {
                        LeanTween.rotateAroundLocal(textComponent.gameObject, Vector3.forward, 30f, 0.3f).setLoopPingPong(-1);
                    }

            }

            isRunning = true;

            if (waitUntilFinished)
            {
                Continue();
            }
        }

        //Punch baloon dialog sequence
        protected IEnumerator PunchBaloon(float waitPunch)
        {
            if (balloonSprite != null)
            {
                posxy = new Vector3(blnPositionX, blnPositionY, 1);
                var spwn = posxy;
                GameObject blob0 = Instantiate(balloonSprite, spwn, Quaternion.identity) as GameObject;                
                blob0.name = balloonSprite.name + "_baloonpop";
                blob0.transform.SetParent(canvas.transform, false);
                blob0.transform.SetAsFirstSibling();
                blob0.transform.localScale = new Vector3(sDialogWidth, sDialogHeight, 0f);
                blob0.SetActive(true);
                LeanTween.scale(blob0.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.punch);

                yield return new WaitForSeconds(0.3f);

                GameObject blob1 = Instantiate(balloonSprite, spwn + new Vector3(mDialogX, mDialogY, 0f), Quaternion.identity) as GameObject;
                blob1.name = balloonSprite.name + "_baloonpop";
                blob1.transform.SetParent(canvas.transform, false);
                blob1.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
                blob1.SetActive(true);
                LeanTween.scale(blob1.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.punch);

                yield return new WaitForSeconds(0.3f);

                GameObject blob2 = Instantiate(balloonSprite, spwn + new Vector3(bDialogX, bDialogY, 0), Quaternion.identity) as GameObject;
                blob2.name = balloonSprite.name + "_baloonpop";
                blob2.transform.SetParent(canvas.transform, false);
                blob2.transform.localScale = new Vector3(2f, 2f, 0f);
                blob2.SetActive(true);
                LeanTween.scale(blob2.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.punch);

                textComponent.transform.localPosition = blob2.transform.localPosition + new Vector3(txtX, txtY, 0);
                textComponent.SetActive(true);
                textComponent.transform.SetAsLastSibling();

                if(rotateAround)
                {
                    LeanTween.rotateAroundLocal(textComponent.transform.gameObject, Vector3.forward, 30f, 0.3f).setLoopPingPong(-1);
                }
                else
                {
                    LeanTween.scale(textComponent.gameObject, Vector3.zero, 0.8f).setEase(LeanTweenType.punch).setLoopClamp();
                }
            }

            isRunning = true;

            if (waitUntilFinished)
            {
                Continue();
            }
        }
        //Destroy all instantiated objects
        protected IEnumerator StopAnim(float searchNdestroy)
        {
            var dObjects = Resources.FindObjectsOfTypeAll<Image>().Where(obj => obj.name.Contains("_baloonpop"));
            foreach (Image clone in dObjects)
            {
                if (clone != null)
                {
                    LeanTween.scale(clone.gameObject, new Vector3(-1f, 0f, 0f), 0.2f).setDestroyOnComplete(true);
                }
            }

            if (balloonSprite != null)
            {
                LeanTween.cancel(balloonSprite.transform.gameObject, true);
                balloonSprite.SetActive(false);
            }

            yield return new WaitForSeconds(searchNdestroy);

            if (textComponent != null)
            {
                LeanTween.cancel(textComponent.transform.gameObject, true);
                textComponent.transform.position = textComponent.transform.localPosition;
                textComponent.SetActive(false);
            }

            isRunning = false;
            indexNum = 0;

            StopAllCoroutines();

            if (waitUntilFinished)
            {
                Continue();
            }
        }
        protected virtual void OnComplete()
        {
            Continue();
        }
        #region Public members
        public override string GetSummary()
        {
            string spawnSummary = "";
            string textSummary = "";

            if (effectType != transiType2.None)
            {
                if (balloonSprite == null)
                {
                    return "Error: Image can't be empty";
                }

                if (canvas == null)
                {
                    return "Error: Canvas can't be empty";
                }
            }
            return spawnSummary + textSummary;
        }
        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }
        public override void OnEnter()
        {
            Canvas.ForceUpdateCanvases();

            if (effectType == transiType2.None)
            {
                saveWait();
            }

            switch (effectType)
            {
                case (transiType2.FadeIn):
                    FadeIns();
                    break;
                case (transiType2.Punch):
                    Punchy();
                    break;
                case (transiType2.None):
                    break;
            }

            if (!waitUntilFinished)
            {
                Continue();
            }
        }
        public override void OnCommandAdded(Block parentBlock)
        {
            effectType = transiType2.None;
        }
        #endregion
    }
}