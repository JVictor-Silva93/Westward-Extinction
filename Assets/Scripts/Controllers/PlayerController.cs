using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[System.Serializable]
public class PlayerController : MonoBehaviour
{
    // player variables
    [SerializeField] private Player playerStats;
    [SerializeField] private GameStateSO gameState;
    [SerializeField] private GameObject movePoint;
    private Animator animator;

    // weapon related variables
    // [SerializeField] private WeaponSO weapon;
    [SerializeField] private ProjectileController projectilePrefab;
    private bool canAttack = true;

    // controller variables
    private CharacterController controller;
    private Vector2 input;
    private bool interact = false;
    private bool shoot = false;

    // layer comparisons
    [SerializeField] private LayerMask collisions;
    [SerializeField] private LayerMask interactables;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        var p = Instantiate(projectilePrefab, transform);
    }

    private void Update()
    {
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
                    controller.transform.position.x + input.x * 0.5f,   // x pos
                    controller.transform.position.y + input.y * 0.5f,   // y pos
                    0f);                                                // z pos

                    animator.SetFloat("moveX", input.x);
                    animator.SetFloat("moveY", input.y);
                    controller.transform.position = Vector3.MoveTowards(transform.position,
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
        Debug.Log("before");
        interact = _context.action.triggered;

        Debug.Log("after");
    }

    public void OnAttack(InputAction.CallbackContext _context)
    {
        Debug.Log("will shoot: " + shoot);
        shoot = _context.action.triggered;
        Debug.Log("shot" + shoot);
    }

    public IEnumerator FireWeapon()
    {
        Debug.Log("can attack: " + canAttack);
        if (canAttack)
        {
            canAttack = false;
            Debug.Log("Attack");
            ProjectileManager.Instance.InstantiateProjectile(projectilePrefab, transform); // idk im tired
            // do math to set its right vector towards the target position
            // loop instantiate with an offset that accounts for number of projectiles
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
        var collider = Physics2D.OverlapBox(interactPos, new Vector2(0.2f, 0.2f), interactables);
        if (collider != null)
        {
            new WaitForEndOfFrame();
            animator.SetBool("isMoving", false); // sets player movement to idle
            collider.GetComponent<Interactable>()?.Interact(); 
            // runs interact function from the interactable if a val

            Debug.Log("Interact Successful!");
        } else
            Debug.Log("Interact Failed.");
    }

    private bool IsTraversable(Vector3 _targetPos)
    {
        if (Physics2D.OverlapBox(_targetPos, new Vector2(0.2f, 0.2f), interactables))
            return false;
        return true;
    }

    public void ModifyHp(float _value)
    {
        if (playerStats.hp + _value >= playerStats.maxHp)
            playerStats.hp = playerStats.maxHp;
        else if (playerStats.hp + _value <= 0)
            Debug.Log("Player is Dead");
        else
            playerStats.hp += _value;
    }

    public Player PlayerStats { get; private set; }
}