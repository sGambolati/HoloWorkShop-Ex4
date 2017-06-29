using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class HoldRotator : MonoBehaviour, IHoldHandler
{
    public float MovementVelocity = 30f;

    private bool isHolding = false;

    public void OnHoldCanceled(HoldEventData eventData)
    {
        isHolding = false;
    }

    public void OnHoldCompleted(HoldEventData eventData)
    {
        isHolding = false;
    }

    public void OnHoldStarted(HoldEventData eventData)
    {
        isHolding = true;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isHolding)
        {
            this.transform.Rotate(Vector3.up * Time.deltaTime * this.MovementVelocity, Space.World);
        }
    }
}
