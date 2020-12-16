// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Fungus
{
    public enum threeFramu2
    {
        Enable,
        Disable
    }
    /// <summary>
    /// Tween sequence
    /// </summary>
    [CommandInfo("Scripting",
                 "Clicker tool",
                 "This is a dev-tool. Disable this on build")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class AutoClickerTool : Command
    {
        [SerializeField] public threeFramu2 splashSelect;
        public static bool isrunning = false;
        protected float delay = 0.2f;
        //public DialogInput dialogInput;
        protected List<Command> commandList = new List<Command>();
        private int comIn = 0;

        public IEnumerator Clicker()
        {
            isrunning = true;
            WaitForSeconds waiting = new WaitForSeconds(delay);
            
            while(isrunning)
            {
                comIn++;
                if(isrunning == true)
                {
                    if (ParentBlock != null)
                    {
                        ParentBlock.JumpToCommandIndex = comIn++;
                        yield return waiting;
                    }
                    
                }
                else
                {
                    yield break;
                }
            }
        }

        public override void OnEnter()
        {
            Canvas.ForceUpdateCanvases();

            //SayDialog sayDialog = SayDialog.GetSayDialog();
            //dialogInput = sayDialog.GetComponent<DialogInput>();
            switch (splashSelect)
            {
                case (threeFramu2.Disable):
                    isrunning = false;
                    break;
                case (threeFramu2.Enable):
                    StartCoroutine(Clicker());
                    break;
            }
            Continue();
        }

        public override void OnCommandAdded(Block parentBlock)
        {
            splashSelect = threeFramu2.Disable;
        }
    }
}
