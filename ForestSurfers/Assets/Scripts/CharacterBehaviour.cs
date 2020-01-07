using System;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public enum PlayerPosition
    {
        Left,
        Center,
        Right
    }
    public PlayerPosition playerPosition;
    public float sideMovementSpeed;
    public new SphereCollider collider;

    private Vector3 m_LeftPosition;
    private Vector3 m_CenterPosition;
    private Vector3 m_RightPosition;
    private Vector3 m_DestPosition;
    private Animator m_Animator;

    private void Awake()
    {
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
        playerPosition = PlayerPosition.Center;
        m_Animator = GetComponent<Animator>();
    }
    private void Start()
    {
        // Set side positions
        var position = transform.position;
        m_LeftPosition = position;
        m_CenterPosition = position;
        m_RightPosition = position;

        const float offSet = 2f;
        m_LeftPosition.x = offSet;
        m_RightPosition.x = -offSet;
        m_DestPosition = m_CenterPosition;
    }
    private void Update()
    {
        if (OnAnimation(PlayerAnims.Run))
           if(collider.center != Vector3.zero)
               collider.center = Vector3.zero;
        if(OnAnimation(PlayerAnims.Slide))
            collider.center = new Vector3(0,-0.6f,0);

        // PC Debug Inputs
        if (Input.GetKeyDown(KeyCode.A))
            Left();
        if (Input.GetKeyDown(KeyCode.D))
            Right();
        if (Input.GetKeyDown(KeyCode.W))
            Jump();
        if (Input.GetKeyDown(KeyCode.S))
            Slide();
    }
    private void FixedUpdate()
    {
        var dist = Vector3.Distance(transform.position, m_DestPosition);
        if (dist > 0.2f)
        {
            var dir = (m_DestPosition - transform.position).normalized;
            transform.position += dir * ((sideMovementSpeed + dist) * Time.fixedDeltaTime);
        }
    }

    public void ResetBehaviour()
    {
        m_Animator.Play(PlayerAnims.Run);
        playerPosition = PlayerPosition.Center;
        transform.position = m_CenterPosition;
    }
    public void HitWall()
    {
        PlayAnimation(PlayerAnims.HitWall);
    }
    public void FallFlat()
    {
        PlayAnimation(PlayerAnims.FallFlat);
    }

    public bool OnAnimation(string anim)
    {
        return m_Animator.GetCurrentAnimatorStateInfo(0).IsName(anim);
    }
    private void SwipeDetector_OnSwipe(SwipeData data)
    {
        Debug.Log("Swipe in Direction: " + data.Direction);
        switch (data.Direction)
        {
            case SwipeDirection.Up:
                Jump();
                break;
            case SwipeDirection.Down:
                Slide();
                break;
            case SwipeDirection.Left:
                Left();
                break;
            case SwipeDirection.Right:
                Right();
                break;
            default:
                Debug.LogWarning("Invalid Swipe!");
                break;
        }
    }
    private void Jump()
    {
        if(OnAnimation(PlayerAnims.Run))
            PlayAnimation(PlayerAnims.Jump);
    }
    private void Slide()
    {
        if (OnAnimation(PlayerAnims.Run))
            PlayAnimation(PlayerAnims.Slide);
    }
    private void Left()
    {
        switch (playerPosition)
        {
            case PlayerPosition.Left:
                return;
            case PlayerPosition.Center:
                playerPosition = PlayerPosition.Left;
                m_DestPosition = m_LeftPosition;
                break;
            case PlayerPosition.Right:
                playerPosition = PlayerPosition.Center;
                m_DestPosition = m_CenterPosition;
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void Right()
    {
        switch (playerPosition)
        {
            case PlayerPosition.Left:
                playerPosition = PlayerPosition.Center;
                m_DestPosition = m_CenterPosition;
                break;
            case PlayerPosition.Center:
                playerPosition = PlayerPosition.Right;
                m_DestPosition = m_RightPosition;
                break;
            case PlayerPosition.Right:
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void PlayAnimation(string anim)
    {
        m_Animator.SetTrigger(anim);
    }
}

public static class PlayerAnims
{
    public static string Run = "Run";
    public static string Jump = "Jump";
    public static string Slide = "Slide";
    public static string HitWall = "HitWall";
    public static string FallFlat = "FallFlat";
}