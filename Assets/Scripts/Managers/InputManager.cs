using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    internal PlayerInputs inputActions;

    [SerializeField] internal EventSystem eventSystem;

    public static InputManager Instance { get; private set; }

    void Awake()
    {
        inputActions = new PlayerInputs();

        SwitchToGameplay();

        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void SwitchToGameplay()
    {
        inputActions.UI.Disable();
        inputActions.Gameplay.Enable();
    }

    public void SwitchToUI()
    {
        inputActions.UI.Enable();
        inputActions.Gameplay.Disable();
    }

    public void DisableControls()
    {
        inputActions.UI.Disable();
        inputActions.Gameplay.Enable();
    }
}