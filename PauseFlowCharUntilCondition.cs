// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
    /// <summary>
    /// Waits for period of time before executing the next command in the block.
    /// </summary>
    [CommandInfo("Flow",
                 "Pause flowchart Until Condition",
                 "Waits for period of time before executing the next command in the block. This also mean, you can execute blocks in parallel, while waiting. FYI, this a single instance command")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class PauseFlowCharUntilCondition : Command
    {
        private static bool flowIsPaused = false;
        [HideInInspector] protected string key = "";
        [Tooltip("Variable to store the value in.")]
        [VariableProperty(typeof(IntegerVariable))]
        [SerializeField] protected Variable variable;
        [SerializeField] protected int valueComparer;
        [SerializeField] protected string blockName;

        protected virtual IEnumerator WhilePause()
        {
            if (variable != null && valueComparer != 0)
            {
                if (blockName != "")
                {
                    key = variable.name;
                    if (key == "" ||
                        variable == null)
                    {
                        Continue();
                        yield return null;
                    }
                    var flowchart = GetFlowchart();
                    flowIsPaused = true;

                    // Prepend the current save profile (if any)
                    string prefsKey = SetSaveProfile.SaveProfile + "_" + flowchart.SubstituteVariables(key);
                    System.Type variableType = variable.GetType();
                    IntegerVariable integerVariable = variable as IntegerVariable;
                    //FloatVariable floatVariable = variable as FloatVariable;

                    //Call another block based on it's name
                    Flowchart flowcharty = GetComponent<Flowchart>();
                    flowcharty.ExecuteBlock(blockName);

                    //Debug.Log("after block was called");

                    WaitForSeconds waiting = new WaitForSeconds(0.1f);

                    //Halts the flow, waits until condition is met
                    while (flowIsPaused)
                    {
                        //You don't have to check every frame, thus this sort of needed for easing cpu
                        yield return waiting;

                        if (valueComparer == integerVariable.Value)
                        {
                            MoveOn();
                            //Debug.Log("Condition was met! SUCCESS");
                            yield break;
                        }
                    }
                }
            }
        }

        protected void MoveOn()
        {
            flowIsPaused = false;
            StopAllCoroutines();
            Continue();
        }

        #region Public members

        public override void OnEnter()
        {
            if (variable != null && valueComparer != 0)
            {
                StartCoroutine(WhilePause());
            }
            else
            {
                Continue();
            }
        }
        public override string GetSummary()
        {
            string noVar = "";
            string noVal = "";
            if (valueComparer == 0)
            {
                noVal = "Variable cannot be empty or 0!";
            }
            if (valueComparer == 0)
            {
                noVar = "No variable selected!";
            }
            return noVar + " : " + noVal;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        public override bool HasReference(Variable in_variable)
        {
            return this.variable == in_variable ||
                base.HasReference(in_variable);
        }

        #endregion
    }
}