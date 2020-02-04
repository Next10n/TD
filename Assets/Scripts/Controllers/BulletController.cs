using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BulletController : MonoBehaviour
{
    [Inject]
    protected float _bulletSpeed;
    [Inject]
    protected int _bulletDamage;
    [Inject]
    [SerializeField]
    protected Transform _target;
    [Inject]
    [SerializeField]
    protected Transform _startPosition;

    private void Awake()
    {
        transform.position = _startPosition.position;
    }

    private void FixedUpdate()
    {
        if(_target == null)// не оптимизированная проверка
        {
            Destroy(this.gameObject);
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _bulletSpeed);
    }

    public class Factory : PlaceholderFactory<float, int, Transform, Transform, BulletController>
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        UnitController unitController;
        if (collision.gameObject.TryGetComponent<UnitController>(out unitController))
            unitController.TakeDamage(_bulletDamage);
        Destroy(this.gameObject); //так же сделать пулинг
    }

}
