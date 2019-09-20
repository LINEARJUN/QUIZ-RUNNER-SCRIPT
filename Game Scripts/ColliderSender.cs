using UnityEngine;

public class ColliderSender : MonoBehaviour
{
    public delegate void ColliderSenderEvent(Collider other);
    public event ColliderSenderEvent OnTriggerEnterCall;
    public event ColliderSenderEvent OnTriggerStayCall;
    public event ColliderSenderEvent OnTriggerExitCall;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterCall?.Invoke(other);
    }
    private void OnTriggerStay(Collider other)
    {
        OnTriggerStayCall?.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitCall?.Invoke(other);
    }
}
