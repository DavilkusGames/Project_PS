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
    public float lerpK = 3f;

    public Transform spawn;
    public LineRenderer pathLine;
    public Transform boxParentPoint;

    private List<CommandBlock> program = new List<CommandBlock>();
    private Transform trans;
    private Rigidbody rb;
    private Animator anim;
    private int cmdId = 0;
    private bool isExecuting = false;
    private int curLineIdVrtx = 1;
    private GameObject cubeLoaded;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Start()
    {
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        if (!isExecuting) return;
        if (trans.position != targetPosition) trans.position = Vector3.Lerp(trans.position, targetPosition, lerpK * Time.deltaTime);
        if (trans.rotation != targetRotation) trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, lerpK * Time.deltaTime);
        pathLine.SetPosition(curLineIdVrtx, trans.position + Vector3.up * 0.5f);
    }

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
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        trans.position = spawn.position;
        trans.rotation = spawn.rotation;
        targetPosition = trans.position;
        targetRotation = trans.rotation;

        pathLine.positionCount = 2;
        pathLine.SetPosition(0, spawn.position+Vector3.up*0.2f);
        curLineIdVrtx = 1;
        GameManager.Instance.BlocksPanelState(false);

        cubeLoaded = null;
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (var box in boxes) box.GetComponent<BoxCntrl>().Respawn();

        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (var button in buttons) button.GetComponent<ButtonCntrl>().Reset();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            GameManager.Instance.LevelCompleted();
        }
        else if (other.CompareTag("Obstacle"))
        {
            StopProgram();
        }
        else if (other.CompareTag("Box") && other.gameObject != cubeLoaded)
        {
            StopProgram();
        }
    }

    public void StopProgram()
    {
        if (isExecuting)
        {
            StopCoroutine(nameof(ExecuteProgram));
        }
        isExecuting = false;
        executeBtn.gameObject.SetActive(true);
        stopBtn.gameObject.SetActive(false);
        clearBtn.interactable = true;
        foreach (CommandBlock command in program) { command.LockState(false); }
        GameManager.Instance.BlocksPanelState(true);
    }

    public void BoxAttach()
    {
        cubeLoaded.transform.SetParent(boxParentPoint);
    }

    public void BoxRelease()
    {
        cubeLoaded.transform.SetParent(null);
        cubeLoaded = null;
    }

    private float ExecuteCmd(Command cmd)
    {
        RaycastHit hit;
        switch (cmd)
        {
            case Command.Forward:
                targetPosition = trans.position + trans.right * 2f;
                break;
            case Command.Backwards:
                targetPosition = trans.position - trans.right * 2f;
                break;
            case Command.TurnRight:
                //targetRotation = trans.rotation;
                break;
            case Command.TurnLeft:
                break;
            case Command.Pickup:
                if (cubeLoaded != null) StopProgram();
                if (Physics.Raycast(trans.position + Vector3.up * 0.2f, Vector3.right, out hit, 2f))
                {
                    if (hit.collider.CompareTag("Box"))
                    {
                        cubeLoaded = hit.collider.gameObject;
                        anim.Play("PickupBox");
                        return 1f;
                    }
                }
                else StopProgram();
                break;
            case Command.Put:
                if (cubeLoaded == null) StopProgram();
                if (Physics.Raycast(trans.position + Vector3.up * 0.2f, Vector3.right, out hit, 2f))
                {
                    if (hit.collider.CompareTag("Button"))
                    {
                        anim.Play("PutBox");
                        return 1f;
                    }
                }
                else StopProgram();
                break;
        }
        return 0f;
    }

    private IEnumerator ExecuteProgram()
    {
        isExecuting = true;
        cmdId = 0;
        yield return new WaitForSeconds(0.2f);
        while (cmdId < program.Count && isExecuting)
        {
            program[cmdId].LightState(true);
            float extraTime = ExecuteCmd(program[cmdId].cmd);
            yield return new WaitForSeconds(cmdDelay + extraTime);
            trans.position = targetPosition;
            trans.rotation = targetRotation;

            if (!Physics.Raycast(trans.position+Vector3.up*0.5f, Vector3.down, 0.5f))
            {
                StopProgram();
                rb.useGravity = true;
            }
            else
            {
                curLineIdVrtx++;
                if (cmdId + 1 < program.Count) pathLine.positionCount++;
                program[cmdId].LightState(false);
                cmdId++;
            }
        }
        isExecuting = false;
        StopProgram();
    }
}
