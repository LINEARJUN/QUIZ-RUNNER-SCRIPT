using UnityEngine;

public interface ISpawnable
{
    void Initialize();
    void Initialize(Vector3 direction, int id = 0);
}
