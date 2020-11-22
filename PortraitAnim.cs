// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public enum actPorAnim
{
    Enable,
    Disable
}

namespace Fungus
{
    /// <summary>
    /// Controls a character portrait.
    /// </summary>
    [CommandInfo("Narrative",
                 "PortraitAnim",
                 "Character frame-by-frame animation using portrait lists. Cycle = Stopping the animation based on how many loops. IMPORTANT! Do not use Dim or any other fancy settings in Stage")]
    public class PortraitAnim : Command
    {
        [HideInInspector] protected DisplayType display = DisplayType.Show;
        [SerializeField] public actPorAnim enableAnimation;
        [Tooltip("Stage to display portrait on")]
        [SerializeField] protected Stage stage;
        [Tooltip("Character to display")]
        [SerializeField] protected Character character;
        [Tooltip("Portrait to display")]
        [SerializeField] protected Sprite[] portrait1;
        [Tooltip("Delay between sequence")]
        [SerializeField] protected float frameDelay = 0.1f;
        [Tooltip("Reverse the loop after completion of the 1st cycle")]
        [SerializeField] protected bool reverseLoop = false;
        [Tooltip("Defines how many times it would be animated before get disabled automatically")]
        [SerializeField] protected bool useCyclesRange = false;
        [Tooltip("How many cycle animation before gets stopped automatically")]
        [SerializeField] protected int cycles = 2;
        [Tooltip("Random delay based on predefined values, 4 - 12 seconds")]
        [SerializeField] protected bool RandomEndDelay = false;
        [SerializeField] protected float endFrameDelay = 3f;

        #region Public members
        /// <summary>
        /// Stage to display portrait on.
        /// </summary>
        public virtual Stage _Stage { get { return stage; } set { stage = value; } }
        /// <summary>
        /// Character to display.
        /// </summary>
        public virtual Character _Character { get { return character; } set { character = value; } }
        /// <summary>
        /// Portrait to display.
        /// </summary>
        public virtual Sprite[] _Portrait1 { get { return portrait1; } set { portrait1 = value; } }
        private bool isAnimating = false;
        private IEnumerator coroutine;
        protected void sequenceMove()
        {
            coroutine = charAnim(0.2f);
            StartCoroutine(coroutine);
        }
        public override void OnEnter()
        {
            if (stage == null)
            {
                stage = Stage.GetActiveStage();
                if (stage == null)
                {
                    Continue();
                    return;
                }
            }
            if (enableAnimation == actPorAnim.Enable)
            {
                Stage disdim = Stage.GetActiveStage();
                disdim.GetComponent<Stage>().DimPortraits = false;

                for (int i = 0; i < portrait1.Length; i++)
                {
                    if (portrait1[i] != null)
                    {
                        sequenceMove();
                    }
                    else
                    {
                        Continue();
                        return;
                    }
                }
            }

            if (enableAnimation == actPorAnim.Disable)
            {
                PortraitAnim portan = GetComponent<PortraitAnim>();
                portan.disablePortraitAnim(false);
            }
        }
        public virtual void disablePortraitAnim(bool anstate)
        {
            this.isAnimating = false;
            this.StopAllCoroutines();
            Continue();
        }
        private static int amCycle = 0;
        private static int amCycleRes = 0;        
        public IEnumerator charAnim(float delay)
        {
            for (int i = 0; i < portrait1.Length; i++)
            {
                if (portrait1[i] != null)
                {
                    if (enableAnimation == actPorAnim.Enable && character != null)
                    {                        
                        isAnimating = true;
                        PortraitOptions options = new PortraitOptions();
                        options.character = character;
                        options.display = display;                        
                        while (isAnimating)
                        {                            
                            foreach (Sprite jav in portrait1)
                            {
                                options.portrait = jav;
                                stage.RunPortraitCommand(options, Continue);
                                yield return new WaitForSeconds(frameDelay);
                            }
                            
                            if(reverseLoop)
                            {
                                foreach (Sprite jav in portrait1.Reverse())
                                {
                                    options.portrait = jav;
                                    stage.RunPortraitCommand(options, Continue);
                                    yield return new WaitForSeconds(frameDelay);
                                }
                            }

                            amCycle++;
                            var bh = amCycle / portrait1.Length;
                            amCycleRes = bh;

                            //Debug.Log("amcycle:"+ amCycle + "amcycleres:" + amCycleRes);

                            if (RandomEndDelay)
                            {
                                yield return new WaitForSeconds(Random.Range(4f, 12f));
                            }
                            else
                            {
                                yield return new WaitForSeconds(endFrameDelay);
                            }
                            if (useCyclesRange)
                            {
                                if (amCycleRes == cycles)
                                {
                                    this.disablePortraitAnim(false);
                                }
                            }
                        }
                    }
                    else
                    {
                        Continue();
                    }
                }
                else
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
     
            string characterSummary = "";
            string portraitSummary = "";
            string stageSummary = "";

            characterSummary = character.name;

            for(int i = 0; i < portrait1.Length; i++)
            {
                if (enableAnimation == actPorAnim.Enable && character != null && portrait1[i] == null)
                {
                    return "Error: One of portrait slots cannot be empty";
                }
            }
            return characterSummary + ":" + portraitSummary + ":" +stageSummary;
        }
        public override Color GetButtonColor()
        {
            return new Color32(230, 200, 250, 255);
        }
        public override void OnCommandAdded(Block parentBlock)
        {
            display = DisplayType.Show;
            enableAnimation = actPorAnim.Disable;
        }
        #endregion
    }
}