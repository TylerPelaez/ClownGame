using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StiltsIK : MonoBehaviour
{
    // Reference to the clown's Animator component
    public Animator clownAnimator;

    // Reference to the left and right foot IK targets
    public Transform leftFootIKTarget;
    public Transform rightFootIKTarget;

    void OnAnimatorIK(int layerIndex)
    {
        // Set the position of the IK targets to match the current position of the clown's feet
        leftFootIKTarget.position = GetIKTargetPosition(AvatarIKGoal.LeftFoot);
        rightFootIKTarget.position = GetIKTargetPosition(AvatarIKGoal.RightFoot);
    }

    // Method to get the position of the IK target for the specified foot
    private Vector3 GetIKTargetPosition(AvatarIKGoal foot)
    {
        // Get the position of the foot from the animation
        Vector3 footPosition = clownAnimator.GetIKPosition(foot);

        // Convert the foot position from world space to local space of the stilts
        return transform.InverseTransformPoint(footPosition);
    }
}
