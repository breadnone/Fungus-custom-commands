// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Linq;

namespace Fungus
{
    /// <summary>
    /// Sets a game object in the scene to be active / inactive.
    /// </summary>
    [CommandInfo("Scripting",
                 "Bulk Set Active",
                 "Set gameobjects in the scene to be active / inactive.")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class BulkSetActive : Command
    {
        [System.Serializable]
        public class BulkDrop
        {
            public GameObject gamobjects;
            public bool activeStates;
        }
        [SerializeField] public BulkDrop[] _targetGameObject = new BulkDrop[1];
        private IEnumerator coroutine;

        #region Public members
        protected void bulktrigger()
        {
            coroutine = bulkActive(0);
            StartCoroutine(coroutine);
        }
        protected IEnumerator bulkActive(float waitframe)
        {
            for (int i = 0; i < _targetGameObject.Length; i++)
            {
                if (_targetGameObject[i].activeStates == true && _targetGameObject[i].gamobjects != null)
                {
                    _targetGameObject[i].gamobjects.SetActive(true);
                    //wait for next frame
                    yield return new WaitForEndOfFrame();
                }

                if (_targetGameObject[i].activeStates == false && _targetGameObject[i].gamobjects != null)
                {
                    _targetGameObject[i].gamobjects.SetActive(false);
                    //wait for next frame
                    yield return new WaitForEndOfFrame();
                }

            }
        }

        public override void OnEnter()
        {
            bulktrigger();
            Continue();
        }
        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        #endregion
    }
}