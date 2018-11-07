using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{

    [SerializeField] private int Health = 0;
    private bool IsAlive = false;

    void Add(int value)
    {
        Health += value;
        CheckIfIsAlive();
    }

    void CheckIfIsAlive()
    {
        if (Health <= 0)
        {
            IsAlive = false;
        }
    }

    private void Awake()
    {
        IsAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAlive)
        {
            //TODO: Show game over screen
        }
    }
}
