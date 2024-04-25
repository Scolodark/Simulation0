using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBgm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBgm(SoundManager.Bgm.GameOver);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
