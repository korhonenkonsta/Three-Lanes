using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public Player player2;

    public GameObject basePrefab;

    public Transform player1Base1;
    public Transform player1Base2;
    public Transform player1Base3;

    public Transform player2Base1;
    public Transform player2Base2;
    public Transform player2Base3;

    void Start()
    {
        CreateBases();
    }

    /// <summary>
    /// Reset the round, which includes:
    ///     -Bases
    ///     -Non-permanent buildings
    ///     -Units
    /// </summary>
    public void ResetRound()
    {
        SearchAndDestroyTag();
        CreateBases();
    }

    public void SearchAndDestroyTag()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject enemy in enemies)
            GameObject.Destroy(enemy);

    }

    public void CreateBases()
    {
        player1.baseCount = 3;
        player2.baseCount = 3;

        Instantiate(basePrefab, player1Base1.position, player1Base1.rotation).GetComponent<Base>().owner = player1;
        Instantiate(basePrefab, player1Base2.position, player1Base2.rotation).GetComponent<Base>().owner = player1;
        Instantiate(basePrefab, player1Base3.position, player1Base3.rotation).GetComponent<Base>().owner = player1;

        Instantiate(basePrefab, player2Base1.position, player2Base1.rotation).GetComponent<Base>().owner = player2;
        Instantiate(basePrefab, player2Base2.position, player2Base2.rotation).GetComponent<Base>().owner = player2;
        Instantiate(basePrefab, player2Base3.position, player2Base3.rotation).GetComponent<Base>().owner = player2;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ResetRound();
        }
    }
}
