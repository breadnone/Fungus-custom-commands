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
                 "Character frame-by-frame animation using portrait lists. Cycle = Stopping the animation based on how many loops. IMPORTANT! Create separate Stage just for this custom comamand and Do not use Dim or any other fancy settings in Stage")]
    public class PortraitAnim : ControlWithDisplay<DisplayType>
    {
        //[HideInInspector] protected DisplayType display = DisplayType.Show;
        [SerializeField] public actPorAnim enableAnimation;
        public Stage stage;
        [Tooltip("Character to display")]
        [SerializeField] protected Character character;
        [Tooltip("Portrait to display")]
        [SerializeField] protected Sprite portrait1;
        [Tooltip("Portrait to display")]
        [SerializeField] protected Sprite portrait2;
        [Tooltip("Portrait to display")]
        [SerializeField] protected Sprite portrait3;
        [Tooltip("Portrait to display")]
        [SerializeField] protected Sprite portrait4;
        [Tooltip("Portrait to display")]
        [SerializeField] protected Sprite portrait5;
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
        public virtual Sprite _Portrait1 { get { return portrait1; } set { portrait1 = value; } }
        public virtual Sprite _Portrait2 { get { return portrait2; } set { portrait2 = value; } }
        public virtual Sprite _Portrait3 { get { return portrait3; } set { portrait3 = value; } }
        public virtual Sprite _Portrait4 { get { return portrait4; } set { portrait4 = value; } }
        public virtual Sprite _Portrait5 { get { return portrait5; } set { portrait5 = value; } }
        public bool isAnimating = false;
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
                disdim.GetComponent<Stage>().FadeDuration = 0;
                sequenceMove();
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
        public virtual IEnumerator charAnim(float delay)
        {
            if (portrait1 && portrait2 && portrait3 && portrait4 && portrait5 != null)
            {
                if (enableAnimation == actPorAnim.Enable && character != null)
                {
                    var arpor = new Sprite[]{portrait1, portrait2, portrait3, portrait4, portrait5};
                    isAnimating = true;
                    PortraitOptions options = new PortraitOptions();
                    var por1 = new Action(() =>
                    {
                        options.character = character;
                        options.portrait = portrait1;
                        options.display = display;
                        stage.RunPortraitCommand(options, Continue);
                    });

                    var por2 = new Action(() =>
                    {
                        options.character = character;
                        options.portrait = portrait2;
                        options.display = display;
                        stage.RunPortraitCommand(options, Continue);
                    });

                    var por3 = new Action(() =>
                    {
                        options.character = character;
                        options.portrait = portrait3;
                        options.display = display;
                        stage.RunPortraitCommand(options, Continue);
                    });

                    var por4 = new Action(() =>
                    {
                        options.character = character;
                        options.portrait = portrait4;
                        options.display = display;
                        stage.RunPortraitCommand(options, Continue);
                    });

                    var por5 = new Action(() =>
                    {
                        options.character = character;
                        options.portrait = portrait5;
                        options.display = display;
                        stage.RunPortraitCommand(options, null);
                    });

                    //Continue();

                    while (isAnimating)
                    {
                        por1();
                        yield return new WaitForSeconds(frameDelay);
                        por2();
                        yield return new WaitForSeconds(frameDelay);
                        por3();
                        yield return new WaitForSeconds(frameDelay);
                        por4();
                        yield return new WaitForSeconds(frameDelay);
                        por5();

                        if(reverseLoop)
                        {
                            por5();
                            yield return new WaitForSeconds(frameDelay);
                            por4();
                            yield return new WaitForSeconds(frameDelay);
                            por3();
                            yield return new WaitForSeconds(frameDelay);
                            por2();
                            yield return new WaitForSeconds(frameDelay);
                            por1();
                        }
                        
                        amCycle++;                        

                        Debug.Log("amcycle:"+ amCycle);

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

            if (enableAnimation == actPorAnim.Enable && portrait1 && portrait2 && portrait3 && portrait4 && portrait5 == null)
            {
                return "Error: One of portrait slots cannot be empty";
            }

            return characterSummary + ":" + portraitSummary + ":" + stageSummary;
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