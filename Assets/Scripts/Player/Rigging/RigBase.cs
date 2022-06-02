using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigBase : MonoBehaviour
{
    private Rig rig;
    //public bool InTransition { get; private set; }
    public float Weight
    {
        get { return rig.weight; }
        set { rig.weight = value; }
    }
    private void Awake()
    {
        rig = GetComponent<Rig>();
    }
    public IEnumerator ChangeWeightOverTime(float from, float to, float timeToChange)
    {
        //if (InTransition)
        //   yield break;
        float elapsedTime = 0;
        //InTransition = true;
        while (elapsedTime < timeToChange)
        {
            float currentLayerWeight = Mathf.Lerp(from, to, (elapsedTime / timeToChange));//Mathf.SmoothDamp(constraint.weight, to, ref weightChangeVelocity, 0.5f);
            rig.weight = currentLayerWeight;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // InTransition = false;
    }
}
