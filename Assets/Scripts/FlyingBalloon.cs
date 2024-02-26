using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBalloon : MonoBehaviour
{
    public void Pop()
    {
        FindObjectOfType<AudioManager>().Play("PopEffect");
        Destroy(gameObject);
    }
}
