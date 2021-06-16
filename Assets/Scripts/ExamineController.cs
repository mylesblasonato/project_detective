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

    NotesManager _nm;
    Ray _ray;
    RaycastHit _hit;
    PlayerMovement _playerMovement;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _nm = FindObjectOfType<NotesManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(_examineKey))
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, _distance, _interactableLayer))
            {
                _nm.SetIsTalking(true);
                GameManager._instance.SetIsInteracting(true);
                if (!_playerMovement.GetIsInteracting())
                {
                    var flow = _hit.transform.GetComponent<InteractionEntity>()._entityFlowchart;
                    flow.SendFungusMessage(_examine);

                    flow.ExecuteBlock(
                            flow.FindBlock("Examine"),
                            flow.GetExecutingBlocks().Count,
                            delegate { _nm.SetIsTalking(false); GameManager._instance.SetIsInteracting(false); });
                }
            }
        }
    }
}
