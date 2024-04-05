using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    #region Variables
    private Rigidbody _rb;
    public float impulse = 10;
    Vector3 initialPosition;
    bool canShot = true;
    public float resetTime = 5;
    float horizontal_ball_vel = 0.002f;
    float left_pin;// this value is getting the X position of the closer pin to the left side
    float right_pin;// this value is getting the X position of the closer pin to the right side
    float current_ball_mov_X;
    int current_ball = 0;
    #endregion
    void Start()
    {
        #region Start
        _rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        GetMaxPin_X();
        #endregion
    }

    private void FixedUpdate()
    {
        #region PlayerThrowingBall
        if (Input.GetButtonDown("Jump") && canShot)
        {
            FreezeBallRotation(false);
            _rb.AddForce(Vector3.forward * impulse, ForceMode.Impulse);
            canShot = false;

            // We add some noise to the vmCam to make the shot more realistic
            CM_vcam.Noise(true);

            // Sound effect of the ball rolling
            Sound_effects.Instance.RollingBall(true);

            Invoke(nameof(ResetBall), resetTime);
        }
        if (canShot) MoveBallDirection();
        #endregion
        #region Locate Ball X
        if (canShot) current_ball_mov_X = transform.position.x;
        #endregion
    }

    #region ResetBallPos
    public void ResetBall()
    {
        _rb.isKinematic = true;
        transform.position = initialPosition;
        canShot = true;
        _rb.isKinematic = false;
        ScoreManager.Instance.SetScore(current_ball, GameManager.Instance.GetPointsValue());
        GameManager.Instance.points = 0;
        NextBall();

        //We remove the noise effect we added before to the player shot
        CM_vcam.Noise(false);
    }
    #endregion

    #region Next Ball
    void NextBall()
    {
        if (current_ball == 0) current_ball = 1;

        else if (current_ball == 1)
        {
            current_ball = 0;
            ScoreManager.Instance.NextRound();
        }
        else Debug.Log("The number of ball on each round has to be a value between 0 and 1");
    }
    #endregion

    #region Get Current Ball
    public int GetCurrentBall()
    {
        return current_ball;
    }
    #endregion

    #region MoveBallDirection
    void MoveBallDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        FreezeBallRotation(true);
        OffsetBallIfMaxX();
        if (x < 0 && current_ball_mov_X > left_pin && current_ball_mov_X < right_pin)// ball moves left
        {
            transform.position -= new Vector3(horizontal_ball_vel, 0, 0);
        }
        else if(x > 0 && current_ball_mov_X > left_pin && current_ball_mov_X < right_pin)// ball moves right
        {
            transform.position += new Vector3(horizontal_ball_vel, 0, 0);
        }
    }
    #endregion

    #region Offset Ball X If Limit
    void OffsetBallIfMaxX()
    {
        float offset_x = 0.001f;
        if(current_ball_mov_X <= left_pin)// ball is on closer pin to the left side
        {
            transform.position += new Vector3(offset_x, 0, 0);
        }
        else if(current_ball_mov_X >= right_pin)// ball is on closer pin to the right side
        {
            transform.position -= new Vector3(offset_x, 0, 0);
        }
    }
    #endregion

    #region FreezeBallRotation
    void FreezeBallRotation(bool freeze_rot)
    {
        _rb.freezeRotation = freeze_rot;
    }
    #endregion

    #region Get MAX pin X
    void GetMaxPin_X()
    {
        left_pin = GameObject.FindGameObjectWithTag("pin_max_left").transform.position.x;
        right_pin = GameObject.FindGameObjectWithTag("pin_max_right").transform.position.x;
    }
    #endregion

    #region Player does not throw any pin
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("stop_ball"))
        {
            Sound_effects.Instance.RollingBall(false);
        }
    }
    #endregion
}
