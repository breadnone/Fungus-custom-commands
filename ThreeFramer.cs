// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
namespace Fungus
{
    public enum threeFramu
    {
        Enable,
        Disable
    }
    /// <summary>
    /// Tween sequence
    /// </summary>
    [CommandInfo("Animation",
                 "Background Animation",
                 "Background frame-by-frame animation")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class ThreeFramer : Command
    {
        private static ThreeFramer instance;
        [Tooltip("Enable")]
        [SerializeField] public threeFramu splashSelect;
        [Tooltip("Images")]
        [SerializeField] public GameObject[] imgSrc = new GameObject[0];
        protected static GameObject[] imgSrcc;
        [Tooltip("Delay in float")]
        [SerializeField] public float delay = 0.1f;
        public static bool insStatesIsRunning = false;
        public static bool stillTweening = false;
        public static bool StillTweening { get { return stillTweening; } set { stillTweening = value; } }
        private static int sibIndex = 0;
        //uncomment this for sprite renderer
        //float zAxs = 0;
        public static ThreeFramer GetInstance()
        {
            if (instance == null)
            {
                instance = new ThreeFramer();
            }
            return instance;
        }
        void Awake()
        {
            instance = this;
        }
        protected IEnumerator GetSequence()
        {
            if (splashSelect == threeFramu.Enable && stillTweening == false)
            {
                for (int j = 0; j < imgSrc.Length; j++)
                {
                    if (imgSrc[j] != null)
                    {
                        stillTweening = true;
                        insStatesIsRunning = true;
                        imgSrc[j].SetActive(true);
                        yield return null;
                        ThreeFramer.GetInstance().StartCoroutine(loopAnim());
                    }
                    else
                    {
                        ThreeFramer.stillTweening = false;
                        ThreeFramer.insStatesIsRunning = false;
                        yield break;
                    }
                }
            }
        }

        protected IEnumerator loopAnim()
        {
            //stillTweening = true;
            while (true)
            {
                if (stillTweening == true)
                {
                    foreach (GameObject gg in imgSrc)
                    {
                        int hh = gg.transform.GetSiblingIndex();
                        yield return new WaitForSeconds(0.1f);
                        //uncomment this for sprite renderer
                        //gg.transform.localPosition = new Vector3(1f, 1f, zAxs++);                        
                        gg.transform.SetSiblingIndex(hh + sibIndex++);
                    }
                }
                else
                {
                    if(!insStatesIsRunning)
                    {
                        ThreeFramer.insStatesIsRunning = true;
                        InStates();
                        yield break;
                    }
                }
            }
        }
        public void InStates()
        {
            for (int i = 0; i < imgSrc.Length; i++)
            {
                if (imgSrc[i] != null && imgSrc[i].activeInHierarchy == true)
                {
                    imgSrc[i].SetActive(false);
                }
            }
            ThreeFramer.stillTweening = false;
            ThreeFramer.GetInstance().StopAllCoroutines();
        }
        #region Public members
        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }
        public static void GetThreeFramer(bool acstate)
        {
            sibIndex = 0;
            ThreeFramer.stillTweening = acstate;
            ThreeFramer.insStatesIsRunning = acstate;
        }

        public override void OnEnter()
        {
            //Canvas.ForceUpdateCanvases();
            switch (splashSelect)
            {
                case (threeFramu.Disable):
                    ThreeFramer.GetThreeFramer(false);
                    break;
                case (threeFramu.Enable):
                    //ThreeFramer.GetInstance().GetThreeFramer(true);
                    ThreeFramer.GetInstance().StartCoroutine(GetSequence());
                    break;
            }
            Continue();
        }

        public override void OnCommandAdded(Block parentBlock)
        {
            splashSelect = threeFramu.Disable;
        }
        #endregion
    }
}
