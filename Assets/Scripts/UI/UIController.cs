using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField]
    private GameObject victoryPanel;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, playerHealth.MaxHealth * 80);
        
        
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

    public void ShowVictoryMenu()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        victoryPanel.SetActive(true);
    }
    
    public void OnMenuButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
