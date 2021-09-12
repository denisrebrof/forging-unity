using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    Action<GameObject> OnSpawned
    {
        get;
    }
}
