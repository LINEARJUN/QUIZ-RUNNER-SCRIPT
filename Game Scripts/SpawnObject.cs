using UnityEngine;

public class SpawnObject : MonoBehaviour, ISpawnable
{
    [HideInInspector] public int id = 0;
    private Vector3 direction;

    public virtual void Initialize()
    {

    }
    public virtual void Initialize(Vector3 direction, int id = 0)
    {
        this.direction = direction;
        this.id = id;
    }

    private void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, transform.position + direction, Time.deltaTime * Map.instance.speed);
        transform.Translate(direction * Map.instance.speed * Time.deltaTime, Space.World);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
