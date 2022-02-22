using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public float SolveTime = 30f;

    [SerializeField]
    public int LockpickCount = 5;

    private int m_current_lockpick_count;
    private float m_current_time;

    private GameObject m_lock;
    private GameObject m_difficulty_dropdown;
    private GameObject m_skill_level;
    private GameObject m_start_game_button;
    private GameObject m_back_button;
    private GameObject m_lockpick_left;
    private GameObject m_time_left;

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

        m_lock.SetActive(false);
        m_back_button.SetActive(false);
        m_lockpick_left.SetActive(false);
        m_time_left.SetActive(false);

        m_start_game_button.GetComponent<Button>().onClick.AddListener(OnStartGameClicked);
        m_back_button.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_time_left.activeSelf)
        {
            m_current_time -= Time.deltaTime;
            m_time_left.GetComponent<TextMeshProUGUI>().text = (int)m_current_time + "";
        }
    }

    public void DecreasePickLockCount()
    {
        m_current_lockpick_count--;
        m_lockpick_left.GetComponent<TextMeshProUGUI>().text = m_current_lockpick_count + "";
    }

    void OnStartGameClicked()
    {
        ToggleUI(true);
        m_lock.GetComponent<LockPickingController>().Reset();
    }

    void OnBackButtonClicked()
    {
        ToggleUI(false);
    }

    private void ToggleUI(bool toggle)
    {
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
