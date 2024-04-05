using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    #region Singleton Management
    //Campo privado que referencia a esta instancia
    static ScoreManager instance;

    /// <summary>
    /// Propiedad pública que devuelve una referencia a esta instancia
    /// Pertenece a la clase, no a esta instancia
    /// Proporciona un punto de acceso global a esta instancia
    /// </summary>
    public static ScoreManager Instance
    {
        get { return instance; }
    }

    //Constructor
    void Awake()
    {
        
        //Asigna esta instancia al campo instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(gameObject);  //Garantiza que sólo haya una instancia de esta clase
    }
    #endregion

    #region Variables
    public TMP_Text text_round;
    public TMP_Text [] text_score_ball = new TMP_Text[2];
    int[] score_ball = {0,0};
    public TMP_Text text_total_score;
    int round = 1, total_score = 0;
    #endregion

    #region Texts Start & Update
    private void Start()
    {
        SetTextVariables();
    }

    private void Update()
    {
        SetTextVariables();
    }

    void SetTextVariables()
    {
        text_round.text = "Round " + round;
        text_score_ball[0].text = score_ball[0] + "";
        text_score_ball[1].text = score_ball[1] + "";
        text_total_score.text = total_score + "";
    }
    #endregion

    #region Set Score after pins falled
    public void SetScore(int ball, int points) // Ball represents the actual turn of the round, cause each round has 2 turns
        // It helps us to know which variable are we going to change, score_ball[0] or score_ball[1]
    {
        if(ball != 0 && ball != 1)
        {
            Debug.LogError("The number of ball on each round has to be a value between 0 and 1");
        }
        else
        {
            score_ball[ball] += points;
            total_score += points;
        }
    }
    #endregion

    #region Next Round
    public void NextRound()
    {
        round++;
        score_ball[0] = 0;
        score_ball[1] = 0;
        GameManager.Instance.RestartCurrentPins();
        GameManager.Instance.points = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}
