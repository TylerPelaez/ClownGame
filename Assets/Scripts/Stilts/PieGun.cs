using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieGun : MonoBehaviour
{
    public GameObject stilts;

    private StiltController stiltController;

    [HideInInspector]
    public bool isPieGunVisible;

    void Start()
    {
        stiltController = stilts.GetComponent<StiltController>();

        this.GetComponent<MeshRenderer>().enabled = false;

        isPieGunVisible = false;       
    }

    // Update is called once per frame
    void Update()
    {
        if (stiltController != null)
        {
            Vector3 stiltsCurrentScale = stilts.transform.localScale;

            if (stiltsCurrentScale.y <= 0f)
            {
                this.GetComponent<MeshRenderer>().enabled = true;

                isPieGunVisible = true;
            }
            else
            {
                this.GetComponent <MeshRenderer>().enabled = false;
                isPieGunVisible = false;
            }
        }
        else
        {
            Debug.LogWarning("StiltController not found on stilts gameObject.");
        }
    }
}
