// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Fungus
{
    /// <summary>
    /// Waits for period of time before executing the next command in the block.
    /// </summary>
    [CommandInfo("Flow", 
                 "Better Wait", 
                 "Waits for period of time before executing the next command in the block.")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class BetterWait : Command
    {
        [Tooltip("Duration to wait for")]
        [SerializeField] protected FloatData _duration = new FloatData(1);

        protected virtual void OnWaitComplete()
        {
            SayDialog sayDialog = SayDialog.GetSayDialog();
            sayDialog.GetComponent<DialogInput>().enabled = true;
            //Not necessary, but hey
            StopCoroutine(OnWaiting());
            Continue();
        }
        protected virtual IEnumerator OnWaiting()
        {
            SayDialog sayDialog = SayDialog.GetSayDialog();
            sayDialog.GetComponent<DialogInput>().enabled = false;
            yield return new WaitForSeconds(_duration);
            OnWaitComplete();
        }

        #region Public members

        public override void OnEnter()
        {
            StartCoroutine(OnWaiting());
        }

        public override string GetSummary()
        {
            return _duration.Value.ToString() + " seconds";
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        public override bool HasReference(Variable variable)
        {
            return _duration.floatRef == variable || base.HasReference(variable);
        }

        #endregion

        #region Backwards compatibility

        [HideInInspector] [FormerlySerializedAs("duration")] public float durationOLD;

        protected virtual void OnEnable()
        {
            if (durationOLD != default(float))
            {
                _duration.Value = durationOLD;
                durationOLD = default(float);
            }
        }

        #endregion
    }
}