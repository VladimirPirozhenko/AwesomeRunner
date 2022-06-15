using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigController : MonoBehaviour
{
    [SerializeField] private Rig rightHandRig;
    public Rig RightHandRig { get { return rightHandRig; } }
    //public void ChangeRigWeight(float from, float to, float timeToChange)
    //{
    //    RightHandRig.ChangeWeightOverTime(from, to, timeToChange);//rightHandRig.Weight
    //}
    public void ChangeRigWeight(Rig rig,float from, float to, float timeToChange) //!!!!Rig rigToChange,
    {
        rig.ChangeWeightOverTime(from, to, timeToChange);
    }
    public void ChangeRigWeight(Rig rig,float to)
    {
        rig.weight = to;
    }
}
