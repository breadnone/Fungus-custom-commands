// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;
public enum TMProEffectList
{
    None,
    wave,
    bounce,
    excited,
    spooky,
    angry,
    unknowable,
    jitter,
    swing
}

namespace Fungus
{
    /// <summary>
    /// Writes text in a dialog box.
    /// </summary>
    [CommandInfo("Narrative",
                 "TMPro Auto Text Effects",
                 "Wraps certain texts with TMPro link tags and must be placed at the very first order in your block")]
    [AddComponentMenu("")]
    public class TMProAutoEffectLoader : Command
    {
        [System.Serializable]
        public class BulkWords
        {
            [SerializeField] public string wordList;
            [SerializeField] public TMProEffectList effect;
        }
        [Tooltip("Words to animate")]
        [SerializeField] protected BulkWords[] words = new BulkWords[1];
        [Tooltip("Flowchart")]
        [SerializeField] protected Flowchart flowchart;
        [Tooltip("Add delay before progressing to the next command")]
        [SerializeField] protected float waitTime = 2f;

        #region Public members

        protected IEnumerator TMLoader()
        {
            Say[] says = flowchart.GetComponents<Say>();

            for (int i = 0; i < words.Length; i++)
            {
                var dWord = words[i].wordList;
                if (!String.IsNullOrEmpty(dWord) && words[i].effect != TMProEffectList.None)
                {
                    for (int j = 0; j < says.Length; j++)
                    {
                        if (says[j] != null && !String.IsNullOrEmpty(says[j].storyText))
                        {
                            var say = says[j];

                            string bqm = "<link=\"" + words[i].effect.ToString() + "\">" + dWord + "</link>";
                            string tmpStrings = say.storyText.Replace(dWord, bqm);
                            say.storyText = tmpStrings;
                        }
                    }
                }
                else
                {
                    yield break;
                }
            }

            yield return new WaitForSeconds(waitTime);
            Continue();
        }
        public override void OnEnter()
        {
            if (flowchart != null)
            {
                StartCoroutine(TMLoader());
            }
            else
            {
                Continue();
            }
        }
        public override string GetSummary()
        {
            string nullFlowchart = "";

            if (flowchart == null)
            {
                return nullFlowchart = "Error: No flowchart selected";
            }

            return nullFlowchart;
        }

        public override Color GetButtonColor()
        {
            return new Color32(184, 210, 235, 255);
        }

        #endregion
    }
}