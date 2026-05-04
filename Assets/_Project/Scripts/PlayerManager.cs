using UnityEngine;

public class PlayerManager : GenericSingleton<PlayerManager>
{
    private GameObject _currentPlayer;
    public GameObject CurrentPlayer => _currentPlayer;

    public void RegisterPlayer(GameObject player)
    {
        _currentPlayer = player;
        Debug.Log($"[PlayerManager] Player registrato: {player.name}");
    }
}