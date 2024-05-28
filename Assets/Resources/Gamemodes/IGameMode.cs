using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMode<T>
{
    public T GetSingleton();
}
