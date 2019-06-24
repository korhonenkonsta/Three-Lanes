using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public Player player2;

    public GameObject player2Prefab;

    public Hand player1Hand;
    public Hand player2Hand;

    public Deck player1Deck;
    public Deck player2Deck;

    public DiscardPile player1DiscardPile;
    public DiscardPile player2DiscardPile;

    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    public TextMeshProUGUI player1ResourcesText;
    public TextMeshProUGUI player2ResourcesText;

    public GameObject basePrefab;

    public Transform player1Base1;
    public Transform player1Base2;
    public Transform player1Base3;

    public Transform player2Base1;
    public Transform player2Base2;
    public Transform player2Base3;

    public Lane lane1;
    public Lane lane2;
    public Lane lane3;

    public GameObject buildAreaPrefab;
    public Transform player1BuildAreaRef;
    public Transform player2BuildAreaRef;

    public GameObject gameOverPanel;

    [Range(0.0f, 10.0f)]
    public float timeScaleValue = 1f;

    //public GameObject buildingToBuild;

    //Singleton
    public static GameManager Instance { get; private set; }

    //Make sure that only one GameManager exists
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        StartMatch();
    }

    public void StartMatch()
    {
        CreatePlayer2();
        player1.opponent = player2;
        player1.roundScore = 0;
        UpdateScoreTexts();
        CreateBases();
        CreateBuildAreas(15);
    }

    public void CreatePlayer2()
    {
        player2 = Instantiate(player2Prefab, transform.position, transform.rotation).GetComponent<Player>();
        player2.resources = 20;
        player2.startingResources = player2.resources;
        player2.opponent = player1;
        player2.gm = this;
        player2.discardPile = player2DiscardPile;
        player2.discardPile.owner = player2;
        player2.hand = player2Hand;
        player2.hand.owner = player2;
        player2.deck = player2Deck;
        player2.deck.owner = player2;
        player2.deck.AddChildCardsToList();
        player2.gameObject.AddComponent<AI>().p = player2;

        player2.hand.DrawHand(5);
        player2.hand.StartDrawing();

        player1.deck.AddChildCardsToList();
        player1.hand.DrawHand(5);
        player1.hand.StartDrawing();
        player1.startingResources = player1.resources;

    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void RestartMatch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartMatch();
    }

    /// <summary>
    /// Reset the round, which includes:
    ///     Bases,
    ///     Non-permanent buildings,
    ///     Units
    /// </summary>
    public void ResetRound()
    {
        SearchAndDestroyTag();
        CreateBases();
        player1.ClearEnemyLists();
        player2.ClearEnemyLists();
        player1.ResetBuildAreas();
        player2.ResetBuildAreas();

        player1.roundExtraResources = 0;
        player2.roundExtraResources = 0;
    }

    public void SearchAndDestroyTag()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject unit in units)
            Destroy(unit);
    }

    public void CreateBases()
    {
        print("bases created");
        player1.baseCount = 3;
        player2.baseCount = 3;

        float laneGap = 2f;

        for (int i = 0; i < 3; i++)
        {
            Base tempBase = Instantiate(basePrefab, player1Base1.position + new Vector3(i * laneGap, 0f, 0f), player1Base1.rotation).GetComponent<Base>();
            tempBase.owner = player1;
            tempBase.gameObject.layer = 2;
            tempBase = Instantiate(basePrefab, player2Base1.position + new Vector3(i * laneGap, 0f, 0f), player2Base1.rotation).GetComponent<Base>();
            tempBase.owner = player2;
            tempBase.gameObject.layer = 2;
        }
    }

    public void CreateBuildAreas(int countPerSide)
    {
        float buildAreaWidth = 0.2f + 0.1f;
        float laneGap = 0.5f;
        int gapCount = 0;
        int buildAreasPerLane = 5;

        int j = 0;

        for (int i = 0; i < countPerSide; i++)
        {
            if (j >= buildAreasPerLane)
            {
                gapCount++;
                j = 0;
            }
            BuildArea tempArea = Instantiate(buildAreaPrefab, player1BuildAreaRef.position + new Vector3(i * buildAreaWidth + gapCount * laneGap, 0f, 0f), player1BuildAreaRef.rotation).GetComponent<BuildArea>();
            tempArea.owner = player1;
            player1.availableBuildAreas.Add(tempArea);
            player1.allBuildAreas.Add(tempArea);

            if (gapCount + 1 == 1)
            {
                tempArea.currentLane = lane1;
            }
            else if (gapCount + 1 == 2)
            {
                tempArea.currentLane = lane2;
            }
            else
            {
                tempArea.currentLane = lane3;
            }

            tempArea = Instantiate(buildAreaPrefab, player2BuildAreaRef.position + new Vector3(i * buildAreaWidth + gapCount * laneGap, 0f, 0f), player2BuildAreaRef.rotation).GetComponent<BuildArea>();
            tempArea.owner = player2;
            player2.availableBuildAreas.Add(tempArea);
            player2.allBuildAreas.Add(tempArea);

            if (gapCount + 1 == 1)
            {
                tempArea.currentLane = lane1;
            }
            else if (gapCount + 1 == 2)
            {
                tempArea.currentLane = lane2;
            }
            else
            {
                tempArea.currentLane = lane3;
            }
            j++;
        }
    }

    public void UpdateScoreTexts()
    {
        //Need to set players on match start, instantiate at least player2
        player1ScoreText.text = player1.roundScore.ToString();
        player2ScoreText.text = player2.roundScore.ToString();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            player1.LoseRound();
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            player1.roundScore++;
            UpdateScoreTexts();
            ResetRound();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            foreach (GameObject c in player1.hand.cards)
            {
                //c.GetComponent<Card>().buildingPrefab;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (timeScaleValue != 0f)
            {
                timeScaleValue = 0f;
            }
            else
            {
                timeScaleValue = 1f;
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            player1.hand.DrawHand(3);
            player2.hand.DrawHand(3);
        }

        player1ResourcesText.text = "Mana: " + player1.resources.ToString() + " + " + player1.roundExtraResources.ToString();
        player2ResourcesText.text = "Mana: " + player2.resources.ToString() + " + " + player2.roundExtraResources.ToString();

        Time.timeScale = timeScaleValue;
    }
}
