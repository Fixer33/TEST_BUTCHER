using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectible : MonoBehaviour
{
    public CollectibleType Type;

    private Dictionary<CollectibleType, Action<Entity>> _actions = new Dictionary<CollectibleType, Action<Entity>>();

    private void Start()
    {
        _actions.Add(CollectibleType.Gem, new Action<Entity>(GemPickup));
        _actions.Add(CollectibleType.NewEntity, new Action<Entity>(NewEntityPickup));
    }

    private void NewEntityPickup(Entity ent)
    {
        PlayerSpace.instance.SpawnEntityFromGlobal(transform.position);
        Destroy(gameObject);
    }

    private void GemPickup(Entity ent)
    {
        UI.instance.Score += 1;
        Destroy(gameObject);
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
