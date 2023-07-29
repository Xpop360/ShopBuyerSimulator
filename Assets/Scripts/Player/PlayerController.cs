using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject SignalToInteractGO;
    [SerializeField] private Animator clothesAnimator;

    private Rigidbody2D rb;
    private Animator animator;

    private PlayerInventory inventory;

    private GameObject InteractableGO;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        inventory = GetComponent<PlayerInventory>();

        InputManager.Instance.inputActions.Gameplay.Move.performed += PerformMovement;
        InputManager.Instance.inputActions.Gameplay.Move.canceled += PerformMovement;
        InputManager.Instance.inputActions.Gameplay.Interact.started += Interact;
        InputManager.Instance.inputActions.Gameplay.OpenInventory.started += OpenWindow;
    }

    private void OnEnable()
    {
        //InputManager.Instance.inputActions.Gameplay.Move.performed += PerformMovement;
        //InputManager.Instance.inputActions.Gameplay.Move.canceled += PerformMovement;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Gameplay.Move.performed -= PerformMovement;
        InputManager.Instance.inputActions.Gameplay.Move.canceled -= PerformMovement;
        InputManager.Instance.inputActions.Gameplay.Interact.started -= Interact;
        InputManager.Instance.inputActions.Gameplay.OpenInventory.started -= OpenWindow;
    }

    private void OpenWindow(InputAction.CallbackContext obj)
    {
        if (inventory == null) return;

        inventory.OpenInventory();
    }

    private void PerformMovement(InputAction.CallbackContext obj)
    {
        if (rb == null) return;

        var direction = obj.ReadValue<Vector2>();

        rb.velocity = direction * movementSpeed;

        if (animator == null) return;

        if(direction.x > 0)
        {
            direction.x = 1;
        }
        else if (direction.x < 0) 
        {
            direction.x = -1;
        }

        if (direction.y > 0)
        {
            direction.y = 1;
        }
        else if (direction.y < 0)
        {
            direction.y = -1;
        }

        animator.SetInteger("WalkHorizontal", (int)direction.x);
        animator.SetInteger("WalkVertical", (int)direction.y);

        if (clothesAnimator.runtimeAnimatorController != null)
        {
            clothesAnimator.SetInteger("WalkHorizontal", (int)direction.x);
            clothesAnimator.SetInteger("WalkVertical", (int)direction.y);
        }
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if(InteractableGO == null) return;

        switch (InteractableGO.tag)
        {
            case "Shop":
                var shopKeeper = InteractableGO.GetComponent<ShopKeeper>();
                shopKeeper.OpenShop(inventory);
                break;
            case "WorldSwitch":
                var worldChange = InteractableGO.GetComponent<WorldChange>();
                worldChange.EnterWorld();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SignalToInteractGO?.SetActive(true);

        InteractableGO = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SignalToInteractGO?.SetActive(false);

        InteractableGO = null;
    }
}