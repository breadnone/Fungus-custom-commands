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
        [Tooltip("Enable")]
        [SerializeField] public threeFramu splashSelect;
        [Tooltip("Images")]
        [SerializeField] public GameObject[] imgSrc;
        [Tooltip("Delay in float")]
        [SerializeField] public float delay = 0.2f;
        [HideInInspector] protected bool stillTweening = false;
        private IEnumerator coroutine;    
        protected void sequenceMove()
        {
            coroutine = SequenceOfLines(0.2f);
            StartCoroutine(coroutine);
        }

        private static int sibIndex = 0;
        private static int avobj = 0;

        public IEnumerator SequenceOfLines(float seqMove)
        {
            foreach(GameObject bobo in imgSrc)
            {
                if(bobo != null)
                {
                    avobj++;
                }
            }
            if(imgSrc.Length > 0 && stillTweening == false && avobj == imgSrc.Length)
            {
                for(int j=0; j < imgSrc.Length; j++)
                {    
                    if(imgSrc[j].activeInHierarchy == false)
                    {
                        imgSrc[j].SetActive(true);
                    }
                }
                stillTweening = true;
                Continue();
                while(stillTweening)
                {
                    foreach(GameObject gg in imgSrc)
                    {
                        sibIndex++;
                        var hh = gg.transform.GetSiblingIndex();
                        gg.transform.SetSiblingIndex(hh+sibIndex);                                 
                        yield return new WaitForSeconds(delay);                      
                    }                    
                }
            }
            else
            {
                Continue();
            }
        }
        protected void inStates()
        {
            if (stillTweening == true)
            {
                for(int i=0; i < imgSrc.Length; i++)
                {
                    var imagecol = imgSrc[i];
                    if(imagecol.activeInHierarchy == true)
                    {
                        imagecol.SetActive(false);
                    }
                }
            }
        }
        #region Public members
        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }
        public virtual void bbbb(bool acstate)
        {
            this.stillTweening = false;
            this.inStates();
            this.StopCoroutine(coroutine);
        }
        public override void OnEnter()
        {
            Canvas.ForceUpdateCanvases();
            switch (splashSelect)
            {
                case (threeFramu.Disable):
                    ThreeFramer threefrm = GetComponent<ThreeFramer>();
                    threefrm.bbbb(false);
                    Continue();
                    break;
                case (threeFramu.Enable):
                    sequenceMove();
                    break;
            }
        }
        public override void OnCommandAdded(Block parentBlock)
        {
            splashSelect = threeFramu.Disable;
        }
        #endregion
    }
}
