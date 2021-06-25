using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static Vector2 resolution = new Vector2(1024f, 768f);
    public GameObject helpPanel;
    public GameObject infoPanel;
    public GameObject fade;
    private bool infoPanelState = false;
    public static bool helpState = false;
    public static bool endPanelState = false;
    public Button chest;
    public GameObject endPanel;
    private Image spriteRenderer;
    public Sprite openChest;
    public Text finalScore;
    public Text finalTitle;

    public static int points = 0;
    public static int[] result_answers = new int[18]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    private bool gameOver = false;
    public GameObject progressSquare;
    public GameObject progressBar;

    // void Start()
    // {
    //     fillProgressBar();
    //     IndicateLevel();
    // }

    // compatibilityCheck:function(e,t,r){t();}

    void Update()
    {
        if (!gameOver)
        {
            gameOver = isFinished();
        }
    }
    
    public void ShowHelp()
    {
        helpPanel.SetActive(!helpState);
        helpState = !helpState;
    }

    public void HoverPanel()
    {
        infoPanel.SetActive(!infoPanelState);
        infoPanelState = !infoPanelState;
    }

    public void EndPanel()
    {
        endPanel.SetActive(!endPanelState);
        fade.SetActive(!endPanelState);
        helpPanel.SetActive(false);
        helpState = false;
        endPanelState = !endPanelState;

        endPanel.transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        endPanel.transform.localScale = new Vector2(Camera.main.orthographicSize * 0.0009f, Camera.main.orthographicSize * 0.0009f);
        
        if (endPanelState)
        {
            Camera.main.backgroundColor = new Color(0.0039f, 0.2f, 0.3f, 1f);
        } else
        {
            Camera.main.backgroundColor = new Color(0.01176f, 0.49f, 0.73f, 1f);
        }
    }
    
    bool isFinished()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Solved");

        if (goals.Length == 6)
        {
            Finished();
            return true;
        }

        return false;
    }

    void Finished()
    {
        spriteRenderer = chest.GetComponent<Image>();
        spriteRenderer.sprite = openChest;
        chest.interactable = true;
        fillProgressBar();
        IndicateLevel();
    }

    void fillProgressBar()
    {
        GameObject bottom = progressBar.transform.GetChild(1).gameObject;
        if (points == 1)
        {
            bottom.gameObject.SetActive(true);
        } else if (points == 18)
        {
            progressBar.transform.GetChild(0).gameObject.SetActive(true);
        } else if (points > 0)
        {
            bottom.gameObject.SetActive(true);
            for (int i = 1; i < points; i++)
            {
                Vector2 step = bottom.transform.localPosition;
                step.x = 0;
                step.y += i * 78;
                GameObject pointSquare = Instantiate(progressSquare, new Vector2(0, 0), Quaternion.identity) as GameObject;
                pointSquare.transform.SetParent(progressBar.transform);
                pointSquare.transform.localScale = new Vector2(1, 1);
                pointSquare.transform.localPosition = step;
            }
        }
    }

    void IndicateLevel()
    {
        GameObject dumboo = progressBar.transform.GetChild(2).gameObject;
        GameObject lowerInter = progressBar.transform.GetChild(3).gameObject;
        GameObject upperInter = progressBar.transform.GetChild(4).gameObject;
        GameObject expert = progressBar.transform.GetChild(5).gameObject;

        Color fullColor = new Color(255, 255, 255, 1f);
        if (points == 0)
        {
            finalScore.text = "Osvojili ste " + points + " poena!";
            finalTitle.text = "Posedujete neodrživo znanje o Agendi 2030.";
        } else if (points < 6)
        {
            dumboo.GetComponent<Image>().color = fullColor;
            finalScore.text = "Osvojili ste " + points + " poena!";
            finalTitle.text = "Posedujete osnovno znanje o Agendi 2030.";
        } else if (points >= 6 && points < 12)
        {
            dumboo.GetComponent<Image>().color = fullColor;
            lowerInter.GetComponent<Image>().color = fullColor;
            finalScore.text = "Osvojili ste " + points + " poena!";
            finalTitle.text = "Na dobrom ste putu ka održivom poznavanju Agende 2030.";
        } else if (points >= 12 && points < 18)
        {
            dumboo.GetComponent<Image>().color = fullColor;
            lowerInter.GetComponent<Image>().color = fullColor;
            upperInter.GetComponent<Image>().color = fullColor;
            finalScore.text = "Osvojili ste " + points + " poena!";
            finalTitle.text = "Posedujete održivo znanje o Agendi 2030.";
        } else if (points == 18)
        {
            dumboo.GetComponent<Image>().color = fullColor;
            lowerInter.GetComponent<Image>().color = fullColor;
            upperInter.GetComponent<Image>().color = fullColor;
            expert.GetComponent<Image>().color = fullColor;
            finalScore.text = "Osvojili ste " + points + " poena!";
            finalTitle.text = "Posedujete ekspertsko poznavanje Agende 2030.";
        }
    }
}
