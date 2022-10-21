using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    public float maxHealth = 100;
    public Image healthFillImage;

    public float _currentHealth { get; set; }


    private GameManager _gameManager;
    //public Animator isHurtAnimator;
    private FeedbackFlashHUD feedbackFlashHUD;


    void Start()
    {
        _currentHealth = maxHealth;
       
        _gameManager = Object.FindObjectOfType<GameManager>();
        feedbackFlashHUD = Object.FindObjectOfType<FeedbackFlashHUD>();
        healthFillImage.fillAmount = _currentHealth / maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        //HealthBar.value = _currentHealth;
        feedbackFlashHUD.OnTakeDamage();
        healthFillImage.fillAmount = _currentHealth / maxHealth;
        //flashImage.color = damageFlashColor;
        if (_currentHealth <= 0)
        {
            _gameManager.GameOver();
        }
    }
    public void HealPlayer(float heal)
    {
        _currentHealth += heal;
        if(_currentHealth > maxHealth)
        {
            _currentHealth = maxHealth;
        }
        healthFillImage.fillAmount = _currentHealth / maxHealth;
        //HealthBar.value = _currentHealth;
        feedbackFlashHUD.OnHealed();
    }

    public bool isPlayerCritical()
    {
        if(_currentHealth >= 0 && _currentHealth <= 20)
        {
            return true;
        }
        return false;
    }

   
}