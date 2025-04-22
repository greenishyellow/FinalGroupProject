using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public FearFreeze fearFreeze;
    public float walkSpeed = 1f;
    public float sprintSpeed = 2.5f;
    public KeyCode sprintKey = KeyCode.LeftShift;

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
        if (Input.GetKeyDown(sprintKey))
        {
            currentSpeed = sprintSpeed;
        }
        if (Input.GetKeyUp(sprintKey))
        {
            currentSpeed = walkSpeed;
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