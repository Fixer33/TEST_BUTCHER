using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    [SerializeField] private GameObject ParticleObject;
    [SerializeField] private GameObject Mesh;
    [SerializeField] private TrapType Type;

    private Dictionary<TrapType, Action<Entity>> _actions = new Dictionary<TrapType, Action<Entity>>();

    private void Start()
    {
        _actions.Add(TrapType.Mine, new Action<Entity>(MineTriggered));
        _actions.Add(TrapType.Saw, new Action<Entity>(SawTriggered));
    }

    private void Update()
    {
        if (Type == TrapType.Saw)
            Mesh.transform.Rotate(Vector3.up, 180 * Time.deltaTime, Space.World);
    }

    private void MineTriggered(Entity ent)
    {
        ParticleObject.SetActive(true);
        ent.Die();
        Mesh.SetActive(false);
        Destroy(gameObject);
    }

    private void SawTriggered(Entity ent)
    {
        ent.Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            GameObject colliderObject = other.gameObject;
            if (colliderObject != null)
            {
                Entity ent = colliderObject.GetComponent<Entity>();
                if (ent != null)
                    _actions[Type].Invoke(ent);
            }
        }
    }
}
