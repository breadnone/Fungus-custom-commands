// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
        public static bool allDone = false;
        public static bool stillTweening = false;
        [Tooltip("Ping pong/reverse loop style")]
        [SerializeField] protected bool pingPongLoop = true;
        private static int sibIndex = 0;
        //Cache SiblingIndex
        protected static List<int> cacheIndex = new List<int>();
        WaitForSeconds waiting = new WaitForSeconds(0.1f);
        protected void GetSequence()
        {
            if (splashSelect == threeFramu.Enable && stillTweening == false)
            {                
                for (int j = 0; j < imgSrc.Length; j++)
                {
                    //Cache SiblingIndex to List
                    cacheIndex.Add(imgSrc[j].transform.GetSiblingIndex());

                    if (imgSrc[j] != null)
                    {
                        stillTweening = true;
                        allDone = false;
                        imgSrc[j].SetActive(true);

                        if(imgSrc[j].activeInHierarchy == true)
                        {
                            StartCoroutine(loopAnim());
                        }
                    }
                    else
                    {
                        stillTweening = false;
                    }
                }
            }
        }

        protected IEnumerator loopAnim()
        {            
            while (!allDone)
            {
                if (stillTweening)
                {
                    for(int i = 0; i < imgSrc.Length; i++)
                    {
                        if (i % 1 == 0)
                        {
                            int hh = imgSrc[i].transform.GetSiblingIndex();
                            yield return waiting;
                            imgSrc[i].transform.SetSiblingIndex(hh + sibIndex++);
                        }
                    }
                    //Reverse it
                    if(pingPongLoop)
                    {
                        for (int i = imgSrc.Length - 1; i >= 0; i--)  
                        {
                            if (i % 1 == 0)
                            {
                                int hh = imgSrc[i].transform.GetSiblingIndex();
                                yield return waiting;
                                imgSrc[i].transform.SetSiblingIndex(hh + sibIndex++);
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < imgSrc.Length; i++)
            {
                imgSrc[i].SetActive(false);
                for(int j = 0; j < cacheIndex.Count; j++)
                {
                    //Make sure they are back to it's original order in the hierarchy
                    //This prevents wrong sequence if the same set of images used at a later time in the same scene
                    imgSrc[i].transform.SetSiblingIndex(cacheIndex[j]);
                }
            }

            InStates();
        }
        protected void InStates()
        {
            //Sanity check            
            cacheIndex = new List<int>();
            allDone = true;
            //Debug.Log("This was checked! 01");
        }

        #region Public members
        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }
        public void GetThreeFramer(bool acstate)
        {
            sibIndex = 0;
            stillTweening = acstate;
        }
        public IEnumerator lastWait()
        {
            while (true)
            {
                if(!allDone)
                {
                    yield return null;
                }
                else 
                {
                    StopAllCoroutines();
                    Continue();
                    yield break;
                }
            }
        } 

        public override void  OnEnter()
        {
            Canvas.ForceUpdateCanvases();
            switch (splashSelect)
            {
                case (threeFramu.Disable):
                    GetThreeFramer(false);
                    break;
                case (threeFramu.Enable):
                    GetSequence();
                    break;
            }

            if(splashSelect == threeFramu.Enable)
            {
                Continue();
            }
            if(splashSelect == threeFramu.Disable)
            {
                StartCoroutine(lastWait());
            }
        }

        public override void OnCommandAdded(Block parentBlock)
        {
            splashSelect = threeFramu.Disable;
        }
        #endregion
    }
}
