using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewPlayer : MonoBehaviour
{

    public int diamonds;

    private Rigidbody2D _rigid;
    [SerializeField]
    private float _playerSpeed = 3.0f;
    [SerializeField]
    private float _jumpVal = 5.0f;
    private bool _resetJump = false;
    private bool lastState = false;
    [SerializeField]
    private LayerMask _groundLayer;
    private bool _grounded = false;
    private NewPlayerAnimation _playerAnim;
    private SpriteRenderer _spritePlayer;

    public int Health { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<NewPlayerAnimation>();
        _spritePlayer = GetComponentInChildren<SpriteRenderer>();
        Health = 4;
    }

    // Update is called once per frame
    void FixedUpdate()
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
        
        
        if(move > 0)
        {
            _playerAnim.Move(move, false);
            lastState = false;
        }
        else if (move < 0)
        {
            _playerAnim.Move(move, true);
            lastState = true;
        }
        else if(move == 0)
        {
            _playerAnim.Move(move,false);
        }
        Debug.Log(move);
        //_playerAnim.Move(move, );
        _grounded = IsGrounded();
       
        //Flip(move);
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() == true)
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpVal);
            StartCoroutine(ResetJumpRoutine());
            _playerAnim.Jump(true);
        }


        _rigid.velocity = new Vector2(move * _playerSpeed, _rigid.velocity.y);
        //_playerAnim.Move(move);
        //Quick Load Scene
        if (_rigid.position.y < -6)
        {
            SceneManager.LoadScene("SampleScene");
        }
        
    }

    bool IsGrounded()
    {
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, _groundLayer.value);
        //CHeck RayCast line
        //Debug.DrawRay(transform.position, Vector2.down, Color.green);
        if (rayInfo.collider != null)
        {
            if (_resetJump == false)
            {
                _playerAnim.Jump(false);
                return true;
            }

        }
        return false;
    }
    void Flip(float move)
    {

       if(move > 0)
        {
            Debug.Log("right");
        }else if(move < 0)
        {
            Debug.Log("Left");
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
        //Health--;
        //UIManager.Instance.UpdateHealth(Health);

        //if (Health < 1)
        //{
        //    _playerAnim.Death();
        //}

    }

    public void AddGems(int amount)
    {
        diamonds += amount;
        //UIManager.Instance.UpdateGemCount(diamonds);
    }



}
