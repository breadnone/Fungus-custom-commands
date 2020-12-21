// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;

namespace Fungus
{
    /// <summary>
    /// Makes a sprite visible / invisible by setting the color alpha.
    /// </summary>
    [CommandInfo("Sprite", 
                 "Bulk Show Sprite", 
                 "Makes a sprite visible / invisible by setting the color alpha.")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class BulkShowSprite : Command
    {

        [System.Serializable]
        public class BulkShow
        {
            public SpriteRenderer spriteRenderer;
            public BooleanData _visible = new BooleanData(false);
            public bool affectChildren = true;
        }
        public BulkShow[] sprites = new BulkShow[1];
        protected virtual void SetSpriteAlpha(SpriteRenderer renderer, bool visible)
        {
            Color spriteColor = renderer.color;
            spriteColor.a = visible ? 1f : 0f;
            renderer.color = spriteColor;
        }

        #region Public members

        public override void OnEnter()
        {   
            Canvas.ForceUpdateCanvases();
            for(int i = 0; i < sprites.Length; i++)
            {
                if(sprites[i].spriteRenderer != null)
                {
                    if(sprites[i].affectChildren == true)
                    {
                        var spriteRenderers = sprites[i].spriteRenderer.gameObject.GetComponentsInChildren<SpriteRenderer>();
                        for (int j = 0; j < spriteRenderers.Length; j++)
                        {
                            var sr = spriteRenderers[j];
                            SetSpriteAlpha(sr, sprites[j]._visible.Value);
                        }
                    }
                    else
                    {
                        SetSpriteAlpha(sprites[i].spriteRenderer, sprites[i]._visible.Value);
                    }
                }
            }
            Continue();
        }
        public override bool HasReference(Variable variable)
        {
            //this is funky. Bcos Nullable throws error
            bool tmpBool = true;
            for(int i = 0; i < sprites.Length; i++)
            {
                tmpBool = sprites[i]._visible.booleanRef == variable || base.HasReference(variable);
            }
            return tmpBool;
        }

        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }
        #endregion

    }
}