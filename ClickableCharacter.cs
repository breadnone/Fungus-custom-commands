// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Events;
using System;

namespace Fungus
{
    public enum clicChar2
    {
        Enable,
        Disable
    }
    public enum InvokeTypeClick
    {
        /// <summary> Call a method with an optional constant value parameter. </summary>
        Static,
        /// <summary> Call a method with an optional boolean constant / variable parameter. </summary>
        DynamicBoolean,
        /// <summary> Call a method with an optional integer constant / variable parameter. </summary>
        DynamicInteger,
        /// <summary> Call a method with an optional float constant / variable parameter. </summary>
        DynamicFloat,
        /// <summary> Call a method with an optional string constant / variable parameter. </summary>
        DynamicString
    }
    /// <summary>
    /// Tween sequence
    /// </summary>
    [CommandInfo("Sprite",
                 "Clickable Character",
                 "Adds ability for character to be clickable with the use of BoxCollider and Raycast. This won'r follow character's movements nor animations. To update it's position you must create new one. Note: Offset: 0 : 0 means center. Make sure these requirements met: Camera has a Physics Raycaster, EventSystem, Verify no collider (possibly without a mesh) is obscuring the selectable game object")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class ClickableCharacter : Command
    {
        [Tooltip("Enable")]
        [SerializeField] public clicChar2 status;
        [SerializeField] protected Character character;
        [SerializeField] protected Camera mainCam;
        [SerializeField] protected Vector2 hitBoxSize = new Vector2(13f, 28f);
        [SerializeField] protected Vector2 offsets = new Vector2(0, 0);
        [SerializeField] protected bool enableDebugLog = false;

        [Header("Execute When Clicked")]
        [Tooltip("A description of what this command does. Appears in the command summary.")]
        [SerializeField] protected string description = "";
        [Tooltip("Selects type of method parameter to pass")]
        [SerializeField] protected InvokeTypeClick invokeType;
        [Tooltip("Delay (in seconds) before the methods will be called")]
        [SerializeField] protected float delay;

        [Tooltip("List of methods to call. Supports methods with no parameters or exactly one string, int, float or object parameter.")]
        [SerializeField] protected UnityEvent staticEvent = new UnityEvent();

        [Tooltip("Boolean parameter to pass to the invoked methods.")]
        [SerializeField] protected BooleanData booleanParameter;

        [Tooltip("List of methods to call. Supports methods with one boolean parameter.")]
        [SerializeField] protected BooleanEvent booleanEvent = new BooleanEvent();

        [Tooltip("Integer parameter to pass to the invoked methods.")]
        [SerializeField] protected IntegerData integerParameter;
        
        [Tooltip("List of methods to call. Supports methods with one integer parameter.")]
        [SerializeField] protected IntegerEvent integerEvent = new IntegerEvent();

        [Tooltip("Float parameter to pass to the invoked methods.")]
        [SerializeField] protected FloatData floatParameter;
        
        [Tooltip("List of methods to call. Supports methods with one float parameter.")]
        [SerializeField] protected FloatEvent floatEvent = new FloatEvent();

        [Tooltip("String parameter to pass to the invoked methods.")]
        [SerializeField] protected StringDataMulti stringParameter;

        [Tooltip("List of methods to call. Supports methods with one string parameter.")]
        [SerializeField] protected StringEvent stringEvent = new StringEvent();
        protected Stage stage;
        protected virtual void DoInvoke()
        {
            switch (invokeType)
            {
                default:
                case InvokeTypeClick.Static:
                    staticEvent.Invoke();
                    break;
                case InvokeTypeClick.DynamicBoolean:
                    booleanEvent.Invoke(booleanParameter.Value);
                    break;
                case InvokeTypeClick.DynamicInteger:
                    integerEvent.Invoke(integerParameter.Value);
                    break;
                case InvokeTypeClick.DynamicFloat:
                    floatEvent.Invoke(floatParameter.Value);
                    break;
                case InvokeTypeClick.DynamicString:
                    stringEvent.Invoke(stringParameter.Value);
                    break;
            }
        }
        protected string cacheChar;
        protected bool actives = false;
       
        #region Public members
        [Serializable] public class BooleanEvent : UnityEvent<bool> {}
        [Serializable] public class IntegerEvent : UnityEvent<int> {}
        [Serializable] public class FloatEvent : UnityEvent<float> {}
        [Serializable] public class StringEvent : UnityEvent<string> {}
        void Update()
        { 
            if(actives)
            {
                if(character.State.portraitImage != null)
                {
                    if(character != null && mainCam != null)
                    {
                        if(character.State.portraitImage.name != cacheChar)
                        {
                            cacheChar = character.State.portraitImage.name;
                        }
                        if ( Input.GetMouseButtonDown (0))
                        {
                            CastRay();
                        }
                    }
                }
                else
                {
                    Debug.Log("Character's portrait has not been spawned yet! Make sure it exist before this command");
                }
            }
        }
        protected void CastRay()
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
            if (hit) 
            {
                if(enableDebugLog)
                {
                    Debug.Log ("This gameObject was clicked :" + hit.collider.gameObject.name);
                }
                
                //Invoke starts here!
                if (Mathf.Approximately(delay, 0f))
                {
                    DoInvoke();
                }
                else
                {
                    Invoke("DoInvoke", delay);
                }
            }
        }

        protected void DisableClickableCharacter()
        {
            if (character != null)
            {
                if(!actives)
                {
                    for (int i = character.transform.childCount - 1; i >= 0; i--)
                    {
                        GameObject.Destroy(character.transform.GetChild(i).gameObject);
                    }
                    actives = false;
                }
            }
            else
            {
                return;
            }
        }

//This kinda problematic if user spammed clicks! Disabled for now
/*        
        protected void UpdateColliderPosition()
        {
            //Get the actuall postion on screen
            //Ray ray = mainCam.ScreenPointToRay(new Vector3(character.State.portraitImage.transform.position.x, character.State.portraitImage.transform.position.y));
            //RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
            //Vector2 vv = ray.origin;
            //var b = character.gameObject.GetComponentInChildren<BoxCollider2D>();
            //b.offset = vv;
            
        }
*/
        protected void AddCollider()
        {
            if(character != null  && mainCam != null)
            {
                BoxCollider2D boxy;
                float rectX = character.State.portraitImage.rectTransform.sizeDelta.x;
                float rectY = character.State.portraitImage.rectTransform.sizeDelta.y;
                GameObject myGO = new GameObject("stvphtwod-fclickable" + "_" + character.name);
                myGO.AddComponent<BoxCollider2D>();
                boxy = myGO.GetComponent<BoxCollider2D>();
                boxy.transform.SetParent(character.transform, false);
                boxy.size = hitBoxSize;
                boxy.offset = offsets;                
            }
        }

        protected void UpdateStage()
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
        }

        public override string GetSummary()
        {
            string noCol = "";
            string noCam = "";
            if(status == clicChar2.Enable)
            {
                if (character == null)
                {
                    noCol = "Error: No Character is selected";
                }
            
                if (mainCam == null)
                {
                    noCam = "Error: No active camera is selected";
                }
            }
            if(status == clicChar2.Disable)
            {
                if (character == null)
                {
                    noCol = "Error: No Character selected to be disabled";
                }
            }
            return noCol + " : " + noCam;


            if (!string.IsNullOrEmpty(description))
            {
                return description;
            }

            string summary = invokeType.ToString() + " ";

            switch (invokeType)
            {
            default:
            case InvokeTypeClick.Static:
                summary += staticEvent.GetPersistentEventCount();
                break;
            case InvokeTypeClick.DynamicBoolean:
                summary += booleanEvent.GetPersistentEventCount();
                break;
            case InvokeTypeClick.DynamicInteger:
                summary += integerEvent.GetPersistentEventCount();
                break;
            case InvokeTypeClick.DynamicFloat:
                summary += floatEvent.GetPersistentEventCount();
                break;
            case InvokeTypeClick.DynamicString:
                summary += stringEvent.GetPersistentEventCount();
                break;
            }

            return summary + " methods";
        }
        public override void OnEnter()
        {
            
            //Force 1st frame update
            Canvas.ForceUpdateCanvases();
            UpdateStage();
            switch (status)
            {
                case (clicChar2.Disable):

                    DisableClickableCharacter();

                break;
                case (clicChar2.Enable):

                    actives = true;
                    AddCollider();
                break;
            }
            Continue();            
        }
        public override Color GetButtonColor()
        {
            return new Color32(221, 184, 169, 255);
        }
        public override bool HasReference(Variable variable)
        {
            return booleanParameter.booleanRef == variable || integerParameter.integerRef == variable ||
                floatParameter.floatRef == variable || stringParameter.stringRef == variable ||
                base.HasReference(variable);
        }

        public override void OnCommandAdded(Block parentBlock)
        {
            status = clicChar2.Disable;
        }
        #endregion
    }
}
