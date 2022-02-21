using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPickingController : MonoBehaviour
{
    [SerializeField]
    public float PickRotationSpeed = 1f;

    [SerializeField]
    public float LockRoatationSpeed = 1f;

    private float m_lockpick_position = 0.5f;
    private float m_lock_position;
    private Vector3 m_last_mouse_position;
    private Vector3 m_mouse_delta;

    private Animator m_lock_animator;
    public readonly int lockHash = Animator.StringToHash("LockBlend");
    public readonly int lockPickHash = Animator.StringToHash("LockPickBlend");
    
    // Start is called before the first frame update
    void Start()
    {
        m_lock_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        SetLockPickPosition(m_lockpick_position + m_mouse_delta.x * Time.deltaTime * PickRotationSpeed);
        SetLockPosition(m_lock_position + Input.GetAxisRaw("Horizontal") * Time.deltaTime * LockRoatationSpeed);

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
        m_lock_position = Mathf.Clamp(value, 0f, 1f);
    }
}
