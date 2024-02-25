using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject heartPrefab;

    [SerializeField]
    private Sprite heartFullSprite, heartEmptySprite;


    [SerializeField]
    private Health playerHealth;
    
    [SerializeField]
    private GameObject healthBar;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < playerHealth.MaxHealth; i++)
        {
            Instantiate(heartPrefab, healthBar.transform, true);
        }
        healthBar.GetComponent<HorizontalLayoutGroup>().CalculateLayoutInputHorizontal();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < healthBar.transform.childCount; i++)
        {
            healthBar.transform.GetChild(i).GetComponent<Image>().sprite =
                i < playerHealth.CurrentHealth ? heartFullSprite : heartEmptySprite;
        }
        healthBar.GetComponent<HorizontalLayoutGroup>().CalculateLayoutInputHorizontal();
    }
}
