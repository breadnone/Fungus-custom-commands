// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Fungus
{
    public enum cartoonType3
    {
        /// <summary> Fade In effect sequence. </summary>
        Enable,
        /// <summary> Stop All </summary>
        Disable
    }
    /// <summary>
    /// Tween sequence
    /// </summary>
    [CommandInfo("Animation",
                 "Cartoon Effects2",
                 "Sequence of tween animations. For Overlay mode/object to be Always-On-Top, drag and drop your Camera to inspector")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public partial class ClickableCharacter : Command
    {

        [Tooltip("Enable")]
        [SerializeField] public cartoonType3 enable;
        [Tooltip("Set clickable character")]
        [SerializeField] public Character character;

        protected void crCanvas()
        {
            //Programmatically create an empty Canvas.
            Canvas myCanvas;
            CanvasScaler myCanvasScaler;
            GraphicRaycaster myGraphicRaycaster;
            GameObject myGO = new GameObject("stvphtwod");
            myGO.AddComponent<Canvas>();
            myCanvas = myGO.GetComponent<Canvas>();
            if (optionalMainCamera != null)
            {
                myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                myCanvas.worldCamera = optionalMainCamera;
            }
            else
            {
                myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            myCanvas.pixelPerfect = true;
            myCanvas.sortingOrder = sortingOrder;
            myGO.AddComponent<CanvasScaler>();
            myCanvasScaler = myGO.GetComponent<CanvasScaler>();
            myCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            myCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            myCanvasScaler.matchWidthOrHeight = 1f;
            myGO.AddComponent<GraphicRaycaster>();
            myGraphicRaycaster = myGO.GetComponent<GraphicRaycaster>();
            myGraphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.All;
            nCanvas = myCanvas.GetComponent<Canvas>();
        }
        //Main method
        protected void Splash(int partcNum)
        {   //If Canvas exist, us that, otherwise generate new one

        }

 
        #region Public members
        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }
        public override void OnEnter()
        {
            Canvas.ForceUpdateCanvases();
            //RunForestRun();
            if (mainSprite != null && disableSprite == false)
            {
                mainSprite.enabled = false;
            }
            if (particleSprite != null)
            {
                particleSprite.enabled = false;
            }

            switch (enable)
            {
                case (cartoonType3.Enable):
                    Splash(8);
                    SecondaryEf(2);
                    break;
                case (cartoonType3.Disable):
                    saveWait();
                    break;
            }

            if (!waitUntilFinished)
            {
                Continue();
            }
        }
        public override void OnCommandAdded(Block parentBlock)
        {
            enable = cartoonType3.Disable;
        }
        #endregion
    }
}