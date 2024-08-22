using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText; // reference to score text
    public TMP_Text textMessage;//refernce to completion text
    public TMP_Text gameOverTxt;//reference to game over text
    private int totalGems;//total gem count
    private int gemsCollected=0;//initializing the variable to zero
    private int score = 0; // Variable to store the score

    public Image healthbar;//reference to healthbar image
    public float healthAmount = 100f;//total health amount

    public static UIManager instance;//instance of this script

    void Awake()
    {
        //singleton instance of this script
        if(instance==null)
        {
            instance=this;
        }
    }

    void Update()
    {
        //calling the function in update to keep checking if game is over or not
        DisplayGameOverText();
    }

    void Start()
    {
        //creating an array for all objects with tag "gem" and maintaining its count
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("gem");
        totalGems = collectibles.Length;

        // Hide the message text at the start
        textMessage.gameObject.SetActive(false);

        // Initialize health bar
        UpdateHealthBar(healthAmount);
    }

    // This method is called when the player collides with gem object to update the score
    public void UpdateScore()
    {
        //increment score and gem count
        score++;
        gemsCollected++;

        // Update the score text
        scoreText.text = score.ToString();

        //condition to check if all gems have been collected
        if(gemsCollected>= totalGems)
        {
            textMessage.text = "Collection Complete!";
            textMessage.gameObject.SetActive(true);//enabling the text message
        }

    }

    // Method to update the health bar
    public void UpdateHealthBar(float currentHealth)
    {
        // Assuming healthAmount is the maximum health
        float fillAmount = currentHealth / healthAmount;
        healthbar.fillAmount = fillAmount;
    }

    public void DisplayGameOverText()
    {
        //checking if gameover to enable the gameover text
        if(PlayerScript.instance.gameover)
        {
            gameOverTxt.gameObject.SetActive(true);
        }
    }
    

    
}
