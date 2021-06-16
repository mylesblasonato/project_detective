using EasySurvivalScripts;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour, IInteractable
{
    [SerializeField] LayerMask _interactableLayer;
    [SerializeField] float _distance = 2f;

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
        _nm.SetIsTalking(true);
        if (_nm._activeNote != null)
        {
            _ray = new Ray(transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(_ray, out _hit, _distance, _interactableLayer))
            {
                Cursor.lockState = CursorLockMode.None;
                var ent = _hit.transform.GetComponent<InteractionEntity>();
                if (ent._hasInteracted) return;
                if (!_playerMovement.GetIsInteracting())
                {
                    if (_nm._activeNote == ent._noteToUse)
                    {
                        GameManager._instance.SetIsInteracting(true);
                        ent._entityFlowchart.ExecuteBlock(
                        ent._entityFlowchart.FindBlock("Interact"),
                        ent._entityFlowchart.GetExecutingBlocks().Count,
                        delegate { _nm.SetIsTalking(false); GameManager._instance.SetIsInteracting(false); });

                        ent._entityFlowchart.SendFungusMessage(ent.name);
                    }
                    else
                    {
                        GameManager._instance.SetIsInteracting(true);

                        _nm._examineNotesFlow.ExecuteBlock(
                                _nm._examineNotesFlow.FindBlock(_nm._activeNote.name),
                                _nm._examineNotesFlow.GetExecutingBlocks().Count,
                                delegate { _nm.SetIsTalking(false); GameManager._instance.SetIsInteracting(false); });

                        _nm._examineNotesFlow.SendFungusMessage(_nm._activeNote.name);
                    }
                }
            }
            else
            {
                GameManager._instance.SetIsInteracting(true);

                _nm._examineNotesFlow.ExecuteBlock(
                        _nm._examineNotesFlow.FindBlock(_nm._activeNote.name),
                        _nm._examineNotesFlow.GetExecutingBlocks().Count,
                        delegate { _nm.SetIsTalking(false); GameManager._instance.SetIsInteracting(false); });

                _nm._examineNotesFlow.SendFungusMessage(_nm._activeNote.name);
            }
        }
    }
}