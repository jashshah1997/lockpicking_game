using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject m_lock;
    private GameObject m_difficulty_dropdown;
    private GameObject m_skill_level;
    private GameObject m_start_game_button;
    private GameObject m_back_button;

    // Start is called before the first frame update
    void Start()
    {
        m_lock = GameObject.Find("Lock");
        m_difficulty_dropdown = GameObject.Find("DifficultyDropdown");
        m_skill_level = GameObject.Find("SkillLevel");
        m_start_game_button = GameObject.Find("StartGameButton");
        m_back_button = GameObject.Find("BackButton");

        m_lock.SetActive(false);
        m_back_button.SetActive(false);

        m_start_game_button.GetComponent<Button>().onClick.AddListener(OnStartGameClicked);
        m_back_button.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
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

        m_difficulty_dropdown.SetActive(!toggle);
        m_skill_level.SetActive(!toggle);
        m_start_game_button.SetActive(!toggle);
    }
}
