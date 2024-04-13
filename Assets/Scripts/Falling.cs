using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MonoBehaviour
{
    [SerializeField] Player player;

    // Start is called before the first frame update
    void Start()
    {

    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.DieEffect();
    }
}
