using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class AirTapAnimator : MonoBehaviour, IInputClickHandler
{
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        this.animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        this.animator.Play("Run_Rifle_Back");
    }
}
