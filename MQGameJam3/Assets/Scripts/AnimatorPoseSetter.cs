using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPoseSetter : MonoBehaviour
{
    [SerializeField] private Transform armature;
    [SerializeField] private Transform[] handPoses;

    private void Start()
    {
        SetPose(0);
    }

    public void SetPose(int poseIndex)
    {
        if (poseIndex >= 0 && poseIndex < handPoses.Length)
        {
            RecurseSetPose(handPoses[poseIndex], armature);
        }
    }

    void RecurseSetPose(Transform t, Transform armatureDepth)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            Transform poseChild = t.GetChild(i);
            Transform armChild = armatureDepth.GetChild(i);
            armChild.transform.localPosition = poseChild.localPosition;
            armChild.transform.localRotation = poseChild.localRotation;

            RecurseSetPose(poseChild, armChild);
        }
    }
}
