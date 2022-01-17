using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace HEX.CardSystem
{
	public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
	{

		public void OnPointerEnter(PointerEventData eventData)
		{
			//Debug.Log("OnPointerEnter");
			if (eventData.pointerDrag == null)
				return;

			Card d = eventData.pointerDrag.GetComponent<Card>();
			if (d != null)
			{
				d.dragPosition = this.transform;
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			//Debug.Log("OnPointerExit");
			if (eventData.pointerDrag == null)
				return;

			Card d = eventData.pointerDrag.GetComponent<Card>();
			if (d != null && d.dragPosition == this.transform)
			{
				d.dragPosition = d.startPosition;
			}
		}

		public void OnDrop(PointerEventData eventData)
		{
			Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

			Card d = eventData.pointerDrag.GetComponent<Card>();
			if (d != null)
			{
				d.startPosition = this.transform;
			}

		}
	}
}
