using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBgm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBgm(SoundManager.Bgm.GameClear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
