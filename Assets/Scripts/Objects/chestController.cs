using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestController : MonoBehaviour, Interactable
{
    private SpriteRenderer sprite;
    private bool isOpened = false;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        // open chest sequence
        sprite.color = Color.red;
        Debug.Log("Opened Chest");

        isOpened = true;
        // add treasure
    }
}
