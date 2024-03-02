using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Solo : MonoBehaviour
{

    [Header("UI Elements")]
    public TextMeshProUGUI questionText;
    public Button answer1Button;
    public Button answer2Button;
    public Button answer3Button;
    public Button answer4Button;
    public Slider timerSlider;
    public Button continueButton;
    public TextMeshProUGUI scoreText;
    
    [Header("UI Plus")]
    // sprite pour les boutons
    public Sprite buttonSpriteOK;
    public Sprite buttonSpriteKO;
    public Sprite buttonSpriteNeutral;  

    [Header("Options du jeu")]
    private float timeToAnswer = 20f;
    private float timeRemaining = 0f;
    private bool timerIsRunning = false;
    private Button goodAnswerButton;


    private TextMeshProUGUI answer1Text;
    private TextMeshProUGUI answer2Text;
    private TextMeshProUGUI answer3Text;
    private TextMeshProUGUI answer4Text;
    

    void setupObjects(){
        answer1Text = answer1Button.GetComponentInChildren<TextMeshProUGUI>();
        answer2Text = answer2Button.GetComponentInChildren<TextMeshProUGUI>();
        answer3Text = answer3Button.GetComponentInChildren<TextMeshProUGUI>();
        answer4Text = answer4Button.GetComponentInChildren<TextMeshProUGUI>();
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(delegate { StartCoroutine(SetQuestion()); });
    }

    // Start is called before the first frame update
    void Start()
    {
        // if PlayerPrefs Score doesn't exist, create it
        if (!PlayerPrefs.HasKey("score"))
        {
            PlayerPrefs.SetInt("score", 0);
        } 
        updateScore(PlayerPrefs.GetInt("score"));
        setupObjects();
        StartCoroutine(InitializeGame());
    }

    void updateScore(int addToScore){
        int score = int.Parse(scoreText.text.Split(' ')[2]) + addToScore;
        scoreText.text = "Score : " + score.ToString();
        PlayerPrefs.SetInt("score", score);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            timeRemaining += Time.deltaTime;
            if (timeRemaining >= timeToAnswer)
            {
                timerIsRunning = false;
                no_answer();
            }
        }
        timerSlider.value = timeRemaining / timeToAnswer;

    }

    #region Listener des boutons
    public void no_answer()
    {
        answer1Button.onClick.RemoveAllListeners();
        answer2Button.onClick.RemoveAllListeners();
        answer3Button.onClick.RemoveAllListeners();
        answer4Button.onClick.RemoveAllListeners();
        answer1Button.GetComponent<Image>().sprite = buttonSpriteKO;
        answer2Button.GetComponent<Image>().sprite = buttonSpriteKO;
        answer3Button.GetComponent<Image>().sprite = buttonSpriteKO;
        answer4Button.GetComponent<Image>().sprite = buttonSpriteKO;
        goodAnswerButton.GetComponent<Image>().sprite = buttonSpriteOK;
        timerIsRunning = false;
        continueButton.gameObject.SetActive(true);
        updateScore((int)(-PlayerPrefs.GetInt("score") / 2f));
    }

    public void btn_WrongAnswer(GameObject button)
    {        
        answer1Button.onClick.RemoveAllListeners();
        answer2Button.onClick.RemoveAllListeners();
        answer3Button.onClick.RemoveAllListeners();
        answer4Button.onClick.RemoveAllListeners();
        timerIsRunning = false;
        // change la source de l'image du bouton
        button.GetComponent<Image>().sprite = buttonSpriteKO;
        // find the right answer and change its color
        goodAnswerButton.GetComponent<Image>().sprite = buttonSpriteOK;
        continueButton.gameObject.SetActive(true);
        updateScore((int)(-PlayerPrefs.GetInt("score") / 2f));
    }
    public void btn_RightAnswer(GameObject button)
    {
        answer1Button.onClick.RemoveAllListeners();
        answer2Button.onClick.RemoveAllListeners();
        answer3Button.onClick.RemoveAllListeners();
        answer4Button.onClick.RemoveAllListeners();
        timerIsRunning = false;
        // change la source de l'image du bouton
        button.GetComponent<Image>().sprite = buttonSpriteOK;
        continueButton.gameObject.SetActive(true);
        updateScore(1);
    }
    #endregion


    #region Parsing JSON + Affichage dans l'UI
    [System.Serializable]
    public class QuizQuestion
    {
        public int id;
        public string question;
        public List<Answer> answers;
    }
    [System.Serializable]
    public class Answer
    {
        public string text;
        public bool isCorrect;
    }

    // Méthode pour traiter le JSON
    public void affQuestion(string json)
    {

        // Désérialisation du JSON en objet QuizQuestion
        QuizQuestion quizQuestion = JsonUtility.FromJson<QuizQuestion>(json);

        // Affichage des éléments dans l'UI
        questionText.text = quizQuestion.question;


        // put answers in buttons randomly and add listeners
        List<Answer> answers = quizQuestion.answers;

        int rdm = Random.Range(0, answers.Count);
        answer1Text.text = answers[rdm].text;
        if (answers[rdm].isCorrect)
        {
            answer1Button.onClick.AddListener(delegate { btn_RightAnswer(answer1Button.gameObject); });
            goodAnswerButton = answer1Button;
        }
        else
        {
            answer1Button.onClick.AddListener(delegate { btn_WrongAnswer(answer1Button.gameObject); });
        }
        answers.RemoveAt(rdm);

        rdm = Random.Range(0, answers.Count);
        answer2Text.text = answers[rdm].text;
        if (answers[rdm].isCorrect)
        {
            answer2Button.onClick.AddListener(delegate { btn_RightAnswer(answer2Button.gameObject); });
            goodAnswerButton = answer2Button;
        }
        else
        {
            answer2Button.onClick.AddListener(delegate { btn_WrongAnswer(answer2Button.gameObject); });
        }
        answers.RemoveAt(rdm);

        rdm = Random.Range(0, answers.Count);
        answer3Text.text = answers[rdm].text;
        if (answers[rdm].isCorrect)
        {
            answer3Button.onClick.AddListener(delegate { btn_RightAnswer(answer3Button.gameObject); });
            goodAnswerButton = answer3Button;
        }
        else
        {
            answer3Button.onClick.AddListener(delegate { btn_WrongAnswer(answer3Button.gameObject); });
        }
        answers.RemoveAt(rdm);

        rdm = Random.Range(0, answers.Count);
        answer4Text.text = answers[rdm].text;
        if (answers[rdm].isCorrect)
        {
            answer4Button.onClick.AddListener(delegate { btn_RightAnswer(answer4Button.gameObject); });
            goodAnswerButton = answer4Button;
        }
        else
        {
            answer4Button.onClick.AddListener(delegate { btn_WrongAnswer(answer4Button.gameObject); });
        }
        answers.RemoveAt(rdm);


        // Réinitialisation des couleurs des boutons
        answer1Button.GetComponent<Image>().color = Color.white;
        answer2Button.GetComponent<Image>().color = Color.white;
        answer3Button.GetComponent<Image>().color = Color.white;
        answer4Button.GetComponent<Image>().color = Color.white;
        answer1Button.GetComponent<Image>().sprite = buttonSpriteNeutral;
        answer2Button.GetComponent<Image>().sprite = buttonSpriteNeutral;
        answer3Button.GetComponent<Image>().sprite = buttonSpriteNeutral;
        answer4Button.GetComponent<Image>().sprite = buttonSpriteNeutral;
        


        // Timer
        timeRemaining = 0;
        timerIsRunning = true;
       
    }
    #endregion



    IEnumerator SetQuestion()
    {
        continueButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        UnityWebRequest www = UnityWebRequest.Get(ConfigReader.GetValue("serverAddress")+"/get_rdm_question");
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            affQuestion(www.downloadHandler.text);
        }
        else
        {
            Debug.Log(www.error);
        }
    }



    IEnumerator InitializeGame()
    {
        questionText.text = "Prêt ?";
        answer1Text.text = "3";
        answer2Text.text = "3";
        answer3Text.text = "3";
        answer4Text.text = "3";
        yield return new WaitForSeconds(1);

        answer1Text.text = "2";
        answer2Text.text = "2";
        answer3Text.text = "2";
        answer4Text.text = "2";
        yield return new WaitForSeconds(1);

        answer1Text.text = "1";
        answer2Text.text = "1";
        answer3Text.text = "1";
        answer4Text.text = "1";
        yield return new WaitForSeconds(1);

        StartCoroutine(SetQuestion());
    }

}