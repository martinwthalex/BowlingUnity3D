using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound_effects : MonoBehaviour
{
    #region Singleton Management
    static Sound_effects instance;

    public static Sound_effects Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    #endregion

    #region Variables
    AudioSource src;
    public AudioClip strike, spare, rolling_ball, ball_hit;
    #endregion

    #region Start
    private void Start()
    {
        src = GetComponent<AudioSource>();
    }
    #endregion

    #region Sound effects voids
    public void Strike()
    {
        src.clip = strike;
        src.Play();
    }

    public void Spare()
    {
        src.clip = spare;
        src.Play();
    }

    public void RollingBall(bool rolling)
    {
        if (rolling)
        {
            src.clip = rolling_ball;
            src.Play();
        }
        else
        {
            if(src.clip == rolling_ball)
            {
                src.clip = null;
                src.Stop();
            }
        }
    }

    public void BallHit()
    {
         src.clip = ball_hit;
         src.Play();        
    }
    #endregion
}
