using Unity.Netcode;
using UnityEngine;

public class GoalTrigger : NetworkBehaviour
{
    public bool isTeamA;

    private void OnTriggerEnter(Collider other)
    {
        if(!IsServer) return; // Solo el servidor puede registrar goles

        if(other.CompareTag("Ball"))
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            if(gm != null)
            {
                gm.RegisterGoalServerRpc(isTeamA); // Llama al método en el GameManager para registrar el gol
                gm.ResetPositionsServerRpc(); // Llama al método en el GameManager para reiniciar posiciones
            }
            else
            {
                Debug.LogError("No se encontró el GameManager en la escena.");
            }

        }
    }
}
