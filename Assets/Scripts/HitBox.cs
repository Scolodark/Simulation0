using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum enumHitType
    {
        WallCheck,
        EnemyCheck
    }

    [SerializeField] private enumHitType hitType;
    Player player;
    BoxCollider2D coll;

    void Start()
    {
        player = GetComponentInParent<Player>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //player.TriggerEnter(hitType, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //player.TriggerExit(hitType, collision);
    }

}
