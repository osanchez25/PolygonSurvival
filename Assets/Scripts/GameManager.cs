using FPSControllerLPFP;
using PolygonWar;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Animator GameOverAnimator;
    public Animator VictoryAnimator;
    public GameObject GameOverUI;
    public GameObject VictoryUI;
    private SpawnManager _spawnManager;
    public AudioClip[] DeathSfxClips;


    private GameObject _player;
    private GameObject _bodyCamObject;
    private GameObject _gunCamera;
    private AudioSource _audioSource;

    private bool isGameOver = false;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _bodyCamObject = _player.transform.GetChild(0).gameObject;//GameObject.Find("Assault_Rifle_01_Arms");
        _gunCamera = GameObject.Find("Gun Camera");
        _spawnManager = GetComponentInChildren<SpawnManager>();
        SetupSound();
    }

    public void GameOver()
    {
        GameOverUI.SetActive(true);
        GameOverAnimator.SetBool("IsGameOver", true);
        PlayDeathSound();
        DisableGame();
        _spawnManager.DisableAllEnemies();

    }

    private void Update()
    {
        if (GameOverUI.activeSelf)
        {
            if (GameOverAnimator.GetBool("IsGameOver"))
            {

                Vector3 newPosition = new Vector3(0f, -1.2f, 0);
                _gunCamera.GetComponent<Camera>().cullingMask = 0;
                _bodyCamObject.transform.localRotation = Quaternion.Lerp(_bodyCamObject.transform.localRotation, Quaternion.Euler(-25, 0, -90), 1f * Time.deltaTime);
                _bodyCamObject.transform.localPosition = Vector3.Lerp(_bodyCamObject.transform.localPosition, newPosition, 1f * Time.deltaTime);
            }
        }

    }

    //void DeathCamera()
    //{
    //    var cam  = Camera.main.transform; // get camera transform
    //    cam.parent = null; // detach it from the player
    //    cam.position = _player.transform.position; // place the camera at the start position
    //    cam.LookAt(_player.transform.position); // make it look towards the end position
    //    float t = 0;
    //    while (t < 1)
    //    {
    //        t += Time.deltaTime / 5f;
    //        // move the camera towards the end position each frame
    //        cam.position = Vector3.Lerp(start.position, end.position, t);
    //        yield; // let Unity do other jobs til next frame
    //    }
    //    // movement ended - enable game over screen
    //    showGameOverScreen = true; // enable "Game Over" message in OnGUI
    //}

    public void Victory()
    {
        VictoryUI.SetActive(true);
        VictoryAnimator.SetBool("IsGameOver", true);
        DisableGame();
    }

    private void DisableGame()
    {
        isGameOver = true;
        _player.GetComponent<FpsControllerLPFP>().enabled = false;
        _player.GetComponentInChildren<AutomaticGunScriptLPFP>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    private void SetupSound()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 1f;
    }

    private void PlayDeathSound()
    {
        int index = Random.Range(0, DeathSfxClips.Length);
        _audioSource.clip = DeathSfxClips[index];
        _audioSource.Play();
    }

    public bool checkifGameOver()
    {
        return isGameOver;
    }
}