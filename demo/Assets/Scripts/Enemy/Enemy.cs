using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public GameObject diamondPrefab;

    [SerializeField]
    protected int health;
    [SerializeField]
    protected int speed;
    [SerializeField]
    protected int gems;
    [SerializeField]
    protected Transform pointA, pointB;

    protected Vector3 targetPosition;
    protected Animator anim;
    protected SpriteRenderer sprite;
    protected bool isHit;
    protected bool isDead = false;

    //Fetch Player GameObject
    protected Player player;

    public virtual void Init()
    {
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        Init();
    }

    public virtual void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && anim.GetBool("InCombat") == false)
        {
            return;
        }
        if(isDead == false)
            Movement();
    }

    public virtual void Movement()
    {
        if (targetPosition == pointA.position)
        {
            sprite.flipX = true;
        }
        else if (targetPosition == pointB.position)
        {
            sprite.flipX = false;
        }

        if (transform.position == pointA.position)
        {
            targetPosition = pointB.position;
            anim.SetTrigger("Idle");
            //_skeletonSprite.flipX = false;
        }
        else if (transform.position == pointB.position)
        {
            targetPosition = pointA.position;
            anim.SetTrigger("Idle");
            //_skeletonSprite.flipX = true;
        }

        if(isHit == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        //Get Enemy to Player Distance
        float toPlayerDistance = Vector3.Distance(transform.position, player.transform.position);
        
        if(toPlayerDistance > 5.0f)
        {
           
            isHit = false;
            anim.SetBool("InCombat", false);
        }

        //Check Player Direction on Position
        Vector3 playerDirection = player.transform.localPosition - transform.localPosition;
        //Debug.Log("X side: " + playerDirection.x);
        if (playerDirection.x > 0 && anim.GetBool("InCombat") == true)
        {
            sprite.flipX = false;
        }
        else if (playerDirection.x < 0 && anim.GetBool("InCombat") == true)
        {
            sprite.flipX = true;
        }
    }
}
