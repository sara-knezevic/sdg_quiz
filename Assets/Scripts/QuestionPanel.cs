using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanel : MonoBehaviour
{
    public GameObject questionPanel;
    public GameObject helpPanel;
    public GameObject infoPanel;
    public GameObject fade;
    public static bool questionPanelState = false;
    private bool infoPanelState = false;
    private Image spriteRenderer;
    public Animator animator;
    public Sprite panelBackground;
    public Sprite coloredGoal;
    public Sprite correct, wrong, neutral;
    public GameObject goalPath;
    public GameObject trees;
    public Button goalButton;
    public Button answerOne, answerTwo, answerThree;
    private List<Button> answers;
    public Button nextButton, lastButton, finishButton, closeButton;
    private string goalTag;

    public Text questionText;
    public Text answerOneText, answerTwoText, answerThreeText;
    
    private TextAsset questionsFile;
    private string[] questionData;

    private int q = 1;
    private int answerCount = 0;
    private int[] answered = new int[3]{0, 0, 0};

    void Start()
    {
        goalTag = goalButton.tag;
        spriteRenderer = gameObject.transform.GetChild(1).GetComponent<Image>();
        spriteRenderer.sprite = panelBackground;

        goalButton.GetComponent<Button>().onClick.AddListener(OnClick);
        nextButton.GetComponent<Button>().onClick.AddListener(NextQuestion);
        lastButton.GetComponent<Button>().onClick.AddListener(LastQuestion);
        finishButton.GetComponent<Button>().onClick.AddListener(TogglePanel);
        closeButton.GetComponent<Button>().onClick.AddListener(TogglePanel);

        answerOne.GetComponent<Button>().onClick.AddListener(delegate{CheckAnswer(answerOne);});
        answerTwo.GetComponent<Button>().onClick.AddListener(delegate{CheckAnswer(answerTwo);});
        answerThree.GetComponent<Button>().onClick.AddListener(delegate{CheckAnswer(answerThree);});

        answers = new List<Button>(){answerOne, answerTwo, answerThree};

        questionsFile = Resources.Load(goalTag + ".txt") as TextAsset;
        questionData = questionsFile.text.Split('\n');
    }

    void OpenQuestionPanel()
    {
        questionPanel.transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        questionPanel.transform.localScale = new Vector2(Camera.main.orthographicSize * 0.0008f, Camera.main.orthographicSize * 0.0008f);
        
        TogglePanel();
        FormQuestion(q);
    }

    void CheckAnswer(Button button)
    {
        if (button == answerOne)
        {
            FillResultArray(goalTag, q, 1);
        } else if (button == answerTwo)
        {
            FillResultArray(goalTag, q, 2);
        } else if (button == answerThree)
        {
            FillResultArray(goalTag, q, 3);
        }

        // true if correct answer
        string check = button.tag; 
        if (check == "True")
        {
            GameManager.points++;
        }

        // nextButton.interactable = true;
        // finishButton.interactable = true;

        answered[q - 1] = 1;
        answerCount++;

        // End of goal and answered all questions
        if (answerCount == 3)
        {
            // Show goal in color and make path red.
            this.gameObject.tag = "Solved";

            spriteRenderer = goalButton.GetComponent<Image>();
            spriteRenderer.sprite = coloredGoal;

            ShowTree(goalTag);
            goalPath.SetActive(true);
            animator.SetBool("Solved", true);
        }
    
        FormQuestion(q);
    }

    void DisableQuestion()
    {
        for (int i = 0; i < 3; i++)
        {
            if (answers[i].tag == "True") {
                spriteRenderer = answers[i].GetComponent<Image>();
                spriteRenderer.sprite = correct;
            } else {
                spriteRenderer = answers[i].GetComponent<Image>();
                spriteRenderer.sprite = neutral;
            }

            answers[i].interactable = false;
        }

        if (answers[GameManager.result_answers[FindResultArray(goalTag, q)] - 1].tag == "False") {
            spriteRenderer = answers[GameManager.result_answers[FindResultArray(goalTag, q)] - 1].GetComponent<Image>();
            spriteRenderer.sprite = wrong;
        }
    }

    void EnableQuestion()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer = answers[i].GetComponent<Image>();
            spriteRenderer.sprite = neutral;

            answers[i].interactable = true;
        }
    }

    public void OnClick()
    {
        helpPanel.SetActive(false);
        GameManager.helpState = false;
		OpenQuestionPanel();
	}

    void TogglePanel()
    {
        questionPanel.SetActive(!questionPanelState);
        fade.SetActive(!questionPanelState);
        questionPanelState = !questionPanelState;

        if (questionPanelState)
        {
            Camera.main.backgroundColor = new Color(0.0039f, 0.2f, 0.3f, 1f);
        } else
        {
            Camera.main.backgroundColor = new Color(0.01176f, 0.49f, 0.73f, 1f);
        }
    }

    public void HoverPanel()
    {
        infoPanel.SetActive(!infoPanelState);
        infoPanelState = !infoPanelState;
    }

    void ToggleButtons()
    {
        switch (q)
        {
            case 1:
                lastButton.gameObject.SetActive(false);
                finishButton.gameObject.SetActive(false);
                break;
            case 3:
                nextButton.gameObject.SetActive(false);
                finishButton.gameObject.SetActive(true);
                break;
            default:
                lastButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(true);
                finishButton.gameObject.SetActive(false);
                break;
        }
    }

    void FormQuestion(int num)
    {
        int step = (num-1) * 4;

        questionText.text = questionData[0 + step];
        answerOneText.text = questionData[1 + step];
        answerTwoText.text = questionData[2 + step];
        answerThreeText.text = questionData[3 + step];

        for (int i = 1; i <= 3; i++)
        {
            string searchResult = SearchTrueAnswer(questionData[i + step]);
            if (searchResult != null)
            {
                answers[i - 1].tag = "True";
                answers[i - 1].gameObject.transform.GetChild(0).GetComponent<Text>().text = searchResult;
            } else
            {
                answers[i - 1].tag = "False";
            }
        }

        if (answered[num - 1] == 0)
        {
            EnableQuestion();
            nextButton.interactable = false;
            finishButton.interactable = false;
        } else
        {
            DisableQuestion();
            nextButton.interactable = true;
            finishButton.interactable = true;
        }
        
        ToggleButtons();
    }

    void FillResultArray(string goalTag, int q, int a)
    {
        int step = 3;
        int q_step = q - 1;
        switch (goalTag)
        {
            case "SDG_Goal_6":
                GameManager.result_answers[q_step] = a;
                break;
            case "SDG_Goal_7":
                GameManager.result_answers[step + q_step] = a;
                break;
            case "SDG_Goal_12":
                GameManager.result_answers[step * 2 + q_step] = a;
                break;
            case "SDG_Goal_13":
                GameManager.result_answers[step * 3 + q_step] = a;
                break;
            case "SDG_Goal_14":
                GameManager.result_answers[step * 4 + q_step] = a;
                break;
            case "SDG_Goal_15":
                GameManager.result_answers[step * 5 + q_step] = a;
                break;
        }
    }

    int FindResultArray(string goalTag, int q)
    {
        int step = 3;
        int q_step = q - 1;
        int result;

        switch (goalTag)
        {
            case "SDG_Goal_6":
                result = q_step;
                break;
            case "SDG_Goal_7":
                result = step + q_step;
                break;
            case "SDG_Goal_12":
                result = step * 2 + q_step;
                break;
            case "SDG_Goal_13":
                result = step * 3 + q_step;
                break;
            case "SDG_Goal_14":
                result = step * 4 + q_step;
                break;
            case "SDG_Goal_15":
                result = step * 5 + q_step;
                break;
            default:
                result = -1;
                break;
        }

        return result;
    }

    string SearchTrueAnswer(string answer)
    {
        if (answer.Substring(answer.Length - 2).Trim().Equals("T"))
        {
            answer = answer.Substring(0, answer.Length - 2);
            return answer;
        }

        return null;
    }

    void NextQuestion()
    {
        if (q < 3) {
            q++;
        }

        FormQuestion(q);
    }

    void LastQuestion()
    {
        if (q > 1) {
            q--;
        }

        FormQuestion(q);
    }

    void ShowTree(string goalTag)
    {
        switch (goalTag)
        {
            case "SDG_Goal_6":
                trees.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "SDG_Goal_7":
                trees.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case "SDG_Goal_12":
                trees.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case "SDG_Goal_13":
                trees.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case "SDG_Goal_14":
                trees.transform.GetChild(4).gameObject.SetActive(true);
                break;
        }
    }
}
