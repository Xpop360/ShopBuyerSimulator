using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private float _transtionTime;

    private GameObject worldA;
    private GameObject worldB;

    private Vector2 newPosition;

    public static TransitionManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SwitchBetweenWorlds(GameObject l, GameObject e, Vector2 nP)
    {
        if (_animator == null) return;

        worldA = l;
        worldB = e;

        newPosition = nP;

        StartCoroutine(BeginTransition());
    }

    IEnumerator BeginTransition()
    {
        _animator.SetTrigger("Start");

        yield return new WaitForSeconds(_transtionTime);

        worldA.SetActive(false);
        worldB.SetActive(true);        

        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = newPosition;

        _animator.SetTrigger("End");

        yield return new WaitForSeconds(_transtionTime);

        InputManager.Instance.SwitchToGameplay();

        _animator.ResetTrigger("Start");
        _animator.ResetTrigger("End");
    }
}
