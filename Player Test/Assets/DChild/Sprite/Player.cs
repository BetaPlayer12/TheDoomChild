﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public int diamonds;

    private Rigidbody2D _rigid;
    [SerializeField]
    private float _playerSpeed = 5.0f;
    [SerializeField]
    private float _jumpVal = 8.0f;
    private bool _resetJump = false;
    [SerializeField]
    private LayerMask _groundLayer;
    private bool _grounded = false;
    private PlayerAnimation _playerAnim;
    private SpriteRenderer _spritePlayer;
    private float _oldVal;

    public int Health { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponentInChildren<PlayerAnimation>();
        _spritePlayer = GetComponentInChildren<SpriteRenderer>();
        Health = 4;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        //Attack
        if (Input.GetMouseButtonDown(0) && IsGrounded() == true)
        {
            _playerAnim.Attack();
        }
        
    }

    void Movement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        int speedMultiplier = 1;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedMultiplier = 2;
        }
        _grounded = IsGrounded();
        //Flip Sprite
        //Flip(move);
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() == true)
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpVal + speedMultiplier);
            StartCoroutine(ResetJumpRoutine());
            //_playerAnim.Jump(true);
        }


        _rigid.velocity = new Vector2(move * _playerSpeed * speedMultiplier, _rigid.velocity.y);
        _playerAnim.Move(move);
        //Quick Load Scene
        if (_rigid.position.y < -6)
        {
            SceneManager.LoadScene("SampleScene");
        }

    }

    bool IsGrounded()
    {
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, _groundLayer.value);

        if (this.transform.position.y > _oldVal)
        {
            Debug.Log("Inc");
            _playerAnim.Jump(true, true);
        }
        else if (this.transform.position.y < _oldVal)
        {
            Debug.Log("Dec");
            _playerAnim.Jump(true, false);
        }
        else
        {
            Debug.Log("No Change");
        }
        _oldVal = this.transform.position.y;

        //CHeck RayCast line
        Debug.DrawRay(transform.position, Vector2.down*0.2f, Color.green);
        if (rayInfo.collider != null)
        {
            if (_resetJump == false)
            {
                //_playerAnim.Jump(false);
                return true;
            }

        }
        return false;
    }
    void Flip(float move)
    {

        if (move > 0)
        {
            _spritePlayer.flipX = false;
        }
        else if (move < 0)
        {
            _spritePlayer.flipX = true;
        }
    }
    IEnumerator ResetJumpRoutine()
    {
        _resetJump = true;
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }

    public void Damage(int damageAmount)
    {
        if (Health < 1)
        {
            return;
        }
        Debug.Log("Player Damage");
        Health--;



    }

    public void AddGems(int amount)
    {
        diamonds += amount;
    }



}
