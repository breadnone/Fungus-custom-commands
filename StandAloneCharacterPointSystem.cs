// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Fungus
{
    /// <summary>
    /// Loads a saved value and stores it in a Boolean, Integer, Float or String variable. If the key is not found then the variable is not modified.
    /// </summary>
    [CommandInfo("Variable", 
                 "Standalone Character's Point System", 
                 "Point/affinity system like in HP, MP or point systems for characters in visual novel. IMPORTANT: Use On-Update only when this command works as a standalone/in disconnected block ")]
    [AddComponentMenu("")]
    public class StandAloneCharacterPointSystem : Command
    {
        [HideInInspector] protected string key = "";
        [Tooltip("Variable to store the value in.")]
        [VariableProperty(typeof(IntegerVariable),
                          typeof(FloatVariable))]
        [SerializeField] protected Variable variable;
        [SerializeField] protected float maxValueOf;
        [SerializeField] protected Slider slider;        
        [SerializeField] protected bool useTextUI = false;
        [Tooltip("If Text component is used, Slider component will be ignored")]
        [SerializeField] protected Text textComponent;
        [Header("Realtime Update. MUST be standalone/disconnected block")]
        [Tooltip("Use this only when this command works as a standalone(separate block/disconnected block)")]
        [SerializeField] protected bool onUpdate = true;

        //Slight delay
        float time = 0.1f;

        void Update()
        {
            if(onUpdate)
            {
                if (time >= 0)
                {
                    time -= Time.deltaTime;
                    return;
                }
                else
                {
                    GetCharacterPoint();
                }
            }
        }

        public void GetCharacterPoint()
        {
            if(variable != null)
            {
                key = variable.name;
                if (key == "" ||
                    variable == null)
                {
                    Continue();
                    return;
                }
                var flowchart = GetFlowchart();

                // Prepend the current save profile (if any)
                string prefsKey = SetSaveProfile.SaveProfile + "_" + flowchart.SubstituteVariables(key);
                System.Type variableType = variable.GetType();

                if (variableType == typeof(IntegerVariable))
                {

                    IntegerVariable integerVariable = variable as IntegerVariable;
                    if(textComponent != null && useTextUI == true)
                    {
                        textComponent.text = integerVariable.Value.ToString();
                    }
                    if(slider != null && useTextUI == false)
                    {                    
                        slider.maxValue = maxValueOf;
                        slider.wholeNumbers = true;
                        //slider.value = (float)integerVariable.Value;
                        //Debug.Log(integerVariable.Value);
                        var tmpfinttofloat = slider.GetComponent<Slider>().value;
                        //Debug.Log(tmpfinttofloat);
                        //float animfloat = 
                        slider.value = (float)integerVariable.Value;
                        
                    }
                }
                else if (variableType == typeof(FloatVariable))
                {
                    FloatVariable floatVariable = variable as FloatVariable;
                    if(textComponent != null && useTextUI == true)
                    {
                        textComponent.text = floatVariable.Value.ToString();
                    }
                    if(slider != null && useTextUI == false)
                    {                    
                        slider.maxValue = maxValueOf;
                        slider.wholeNumbers = true;
                        var tmpfinttofloat = slider.GetComponent<Slider>().value;
                        slider.value = floatVariable.Value;
                        
                    }
                }
            }
        }

        #region Public members
        public override void OnEnter()
        {
            if(!onUpdate)
            {
                GetCharacterPoint();
            }
            Continue();
        }        
        public override string GetSummary()
        {
            if (variable == null)
            {
                return "Error: No variable selected";
            }
            if(textComponent == null && textComponent == true)
            {
                return "Error: No Text component available";
            }
            if(slider == null && textComponent == false)
            {
                return "Error: No Slider component available";
            }
            return variable.Key;
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
        #region Editor caches
        #if UNITY_EDITOR
        protected override void RefreshVariableCache()
        {
            base.RefreshVariableCache();
            var f = GetFlowchart();
            f.DetermineSubstituteVariables(key, referencedVariables);
        }
        #endif
        #endregion Editor caches
    }
}