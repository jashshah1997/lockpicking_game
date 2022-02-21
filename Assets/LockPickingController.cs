using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPickingController : MonoBehaviour
{
    [SerializeField]
    public float PickRotationSpeed = 1f;

    [SerializeField]
    public float LockRoatationSpeed = 1f;

    [SerializeField]
    public float LockResetSpeed = 0.4f;

    [SerializeField]
    public float LockThreshold = 0.1f;

    [SerializeField]
    public float LockPickBreakingSpeed = 1f;

    [SerializeField]
    public float LockPickBreakingTime = 1f;

    private float m_lockpick_position = 0.5f;
    private float m_lock_position;
    private float m_target_lockpick_position;
    private float m_shaking_time = 0f;
    private bool m_game_paused = false;
    
    private Vector3 m_last_mouse_position;
    private Vector3 m_mouse_delta;

    private Animator m_lock_animator;
    private GameObject m_lockpick;
    private AudioSource m_unlock_sound;
    private AudioSource m_lockpick_shake_sound;
    private AudioSource m_lockpick_break_sound;

    public readonly int lockHash = Animator.StringToHash("LockBlend");
    public readonly int lockPickHash = Animator.StringToHash("LockPickBlend");
    public readonly int lockpickShakeHash = Animator.StringToHash("Shake");
    
    // Start is called before the first frame update
    void Start()
    {
        m_lock_animator = GetComponent<Animator>();
        m_lockpick = GameObject.Find("Lockpick");
        m_unlock_sound = GameObject.Find("UnlockSound").GetComponent<AudioSource>();
        m_lockpick_shake_sound = GameObject.Find("LockPickShakeSound").GetComponent<AudioSource>();
        m_lockpick_break_sound = GameObject.Find("LockPickBreakSound").GetComponent<AudioSource>();
        InitializeLock();
    }

    // Update is called once per frame
    void Update()
   {
        if (m_game_paused)
        {
            return;
        }

        // Check win condition
        if (m_lock_position > 0.99)
        {
            OnWinGame();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_last_mouse_position = Input.mousePosition;
            m_mouse_delta = new Vector3(0, 0, 0);
        }
        else if (Input.GetMouseButton(0))
        {
            m_mouse_delta = Input.mousePosition - m_last_mouse_position;
            m_last_mouse_position = Input.mousePosition;
        }
        else
        {
            m_mouse_delta = new Vector3(0, 0, 0);
        }

        // Update positions
        if (Input.GetAxisRaw("Horizontal") == 0 && m_lockpick.activeSelf)
            SetLockPickPosition(m_lockpick_position + m_mouse_delta.x * Time.deltaTime * PickRotationSpeed);

        if (m_lockpick.activeSelf)
            SetLockPosition(m_lock_position + Input.GetAxisRaw("Horizontal") * Time.deltaTime * LockRoatationSpeed);

        SetLockPosition(m_lock_position - LockResetSpeed * Time.deltaTime);

        // Update the animator
        m_lock_animator.SetFloat(lockPickHash, m_lockpick_position);
        m_lock_animator.SetFloat(lockHash, m_lock_position);
    }

    private void SetLockPickPosition(float value)
    {
        m_lockpick_position = Mathf.Clamp(value, 0f, 1f);
    }

    private void SetLockPosition(float value)
    {
        float maxRotation = 1f - Mathf.Abs(m_target_lockpick_position - m_lockpick_position) + LockThreshold;
        m_lock_position = Mathf.Clamp(value, 0f, maxRotation);

        if (maxRotation - m_lock_position < 0.05)
        {
            if (!m_lockpick_shake_sound.isPlaying)
                m_lockpick_shake_sound.Play();

            m_lock_animator.SetBool(lockpickShakeHash, true);
            m_shaking_time += Time.deltaTime * LockPickBreakingSpeed;

            if (m_shaking_time > LockPickBreakingTime)
            {
                InvokeRepeating("OnLockPickBreak", 0, 0.1f);
            }
        }
        else
        {
            m_lock_animator.SetBool(lockpickShakeHash, false);
            m_shaking_time = 0;
            m_lockpick_shake_sound.Stop();
        }
    }

    private void OnWinGame()
    {
        Debug.Log("Unlocked!");
        m_unlock_sound.Play();
        m_lock_animator.SetBool(lockpickShakeHash, false);
        m_lockpick_shake_sound.Stop();
        m_game_paused = true;
    }

    private void InitializeLock()
    {
        m_target_lockpick_position = Random.value;
        m_shaking_time = 0;
        m_lockpick_shake_sound.Stop();
    }

    public void Reset()
    {
        m_game_paused = false;
        m_lockpick_position = 0.5f;
        m_lock_position = 0f;
    }

    private void OnLockPickBreak()
    {
        if (m_lockpick.activeSelf)
        {
            Debug.Log("The lockpick broke!");
            m_shaking_time = 0;
            m_lockpick_shake_sound.Stop();
            m_lockpick.SetActive(false);
            m_lockpick_break_sound.Play();
        }

        if (m_lock_position < 0.01)
        {
            CancelInvoke("OnLockPickBreak");
            m_lockpick.SetActive(true);
        }
    }
}
