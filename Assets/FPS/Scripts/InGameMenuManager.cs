using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameMenuManager : MonoBehaviour
{
    [Tooltip("Root GameObject of the menu used to toggle its activation")]
    public GameObject menuRoot;
    [Tooltip("Master volume when menu is open")]
    [Range(0.001f, 1f)]
    public float volumeWhenMenuOpen = 0.5f;
    //[Tooltip("Slider component for look sensitivity")]
    //public Slider lookSensitivitySlider;
    //[Tooltip("Toggle component for shadows")]
    //public Toggle shadowsToggle;
    //[Tooltip("Toggle component for invincibility")]
    //public Toggle invincibilityToggle;
    //[Tooltip("Toggle component for framerate display")]
    //public Toggle framerateToggle;
    [Tooltip("GameObject for the controls")]
    public GameObject controlImage;
    private GameManager _gameManager;
    //PlayerInputHandler m_PlayerInputsHandler;
    //Health m_PlayerHealth;
    //FramerateCounter m_FramerateCounter;

    void Start()
    {
        //m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
        //DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler, this);

        //m_PlayerHealth = m_PlayerInputsHandler.GetComponent<Health>();
        //DebugUtility.HandleErrorIfNullGetComponent<Health, InGameMenuManager>(m_PlayerHealth, this, gameObject);

        //m_FramerateCounter = FindObjectOfType<FramerateCounter>();
        //DebugUtility.HandleErrorIfNullFindObject<FramerateCounter, InGameMenuManager>(m_FramerateCounter, this);

        menuRoot.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //lookSensitivitySlider.value = m_PlayerInputsHandler.lookSensitivity;
        //lookSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);

        //shadowsToggle.isOn = QualitySettings.shadows != ShadowQuality.Disable;
        //shadowsToggle.onValueChanged.AddListener(OnShadowsChanged);

        //invincibilityToggle.isOn = m_PlayerHealth.invincible;
        //invincibilityToggle.onValueChanged.AddListener(OnInvincibilityChanged);

        //framerateToggle.isOn = m_FramerateCounter.uiText.gameObject.activeSelf;
        //framerateToggle.onValueChanged.AddListener(OnFramerateCounterChanged);
    }

    private void Update()
    {

        if (!_gameManager.checkifGameOver())
        {
            // Lock cursor when clicking outside of menu
            if (!menuRoot.activeSelf && Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetButtonDown("Pause Menu")
                || (menuRoot.activeSelf && Input.GetButtonDown("Cancel")))
            {
                if (controlImage.activeSelf)
                {
                    controlImage.SetActive(false);
                    return;
                }

                SetPauseMenuActivation(!menuRoot.activeSelf);

            }
        }
    
    }

    public void ClosePauseMenu()
    {
        SetPauseMenuActivation(false);
    }

    void SetPauseMenuActivation(bool active)
    {
        menuRoot.SetActive(active);

        if (menuRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            //AudioUtility.SetMasterVolume(volumeWhenMenuOpen);

            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            //AudioUtility.SetMasterVolume(1);
        }

    }

   
    public void OnShowControlButtonClicked(bool show)
    {
        controlImage.SetActive(show);
    }
}
