using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Command { Forward, Backwards, TurnRight, TurnLeft, Pickup, Put };

public class CommandBlock : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public enum BlockState { BlocksPanel, Free, InProgram, Locked }

    public Command cmd;
    public GameObject prefab;
    public Canvas canvas;
    public RectTransform rectTransform;
    public Sprite[] cmdSprites;
    public GameObject lightObj;

    private CanvasGroup canvasGroup;
    private BlockState state = BlockState.BlocksPanel;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void InProgram()
    {
        state = BlockState.InProgram;
    }

    public void LockState(bool isLocked)
    {
        canvasGroup.alpha = isLocked ? 0.5f : 1f;
        state = isLocked ? BlockState.Locked : BlockState.InProgram;
    }

    public void LightState(bool isLighted)
    {
        canvasGroup.alpha = isLighted ? 1f : 0.5f;
        lightObj.SetActive(isLighted);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (state == BlockState.BlocksPanel)
        {
            GameObject newBlock = Instantiate(prefab, rectTransform.position, Quaternion.identity);
            newBlock.transform.SetParent(rectTransform.parent);
            CommandBlock cmdBlock = newBlock.GetComponent<CommandBlock>();
            cmdBlock.canvas = canvas;
            cmdBlock.cmd = cmd;
            newBlock.GetComponent<Image>().sprite = cmdSprites[(int)cmd];
        }
        if (state == BlockState.InProgram)
        {
            ProgramPanelCntrl.Instance.RemoveFromProgram(this);
        }
        canvasGroup.alpha = 0.85f;
        canvasGroup.blocksRaycasts = false;
        rectTransform.SetParent(canvas.transform);
        state = BlockState.Free;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (state != BlockState.InProgram) Destroy(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
