using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerController : MonoBehaviour
{
   

    private Rigidbody2D _rigid;
   
    private PlayerAnimation _playerAnim;
    private SpriteRenderer _spritePlayer;

    public int Health { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<PlayerAnimation>();
        _spritePlayer = GetComponentInChildren<SpriteRenderer>();
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _playerAnim.CrouchWalk(true, true, -1);
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            _playerAnim.CrouchWalk(false, true, 1);
        }
    }

}
