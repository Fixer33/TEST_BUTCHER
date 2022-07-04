using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpace : MonoBehaviour
{
    public static PlayerSpace instance { get; private set; }

    [SerializeField] private float MovingSpeed = 1f;
    [SerializeField] private int EntityCount = 10;
    [SerializeField] private float CoordinateScale = 0.02f;
    [SerializeField] private GameObject SpawnCenter;
    [SerializeField] private GameObject EntityPrefab;

    public bool IsMoving { get; private set; } = false;

    private List<Entity> _entities = new List<Entity>();
    private Vector3 _startPosition;

    private void Start()
    {
        instance = this;
        _startPosition = transform.position;
    }

    public void Restart()
    {
        transform.position = _startPosition;
    }

    public void Finish()
    {
        IsMoving = false;
        for (int i = 0; i < _entities.Count; i++)
        {
            _entities[i].StopMoving();
            _entities[i].Disappear();
        }
    }

    public void StartLevel(List<Vector2> touchPoints)
    {
        IsMoving = true;

        if (EntityCount > touchPoints.Count)
        {
            int spawned = 0;
            int perPoint = 1;
            int left = 0;
            perPoint = EntityCount / touchPoints.Count;
            left = EntityCount - (perPoint * touchPoints.Count);
            for (int i = 0; i < EntityCount; i++)
            {
                for (int k = 0; k < perPoint; k++)
                {
                    SpawnEntity(touchPoints[i]);
                    spawned++;
                }
                if (spawned >= EntityCount)
                    break;
            }
            for (int i = 0; i < left; i++)
            {
                SpawnEntity(touchPoints[i]);
            }
        }
        else
        {
            int spawnIndex = 0;
            int interval = touchPoints.Count / EntityCount;
            for (int i = 0; i < EntityCount; i++)
            {
                SpawnEntity(touchPoints[spawnIndex]);
                spawnIndex += interval;
            }
        }

        

        for (int i = 0; i < _entities.Count; i++)
        {
            _entities[i].StartMoving();
        }
    }

    public void Restructurize(List<Vector2> touchPoints)
    {
        if (_entities.Count > touchPoints.Count)
        {
            int affected = 0;
            int perPoint = 1;
            int left = 0;
            perPoint = _entities.Count / touchPoints.Count;
            left = _entities.Count - (perPoint * touchPoints.Count);
            for (int i = 0; i < touchPoints.Count; i++)
            {
                for (int k = 0; k < perPoint; k++)
                {
                    Vector2 normalizedPoint = touchPoints[i] * CoordinateScale;
                    Entity ent = _entities[affected++];
                    ent.ChangeLocalPos(new Vector3(normalizedPoint.x, 0.25f, normalizedPoint.y));
                }
                if (affected >= _entities.Count)
                {
                    break;
                }
            }
            for (int i = 0; i < left; i++)
            {
                Vector2 normalizedPoint = touchPoints[i] * CoordinateScale;
                Entity ent = _entities[affected++];
                ent.ChangeLocalPos(new Vector3(normalizedPoint.x, 0.25f, normalizedPoint.y));
            }
        }
        else
        {
            int changeIndex = 0;
            int interval = touchPoints.Count / _entities.Count;
            for (int i = 0; i < _entities.Count; i++)
            {
                Entity ent = _entities[i];
                Vector2 normalizedPoint = touchPoints[changeIndex] * CoordinateScale;
                ent.ChangeLocalPos(new Vector3(normalizedPoint.x, 0.25f, normalizedPoint.y));
                changeIndex += interval;
            }
        }
    }

    private void SpawnEntity(Vector2 point)
    {
        Vector2 normalizedPoint = point * CoordinateScale;
        Vector3 position = new Vector3(normalizedPoint.x, 0.25f, normalizedPoint.y);
        SpawnEntity(position);
    }

    public void SpawnEntity(Vector3 point)
    {
        GameObject entityObj = SpawnEntityFromGlobal(Vector3.zero);
        entityObj.transform.localPosition = point;
    }

    public GameObject SpawnEntityFromGlobal(Vector3 pos)
    {
        GameObject entityObj = Instantiate(EntityPrefab, pos, Quaternion.identity, SpawnCenter.transform);
        Entity ent = entityObj.GetComponent<Entity>();
        _entities.Add(ent);
        ent.StartMoving();
        return entityObj;
    }

    public void RemoveEntity(Entity ent)
    {
        _entities.Remove(ent);
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            transform.Translate(transform.forward * MovingSpeed * Time.deltaTime);
        }
    }
}
