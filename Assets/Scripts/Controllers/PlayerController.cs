using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[System.Serializable]
public class PlayerController : MonoBehaviour
{
    // player variables
    [SerializeField] private Player playerStats;
    [SerializeField] private GameStateSO gameState;
    [SerializeField] private GameObject movePoint;
    [SerializeField] private GameObject cursor;
    [SerializeField] private Rigidbody playerRigidBody;
    private Animator animator;

    // weapon related variables
    // [SerializeField] private WeaponSO weapon;
    [SerializeField] private ProjectileController projectile;
    private bool canAttack = true;

    // controller variables
    private CharacterController controller;
    private Vector2 input;
    private bool interact = false;
    private bool shoot = false;

    // layer comparisons
    [SerializeField] private LayerMask collisions;
    [SerializeField] private LayerMask interactables;

    // cutscene variables
    [SerializeField] private PlayableDirector playableDirector;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // set cursor position to cursor position, will require support for gamepad
        var cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new(cursorPos.x, cursorPos.y, 0f);

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
                if (Mathf.Abs(input.x) > 0.1f || Mathf.Abs(input.y) > 0.1f)
                    animator.SetBool("isMoving", true);
                else
                    animator.SetBool("isMoving", false);

                if (input != Vector2.zero)
                {
                    movePoint.transform.position = new Vector3(
                    playerRigidBody.position.x + input.x * 0.5f,   // x pos
                    playerRigidBody.position.y + input.y * 0.5f,   // y pos
                    0f);                                                // z pos

                    animator.SetFloat("moveX", input.x);
                    animator.SetFloat("moveY", input.y);
                    playerRigidBody.position = Vector3.MoveTowards(playerRigidBody.position,
                        movePoint.transform.position, playerStats.moveSpeed * Time.deltaTime);
                }
            }

            // Shooting
            if (shoot)
            {
                StartCoroutine(FireWeapon());
                shoot = false;
            }

        }
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        input = _context.ReadValue<Vector2>();
    }
    public void OnInteract(InputAction.CallbackContext _context)
    {
        interact = _context.action.triggered;
    }
    public void OnAttack(InputAction.CallbackContext _context)
    {
        shoot = _context.action.triggered;
    }

    public IEnumerator FireWeapon()
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
        Debug.Log("Player HP: " + playerStats.hp);
        if (playerStats.hp + _value >= playerStats.maxHp)
            playerStats.hp = playerStats.maxHp;
        else if (playerStats.hp + _value <= 0)
        {
            Debug.Log("Player is Dead");
            GameOver(); 
        }
        else
            playerStats.hp += _value;
    }

    // Game Over setup, Works in conjunction with NotifyOnFinish
    private void GameOver()
    {
        StartCoroutine(NotifyOnFinish());
    }

    private IEnumerator NotifyOnFinish()
    {
        playableDirector.Play();
        while (playableDirector.state == PlayState.Playing)
        {
            yield return null;
        }
        SceneManager.LoadScene("MainMenu");
    }

    public Player PlayerStats { get; private set; }
}