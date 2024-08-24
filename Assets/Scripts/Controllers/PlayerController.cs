using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[System.Serializable]
public class PlayerController : MonoBehaviour
{
    // player variables
    [SerializeField] private GameStateSO gameState;
    [SerializeField] private GameObject movePoint;
    [SerializeField] private Rigidbody playerRigidBody;
    private Animator animator;
    private GameObject cursor;

    // weapon related variables
    // [SerializeField] private WeaponSO weapon;
    [SerializeField] private ProjectileController projectile;
    private bool canAttack = true;

    // controller variables
    private Vector2 moveInput;
    private Vector2 aimInput;
    private bool interact = false;
    private bool shoot = false;

    // layer comparisons
    [SerializeField] private LayerMask collisions;
    [SerializeField] private LayerMask interactables;

    static public List<PlayerController> players;

    private PlayerInput playerInput;

    public PlayerListSO playerList;
    private PlayerSO myData;
    private HealthBar healthBar;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cursor = GameObject.Find("P_Cursor");

        if (players == null)
        {
            players = new List<PlayerController>();
        }

        players.Add(this);

        myData = playerList.playerSOs[playerInput.playerIndex];
        myData.Init("Larry");
    }

    private void Start()
    {
        healthBar = myData.healthBar;
    }

    private void Update()
    {
        // set cursor position to cursor position, will require support for gamepad
        var cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new(cursorPos.x, cursorPos.y, 0f);

        if (playerInput.currentControlScheme == "Keyboard")
        {
            Vector2 lookDir = cursor.transform.position - transform.position;
            animator.SetFloat("moveX", lookDir.x);
            animator.SetFloat("moveY", lookDir.y);
        }

        // checks if the GameState is proper
        if (gameState.state == GameState.combat)
        {
            if (interact)// triggers interact function when true
            {
                Interact();
                interact = false;
            }

            // Player movement 
            {
                if (Mathf.Abs(moveInput.x) > 0.1f || Mathf.Abs(moveInput.y) > 0.1f)
                    animator.SetBool("isMoving", true);
                else
                    animator.SetBool("isMoving", false);

                if (moveInput != Vector2.zero)
                {
                    movePoint.transform.position = new Vector3(
                    playerRigidBody.position.x + moveInput.x * 0.5f,   // x pos
                    playerRigidBody.position.y + moveInput.y * 0.5f,   // y pos
                    0f);                                                // z pos

                    playerRigidBody.position = Vector3.MoveTowards(playerRigidBody.position,
                        movePoint.transform.position, myData.moveSpeed * Time.deltaTime);
                }
            }

            // Shooting
            if (shoot)
            {
                if (playerInput.currentControlScheme == "Controller")
                    StartCoroutine(FireWeaponController());
                else if (playerInput.currentControlScheme == "Keyboard")
                    StartCoroutine(FireWeaponKeyboard());

                shoot = false;
            }

        }
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        moveInput = _context.ReadValue<Vector2>();
    }
    public void OnInteract(InputAction.CallbackContext _context)
    {
        interact = _context.action.triggered;
    }
    public void OnAttack(InputAction.CallbackContext _context)
    {
        shoot = _context.action.triggered;
    }
    public void OnAim(InputAction.CallbackContext _context)
    {
        aimInput = _context.ReadValue<Vector2>();

        animator.SetFloat("moveX", aimInput.x);
        animator.SetFloat("moveY", aimInput.y);
    }

    public IEnumerator FireWeaponController()
    {
        if (canAttack)
        {
            canAttack = false;
            projectile = ProjectilePool.SharedInstance.GetPooledProjectiles();
            projectile.gameObject.SetActive(true);
            projectile.ProjectileDireciton(transform.position, aimInput);
            
            // loop definition with an offset that accounts for number of projectiles
            // if distance between start pos and current pos becomes greater than the range,
            // deactivate/destroy projectile
            yield return new WaitForSeconds(0.5f); // take the weapon cooldown as the input
            canAttack = true;
        }
        yield return null;
    }

    public IEnumerator FireWeaponKeyboard()
    {
        if (canAttack)
        {
            canAttack = false;
            projectile = ProjectilePool.SharedInstance.GetPooledProjectiles();
            projectile.gameObject.SetActive(true);
            projectile.Attack(playerRigidBody.position, cursor.transform.position);

            // loop definition with an offset that accounts for number of projectiles
            // if distance between start pos and current pos becomes greater than the range,
            // deactivate/destroy projectile
            yield return new WaitForSeconds(0.5f); // take the weapon cooldown as the input
            canAttack = true;
        }
        yield return null;
    }

    private void Interact() // interactable function 
    {
        Debug.Log("Interact test...");
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"), 0f);
        var interactPos = transform.position + facingDir;

        // checks if the over lapping box is interactable
        var collider = Physics.OverlapSphere(interactPos, 0.2f, interactables);
        foreach (var collision in collider)
        {
            if (collision.GetComponent<Interactable>() != null)
            {
                animator.SetBool("isMoving", false); // sets player movement to idle
                collision.GetComponent<Interactable>()?.Interact();
                Debug.Log("Interact Successful!");
            }
        }
    }

    private bool IsTraversable(Vector3 _targetPos)
    {
        if (Physics.OverlapSphere(_targetPos, 0.2f, interactables) != null)
            return false;
        return true;
    }

    public void ModifyHp(int _value)
    {
        myData.hp += _value;

        bool ifWeTakeDamageAndDontDie = myData.hp > 0;
        bool ifWeTakeDamageAndDie = myData.hp <= 0;

        Debug.Log("Player HP: " + myData.hp);
        if (ifWeTakeDamageAndDontDie)
        {
            healthBar.SetHealth(myData.hp);
        }
        else if (ifWeTakeDamageAndDie)
        {
            healthBar.SetHealth(0);

            Debug.Log("Player is Dead");
            myData.Death();
        }
    }
}