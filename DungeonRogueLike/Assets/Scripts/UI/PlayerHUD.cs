using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles Player UI Info 
/// only handles health for now but can be easily expanded upon
/// </summary>
public class PlayerHUD : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private PlayerController playerHP;

    [Header("Health")]
    [SerializeField] private int maxHP;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private void Start()
    {
        if (playerHP == null) { Debug.LogError("Missing Player Health reference"); }
    }

    private void Update()
    {
        DisplayHealth();
    }


    //Displays the hearts of the player character accordingly
    public void DisplayHealth()
    {
        int currentHealth = playerHP.Health;

        //Making sure the currenthealth doesn't exceed the maximum amount of health
        if (currentHealth > maxHP)
        {
            currentHealth = maxHP;
        }

        //Check loop to turn the HP into visuals of empty and half hearts
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHP)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}