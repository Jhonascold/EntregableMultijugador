using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : NetworkBehaviour
{

    [Header("Spawn Points")]
    public Transform ballSpawnPoint;
    public Transform[] playerSpawnPoints;

    [Header("Referencias")]
    public GameObject ball;


    public TMP_Text scoreText;

    private int scoreTeamA = 0;
    private int scoreTeamB = 0;

    [ServerRpc(RequireOwnership = false)]
    public void ResetPositionsServerRpc()
    {
        if(ball != null)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            if(ballRb != null && !ballRb.isKinematic)
            {
                ballRb.linearVelocity = Vector3.zero;
                ballRb.angularVelocity = Vector3.zero;
            }
            ball.transform.position = ballSpawnPoint.position;
        }

        foreach(var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            GameObject playerObj = client.PlayerObject.gameObject;

            int spawnIndex = (int)(client.ClientId % (ulong)playerSpawnPoints.Length);
            if(spawnIndex < playerSpawnPoints.Length)
            {
                playerObj.transform.position = playerSpawnPoints[spawnIndex].position;
            }

            Rigidbody playerRb = playerObj.GetComponent<Rigidbody>();
            if(playerRb != null && !playerRb.isKinematic)
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
            }
        }

        ResetPositionsClientRpc();
    }

    [ClientRpc]
    private void ResetPositionsClientRpc()
    {
        if(ball != null)
        {
            ball.transform.position = ballSpawnPoint.position;
        }

        foreach(var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            GameObject playerObj = client.PlayerObject.gameObject;

            int spawnIndex = (int)(client.ClientId % (ulong)playerSpawnPoints.Length);
            if(spawnIndex < playerSpawnPoints.Length)
            {
                playerObj.transform.position = playerSpawnPoints[spawnIndex].position;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RegisterGoalServerRpc(bool isTeamA)
    {
        if(isTeamA)
        {
            scoreTeamA++;
        }
        else
        {
            scoreTeamB++;
        }

        UpdateScoreClientRpc(scoreTeamA, scoreTeamB);
    }

    [ClientRpc]
    private void UpdateScoreClientRpc(int newScoreA, int newScoreB)
    {
        if(scoreText)
        {
            scoreText.text = "Team A: " + newScoreA + "\n" + "Team B: " + newScoreB;
        }
        else
        {
            Debug.LogError("El texto de puntuación no está asignado en el GameManager.");
        }
    }
}
