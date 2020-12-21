// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

﻿using UnityEngine;
using System.Collections;

namespace Fungus
{
    /// <summary>
    /// Sets the mouse cursor sprite.
    /// </summary>
    [CommandInfo("Sprite", 
                 "Set Mouse Cursor 2", 
                 "Sets the mouse cursor sprite.")]
    [AddComponentMenu("")]
    public class SetMouseCursor2 : Command 
    {
        [Tooltip("Texture to use for cursor. Will use default mouse cursor if no sprite is specified")]
        [SerializeField] protected Texture2D cursorTexture;

        [Tooltip("The offset from the top left of the texture to use as the target point")]
        [SerializeField] protected Vector2 hotSpot;

        [Tooltip("Texture to use for cursor. Will use default mouse cursor if no sprite is specified")]
        [SerializeField] protected Texture2D cursorTexture2;

        [Tooltip("The offset from the top left of the texture to use as the target point")]
        [SerializeField] protected Vector2 hotSpot2;
        
        [Tooltip("This for safety reasons, in case user spam clicks. Set the number higher. Can't be lower than 0.3, if it's lower, then 0.3 will be used")]
        [SerializeField] protected float clickSpamPrevention = 0.3f;
        // Cached static cursor settings
        protected static Texture2D activeCursorTexture;
        protected static Vector2 activeHotspot;
        protected static Texture2D activeCursorTexture2;
        protected static Vector2 activeHotspot2;
        protected bool clicked = false;
        #region Public members
        public static void ResetMouseCursor()
        {
            // Change mouse cursor back to most recent settings
            Cursor.SetCursor(activeCursorTexture, activeHotspot, CursorMode.Auto);
            Cursor.SetCursor(activeCursorTexture2, activeHotspot2, CursorMode.Auto);
        }
        void Update()
        {
            if(cursorTexture && cursorTexture2 != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if(!clicked)
                    {
                        StartCoroutine(clicking());
                    }
                }
            }
        }
        public IEnumerator clicking()
        {
            if(clicked == false)
            {
                clicked = true;
                Cursor.SetCursor(activeCursorTexture2, activeHotspot2, CursorMode.Auto);
                if(clickSpamPrevention <= 0.3)
                {
                    yield return new WaitForSeconds(0.3f);
                }
                else
                {
                    yield return new WaitForSeconds(clickSpamPrevention);
                }
                BeforeClicking();
            }
        }
        
        public void BeforeClicking()
        {
            if(clicked == true)
            {
                Cursor.SetCursor(activeCursorTexture, activeHotspot, CursorMode.Auto);
                StopCoroutine(clicking());
                clicked = false;
            }
        }

        public override void OnEnter()
        {
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
            Cursor.SetCursor(cursorTexture2, hotSpot2, CursorMode.Auto);
            activeCursorTexture = cursorTexture;
            activeCursorTexture2 = cursorTexture2;
            activeHotspot = hotSpot;
            activeHotspot2 = hotSpot2;

            Continue();
        }        

        public override string GetSummary()
        {
            if (cursorTexture == null)
            {
                return "Error: No cursor sprite selected";
            }
            if (cursorTexture2 == null)
            {
                return "Error: No cursor sprite selected";
            }

            return cursorTexture.name + " : " + cursorTexture2.name;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        #endregion
    }
}