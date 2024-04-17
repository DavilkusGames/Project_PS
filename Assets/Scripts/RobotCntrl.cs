using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotCntrl : MonoBehaviour
{
    public Button executeBtn;
    public Button stopBtn;
    public Button clearBtn;

    public float cmdDelay = 1f;
    private List<CommandBlock> program = new List<CommandBlock>();
    private int cmdId = 0;
    private bool isExecuting = false;

    public void AddCommand(CommandBlock command)
    {
        program.Add(command);
        executeBtn.interactable = true;
    }

    public void RemoveCommand(CommandBlock command)
    {
        program.Remove(command);
        if (program.Count == 0) executeBtn.interactable = false;
    }

    public void ClearProgram()
    {
        for (int i = 0; i < program.Count; i++)
        {
            Destroy(program[i].gameObject);
        }
        program.Clear();
        executeBtn.interactable = false;
    }

    public void StartProgram()
    {
        if (program.Count == 0 || isExecuting) return;
        StartCoroutine(nameof(ExecuteProgram));
        executeBtn.gameObject.SetActive(false);
        stopBtn.gameObject.SetActive(true);
        clearBtn.interactable = false;
        foreach (CommandBlock command in program) { command.LockState(true); }
    }

    public void StopProgram()
    {
        if (isExecuting)
        {
            StopCoroutine(nameof(ExecuteProgram));
            program[cmdId].LightState(false);
        }
        executeBtn.gameObject.SetActive(true);
        stopBtn.gameObject.SetActive(false);
        clearBtn.interactable = true;
        foreach (CommandBlock command in program) { command.LockState(false); }
    }

    private void ExecuteCmd(Command cmd)
    {
        Debug.Log("Executing Command: " + cmd);
    }

    private IEnumerator ExecuteProgram()
    {
        isExecuting = true;
        cmdId = 0;
        yield return new WaitForSeconds(0.2f);
        while (cmdId < program.Count)
        {
            program[cmdId].LightState(true);
            ExecuteCmd(program[cmdId].cmd);
            yield return new WaitForSeconds(cmdDelay);
            program[cmdId].LightState(false);
            cmdId++;
        }
        isExecuting = false;
        StopProgram();
    }
}
