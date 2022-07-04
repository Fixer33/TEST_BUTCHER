using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Finish : MonoBehaviour
{
    [SerializeField] private List<GameObject> ToActivate = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            Entity ent = other.GetComponent<Entity>();
            if (ent != null)
            {
                PlayerSpace.instance.Finish();
                AnimateFinish();
            }
        }
    }

    private void AnimateFinish()
    {
        for (int i = 0; i < ToActivate.Count; i++)
        {
            ToActivate[i].SetActive(true);
        }
    }
}
