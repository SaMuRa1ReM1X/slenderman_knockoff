using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{
    public GUISkin _skin;

    private float _gldDepth = -0.5f;
    private float _startTime = 0.1f;

    public Material _mat;

    private long _tris = 0;
    private long _verts = 0;
    private float _savedTimeScale;
    //private SephiaToneEffect _pauseFilter; // --> Only available in Pro Mode!

    private bool _showFPS;
    private bool _showTRIS;
    private bool _showVTX;
    private bool _showFPSgraph;

    public Color _lowFPSColor = Color.red;
    public Color _highFPSColor = Color.green;
    public int _lowFPS = 30;
    public int _highFPS = 50;

    public GameObject _start;

    public string _url = "unity.html";

    public Color _statColor = Color.yellow;
    public string[] _credits = {
        "Following, A shitty rip off of Slender",
        "Programmed by SaMuRa1ReM1X",
        "Lot of Assitance from Alucard Jay and Unity Wiki"};
    public Texture[] _creditCons;

    public enum Page
    { None, Main, Options, Credits }

    private Page _currentPage;

    private float[] _fpsArray;
    private float _fps;

    private int _toolBarInt = 0;
    private string[] _toolBarStrings = 
        { "Audio", "Graphics", "Stats", "System" };

	// Use this for initialization
	void Start () 
    {
        _fpsArray = new float[Screen.width];
        Time.timeScale = 1;
        //_pauseFilter = Camera.main.GetComponent<SephiaToneEffect> ();
        PauseGame();
	}

    void PostRender()
    {

    }

    void ScrollFPS()
    {

    }

    static bool IsDashboard()
    {
        return Application.platform == RuntimePlatform.OSXDashboardPlayer;
    }

    static bool IsBrowser()
    {
        return (Application.platform == RuntimePlatform.WindowsWebPlayer ||
            Application.platform == RuntimePlatform.OSXWebPlayer);
    }

    void LateUpdate()
    {
        if (_showFPS || _showFPSgraph)
            FPSUpdate();

        //Process Escape input for an example pause menu!
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            switch(_currentPage)
            {
                case Page.None:
                    PauseGame();
                    break;
                case Page.Main:
                    if (!IsBeginning())
                        UnPauseGame();
                    break;
                default:
                    _currentPage = Page.Main;
                    break;
            }
        }
    }

    void OnGUI()
    {

    }

    void ShowLegal()
    {

    }

    bool IsLegal()
    {
        return true;
    }

    void ShowToolbar()
    {

    }

    void ShowCredits()
    {

    }

    void ShowBackButton()
    {

    }

    void ShowDevice()
    {

    }

    void Qualities()
    {

    }

    void QualityControl()
    {

    }

    void VolumeControl()
    {

    }

    void StatControl()
    {

    }

    void FPSUpdate()
    {

    }

    void ShowStatsNums()
    {
    }

    void BeginPage(int width, int height)
    {

    }

    void EndPage()
    {

    }

    bool IsBeginning()
    {
        return Time.time < _startTime;
    }

    void MainPauseMenu()
    {

    }

    void GetObjectStats()
    {

    }

    void GetObjectStats(GameObject obj)
    {

    }

    void PauseGame()
    {

    }

    void UnPauseGame()
    {

    }

    bool IsGamePaused()
    {
        return Time.timeScale == 0;
    }

    void OnApplicationPause(bool pause)
    {
        if (IsGamePaused())
        {
            AudioListener.pause = true;
        }
    }

}
