// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fungus
{
    public partial class CartoonEffects
    {
        //Mostly secondary tween
        protected void SecondaryEf(int partdNum)
        {
            if (secondarySprite != null)
            {
                lastIndexNum = onMid;

                effectPosition = new Vector3(posX, posY, 1f);
                var spunPoint = effectPosition;

                for (int i = 0; i < partdNum; i++)
                {

                    Image blob1 = Instantiate(secondarySprite, spunPoint, Quaternion.identity) as Image;
                    blob1.name = "splashpop2-" + (i + 1);
                    blob1.transform.SetParent(nCanvas.transform, false);
                    blob1.transform.SetSiblingIndex(lastIndexNum++);
                    blob1.transform.localScale = new Vector3(secondaryScale, secondaryScale, 1f);
                    //secondarySprite = blob1;
                    partcList.Add(blob1);
                } 
        
                    LeanTween.scale(partcList[8].rectTransform, new Vector3(2.9f, 2.9f, 2.9f), tweenDurations).setEaseInQuad().setLoopClamp();
                    LeanTween.color(partcList[8].rectTransform, Color.red, 0.5f).setRepeat(-1);

                    LeanTween.scale(partcList[9].rectTransform, new Vector3(4.2f, 4.2f, 4.2f), tweenDurations).setEaseInQuad().setLoopClamp();
                    LeanTween.color(partcList[9].rectTransform, Color.blue, 0.5f).setRepeat(-1);

				    LeanTween.rotateAroundLocal(partcList[9].rectTransform, spunPoint + Vector3.forward, -1080f, 1f).setLoopPingPong(-1);
                    //LeanTween.scale(partcList[9].rectTransform, new Vector3(0f, 7f, 0f), tweenDurations).setEaseInQuad().setLoopClamp();

                 
				
	
                    
               
                
                    
                
            }
        }
    }
}