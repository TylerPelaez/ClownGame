using UnityEngine;

public class StiltsIK : MonoBehaviour
{
    public Transform leftFootIKTarget;
    public Transform rightFootIKTarget;
    public Transform leftFootBone;
    public Transform rightFootBone;
    public Transform leftStilt;
    public Transform rightStilt;

    [SerializeField] float offsetY;

    void LateUpdate()
    {
        // Left foot
        if (leftFootIKTarget != null && leftFootBone != null)
        {
            Vector3 leftBonePosition = leftFootBone.position - new Vector3(0f, transform.localScale.y/2.0f - offsetY, 0f);

            // Calculate the difference in position between the current positions of the stilts and the bones
            Vector3 positionDifference = leftBonePosition - leftStilt.position;

            // Apply the position difference to the left stilt
            leftStilt.position += positionDifference;
        }

        // Right foot
        if (rightFootIKTarget != null && rightFootBone != null)
        {
            Vector3 rightBonePosition = rightFootBone.position - new Vector3(0f, transform.localScale.y/2.0f - offsetY, 0f);

            // Calculate the difference in position between the current positions of the stilts and the bones
            Vector3 positionDifference = rightBonePosition - rightStilt.position;

            // Apply the position difference to the right stilt
            rightStilt.position += positionDifference;
        }
    }
}