using UnityEngine;

public class StiltsIK : MonoBehaviour
{
    public Transform leftFootBone;
    public Transform rightFootBone;
    public Transform leftStilt;
    public Transform rightStilt;

    [SerializeField] float offsetY;

    void LateUpdate()
    {
        // Left foot
        if (leftFootBone != null)
        {
            Vector3 leftBonePosition = leftFootBone.position - new Vector3(0f, transform.localScale.y/2.0f - offsetY, 0f);

            // Apply the position difference to the left stilt
            leftStilt.position = leftBonePosition;

        }

        // Right foot
        if (rightFootBone != null)
        {
            var position = rightFootBone.position;
            Vector3 rightBonePosition = position - new Vector3(0f, transform.localScale.y/2.0f - offsetY, 0f);

            // Apply the position difference to the right stilt
            rightStilt.position = rightBonePosition;
        }
    }
}