using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
    public enum transiType
    {
        /// <summary> Applies Shock animation to character. </summary>
        Wave,
        /// <summary>Stop or not doing anything. </summary>
        None
    }
    /// <summary>
    /// Poor man's 2D animation effects.
    /// </summary>
    [CommandInfo("Animation",
                 "2D effects",
                 "Fake 2D particle effects. Use empty GameObject for spawn point. The Tag is case sensitive or else the clones won't be destroyed or piled up over time")]
    [ExecuteInEditMode]
    public class twoDeffects : Command
    {
        public transiType effectType;
        [SerializeField] public Canvas canvasContainer;
        [SerializeField] protected GameObject sprite1;
        [SerializeField] public Transform spawnPoint1;
        [Space(4)]
        [SerializeField] protected float scaleDelaySp1 = 0.3f;
        [SerializeField] protected float scaleDurationSp1 = 0.5f;
        [SerializeField] protected float yAxisSp1 = 15f;
        [SerializeField] protected float yAxisDelaySp1 = 0.7f;
        [SerializeField] protected float xAxisSp1 = 0.5f;
        [SerializeField] protected float xAxisDelaySp1 = 0.3f;
        [Space(5)]
        [SerializeField] protected bool turnOffAlphaSp1 = false;
        [SerializeField] protected GameObject sprite2;
        [SerializeField] public Transform spawnPoint2;
        [Space(4)]
        [SerializeField] protected float scaleDelaySp2 = 0.1f;
        [SerializeField] protected float scaleDurationSp2 = 0.5f;
        [SerializeField] protected float yAxisSp2 = 15f;
        [SerializeField] protected float yAxisDelaySp2 = 1f;
        [SerializeField] protected float xAxisSp2 = -1f;
        [Space(5)]
        [SerializeField] protected bool turnOffAlphaSp2 = false;
        [SerializeField] protected GameObject sprite3;
        [SerializeField] public Transform spawnPoint3;
        [Space(4)]
        [SerializeField] protected float scaleDelaySp3 = 0.5f;
        [SerializeField] protected float scaleDurationSp3 = 0.5f;
        [SerializeField] protected float yAxisSp3 = 15f;
        [SerializeField] protected float yAxisDelaySp3 = 1f;
        [SerializeField] protected float xAxisSp3 = 0.5f;
        [Space(5)]
        [SerializeField] protected bool turnOffAlphaSp3 = false;
        [Space(5)]
        [SerializeField] protected float spawnRate = 0.1f;
        [Header("IMPORTANT! Enter your predefined TAG")]
        [Space(1)]
        [Header("Create your custom TAG in Tag & Layer Manager")]
        //Haven't found a practical way of detecting available tags
        //Must be changed later!
        [SerializeField] public string tagss = "";
        [Space(4)]
        //[Header("Clones list. Do not change this!")]
        [HideInInspector] public List<GameObject> listOfAvailable2DParticles = new List<GameObject>();

        //Delay the clones
        private IEnumerator coroutine;
        protected virtual void OnTweenComplete()
        {
            Continue();
        }
        float[] randomDur = new[] { 4f, 1.3f, 1f, 0.7f, 2f, 1.5f, 2.5f, 5f };
        float[] randomY = new[] { 1f, 1.3f, 2.5f, 0.7f, 2f, 1.5f, 2.5f, 5f };
        Vector3[] spawnPositions = new[] { new Vector3(15, 3, 0), new Vector3(2, 2, 0), new Vector3(2, 1, 0), new Vector3(2, 1, 0) };
        Quaternion spawnRotation = Quaternion.identity;

        protected void Wave()
        {
            coroutine = WaitAndExec(spawnRate);
            //Start all delayed calls
            StartCoroutine(coroutine);
        }
        protected IEnumerator WaitAndExec(float waitTime)
        {
            
            yield return new WaitForSeconds(waitTime);
            //Check if sprites & canvas are not empty
            if (sprite1 != null && canvasContainer != null)
            {
                sprite1.SetActive(true);
                for (int i = 0; i < spawnPositions.Length; i++)
                {
                    GameObject blob0 = Instantiate(sprite1, spawnPositions[Random.Range(0, spawnPositions.GetLength(0))], spawnRotation) as GameObject;
                    blob0.name = "twod";
                    blob0.tag = tagss;
                    listOfAvailable2DParticles.Add(blob0);
                    yield return new WaitForSeconds(waitTime);

                    if (spawnPoint1 != null)
                    {
                        blob0.transform.position = spawnPoint1.position;
                    }

                    for (int j = 0; j < randomDur.Length; j++)
                    {
                        blob0.transform.SetParent(canvasContainer.transform, false);
                        LeanTween.scale(blob0.transform.gameObject, new Vector3(1.2f, 1.2f, randomDur[Random.Range(0, spawnPositions.GetLength(0))]), scaleDurationSp1).setDelay(scaleDelaySp1).setEaseInQuad().setLoopClamp();
                        LeanTween.moveY(blob0.transform.gameObject, yAxisSp1, 2f).setEaseInQuad().setDelay(yAxisDelaySp1).setLoopClamp();
                        LeanTween.moveX(blob0.transform.gameObject, xAxisSp1, randomDur[Random.Range(0, spawnPositions.GetLength(0))]).setEaseInQuad().setDelay(xAxisDelaySp1).setLoopClamp();
                        if (turnOffAlphaSp1 == false)
                        {
                            LeanTween.alpha(blob0.transform.gameObject, 0f, 0.8f).setEaseInOutQuad().setDelay(0.2f).setLoopPingPong();
                        }
                    }
                }
            }

            //Check if sprites & canvas are not empty

            if (sprite2 != null && canvasContainer != null)
            {
                sprite2.SetActive(true);
                for (int i = 0; i < spawnPositions.Length; i++)
                {
                    GameObject blob1 = Instantiate(sprite2, spawnPositions[Random.Range(0, spawnPositions.GetLength(0))], spawnRotation) as GameObject;

                    blob1.name = "twod";
                    blob1.tag = tagss;
                    listOfAvailable2DParticles.Add(blob1);
                    yield return new WaitForSeconds(waitTime);

                    if (spawnPoint2 != null)
                    {
                        blob1.transform.position = spawnPoint2.position;
                    }

                    for (int j = 0; j < randomDur.Length; j++)
                    {
                        blob1.transform.SetParent(canvasContainer.transform, false);
                        LeanTween.scale(blob1.transform.gameObject, new Vector3(1.2f, 1.2f, randomDur[Random.Range(0, spawnPositions.GetLength(0))]), scaleDurationSp2).setDelay(scaleDelaySp2).setEaseInQuad().setDestroyOnComplete(true).setLoopClamp();
                        LeanTween.moveY(blob1.transform.gameObject, yAxisSp2, 3f).setEaseInQuad().setLoopClamp();
                        LeanTween.moveX(blob1.transform.gameObject, xAxisSp2, randomDur[Random.Range(0, spawnPositions.GetLength(0))]).setEaseInQuad().setLoopClamp();
                        if (turnOffAlphaSp2 == false)
                        {
                            LeanTween.alpha(blob1.transform.gameObject, 0f, randomDur[Random.Range(0, spawnPositions.GetLength(0))]).setEaseInOutQuad().setDelay(0.4f).setLoopPingPong();
                        }
                    }
                }
            }

            //Check if sprites & canvas are not empty

            if (sprite3 != null && canvasContainer != null)
            {
                sprite3.SetActive(true);
                for (int i = 0; i < spawnPositions.Length; i++)
                {
                    GameObject blob2 = Instantiate(sprite3, spawnPositions[Random.Range(0, spawnPositions.GetLength(0))], spawnRotation) as GameObject;
                    blob2.name = "twod";
                    blob2.tag = tagss;
                    listOfAvailable2DParticles.Add(blob2);
                    yield return new WaitForSeconds(waitTime);

                    if (spawnPoint3 != null)
                    {
                        blob2.transform.position = spawnPoint3.position;
                    }

                    for (int j = 0; j < randomDur.Length; j++)
                    {
                        blob2.transform.SetParent(canvasContainer.transform, false);
                        LeanTween.scale(blob2.transform.gameObject, new Vector3(1.2f, 1.2f, randomDur[Random.Range(0, spawnPositions.GetLength(0))]), scaleDurationSp3).setDelay(scaleDelaySp3).setEaseInQuad().setDestroyOnComplete(true).setLoopClamp();
                        LeanTween.moveY(blob2.transform.gameObject, yAxisSp3, 1.5f).setEaseInQuad().setDelay(0.3f).setLoopClamp();
                        LeanTween.moveX(blob2.transform.gameObject, xAxisSp3, randomDur[Random.Range(0, spawnPositions.GetLength(0))]).setEaseInQuad().setDelay(0.3f).setLoopClamp();
                        if (turnOffAlphaSp3 == false)
                        {
                            LeanTween.alpha(blob2.transform.gameObject, 0f, randomDur[Random.Range(0, spawnPositions.GetLength(0))]).setEaseInOutQuad().setDelay(0.1f).setLoopPingPong();
                        }
                    }
                }
            }

            OnTweenComplete();
        }

        //Destroy all clones
        protected void StopAnim()
        {
            if (sprite1 != null)
            {
                sprite1.SetActive(false);
            }
            if (sprite2 != null)
            {
                sprite2.SetActive(false);
            }
            if (sprite3 != null)
            {
                sprite3.SetActive(false);
            }

            GameObject[] blasts2 = GameObject.FindGameObjectsWithTag(tagss);
            foreach (GameObject clone in blasts2)
            {
                if (clone.name == "twod")
                {
                    if (clone != null)
                    {
                        //This somewhat heavy for stopping all instantiated objects
                        //LeanTween.cancel(clone);

                        //For reusable. Scrapped, as users might confused by it's usage
                        //clone.SetActive(false);

                        GameObject.Destroy(clone);
                        //Stop all coroutines
                        StopAllCoroutines();
                    }
                }
            }
        }

        #region public members
        public override void OnEnter()
        {
            Canvas.ForceUpdateCanvases();

            switch (effectType)
            {
                case (transiType.Wave):
                    Wave();
                    break;
                case (transiType.None):
                    StopAnim();
                    break;
            }
            Continue();
        }
        public override string GetSummary()
        {
            string spawnSummary = "";
            string containerSummary = "";

            if (effectType != transiType.None)
            {
                if (spawnPoint1 == null)
                {
                    return "Error: Can't be empty";
                }
                if (spawnPoint2 == null)
                {
                    return "Error: Can't be empty";
                }
                if (spawnPoint3 == null)
                {
                    return "Error: Can't be empty";
                }

                if (canvasContainer == null)
                {
                    return "Error: Can't be empty";
                }
            }

            return spawnSummary + containerSummary;
        }
        public override Color GetButtonColor()
        {
            return new Color32(230, 200, 250, 255);
        }
        public override void OnCommandAdded(Block parentBlock)
        {
            effectType = transiType.None;
        }
        #endregion
    }
}