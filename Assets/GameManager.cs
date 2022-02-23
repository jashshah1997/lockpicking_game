using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EasySettings
{
    public static float LockThreshold = 0.2f;
    public static float LockPickBreakingSpeed = 0.5f;
};

public class MediumSettings
{
    public static float LockThreshold = 0.1f;
    public static float LockPickBreakingSpeed = 1f;
};

public class HardSettings
{
    public static float LockThreshold = 0.05f;
    public static float LockPickBreakingSpeed = 2f;
};

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public float SolveTime = 30f;

    [SerializeField]
    public int LockpickCount = 5;

    private int m_current_lockpick_count;
    private float m_current_time;
    private bool m_reinitialize = false;
    private bool m_game_over = false;

    private GameObject m_lock;
    private GameObject m_difficulty_dropdown;
    private GameObject m_skill_level;
    private GameObject m_start_game_button;
    private GameObject m_back_button;
    private GameObject m_lockpick_left;
    private GameObject m_time_left;
    private GameObject m_end_game_label;

    // Start is called before the first frame update
    void Start()
    {
        m_lock = GameObject.Find("Lock");
        m_difficulty_dropdown = GameObject.Find("DifficultyDropdown");
        m_skill_level = GameObject.Find("SkillLevel");
        m_start_game_button = GameObject.Find("StartGameButton");
        m_back_button = GameObject.Find("BackButton");
        m_lockpick_left = GameObject.Find("LockPickLeft");
        m_time_left = GameObject.Find("TimeLeft");
        m_end_game_label = GameObject.Find("EndGameLabel");

        m_lock.SetActive(false);
        m_back_button.SetActive(false);
        m_lockpick_left.SetActive(false);
        m_time_left.SetActive(false);
        m_end_game_label.SetActive(false);

        m_start_game_button.GetComponent<Button>().onClick.AddListener(OnStartGameClicked);
        m_back_button.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_time_left.activeSelf)
        {
            if (!m_game_over)
                m_current_time -= Time.deltaTime;

            m_time_left.GetComponent<TextMeshProUGUI>().text = (int)m_current_time + "";

            if (m_current_time < 0)
            {
                SetEndGame("Time is Up!");
            }

            if (m_current_lockpick_count < 1)
            {
                SetEndGame("No Lockpicks Left!");
            }
        }
    }

    public void SetEndGame(string label)
    {
        if (m_end_game_label.activeSelf) return;

        m_game_over = true;
        m_reinitialize = true;
        m_end_game_label.SetActive(true);
        m_end_game_label.GetComponent<TextMeshProUGUI>().text = label;
        m_lock.GetComponent<LockPickingController>().PauseGame();
    }

    public void DecreasePickLockCount()
    {
        m_current_lockpick_count--;
        m_lockpick_left.GetComponent<TextMeshProUGUI>().text = m_current_lockpick_count + "";
    }

    void OnStartGameClicked()
    {
        int diff = m_difficulty_dropdown.GetComponent<Dropdown>().value;
        float lockpickBreakingSpeed = -1f;
        float lockThreshold = -1f;
        switch (diff)
        {
            case 0:
                lockpickBreakingSpeed = EasySettings.LockPickBreakingSpeed;
                lockThreshold = EasySettings.LockThreshold;
                break;
            case 1:
                lockpickBreakingSpeed = MediumSettings.LockPickBreakingSpeed;
                lockThreshold = MediumSettings.LockThreshold;
                break;
            case 2:
                lockpickBreakingSpeed = HardSettings.LockPickBreakingSpeed;
                lockThreshold = HardSettings.LockThreshold;
                break;
        }

        m_lock.GetComponent<LockPickingController>().LockPickBreakingSpeed = lockpickBreakingSpeed;
        m_lock.GetComponent<LockPickingController>().LockThreshold = lockThreshold;

        m_game_over = false;
        ToggleUI(true);
        m_lock.GetComponent<LockPickingController>().Reset();

        if (m_reinitialize)
        {
            m_reinitialize = false;
            m_lock.GetComponent<LockPickingController>().InitializeLock();
        }

        
    }

    void OnBackButtonClicked()
    {
        ToggleUI(false);
    }

    private void ToggleUI(bool toggle)
    {
        m_end_game_label.SetActive(false);

        m_lock.SetActive(toggle);
        m_back_button.SetActive(toggle);
        m_lockpick_left.SetActive(toggle);
        m_time_left.SetActive(toggle);

        if (toggle)
        {
            m_current_lockpick_count = LockpickCount;
            m_lockpick_left.GetComponent<TextMeshProUGUI>().text = m_current_lockpick_count + "";

            m_current_time = SolveTime;
            m_time_left.GetComponent<TextMeshProUGUI>().text = m_current_time + "";
        }

        m_difficulty_dropdown.SetActive(!toggle);
        m_skill_level.SetActive(!toggle);
        m_start_game_button.SetActive(!toggle);
    }
}
