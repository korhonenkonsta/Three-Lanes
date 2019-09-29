using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public Player player2;

    public GameObject player2Prefab;

    public Hand player2Hand;
    public Deck player2Deck;
    public DiscardPile player2DiscardPile;
    public Inventory player2Inventory;

    public GameObject playerInfoPanel;
    public TextMeshProUGUI playe2AIText;

    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    public TextMeshProUGUI player1ResourcesText;
    public TextMeshProUGUI player2ResourcesText;

    public GameObject basePrefab;

    public Transform player1Base1;
    //public Transform player1Base2;
    //public Transform player1Base3;

    public Transform player2Base1;
    //public Transform player2Base2;
    //public Transform player2Base3;

    public Lane lane1;
    public Lane lane2;
    public Lane lane3;

    public GameObject buildAreaPrefab;
    public Transform player1BuildAreaRef;
    public Transform player2BuildAreaRef;

    public GameObject gameOverPanel;

    [Range(0.0f, 10.0f)]
    public float timeScaleValue = 1f;

    public List<GameObject> allCardPrefabs = new List<GameObject>();
    public List<GameObject> allItemPrefabs = new List<GameObject>();
    public List<GameObject> tempAllCardPrefabs = new List<GameObject>();
    public Transform rewardsLayoutGroup;
    public int rewardCount = 3;
    public int rewardsPickedCount;
    public int rewardDraftAmount = 3;

    private bool showInventory;

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
        LoadNextScene();
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        if (scene.name == "Match")
        {
            StartMatch();
        }

        if (scene.name == "Reward")
        {
            playerInfoPanel.SetActive(false);
            GiveRewards(allCardPrefabs);
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadNextScene()
    {
        //player1.hand.ShuffleHandToDeck();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene()
    {
        gameOverPanel.SetActive(false);
        player1.hand.ShuffleHandToDeck();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void PickReward(Transform button)
    {
        GameObject pickedReward = rewardsLayoutGroup.GetChild(button.GetSiblingIndex()).gameObject;

        if (pickedReward.GetComponent<Item>())
        {
            //player1.inventory.items.Add(pickedReward); NOT NEEDED CURRENTLY, ALREADY ADDED AT START OF MATCH
            pickedReward.transform.SetParent(player1.inventory.transform);
        }
        else
        {
            //player1.deck.cards.Add(pickedReward); NOT NEEDED CURRENTLY, ALREADY ADDED AT START OF MATCH
            pickedReward.transform.SetParent(player1.deck.transform);
        }
        

        for (int i = 0; i < rewardsLayoutGroup.childCount; i++)
        {
            Destroy(rewardsLayoutGroup.GetChild(i).gameObject);
        }

        rewardsPickedCount++;

        if (rewardsPickedCount < rewardDraftAmount - 1)
        {
            GiveRewards(allCardPrefabs);
        }
        else if(rewardsPickedCount < rewardDraftAmount)
        {
            GiveRewards(allItemPrefabs);
        }
        else
        {
            rewardsPickedCount = 0;
            rewardsLayoutGroup.parent.gameObject.SetActive(false);
            LoadNextScene();
        }
    }

    public void GiveRewards(List<GameObject> rewardPrefabs)
    {
        rewardsLayoutGroup.parent.gameObject.SetActive(true);
        
        tempAllCardPrefabs.Clear();

        foreach (GameObject g in rewardPrefabs)
        {
            tempAllCardPrefabs.Add(g);
        }

        int randomIndex = 0;
        for (int i = 0; i < rewardCount; i++)
        {
            randomIndex = Random.Range(0, rewardPrefabs.Count);
            GameObject reward = Instantiate(rewardPrefabs[randomIndex], Vector3.zero, Quaternion.identity);
            reward.transform.SetParent(rewardsLayoutGroup);
            rewardPrefabs.Remove(rewardPrefabs[randomIndex]);
        }

        rewardPrefabs.Clear();

        foreach (GameObject g in tempAllCardPrefabs)
        {
            rewardPrefabs.Add(g);
        }
    }

    public void StartMatch()
    {
        playerInfoPanel.SetActive(true);

        print("Match starting");
        CreatePlayer2();
        player1.opponent = player2;
        player1.roundScore = 0;

        player1.ClearEnemyLists();
        player1.ClearBuildAreaLists();
        UpdateScoreTexts();
        CreateBases();
        CreateBuildAreas(15);

        player1.inventory.InitSpawners();
        player2.inventory.InitSpawners();
    }

    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(Random.Range(0, A.Length));
        return V;
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
        player2.inventory = player2Inventory;
        player2.inventory.owner = player2;

        player2.gameObject.AddComponent<AI>().p = player2;
        player2.GetComponent<AI>().type = GetRandomEnum<AI.AIType>();
        playe2AIText.text = "Opponent: " + player2.GetComponent<AI>().type;

        player2.deck.AddChildCardsToList();
        player2.inventory.AddChildItemsToList();
        player2.hand.DrawHandShuffleIfNeeded(player2.hand.handSize);
        //player2.hand.StartDrawing();

        if (player1.winCount > 0)
        {
            player1.hand.DiscardAll();
        }
       
        player1.deck.AddChildCardsToList();
        player1.inventory.AddChildItemsToList();
        player1.hand.DrawHandShuffleIfNeeded(player1.hand.handSize);
        //player1.hand.StartDrawing();

        player1.startingResources = player1.resources;

    }

    public void GameOver(Player loser)
    {
        SearchAndDestroyTag();
        SearchAndDestroyTagPermanent();
        player1.hand.StopAllCoroutines();
        player2.hand.StopAllCoroutines();

        if (loser == player1)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            LoadPreviousScene();
        }
        
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
        SearchAndResetCooldownTagPermanent();
        CreateBases();
        player1.ClearEnemyLists();
        player2.ClearEnemyLists();
        player1.ResetBuildAreas();
        player2.ResetBuildAreas();

        player1.roundExtraResources = 0;
        player2.roundExtraResources = 0;

        player1.inventory.InitSpawners();
        player2.inventory.InitSpawners();
    }

    public void SearchAndDestroyTag()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject unit in units)
            Destroy(unit);
    }

    public void SearchAndDestroyTagPermanent()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Permanent");
        foreach (GameObject unit in units)
            Destroy(unit);
    }

    public void SearchAndResetCooldownTagPermanent()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Permanent");
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<Spawner>())
            {
                unit.GetComponent<Spawner>().Init();
            }
        }
            
    }

    public void CreateBases()
    {
        player1.baseCount = 3;
        player2.baseCount = 3;

        float laneGap = 20f;

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
        float buildAreaWidth = 2f + 1f;
        float laneGap = 5f;
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

        if (Input.GetKeyUp(KeyCode.G))
        {
            player2.LoseRound();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            player1.roundExtraResources += 5;
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

        if (Input.GetKeyUp(KeyCode.I))
        {
            showInventory = !showInventory;

            if (showInventory)
            {
                player1.inventory.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
                player1.inventory.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                player1.inventory.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
                player1.inventory.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            player1.hand.DrawHandShuffleIfNeeded(3);
            player2.hand.DrawHandShuffleIfNeeded(3);
        }

        if (player1ResourcesText && player2ResourcesText && player1 && player2)
        {
            player1ResourcesText.text = "Local: " + player1.roundExtraResources.ToString() + "\n" + "Global: " + player1.resources.ToString();
            player2ResourcesText.text = "Global: " + player2.resources.ToString() + "\n" + "Local: " + player2.roundExtraResources.ToString();
        }

        Time.timeScale = timeScaleValue;
    }
}
