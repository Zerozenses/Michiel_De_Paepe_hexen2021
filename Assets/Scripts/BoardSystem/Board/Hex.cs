using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using HEX.CardSystem;

namespace HEX.BoardSystem
{
    public class PositionEventArgs : EventArgs
    {
        public Hex Position { get; }
        public Card CurrentCard { get; }

        public PositionEventArgs(Hex position, Card card)
        {
            Position = position;
            CurrentCard = card;
        }
    }

    [SelectionBase]
    public class Hex : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private  Material _highlightMaterial = null;
        [SerializeField]
        private  Material _normalMaterial = null;
        [SerializeField]
        private  MeshRenderer _meshRenderer = null;

        private bool _isSelected;
        public event EventHandler HexSelected;
        public event EventHandler HexDeselected;

        [SerializeField]
        private UnityEvent OnActivate;

        [SerializeField]
        private UnityEvent OnDeactivate;

        public event EventHandler<PositionEventArgs> Clicked;
        public event EventHandler<PositionEventArgs> NotClicked;
        public event EventHandler<PositionEventArgs> CardHexEntered;
        public event EventHandler<PositionEventArgs> CardHexDropped;
        public event EventHandler<PositionEventArgs> CardHexExited;
        public Position Position { get; set; }

        public bool IsHighlighted
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnHighlightStatusChanged(this,EventArgs.Empty);
            }
        }

        public void PositionDeactivated(object sender, EventArgs e)
            => OnDeactivate.Invoke();

        public void PositionActivated(object sender, EventArgs e)
            => OnActivate.Invoke();

        public void OnDrop(PointerEventData eventData)
        {
            //Notification on which card used on which hex
            Debug.Log(eventData.pointerDrag.name + " was used on " + gameObject.name);
            var card = eventData.pointerDrag.GetComponent<Card>();

            //OnCardDropping
            OnCardDropping(this, new PositionEventArgs(this, card));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //if dragged, false
            if (eventData.pointerDrag == null)
                return;
            var card = eventData.pointerDrag.GetComponent<Card>();

            //OnCardEntering
            OnCardEntering(this, new PositionEventArgs(this, card));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            IsHighlighted = !IsHighlighted;
            //var obj = eventData.selectedObject;
            //OnClicking(this, new PositionEventArgs(this));
            //Debug.Log("On " + eventData.pointerDrag.name + " Clicked");
            
            //OnClicking(this, new PositionEventArgs(this));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //if dragged, false
            if (eventData.pointerDrag ==null)
                return;

            var card = eventData.pointerDrag.GetComponent<Card>();

            //Physics.RaycastAll.

            //OnCardExiting
            OnCardExiting(this, new PositionEventArgs(this, card));
        }

        protected virtual void OnHighlightStatusChanged(object source, EventArgs args)
        {
            var handler = HexSelected;
            _meshRenderer.material = IsHighlighted ? _highlightMaterial : _normalMaterial;
            handler?.Invoke(this,args);
        }

        protected virtual void OnClicking(object source, PositionEventArgs e)
        {
            var handler = Clicked;
            handler?.Invoke(this, e);
        }

        protected virtual void OnCardEntering(object source, PositionEventArgs e)
        {
            var handler = CardHexEntered;
            handler?.Invoke(this, e);
        }

        protected virtual void OnCardDropping(object source, PositionEventArgs e)
        {
            var handler = CardHexDropped;
            handler?.Invoke(this, e);
        }

        protected virtual void OnCardExiting(object source, PositionEventArgs e)
        {
            var handler = CardHexExited;
            handler?.Invoke(this, e);
        }
    }
}
