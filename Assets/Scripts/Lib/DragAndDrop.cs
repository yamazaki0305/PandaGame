//[UI] ドラッグアンドドロップでUIのイメージを動かす
//http://negi-lab.blog.jp/DragAndDrop

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Imageコンポーネントを必要とする
[RequireComponent(typeof(Image))]

// ドラッグとドロップに関するインターフェースを実装する
public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
, IPointerDownHandler
, IPointerEnterHandler
, IPointerExitHandler
, IPointerUpHandler
{
    // ドラッグ前の位置
    private Vector2 prevPos;

    public void OnPointerEnter(PointerEventData eventData) { Debug.Log("OnPointerEnter"); }
    public void OnPointerDown(PointerEventData eventData)
    {
        // ドラッグアイテムのサイズを大きく
        this.GetComponent<Image>().transform.localScale = new Vector2(1.5f, 1.5f);
        Debug.Log("OnPointerDown");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // ドラッグアイテムのサイズを元に戻す
        this.GetComponent<Image>().transform.localScale = new Vector2(1.0f, 1.0f);
        Debug.Log("OnPointerUp");
    }
    public void OnPointerExit(PointerEventData eventData) { Debug.Log("OnPointerExit"); }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ドラッグ前の位置を記憶しておく
        prevPos = transform.position;

        // ドラッグアイテムのサイズを大きく
        this.GetComponent<Image>().transform.localScale = new Vector2(1.5f, 1.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ドラッグ中は位置を更新する
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ドラッグ前の位置に戻す
        transform.position = prevPos;

        // ドラッグアイテムのサイズを元に戻す
        this.GetComponent<Image>().transform.localScale = new Vector2(1.0f, 1.0f);

        // ハンマー処理を行う
        GameObject.Find("GameRoot").GetComponent<PuzzleMain>().DragHummerButton();

    }

    public void OnDrop(PointerEventData eventData)
    {
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var hit in raycastResults)
        {
            // もし DroppableField の上なら、その位置に固定する
            /*
            if (hit.gameObject.CompareTag("DroppableField"))
            {
                transform.position = hit.gameObject.transform.position;
                this.enabled = false;
            }
            */
        }
    }
}