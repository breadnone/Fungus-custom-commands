// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
        public static bool allDone = false;
        public static bool insStatesIsRunning = false;
        public static bool stillTweening = false;
        public static bool StillTweening { get { return stillTweening; } set { stillTweening = value; } }
        private static int sibIndex = 0;
        //Cache SiblingIndex
        protected static List<int> cacheIndex = new List<int>();

        protected IEnumerator GetSequence()
        {
            if (splashSelect == threeFramu.Enable && stillTweening == false)
            {
                for (int j = 0; j < imgSrc.Length; j++)
                {
                    //Cache SiblingIndex to List
                    var b = transform.GetSiblingIndex();
                    cacheIndex.Add(b);
                    //Debug.Log(cacheIndex[j].name);
                    if (imgSrc[j] != null)
                    {
                        stillTweening = true;

                        //Start on next frame after each iteration, just to be safe.
                        if (j % 1 == 0)
                        {
                            imgSrc[j].SetActive(true);
                            yield return null;
                        }
                        if(imgSrc[j].activeInHierarchy == true)
                        {
                            StartCoroutine(loopAnim());
                        }
                    }
                    else
                    {
                        stillTweening = false;
                        yield break;
                    }
                }
            }
        }

        protected IEnumerator loopAnim()
        {
            //stillTweening = true;
            while (allDone == false)
            {
                if (stillTweening == true)
                {
                    foreach (GameObject gg in imgSrc)
                    {
                        int hh = gg.transform.GetSiblingIndex();
                        yield return new WaitForSeconds(0.1f);
                        gg.transform.SetSiblingIndex(hh + sibIndex++);
                    }
                }
                else
                {
                    InStates();
                    yield break;
                }
            }
        }
        protected void InStates()
        {
            for (int i = 0; i < imgSrc.Length; i++)
            {
                imgSrc[i].SetActive(false);
                for(int j = 0; j < cacheIndex.Count; j++)
                {
                    //Make sure they are back to it's original order in the hierarchy
                    //This prevents wrong sequence if the same images used at a later time in the same scene
                    imgSrc[i].transform.SetSiblingIndex(cacheIndex[j]);
                }
            }
            StopAllCoroutines();
            cacheIndex = new List<int>();
        }
        #region Public members
        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }
        public static void GetThreeFramer(bool acstate)
        {
            sibIndex = 0;
            stillTweening = acstate;
            insStatesIsRunning = acstate;
        }

        public override void OnEnter()
        {
            Canvas.ForceUpdateCanvases();
            switch (splashSelect)
            {
                case (threeFramu.Disable):
                    GetThreeFramer(false);
                    break;
                case (threeFramu.Enable):
                    StartCoroutine(GetSequence());
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
