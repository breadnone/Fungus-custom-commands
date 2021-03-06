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
            public GameObjectData gamobjects;
            public BooleanData activeState;
            public float delay = 0;
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
                if (_targetGameObject[i].activeState == true && _targetGameObject[i].gamobjects.Value != null)
                {
                    //Delay after each iteration.
                    if (i % 1 == 0)
                    {
                        _targetGameObject[i].gamobjects.Value.SetActive(true);
                        yield return new WaitForSeconds(_targetGameObject[i].delay);
                    }
                }

                if (_targetGameObject[i].activeState == false && _targetGameObject[i].gamobjects.Value != null)
                {
                    //Delay after each iteration.
                    if (i % 1 == 0)
                    {
                        _targetGameObject[i].gamobjects.Value.SetActive(false);
                        yield return new WaitForSeconds(_targetGameObject[i].delay);
                    }
                }
            }
        }


        public override bool HasReference(Variable variable)
        {
            //this is funky :0
            bool tmpBool = true;
            for(int i = 0; i < _targetGameObject.Length; i++)
            {
                tmpBool = _targetGameObject[i].gamobjects.gameObjectRef == variable || _targetGameObject[i].activeState.booleanRef == variable ||
                    base.HasReference(variable);
                    
            }
            return tmpBool;
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