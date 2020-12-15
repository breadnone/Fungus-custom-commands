// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;

namespace Fungus
{
    /// <summary>
    /// Fades a sprite to a target color over a period of time.
    /// </summary>
    [CommandInfo("Sprite", 
                 "Bulk Fade Sprite", 
                 "Fades a sprite to a target color over a period of time.")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class BulkFadeSprite : Command
    {
        [System.Serializable]
        public class BulkFade
        {
            [Tooltip("Sprite object to be faded")]
            [SerializeField] public SpriteRenderer spriteRenderer;

            [Tooltip("Length of time to perform the fade")]
            [SerializeField] public FloatData _duration = new FloatData(1f);

            [Tooltip("Target color to fade to. To only fade transparency level, set the color to white and set the alpha to required transparency.")]
            [SerializeField] public ColorData _targetColor = new ColorData(Color.white);

        }
        [SerializeField] protected BulkFade[] sprites = new BulkFade[1];


        #region Public members

        public override void OnEnter()
        {
            for(int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i].spriteRenderer == null)
                {
                    Continue();
                    return;
                }
            
                if(sprites[i].spriteRenderer != null)
                {
                    SpriteFader.FadeSprite(sprites[i].spriteRenderer, sprites[i]._targetColor.Value, sprites[i]._duration.Value, Vector2.zero);
                }
            }            
        }

        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }

        public override bool HasReference(Variable variable)
        {
            //this is funky :0
            bool tmpBool = true;
            for(int i = 0; i < sprites.Length; i++)
            {
                tmpBool = sprites[i]._duration.floatRef == variable || sprites[i]._targetColor.colorRef == variable ||
                    base.HasReference(variable);
                    
            }
            return tmpBool;
        }

        #endregion

        #region Backwards compatibility

        [HideInInspector] [FormerlySerializedAs("duration")] public float durationOLD;
        [HideInInspector] [FormerlySerializedAs("targetColor")] public Color targetColorOLD;

        protected virtual void OnEnable()
        {
            for(int i = 0; i < sprites.Length; i++)
            {
                if (durationOLD != default(float))
                {
                    sprites[i]._duration.Value = durationOLD;
                    durationOLD = default(float);
                }
                if (targetColorOLD != default(Color))
                {
                    sprites[i]._targetColor.Value = targetColorOLD;
                    targetColorOLD = default(Color);
                }
            }
        }

        #endregion
    }
}