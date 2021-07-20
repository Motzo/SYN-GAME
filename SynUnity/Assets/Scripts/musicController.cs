using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicController : MonoBehaviour
{
    public AudioSource loopMusic;
    public AudioSource introMusic;
    public float introMusicEndTime = 15;

    void Update()
    {
        if(introMusic.time >= introMusicEndTime && !loopMusic.isPlaying){
            loopMusic.Play();
        }
    }
}
