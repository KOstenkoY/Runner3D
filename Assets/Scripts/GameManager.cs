using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _startingPoint;
    [SerializeField] private GameObject _startCameraPosition;

    [SerializeField] private CameraFollow _cameraFollow;

    [SerializeField] private PlayerController _playerPrefab;
    private Transform _currentPlayer;

    [SerializeField] private RectTransform _mainMenu;
    [SerializeField] private RectTransform _gameplayMenu;
    [SerializeField] private RectTransform _registrationPanel;
    [SerializeField] private RectTransform _loginPanel;

    // count steps
    [SerializeField] private Text _pedomentr;
    [SerializeField] private Text _bestRecordUi;

    [SerializeField] private GameObject _advertisement;

    [SerializeField] private RoadGenerator _roadGenerator;

    private StringBuilder _countSteps;
    private float _countStepsRecord = 0;
    private float _bestRecord = 0;

    private bool _isStart = false;

    private FirebaseAuthManager _firebaseAuthManager;

    private void Awake()
    {
        _countSteps = new StringBuilder(1000);
        _registrationPanel.gameObject.SetActive(true);

        _firebaseAuthManager = GetComponent<FirebaseAuthManager>();
    }

    public void StartGame()
    {
        OnReset();
    }

    public void OpenLoginPanel()
    {
        _loginPanel.gameObject.SetActive(true);
        _registrationPanel.gameObject.SetActive(false);
    }

    public void OpenRegisterPanel()
    {
        _registrationPanel.gameObject.SetActive(true);
        _loginPanel.gameObject.SetActive(false);
    }

    public void OnReset()
    {
        if (_mainMenu)
            _mainMenu.gameObject.SetActive(false);

        if (_loginPanel)
            _loginPanel.gameObject.SetActive(false);

        if (_registrationPanel)
            _registrationPanel.gameObject.SetActive(false);

        if (_gameplayMenu)
            _gameplayMenu.gameObject.SetActive(true);

        if (_pedomentr)
            _pedomentr.gameObject.SetActive(true);

        if (_bestRecord < _countStepsRecord)
        {
            _bestRecord = _countStepsRecord;
            _bestRecordUi.text = "Record " + _bestRecord;
        }
        else
        {
            _bestRecord = 0;
        }

        CreateNewPlayer();

        Time.timeScale = 1.0f;
        _isStart = true;
    }

    void CreateNewPlayer()
    {
        PlayerController newPlayer = (PlayerController)Instantiate(_playerPrefab, _startingPoint.transform.position, Quaternion.identity);

        _currentPlayer = newPlayer.GetComponent<Transform>();

        _cameraFollow.target = _currentPlayer;
        _cameraFollow.transform.position = _startCameraPosition.transform.position;
        _cameraFollow.SetCamera();
    }

    public void SetPaused(bool paused, int countDeath)
    {
        if (paused)
        {
            Time.timeScale = 0.0f;
            _mainMenu.gameObject.SetActive(true);
            _gameplayMenu.gameObject.SetActive(false);

            _roadGenerator.ResetLevel();
            _roadGenerator.StartLevel();

            _isStart = false;
        }
        else
        {
            Time.timeScale = 1.0f;
            _mainMenu.gameObject.SetActive(false);
            _gameplayMenu.gameObject.SetActive(true);
            _isStart = true;
        }
        _advertisement.GetComponent<InterAd>().ShowAd();
    }

    public void RestartGame()
    {
        Destroy(_currentPlayer.gameObject);
        _currentPlayer = null;

        OnReset();
    }

    public void OnExit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (_isStart != true)
            return;

        _countSteps.Clear();
        _countStepsRecord = Mathf.Round(_currentPlayer.position.z);
        _countSteps.Append(_countStepsRecord.ToString());
        _pedomentr.text = _countSteps.ToString();
    }
}
