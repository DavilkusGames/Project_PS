using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProgramPanelCntrl : Singleton<ProgramPanelCntrl>, IDropHandler
{
    public RobotCntrl robot;
    public RectTransform rectTransform;

    public void OnDrop(PointerEventData eventData)
    {
        if (!eventData.pointerDrag.CompareTag("CommandBlock")) return;
        if (eventData.pointerDrag != null)
        {
            CommandBlock cmdBlock = eventData.pointerDrag.GetComponent<CommandBlock>();
            cmdBlock.rectTransform.SetParent(rectTransform);
            cmdBlock.InProgram();
            robot.AddCommand(cmdBlock);
        }
    }

    public void RemoveFromProgram(CommandBlock cmdBlock)
    {
        robot.RemoveCommand(cmdBlock);
    }
}
