using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace HEX.CardSystem
{
    public class CardEventArgs : EventArgs
    {
        public Card Card { get; }
        public CardEventArgs(Card card) => Card = card; 
    }


    public class Card: MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, ICard
    {
        [SerializeField]
        private Texture2D _image;
        [SerializeField]
        private string _name;
        /*[SerializeField]
        private bool _played;*/
        [SerializeField]
        private CardType _cardType;

        public Transform startPosition =null;
        public Transform dragPosition = null;

        private RectTransform _cardTransform;
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;

        public string Name => _name;

        public Texture2D CardImage => _image;

        //public Board<Posit>
        public CardType Type { get => _cardType; set => _cardType = value; }

        private void Start()
        {
            _cardTransform = GetComponent<RectTransform>();
            /*this.gameObject.name = Name;*/
            startPosition = this.transform;
            //this.gameObject.GetComponent<Image>().mainTexture = Image;
            _canvas = FindObjectOfType<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Used()
        {
            GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
            Destroy(this.gameObject);
        }

        public event EventHandler<CardEventArgs> Clicked;
        public event EventHandler<CardEventArgs> Selected;
        public event EventHandler<CardEventArgs> Deselected;
        public event EventHandler<CardEventArgs> Dragging;


        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnClicked");
            OnClicking(this, new CardEventArgs(this));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
            _canvasGroup.blocksRaycasts = false;
            GetComponentInParent<HorizontalLayoutGroup>().enabled = false;

            OnBeginDragging(this, new CardEventArgs(this));
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("OnDrag");
            this.transform.position = eventData.position;
            _cardTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
            dragPosition = _cardTransform.transform;
            OnDragging(this, new CardEventArgs(this));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");

            //bool usedCard = UseCard(eventData);

            OnEndDragging(this, new CardEventArgs(this));
            _canvasGroup.blocksRaycasts = true;
            GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
            this.transform.position = startPosition.position;

            /*if (usedCard)
            {
                Destroy(gameObject);
            }
            else
            {
                this.transform.position = startPosition.position;
            }*/
        }

        protected virtual void OnClicking(object source, CardEventArgs e)
        {
            var handler = Clicked;
            handler?.Invoke(this, e);
        }

        protected virtual void OnBeginDragging(object source, CardEventArgs e)
        {
            var handler = Selected;
            handler?.Invoke(this, e);
        }

        protected virtual void OnDragging(object source, CardEventArgs e)
        {
            var handler = Dragging;
            handler?.Invoke(this, e);
        }

        protected virtual void OnEndDragging(object source, CardEventArgs e)
        {
            var handler = Deselected;
            handler?.Invoke(this, e);
        }

        //Checks whether space can be selected or not
        protected virtual bool CanUtilize(PointerEventData eventData)
        {
            return false;
        }

        // Uses card or returns false when it couldn't be used
        protected virtual bool UseCard(PointerEventData eventData)
        {
            return false;
        }

        public void Activate(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}