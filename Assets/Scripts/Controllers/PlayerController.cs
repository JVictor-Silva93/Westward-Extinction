using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    // player variables
    [SerializeField] private Player playerStats;
    [SerializeField] private GameStateSO gameState;
    [SerializeField] private GameObject movePoint;
    private Vector2 input;
    private Animator animator;

    // layer comparisons
    [SerializeField] private LayerMask collisions;
    [SerializeField] private LayerMask interactables;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // checks if the GameState is proper
        if (gameState.state == GameState.combat)
        {
            // iteract with interactable object
            if (Input.GetKeyDown(KeyCode.E))
                Interact();

            // Player movement 
            {
                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");

                if (Mathf.Abs(input.x) > 0.1f || Mathf.Abs(input.y) > 0.1f)
                    animator.SetBool("isMoving", true);
                else
                    animator.SetBool("isMoving", false);

                if (input != Vector2.zero)
                {
                    movePoint.transform.position = new Vector3(
                    transform.position.x + input.x * 0.5f, // x pos
                    transform.position.y + input.y * 0.5f, // y pos
                    0f);                                   // z pos

                    animator.SetFloat("moveX", input.x);
                    animator.SetFloat("moveY", input.y);

                    if (IsTraversable(movePoint.transform.position)) // accounts for collision without jitter
                        transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position,
                        playerStats.MoveSpeed * Time.deltaTime);
                }
            }

            // Shooting



        }
    }
    private void Interact() // interactable function 
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"), 0f);
        var interactPos = transform.position + facingDir;

        // checks if the over lapping box is interactable
        var collider = Physics2D.OverlapBox(interactPos, new Vector2(0.2f, 0.2f), interactables);
        if (collider != null)
        {
            new WaitForEndOfFrame();
            animator.SetBool("isMoving", false); // sets player movement to idle
            collider.GetComponent<Interactable>()?.Interact(); // runs interact function from the interactable if a val
            
            Debug.Log("Interact");
        }
    }

    private bool IsTraversable(Vector3 _targetPos)
    {
        if (Physics2D.OverlapBox(_targetPos, new Vector2(0.2f, 0.2f), interactables))
            return false;
        return true;
    }
}

[System.Serializable]
public class Player
{
    [SerializeField] private string playerName;         // Logs the name of this player
    [SerializeField] private float moveSpeed = 20f;     // Controls the speed at which the player moves
    [SerializeField] private float aimSpeed = 1f;       // Controlls speed of directional rotation

    public Player()
    {
        this.playerName = "Default";
        this.moveSpeed = 0.25f;
        this.aimSpeed = 1f;
    }

    public Player(string _playerName, float _moveSpeed, float _aimSpeed)
    {
        this.playerName = _playerName;
        this.moveSpeed = _moveSpeed;
        this.aimSpeed = _aimSpeed;
    }

    public string PlayerName { get { return this.playerName; } }
    public float MoveSpeed { get { return this.moveSpeed; } }
    public float AimSpeed { get { return this.aimSpeed; } }
}