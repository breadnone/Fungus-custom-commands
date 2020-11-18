// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus
{
    public enum styleTypee
    {
        /// <summary> Allign text center. </summary>
        Center,
        /// <summary> Allign text left center. </summary>
        LeftCenter,
        /// <summary> Allign text right center. </summary>
        RightCenter,
        /// <summary> Allign text top center. </summary>
        TopCenter,
        /// <summary> Allign text top left. </summary>
        TopLeft,
        /// <summary> Allign text top right. </summary>
        TopRight,
        /// <summary> Allign text bottom center. </summary>
        BottomCenter,
        /// <summary> Allign text Bottom left. </summary>
        BottomLeft,
        /// <summary> Allign text Bottom right. </summary>
        BottomRight,
        /// <summary> Default. </summary>
        None
    }
    public enum styleType2
    {
        /// <summary> Regular text. </summary>
        Regular,
        /// <summary> Italic text. </summary>
        Italic,
        /// <summary> Bold text. </summary>
        Bold,
        /// <summary> Bold and italic text. </summary>
        BoldItalic
    }
    /// <summary>
    /// Pauses the game/by setting timescale to 0.
    /// </summary>
    [CommandInfo("Scene",
                 "Pause Screen",
                 "Applies pause to the game as well as ability to load your custom Pause GUI. IMPORTANT: This command must be placed inside unconnected/independent blocks in flowcharts with Execute On Event in that block is null/None. (Optional) Design your custom pause menu inside Canvas. Only 2 supported events such as; KeyDown and KeyUp")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class PauseScreen : Command
    {
        [Tooltip("The type of keypress to activate on")]
        [SerializeField] protected KeyPressType keyPressType;
        [Tooltip("Keycode of the key to activate on")]
        [SerializeField] protected KeyCode keyCode;
        [Tooltip("GameObject/Canvas as screen overlay")]
        [SerializeField] protected GameObject canvas;
        [Tooltip("Replace default text with custom")]
        [SerializeField] public string customText = "Game Paused";
        [Tooltip("Text aligment")]
        [SerializeField] protected styleTypee alignments;
        [Tooltip("Text size")]
        [SerializeField] protected int textSize = 50;
        [Tooltip("Text style")]
        [SerializeField] protected styleType2 textStyle;
        [Tooltip("Disable background box layout")]
        [SerializeField] protected bool disableBoxOverlay = false;
        bool isPaused = false;
        void OnGUI()
        {
            GUIStyle style = new GUIStyle();

            if (isPaused)
            {
                //custom styling, otherwise, just leave it
                if (alignments == styleTypee.Center)
                {
                    style.alignment = TextAnchor.MiddleCenter;
                }
                if (alignments == styleTypee.LeftCenter)
                {
                    style.alignment = TextAnchor.MiddleLeft;
                }
                if (alignments == styleTypee.RightCenter)
                {
                    style.alignment = TextAnchor.MiddleRight;
                }
                if (alignments == styleTypee.TopCenter)
                {
                    style.alignment = TextAnchor.UpperCenter;
                }
                if (alignments == styleTypee.TopLeft)
                {
                    style.alignment = TextAnchor.UpperLeft;
                }
                if (alignments == styleTypee.TopRight)
                {
                    style.alignment = TextAnchor.UpperRight;
                }
                if (alignments == styleTypee.BottomCenter)
                {
                    style.alignment = TextAnchor.LowerCenter;
                }
                if (alignments == styleTypee.BottomLeft)
                {
                    style.alignment = TextAnchor.LowerLeft;
                }
                if (alignments == styleTypee.BottomRight)
                {
                    style.alignment = TextAnchor.LowerRight;
                }

                style.fontSize = textSize;
                style.normal.textColor = Color.white;

                if (textStyle == styleType2.BoldItalic)
                {
                    style.fontStyle = FontStyle.BoldAndItalic;
                }
                if (textStyle == styleType2.Bold)
                {
                    style.fontStyle = FontStyle.Bold;
                }
                if (textStyle == styleType2.Italic)
                {
                    style.fontStyle = FontStyle.Italic;
                }
                if (textStyle == styleType2.Regular)
                {
                    style.fontStyle = FontStyle.Normal;
                }

                // Make a group on the center of the screen
                GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
                
                // Make a box so you can see where the group is on-screen.
                if (!disableBoxOverlay)
                {
                    GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
                    GUI.color = new Color(0.6f, 0.6f, 0.6f);
                    
                    GUI.Label(new Rect(0, 0, Screen.width, Screen.height), customText, style);
                    
                //LeanTween.scale(b, Vector3.zero, 1f).setEase(LeanTweenType.punch).setRecursive(false);

                }

                GUI.EndGroup();
            }
        }

        // Pause Screen
        protected void Pause()
        {
            Time.timeScale = 0;
            isPaused = true;
            GUI.enabled = true;
            SayDialog sayDialog = SayDialog.GetSayDialog();
            sayDialog.GetComponent<DialogInput>().enabled = false;
            if (canvas != null)
            {
                canvas.SetActive(true);
                if (disableBoxOverlay)
                {
                    disableBoxOverlay = true;
                }
                else
                {
                    disableBoxOverlay = false;
                }
            }
        }
        protected void Resume()
        {
            Time.timeScale = 1;
            isPaused = false;
            GUI.enabled = false;
            SayDialog sayDialog = SayDialog.GetSayDialog();
            sayDialog.GetComponent<DialogInput>().enabled = true;
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
        protected virtual void OnComplete()
        {
            Continue();
        }
        public virtual void Update()
        {
            switch (keyPressType)
            {
                case KeyPressType.KeyDown:
                    if (Input.GetKeyDown(keyCode))
                    {
                        if (!isPaused)
                        {
                            Pause();
                        }
                        else
                        {
                            Resume();
                        }
                    }
                    break;
                case KeyPressType.KeyUp:
                    if (Input.GetKeyUp(keyCode))
                    {
                        if (!isPaused)
                        {
                            Pause();
                        }
                        else
                        {
                            Resume();
                        }
                    }
                    break;
            }
        }
        #region Public members
        public override string GetSummary()
        {
            string keycodesummary = "";
            if (keyCode == KeyCode.None)
            {
                return "Error: Assign your button key";
            }

            return keycodesummary;
        }
        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }
        public override void OnEnter()
        {
            Canvas.ForceUpdateCanvases();
            //Was meant for another thing, remove later
            switch (alignments)
            {
                case (styleTypee.Center):
                    break;
                case (styleTypee.LeftCenter):
                    break;
                case (styleTypee.RightCenter):
                    break;
                case (styleTypee.TopCenter):
                    break;
                case (styleTypee.TopRight):
                    break;
                case (styleTypee.TopLeft):
                    break;
                case (styleTypee.BottomCenter):
                    break;
                case (styleTypee.BottomLeft):
                    break;
                case (styleTypee.BottomRight):
                    break;
            }
            switch (textStyle)
            {
                case (styleType2.Regular):
                    break;
                case (styleType2.Italic):
                    break;
                case (styleType2.Bold):
                    break;
                case (styleType2.BoldItalic):
                    break;
            }

            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
        public override void OnCommandAdded(Block parentBlock)
        {
            //Default to display type: KeyDown
            keyPressType = KeyPressType.KeyDown;
            //Default to display alignments: Center
            alignments = styleTypee.Center;
            //Default to display text style: BoldItalic
            textStyle = styleType2.BoldItalic;
        }
        #endregion
    }
}