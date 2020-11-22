// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using System.Text;
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
                 "Character frame-by-frame animation using portrait lists. Cycle = Stopping the animation based on how many loops")]
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
            if (enableAnimation == actPorAnim.Enable)
            {
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
            if (stage == null)
            {
                stage = Stage.GetActiveStage();
                if (stage == null)
                {
                    Continue();
                    return;
                }
            }
            if (enableAnimation == actPorAnim.Disable)
            {
                PortraitAnim portan = GetComponent<PortraitAnim>();
                portan.disablePortraitAnim(false);
                Continue();
            }
        }
        public virtual void disablePortraitAnim(bool anstate)
        {
            this.isAnimating = false;
            this.StopCoroutine(coroutine);
        }
        private static int amCycle = 0;
        public IEnumerator charAnim(float delay)
        {
            for (int i = 0; i < portrait1.Length; i++)
            {
                if (portrait1[i] != null)
                {
                    if (enableAnimation == actPorAnim.Enable && character != null)
                    {
                        PortraitOptions options = new PortraitOptions();
                        isAnimating = true;
                        options.character = character;
                        options.display = display;
                        Continue();
                        while (isAnimating)
                        {
                            amCycle++;
                            foreach (Sprite jav in portrait1)
                            {
                                options.portrait = jav;
                                stage.RunPortraitCommand(options, null);
                                yield return new WaitForSeconds(frameDelay);
                            }
                            if(reverseLoop)
                            {
                                foreach (Sprite jav2 in portrait1.Reverse())
                                {
                                    options.portrait = jav2;
                                    stage.RunPortraitCommand(options, null);
                                    yield return new WaitForSeconds(frameDelay);
                                }
                            }
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
                                if (amCycle == cycles)
                                {
                                    PortraitAnim portan = GetComponent<PortraitAnim>();
                                    portan.disablePortraitAnim(false);
                                    yield return new WaitForSeconds(0);
                                    Continue();
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
            if (stage != null)
            {
                stageSummary = " on \"" + stage.name + "\"";
            }

            if (enableAnimation == actPorAnim.Enable && character == null)
            {
                return "Error: No character selected";
            }
            for(int i = 0; i < portrait1.Length; i++)
            {
                if (enableAnimation == actPorAnim.Enable && character != null && portrait1[i] == null)
                {
                    return "Error: One of portrait slots cannot be empty";
                }

                if (portrait1[i] != null)
                {
                    var bg = portrait1[i];
                    StringBuilder builder = new StringBuilder();
                    foreach(Sprite oo in portrait1)
                    {
                        var jj = builder.Append(oo.name).Append(",");
                    }
                    portraitSummary = " " + builder.ToString().TrimEnd(new char[] { ',' });
                }
            }
            return characterSummary + portraitSummary + "\"" + stageSummary;
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