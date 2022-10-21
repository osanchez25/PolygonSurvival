using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    private Button _button;
   


    void Start()
    {
        _button = GetComponentInChildren <Button>();
        _button.onClick.AddListener(ClickPlayAgain);
    }

    public void ClickPlayAgain()
    {

      
        SceneManager.LoadScene("Main_Scene_City");
    }


}