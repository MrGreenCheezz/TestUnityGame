using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMode<T>
{
    public T GetSingleton();

    public void PlayerJoined(ulong clientId);

    public void MigratePlayer(ulong clientId, bool isPlayerJoiningGamemode);
}
