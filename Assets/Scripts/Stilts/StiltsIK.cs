using UnityEngine;

public class StiltsIK : MonoBehaviour
{
    public Transform leftFootIKTarget;
    public Transform rightFootIKTarget;
    public Transform leftFootBone;
    public Transform rightFootBone;
    public Transform leftStilt;
    public Transform rightStilt;

    void LateUpdate()
    {
        // Update left foot IK target position
        if (leftFootIKTarget != null && leftFootBone != null)
        {
            leftFootIKTarget.position = leftFootBone.position + new Vector3(0f,-1.25f,0f);

            // Calculate the difference in position between the current positions of the stilts and the IK targets
            Vector3 positionDifference = leftFootIKTarget.position - leftStilt.position;

            // Apply the position difference to the left stilt
            leftStilt.position += positionDifference;
        }

        // Update right foot IK target position
        if (rightFootIKTarget != null && rightFootBone != null)
        {
            rightFootIKTarget.position = rightFootBone.position + new Vector3(0f, -1.25f, 0f); 

            // Calculate the difference in position between the current positions of the stilts and the IK targets
            Vector3 positionDifference = rightFootIKTarget.position - rightStilt.position;

            // Apply the position difference to the right stilt
            rightStilt.position += positionDifference;
        }
    }
}