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
        [Tooltip("Disable & Set Active to False")]
        [SerializeField] public bool disableAndHide = true;
        [SerializeField] protected bool stillTweening = false;
        private IEnumerator coroutine;    
        protected void sequenceMove()
        {
            coroutine = SequenceOfLines(0.2f);
            StartCoroutine(coroutine);
        }

        private static int sibIndex = 0;
        private static int avobj = 0;

        //uncomment this for sprite renderer
        //float zAxs = 0;

        public virtual IEnumerator SequenceOfLines(float seqMove)
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
                while(true)
                {
                    sibIndex++;
                    if(stillTweening)
                    {
                        foreach(GameObject gg in imgSrc)
                        {
                            var hh = gg.transform.GetSiblingIndex();
                            //uncomment this for sprite renderer
                            //gg.transform.localPosition = new Vector3(1f, 1f, zAxs++);
                            gg.transform.SetSiblingIndex(hh+sibIndex);                                 
                            yield return new WaitForSeconds(delay);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                Continue();
            }
        }
        public virtual void inStates()
        {
            if (stillTweening == true)
            {
                if(disableAndHide)
                {
                    for(int i=0; i < imgSrc.Length; i++)
                    {
                        var imagecol = imgSrc[i];
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
            avobj = 0;
            sibIndex = 0;
            this.inStates();
            this.StopAllCoroutines();
        }
        public override void OnEnter()
        {
            Canvas.ForceUpdateCanvases();
            switch (splashSelect)
            {
                case (threeFramu.Disable):
                    ThreeFramer threefrm = GetComponent<ThreeFramer>();
                    threefrm.bbbb(false);
                    //Debug.Log("Disabled");
                    Continue();
                    break;
                case (threeFramu.Enable):
                    //Debug.Log("Enabled");
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
