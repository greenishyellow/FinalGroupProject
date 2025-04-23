using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public FearFreeze fearFreeze;
    public float walkSpeed = 0.75f;
    public float sprintSpeed = 2.5f;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public float maxStamina = 1f;
    public float currentStamina;
    public float staminaDepletionRate = 0.6f;
    public float staminaRegenRate = 0.4f;
    public UnityEngine.UI.Image staminaBar;
    public float staminaRegenDelay = 3.0f;
    private bool isStaminaDepleted = false;
    private float staminaDepletedTime;

    private float currentSpeed;
    private Rigidbody rb;

    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private AudioSource m_AudioSource;
    private Vector3 m_Movement;
    private Quaternion m_Rotation = Quaternion.identity;


    void Start()
    {

        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;

    }

    void Update()
    {
        HandleMovement();
        HandleSprint();
        UpdateStamina();
    }

    void FixedUpdate()
    {
        if (!fearFreeze.isFrozen)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            m_Movement.Set(horizontal, 0f, vertical);
            m_Movement.Normalize();

            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
            bool isWalking = hasHorizontalInput || hasVerticalInput;

            m_Animator.SetBool("IsWalking", isWalking);

            if (isWalking)
            {
                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.Play();
                }
            }
            else
            {
                m_AudioSource.Stop();
            }

            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);
        }
        else
        {
            m_Animator.SetBool("IsWalking", false);
            m_AudioSource.Stop();
            m_Movement = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero; // Stop rotation

        }
    }

    void HandleMovement()
    {
        if (fearFreeze.isFrozen)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * currentSpeed;
    }


    void HandleSprint()
    {
        if (Input.GetKey(sprintKey) && currentStamina > 0 && !isStaminaDepleted)
        {
            currentSpeed = sprintSpeed;
            currentStamina -= staminaDepletionRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            if (currentStamina <= 0)
            {
                isStaminaDepleted = true;
                staminaDepletedTime = Time.time;
            }
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    void UpdateStamina()
    {
        if (isStaminaDepleted)
        {
            if (Time.time - staminaDepletedTime >= staminaRegenDelay)
            {
                isStaminaDepleted = false;
            }
        }
        else
        {
            if (currentSpeed != sprintSpeed)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }

        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }

    void OnAnimatorMove()
    {

        if (!fearFreeze.isFrozen)
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
            m_Rigidbody.MoveRotation(m_Rotation);
        }
    }
}