// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System;
using UnityEngine.UI;

namespace Fungus
{
    /// <summary>
    /// Fungus Custom Command Template.
    /// </summary>
    [CommandInfo("flow",
                 "Dialog Auto Play",
                 "Auto play dialogue")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class DialogAutoPlay : Command
    {
        [SerializeField] protected SayDialog sayd;
        [SerializeField] protected float delayBeforeContinue = 0f; 

        protected bool isRunning = false;
        protected DialogInput dInput;
        protected Writer writer;
        protected bool wasPressed = false;
        public override void OnEnter()
        {
            dInput = sayd.GetComponent<DialogInput>();
            writer = sayd.GetComponent<Writer>();
            AutoPlay(true);
            Continue();
        }

        public void AutoPlay(bool state)
        {
            if(state)
            {
                if(!isRunning)
                {
                    isRunning = true;
                    wasPressed = false;
                    StartCoroutine(AutoPlayFunction(true));
                }
            }
            else
            {
                isRunning = false;
                wasPressed = false;
                StopCoroutine(AutoPlayFunction(false));
            }
        }        
        protected IEnumerator AutoPlayFunction(bool state)
        {
            while(isRunning)
            {
                yield return null;
                if(!writer.IsWaitingForInput && !wasPressed)
                {                    
                    wasPressed = true;
                    yield return new WaitForSeconds(delayBeforeContinue);
                    dInput.SetNextLineFlag();
                    wasPressed = false;
                }
            }
        }
    }
}