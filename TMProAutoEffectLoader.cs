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
                 "TMPro auto text effects", 
                 "Wraps certain texts with TMPro link tag")]
    [AddComponentMenu("")]
    public class TMProAutoEffectLoader : Command
    {
        [System.Serializable]
        public class BulkWords
        {
            [SerializeField] public string wordList;
            [SerializeField] public TMProEffectList effect;
        }
        [SerializeField] protected BulkWords[] words = new BulkWords[1];

        [SerializeField] protected Flowchart flowchart;
        
        [SerializeField] protected float saveWait = 2f;

        #region Public members

        protected IEnumerator TMLoader()
        {
                Say[] says = flowchart.GetComponents<Say>();

                    for(int i = 0; i < words.Length; i++)
                    {
                        var dWord = words[i].wordList;
                        if(!String.IsNullOrEmpty(dWord) && words[i].effect != TMProEffectList.None )
                        {
                            for(int j = 0; j < says.Length; j++)
                            {
                                if (says[j] != null)
                                {
                                    var c = says[j];

                                    if(c.storyText != "")
                                    {
                                        string bqm = "<link=\"" + words[i].effect.ToString() + "\">" + dWord + "</link>";
                                        string a = c.storyText.Replace(dWord, bqm);
                                        c.storyText = a;
                                    }

                                }
                            }
                        }
                    }

                yield return new WaitForSeconds(saveWait);
                Continue();
            
        }

        public override void OnEnter()
        {
            if(flowchart != null)
            {
                StartCoroutine(TMLoader());
            }
            else
            {
                Continue();
            }
        }

        public override Color GetButtonColor()
        {
            return new Color32(184, 210, 235, 255);
        }

        #endregion
    }
}