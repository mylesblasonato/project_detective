using EasySurvivalScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineController : MonoBehaviour
{
    [SerializeField] LayerMask _interactableLayer;
    [SerializeField] float _distance = 2f;
    [SerializeField] KeyCode _examineKey;
    [SerializeField] string _examine = "Examine", _tooFar = "Too Far";

    Ray _ray;
    RaycastHit _hit;
    PlayerMovement _playerMovement;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(_examineKey))
        {
            GameManager._instance.SetIsInteracting(true);
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, _distance, _interactableLayer))
            {
                if (!_playerMovement.GetIsInteracting())
                {
                    _hit.transform.GetComponent<InteractionEntity>()._entityFlowchart.SendFungusMessage(_examine);
                }
            }
            else
            {
                GameManager._instance.SetIsInteracting(false);
                if (Physics.Raycast(_ray, out _hit, 100f, _interactableLayer))
                {
                    _hit.transform.GetComponent<InteractionEntity>()._entityFlowchart.SendFungusMessage(_tooFar);
                }
            }
        }
    }
}
