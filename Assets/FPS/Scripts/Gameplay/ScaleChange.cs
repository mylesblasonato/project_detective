using System;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;

enum ScaleState
{
    SMALL,
    MEDIUM,
    LARGE,
}

public class ScaleChange : MonoBehaviour
{
    Vector3 _destinationScale;
    ScaleState _scaleState;
    bool _changeScale = false;

    void Start()
    {
        _scaleState = ScaleState.MEDIUM;
    }

    void Update()
    {
        if (_changeScale)
            ChangeScale();
    }

    WeaponController _activeWeapon;

    bool _pushed = false;
    Vector3 _direction;

    public void Shrink(WeaponController activeWeapon)
    {
        if (activeWeapon.ShootType != WeaponShootType.Charge) return;
        _activeWeapon = activeWeapon;
        _timeElapsed = 0f;
        switch (_scaleState)
        {
            case ScaleState.MEDIUM:
                _scaleState = ScaleState.SMALL;
                break;
            case ScaleState.LARGE:
                _scaleState = ScaleState.MEDIUM;
                break;
            case ScaleState.SMALL:
                break;
        }

        SetScale();
    }

    public void Grow(WeaponController activeWeapon)
    {
        if (activeWeapon.ShootType != WeaponShootType.Charge) return;
        _activeWeapon = activeWeapon;
        _timeElapsed = 0f;
        switch (_scaleState)
        {
            case ScaleState.MEDIUM:
                _scaleState = ScaleState.LARGE;
                break;
            case ScaleState.SMALL:
                _scaleState = ScaleState.MEDIUM;
                break;
            case ScaleState.LARGE:
                break;
        }

        SetScale();
    }

    void SetScale() => _changeScale = true;

    float _timeElapsed = 0f;
    public float _duration = 3f;

    void ChangeScale()
    {
        switch (_scaleState)
        {
            case ScaleState.SMALL:
                _destinationScale = new Vector3(.5f, .5f, .5f);
                break;
            case ScaleState.MEDIUM:
                _destinationScale = new Vector3(1, 1, 1);
                break;
            case ScaleState.LARGE:
                _destinationScale = new Vector3(2, 2, 2);
                break;
        }

        if (_timeElapsed < _duration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _destinationScale, _timeElapsed / _duration);
            _timeElapsed += Time.deltaTime;
        }
        else
        {
            _changeScale = false;
            _timeElapsed = 0f;

            FindObjectOfType<PlayerWeaponsManager>().isGrowing = false;
            FindObjectOfType<PlayerWeaponsManager>().isShrinking = false;
        }
    }
}