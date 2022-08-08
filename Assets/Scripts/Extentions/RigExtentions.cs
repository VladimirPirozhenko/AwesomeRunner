using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public static class RigExtentions
{
    public static async void ChangeWeightOverTime(this Rig rig, float from, float to,float timeToChange)
    {
        float elapsedTime = 0;
        while (elapsedTime < timeToChange)
        {
            float currentLayerWeight = Mathf.Lerp(from, to, (elapsedTime / timeToChange));//Mathf.SmoothDamp(constraint.weight, to, ref weightChangeVelocity, 0.5f);
            rig.weight = currentLayerWeight;
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }
    }
    public static void ChangeWeight(this Rig rig, float to)
    {
        rig.weight = to;
    }
}