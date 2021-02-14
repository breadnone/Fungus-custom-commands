// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;
public enum TMProTagLoaderList
{
    None,
    bold,
    italic,
    size,
    flash,
    verticalPunchScreen,
    horizontalPunchScreen,
    punchScreen,
    replaceText
}

namespace Fungus
{
    /// <summary>
    /// Writes text in a dialog box.
    /// </summary>
    [CommandInfo("Narrative", 
                 "Auto tags loader", 
                 "Recursively add narrative tags to available Says. Can be combined with TMProAutoEffectLoader if placed after")]
    [AddComponentMenu("")]
    public class TMProAutoTagLoader : Command
    {
        [System.Serializable]
        public class BulkTags
        {
            [SerializeField] public string wordList;
            [SerializeField] public TMProTagLoaderList style;
            [SerializeField] public float floatParamValue;
            [SerializeField] public string stringParamValue;
        }
        [SerializeField] protected BulkTags[] tags = new BulkTags[1];

        [SerializeField] protected Flowchart flowchart;
        
        [SerializeField] protected float saveWait = 2f;

        #region Public members

        protected IEnumerator TMtagLoader()
        {
                Say[] says = flowchart.GetComponents<Say>();

                    for(int i = 0; i < tags.Length; i++)
                    {
                        var dWord = tags[i].wordList;
                        var dTag = tags[i];
                        if(!String.IsNullOrEmpty(dWord) && tags[i].style != TMProTagLoaderList.None )
                        {
                            for(int j = 0; j < says.Length; j++)
                            {
                                if (says[j] != null)
                                {
                                    var c = says[j];

                                    if(c.storyText != "")
                                    {
                                        //Text italic
                                        string italics = "{i}";
                                        string cItalics = "{/i}";

                                        //Text bold
                                        string bold = "{b}";
                                        string cBold = "{/b}";

                                        //Text size
                                        string tSize = "{size=" + tags[i].floatParamValue.ToString() + "}";
                                        string cTsize = "{/size}";

                                        //Flash screen
                                        string sFlash = "{flash=" + tags[i].floatParamValue.ToString() + "}";

                                        //Punch screen
                                        string sPunch = "{punch=10,0.5}";

                                        //Horizontal punch screen
                                        string hPunch = "{hpunch=10,0.5}";

                                        //Vertical punch screen 
                                        string vPunch = "{vpunch=10,0.5}";

                                        if(!String.IsNullOrEmpty(dWord))
                                        {
                                            if(dTag.style == TMProTagLoaderList.bold || dTag.style == TMProTagLoaderList.italic)
                                            {
                                                if(dTag.style == TMProTagLoaderList.bold)
                                                {
                                                    string bqm = bold + dWord + cBold;
                                                    string a = c.storyText.Replace(dWord, bqm);
                                                    c.storyText = a;
                                                }
                                                else
                                                {
                                                    string bqm = italics + dWord + cItalics;
                                                    string a = c.storyText.Replace(dWord, bqm);
                                                    c.storyText = a;
                                                }
                                            }
                                            if(dTag.style == TMProTagLoaderList.size)
                                            {
                                                string bqm = tSize + dWord + cTsize;
                                                string a = c.storyText.Replace(dWord, bqm);
                                                c.storyText = a;
                                            }
                                            if(dTag.style == TMProTagLoaderList.flash)
                                            {
                                                string bqm = dWord + sFlash;
                                                string a = c.storyText.Replace(dWord, bqm);
                                                c.storyText = a;
                                            }
                                            if(dTag.style == TMProTagLoaderList.punchScreen)
                                            {
                                                string bqm = dWord + sPunch;
                                                string a = c.storyText.Replace(dWord, bqm);
                                                c.storyText = a;
                                            }
                                            if(dTag.style == TMProTagLoaderList.verticalPunchScreen)
                                            {
                                                string bqm = dWord + vPunch;
                                                string a = c.storyText.Replace(dWord, bqm);
                                                c.storyText = a;
                                            }
                                            if(dTag.style == TMProTagLoaderList.horizontalPunchScreen)
                                            {
                                                string bqm = dWord + hPunch;
                                                string a = c.storyText.Replace(dWord, bqm);
                                                c.storyText = a;
                                            }
                                            if(dTag.style == TMProTagLoaderList.replaceText)
                                            {
                                                string bqm = tags[i].stringParamValue;
                                                string a = c.storyText.Replace(dWord, bqm);
                                                c.storyText = a;
                                            }
                                        }
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
                StartCoroutine(TMtagLoader());
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