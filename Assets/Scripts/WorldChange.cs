using UnityEngine;

public class WorldChange : MonoBehaviour
{
    [SerializeField] private GameObject entering;
    [SerializeField] private GameObject leaving;

    [SerializeField] private Transform positionToSpawn;

    public void EnterWorld()
    {
        TransitionManager.Instance.SwitchBetweenWorlds(leaving, entering, positionToSpawn.position);

        InputManager.Instance.DisableControls();
    }
}
