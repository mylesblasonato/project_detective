using EasySurvivalScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour, IInteractable
{
    [SerializeField] LayerMask _interactableLayer;
    [SerializeField] float _distance = 2f;
    [SerializeField] string _tooFar = "Too Far";

    NotesManager _nm;
    Ray _ray;
    RaycastHit _hit;
    PlayerMovement _playerMovement;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _nm = FindObjectOfType<NotesManager>();
    }

    public void Interact()
    {
        if (_nm._activeNote != null)
        {
            _ray = new Ray(transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(_ray, out _hit, _distance, _interactableLayer))
            {
                Cursor.lockState = CursorLockMode.None;
                var ent = _hit.transform.GetComponent<InteractionEntity>();
                if (ent._hasInteracted) return;
                GameManager._instance.SetIsInteracting(true);
                if (!_playerMovement.GetIsInteracting())
                {
                    if (_nm._activeNote == ent._noteToUse)
                    {
                        ent._entityFlowchart.SendFungusMessage(ent._message);
                    }
                    else
                    {
                        _nm._examineNotesFlow.SendFungusMessage(_nm._activeNote._examineMessage);
                    }
                }
            }
            else
            {
                GameManager._instance.SetIsInteracting(false);
                if (Physics.Raycast(_ray, out _hit, 100f, _interactableLayer))
                {
                    _hit.transform.GetComponent<InteractionEntity>()._entityFlowchart.SendFungusMessage(_tooFar);
                }
                else
                {
                    _nm._examineNotesFlow.SendFungusMessage(_nm._activeNote._examineMessage);
                }
            }
        }
    }
}
