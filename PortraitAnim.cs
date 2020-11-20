// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using System;
using Random=UnityEngine.Random;

namespace Fungus
{
    /// <summary>
    /// Controls a character portrait.
    /// </summary>
    [CommandInfo("Narrative", 
                 "PortraitAnim", 
                 "Character frame-by-frame animateion using portrait lists")]
    public class PortraitAnim : ControlWithDisplay<DisplayType>
    {
        [Tooltip("Stage to display portrait on")]
        [SerializeField] protected Stage stage;
        [Tooltip("Character to display")]
        [SerializeField] protected Character character;
        [Tooltip("Portrait to display")]
        [SerializeField] protected Sprite portrait1;
        [Tooltip("Portrait to display")]
        [SerializeField] protected Sprite portrait2;
        [Tooltip("Portrait to display")]
        [SerializeField] protected Sprite portrait3;
        [Tooltip("Delay between sequence")]
        [SerializeField] protected float delay = 0.2f;
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
        [HideInInspector]public bool isAnimating = false;
        private IEnumerator coroutine;    
        protected void sequenceMove()
        {
            coroutine = charAnim(0.2f);
            StartCoroutine(coroutine);
        }
        public override void OnEnter()
        {
            // Selected "use default Portrait Stage"
            if(display == DisplayType.None)
            {
                //disablePortraitAnim(false);
                PortraitAnim poranim = GetComponent<PortraitAnim>();
                poranim.disablePortraitAnim(false);
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
            // If no display specified, do nothing
            if (IsDisplayNone(display))
            {
                Continue();
                return;
            }
            sequenceMove();            
        }
        public virtual void disablePortraitAnim(bool anstate)
        {
            this.isAnimating = false;
            this.StopAllCoroutines();
        }
        public IEnumerator charAnim(float delay)
        {
            if (display != DisplayType.None && character != null)
            {
                PortraitOptions options = new PortraitOptions();
                isAnimating = true;                
                options.character = character;
     
                var por1 = new Action(() => {
                options.portrait = portrait1;
                options.display = display;
                stage.RunPortraitCommand(options, null);
                } );
                var por2 = new Action(() => {
                options.portrait = portrait2;
                options.display = display;
                stage.RunPortraitCommand(options, null);
                } );
                var por3 = new Action(() => {
                options.portrait = portrait3;
                options.display = display;
                stage.RunPortraitCommand(options, null);                
                } );
                Continue();
                while(isAnimating)
                {               
                    //yield return new WaitForSeconds(0f);
                    por1();
                    yield return new WaitForSeconds(delay);
                    por2();
                    yield return new WaitForSeconds(delay);
                    por3();
                    if(RandomEndDelay)
                    {
                        yield return new WaitForSeconds(Random.Range(4f, 8f));
                    }
                    else
                    {
                        yield return new WaitForSeconds(endFrameDelay);
                    }
                }
            }
            else
            {
                Continue();
            }
        }        
        public override string GetSummary()
        {
            if (display != DisplayType.None && character == null)
            {
                return "Error: No character or display selected";
            }
            string displaySummary = "";
            string characterSummary = "";
            string stageSummary = "";
            string portraitSummary = "";            
            displaySummary = StringFormatter.SplitCamelCase(display.ToString());
            characterSummary = character.name;
            if (stage != null)
            {
                stageSummary = " on \"" + stage.name + "\"";
            }
            
            if (portrait1 && portrait2 && portrait3 != null)
            {
                portraitSummary = " " + portrait1.name;
                portraitSummary = " " + portrait2.name; 
                portraitSummary = " " + portrait3.name; 
            }
            return displaySummary + " \"" + characterSummary + portraitSummary + "\"" + stageSummary;
        }
        
        public override Color GetButtonColor()
        {
            return new Color32(230, 200, 250, 255);
        }        
        public override void OnCommandAdded(Block parentBlock)
        {
            display = DisplayType.Show;
        }

        #endregion
    }
}