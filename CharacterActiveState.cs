// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.UI;

namespace Fungus
{
    public enum CharStageDisplayType
    {
        /// <summary> Applies Shock animation to character. </summary>
        Rotate,
        /// <summary> Applies Shock animation to character. </summary>
        Shock,
        /// <summary> Applies Happy animation to character. </summary>
        Happy,
        /// <summary> Applies Super Happy animation to character. </summary>
        SuperHappy,
        /// <summary> Applies Panic animation expression to character. </summary>
        Panic,
        /// <summary> Applies Mad animation expression to character. </summary>
        Mad,
        /// <summary> Applies Cyan color transition animationto character. </summary>
        ThrowUp,
        /// <summary> Applies Horizontal Stretch animation to character. </summary>
        StretchWobble,
        /// <summary> Applies Horizontal Stretch + color transition animation to character. </summary>
        StretchWobbleColorize,
        /// <summary> Applies Zoom in animation to character. </summary>
        ZoomIn,
        /// <summary> Applies Zoom with alpha blink animation to character. </summary>
        ZoomInBlink,
        /// <summary> Applies Zoom with alpha blink animation to character. </summary>
        Sinking,
        /// <summary> Applies color Aqua/clear to character. </summary>
        ColorAqua,
        /// <summary> Applies color Pink to character. </summary>
        ColorPink,
        /// <summary> Applies color Cyan to character. </summary>
        ColorCyan,
        /// <summary> Applies color Clear/alpha to character. </summary>
        ColorClearAlpha,
        /// <summary> Disables tween. </summary>
        None
    }
    /// <summary>
    /// Controls character's active state with tweens.
    /// </summary>
    [CommandInfo("Animation",
                 "Character Expression",
                 "Applies animation to both speaking/non-speaking character on stage. The active state valid until next character sprite change OR it should stop manually, both use cases are safe.")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]

    public class CharacterActiveState : ControlWithDisplay<CharStageDisplayType>
    {
        [Tooltip("Applies animation to active/non-active charracters")]
        [SerializeField] protected Character character;
        [SerializeField] protected bool waitUntilFinished = false;
        [Tooltip("Stage to display characters on")]
        [SerializeField] protected Stage stage;
        public virtual Stage _Stage { get { return stage; } }
        //Forbid user inputs while tweening
        protected DialogInput dialInput;
        //Cache position
        protected Vector3 cachePos;
        //Make sure they're done tweening. This won't be affected even if user spam clicks
        protected virtual void OnComplete() 
        {
            if (waitUntilFinished)
            {
                Continue();
            }
            dialInput.enabled = true;
        }
        // Rotates character's sprite
        protected void FlipSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(-1f, 1f, 1f), 0.1f).setEase(LeanTweenType.easeInOutBounce).setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(1f, 1f, 1f), 0f);
                        OnComplete();
                    }
                );
                }
            }
        }
        // Super Happy expression
        protected void SuperHappySpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            //Cache position
            var defPos = character.State.portraitImage.transform.position;
            cachePos = defPos;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.moveY(character.State.portraitImage.rectTransform, 150f, 0.2f).setEaseInQuad().setLoopPingPong(2);
                    LeanTween.color(character.State.portraitImage.rectTransform, Color.yellow, 0.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        character.State.portraitImage.transform.position = cachePos;                  
                        character.State.portraitImage.color = Color.white;
                        OnComplete();
                    }
                );
                }
            }
        }
        // Happy expression
        protected void HappySpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            //Cache position
            var defPos = character.State.portraitImage.transform.position;
            cachePos = defPos;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.moveY(character.State.portraitImage.rectTransform, 100f, 0.2f).setEaseInQuad().setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        character.State.portraitImage.transform.position = cachePos;
                        OnComplete();
                    }
                );
                }
            }
        }
        // Wobble animation
        protected void StretchBounceSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(1.2f, 1f, 1.2f), 0.3f).setEase(LeanTweenType.easeOutBounce).setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(1f, 1f, 1f), 0f);
                        OnComplete();
                    }
                );
                }
            }
        }
        // Sinking down animation
        protected void SinkSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(1.25f, 1f, 1.25f), 1.5f).setDelay(0.1f).setLoopPingPong(1).setOnComplete(
                    () =>
                    {
                        LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(1f, 1f, 1f), 0f);
                        OnComplete();
                    }
                );
                }
            }
        }
        // Zoom In animation
        protected void ZoomInSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(1.4f, 1.4f, 1.4f), 0.3f).setEase(LeanTweenType.easeOutBounce).setDelay(0.2f).setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(1f, 1f, 1f), 0f);
                        OnComplete();
                    }
                );
                }
            }
        }
        // Zoom In + Color animation
        protected void ZoomInColorSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(1.3f, 1.3f, 1.3f), 0.3f).setDelay(0.1f).setEase(LeanTweenType.easeOutBounce).setLoopPingPong(2);
                    LeanTween.color(character.State.portraitImage.rectTransform, Color.clear, 0.3f).setEase(LeanTweenType.easeInQuad).setDelay(0.1f).setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(1f, 1f, 1f), 0f);
                        character.State.portraitImage.color = Color.white;
                        OnComplete();
                    }
                );
                }
            }
        }
        // Stretch Wobble with color animation
        protected void StretchWobbleColorizeSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(1.2f, 1f, 1.2f), 0.3f).setDelay(0.2f).setEase(LeanTweenType.easeOutBounce).setLoopPingPong(2);
                    LeanTween.color(character.State.portraitImage.rectTransform, Color.green, 0.3f).setDelay(0.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        LeanTween.scale(character.State.portraitImage.rectTransform, new Vector3(1f, 1f, 1f), 0f);
                        character.State.portraitImage.color = Color.white;
                        OnComplete();
                    }
                );
                }
            }
        }
        //Panic animation to character
        protected void PanicSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            //Cache position
            var defPos = character.State.portraitImage.transform.position;
            cachePos = defPos;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {                    
                    LeanTween.moveX(character.State.portraitImage.rectTransform, 100f, 0.2f).setEase(LeanTweenType.easeInQuad).setDelay(0.1f).setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        character.State.portraitImage.transform.position = cachePos;
                        OnComplete();
                    }
                );
                }
            }
        }
        // Mad expression
        protected void Mad2SpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.color(character.State.portraitImage.rectTransform, Color.cyan, 0.1f).setEase(LeanTweenType.easeInQuad).setDelay(0.1f).setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        character.State.portraitImage.color = Color.white;
                        OnComplete();
                    }
                );
                }
            }
        }
        // Mad expression
        protected void MadSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.color(character.State.portraitImage.rectTransform, Color.red, 0.1f).setEase(LeanTweenType.easeInQuad).setDelay(0.1f).setLoopPingPong(2
                    ).setOnComplete(
                    () =>
                    {
                        character.State.portraitImage.color = Color.white;
                        OnComplete();
                    }
                );
                }
            }
        }
        // Shock expression
        protected void ShockSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.alpha(character.State.portraitImage.rectTransform, 0f, 0.1f).setLoopPingPong(3).setEase(LeanTweenType.linear).setDelay(0.1f).setOnComplete(
                    () =>
                    {
                        LeanTween.alpha(character.State.portraitImage.rectTransform, 1f, 0.1f);
                        OnComplete();
                    }
                );
                }
            }
        }
        // Aqua Color
        protected void AquaSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.color(character.State.portraitImage.rectTransform, new Color32( 0 , 201 , 254, 1 ), 0.1f).setEase(LeanTweenType.easeInQuad).setDelay(0.2f).setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        character.State.portraitImage.color = Color.white;
                        OnComplete();
                    }
                );
                }
            }
        }
        // Pink color
        protected void PinkSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.color(character.State.portraitImage.rectTransform, new Color32( 232 , 0 , 254, 1 ), 0.1f).setEase(LeanTweenType.easeInQuad).setDelay(0.2f).setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        character.State.portraitImage.color = Color.white;
                        OnComplete();
                    }
                );
                }
            }
        }
        // Cyan color
        protected void CyanSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.color(character.State.portraitImage.rectTransform, Color.cyan, 0.3f).setEase(LeanTweenType.easeInQuad).setDelay(0.2f).setLoopPingPong(2).setOnComplete(
                    () =>
                    {
                        character.State.portraitImage.color = Color.white;
                        OnComplete();
                    }
                );
                }
            }
        }
        // Clear color
        protected void ClearSpeakingPortraits(Character character)
        {
            //Prevent user spam clicks
            dialInput.enabled = false;
            var activeStages = Stage.ActiveStages;
            for (int i = 0; i < activeStages.Count; i++)
            {
                var stage = activeStages[i];
                if (character != null)
                {
                    LeanTween.alpha(character.State.portraitImage.rectTransform, 0f, 0.2f).setLoopPingPong(2).setEase(LeanTweenType.linear).setDelay(0.2f).setOnComplete(
                    () =>
                    {
                        LeanTween.alpha(character.State.portraitImage.rectTransform, 1f, 0.1f).setDelay(0.2f);
                        OnComplete();
                    }
                );
                }
            }
        }
        public override void OnEnter()
        {
            // The canvas may fail to update if it's enabled in the first game frame.
            // To fix this we just need to force a canvas update when the object is enabled.
            Canvas.ForceUpdateCanvases();

            SayDialog sayD = SayDialog.GetSayDialog();
            DialogInput dialI = sayD.GetComponent<DialogInput>();
            dialInput = dialI;

            if (stage == null)
            {
                // If no default specified, try to get any portrait stage in the scene
                stage = FindObjectOfType<Stage>();
                // If portrait stage does not exist, do nothing
                if (stage == null)
                {
                    Continue();
                    return;
                }
            }

            if (IsDisplayNone(display))
            {
                if (stage == null)
                {
                    // If no default specified, try to get any portrait stage in the scene
                    stage = FindObjectOfType<Stage>();
                    // If portrait stage does not exist, do nothing
                    if (stage == null)
                    {
                        Continue();
                        return;
                    }
                }
                Continue();
            }
            switch (display)
            {
                case (CharStageDisplayType.Rotate):
                    FlipSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.Shock):
                    ShockSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.Happy):
                    HappySpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.SuperHappy):
                    SuperHappySpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.Panic):
                    PanicSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.Mad):
                    MadSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.ThrowUp):
                    Mad2SpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.StretchWobble):
                    StretchBounceSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.StretchWobbleColorize):
                    StretchWobbleColorizeSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.ZoomIn):
                    ZoomInSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.ZoomInBlink):
                    ZoomInColorSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.Sinking):
                    SinkSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.ColorAqua):
                    AquaSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.ColorPink):
                    PinkSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.ColorCyan):
                    CyanSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.ColorClearAlpha):
                    ClearSpeakingPortraits(character);
                    break;
                case (CharStageDisplayType.None):
                    break;
            }
            if (!waitUntilFinished)
            {
                Continue();
            }
        }
        public override string GetSummary()
        {
            string displaySummary = "";
            if (character != null)
            {
                displaySummary = StringFormatter.SplitCamelCase(character.ToString());
            }
            else
            {
                return "";
            }
            string stageSummary = "";
            if (stage != null)
            {
                stageSummary = " \"" + stage.name + "\"";
            }
            return displaySummary + stageSummary;
        }
        public override Color GetButtonColor()
        {
            return new Color32(230, 200, 250, 255);
        }
        public override void OnCommandAdded(Block parentBlock)
        {
            //Default to display type: None
            display = CharStageDisplayType.None;
        }
    }
}