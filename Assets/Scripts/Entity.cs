using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private GameObject ParticleSystem;
    [SerializeField] private GameObject Mesh;

    private Animator _animator;
    private bool _startMoving = false;

    private void Start()
    {
        _animator = Mesh.GetComponent<Animator>();
    }

    private void Update()
    {
        if (_startMoving)
        {
            _startMoving = false;
            _animator.SetBool("isRunning", true);
        }
    }

    public void ChangeLocalPos(Vector3 pos)
    {
        StartCoroutine(ChangePosition(pos));
    }

    private IEnumerator ChangePosition(Vector3 pos)
    {
        int iterations = 0;
        while (transform.localPosition != pos)
        {
            yield return null;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, pos, 10 * Time.deltaTime);
            if (iterations++ > 5000)
            {
                transform.localPosition = pos;
                break;
            }

        }
    }

    public void StartMoving()
    {
        _startMoving = true;
    }

    public void StopMoving()
    {
        _animator.SetBool("isRunning", false);
    }

    public void Disappear()
    {
        StartCoroutine(FlyOut());
    }

    private IEnumerator FlyOut()
    {
        while (transform.position.y < 30)
        {
            yield return null;
            transform.Rotate(transform.up, 720 * Time.deltaTime);
            transform.position += Vector3.up * Time.deltaTime * 0.8f;
        }
        Destroy(gameObject);
    }

    public void Die()
    {
        PlayerSpace.instance.RemoveEntity(this);
        StartCoroutine(AnimateDying());
    }

    private IEnumerator AnimateDying()
    {
        Mesh.SetActive(false);
        ParticleSystem.SetActive(true);

        yield return new WaitForSecondsRealtime(0.1f);

        Destroy(gameObject);
    }
}
