using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region Singleton Management
    static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            // As the scene is going to reload, we dont want to loose the variables values
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(gameObject);
    }
    #endregion

    #region Variables
    GameObject[] pins_arr;
    int pins_in_game;
    int current_pins;
    public int points = 0; // each pin adds 1 point to the score if it is shot down

    int i = 0;// this integer is being used for player 2nd ball shot of each round

    public GameObject pause;
    #endregion

    private void Start()
    {
        #region Set empty array of number of pins in game
        pins_in_game = FindObjectsOfType<Pin>().Length;
        pins_arr = new GameObject[pins_in_game];

        current_pins = pins_in_game;

        // Set the array of pins empty, later set there pins falled
        for (int i = 0; i < pins_in_game; i++)
        {
            pins_arr[i] = null;
        }
        #endregion

        #region Show Keys to the player
        if (!pause.activeSelf)
        {
            Pause();
        }
        #endregion
    }

    private void Update()
    {
        #region Pause detector
        if (Input.GetKeyDown(KeyCode.Escape)) Pause();
        #endregion
    }

    #region PinThrown
    public void PinFalled(bool falled, GameObject pin_)
    {
        // This function recieves if the pin has been shot down, the specific pin that has been shot down
        // It saves the pin which has been knocked down
        if (falled)
        {
            current_pins--;

            #region Sound effects
            // The first pin that has been shot down calls the sound effect
            if (current_pins == pins_in_game - 1)
            {
                Sound_effects.Instance.RollingBall(false);
                Sound_effects.Instance.BallHit();
            }

            // This works if it is the 1st ball of the round, but on the 2nd round we also want to play the hit_ball 
            // sound effect. However by then current_pins == pins_in_game will never happen unless the player does
            //not throw any pin in the 1st ball. To only call once the sound effect, we use int i = 0; The first pin thrown
            // will call the hit sound effect. Then we set i = 1 so that the sound effect is only called once.
            else if (GameObject.FindGameObjectWithTag("ball").GetComponent<Ball>().GetCurrentBall() == 1)
            {
                if (i == 0)
                {
                    Sound_effects.Instance.RollingBall(false);
                    Sound_effects.Instance.BallHit();
                    i = 1;
                }

            }
            #endregion

            // The pins are specifically named ending with a number, which is the number of numeration of the pins
            // We get that number of the pin
            char num = pin_.name[pin_.name.Length - 1];
            int n = int.Parse(num.ToString());

            // We set that pin in the same position as their number, but first we need to check if that pin has already been shot down
            if(pins_arr[n] == null)
            {
                pins_arr[n] = pin_;

                // We add 1 point to the variable points
                points++;
            }

            // Now we need to check if the player has shot down all the pins in game
            if (current_pins <= 0)
            {
                // This means the player has done a strike
                StartCoroutine(Strike());
            }
            else StartCoroutine(HidePinsFalled(pin_));// We hide the pins that has been fallen
        }
    }
    IEnumerator HidePinsFalled(GameObject pin_)
    {
        yield return new WaitForSeconds(2);
        
        // First check if that pin has not been destroyed
        if(pin_ != null) pin_.SetActive(false);
    }
    #endregion

    #region Strike!
    public IEnumerator Strike()
    {
        yield return new WaitForSeconds(2);

        // Set first the score
        ScoreManager.Instance.SetScore(FindObjectOfType<Ball>().GetCurrentBall(), GetPointsValue());

        // Sound effect STRIKE! if it is 1st ball of the round
        if (GameObject.FindGameObjectWithTag("ball").GetComponent<Ball>().GetCurrentBall() == 0)
        {
            Sound_effects.Instance.Strike();
        }

        // Sound effect SPARE! if it is 2nd ball of the round
        else
        {
            Sound_effects.Instance.Spare();
        }

        yield return new WaitForSeconds(1);

        // Move then to the next round
        ScoreManager.Instance.NextRound();
    }
    #endregion

    #region Restart Current Pins
    public void RestartCurrentPins()
    {
        // When the round is finished we need to restart the current pins in game
        current_pins = pins_in_game;
        i = 0;
    }
    #endregion

    #region Get Points
    public int GetPointsValue()
    {
        return points;
    }
    #endregion

    #region Print Pins Falled
    public void ShowPins()
    {
        for (int j = 0; j < pins_in_game; j++)
        {
            if (pins_arr[j] != null) Debug.Log(pins_arr[j].name);
        }
    }
    #endregion

    #region Pause
    void Pause()
    {
        if (pause.activeSelf)
        {
            pause.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            pause.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    #endregion

    #region Resume Button
    public void Resume_button()
    {
        if (pause.activeSelf)
        {
            Pause();
        }
    }
    #endregion

    #region Exit Button
    public void Exit_button()
    {
        if (pause.activeSelf)
        {
            Application.Quit();
        }
    }
    #endregion
}
