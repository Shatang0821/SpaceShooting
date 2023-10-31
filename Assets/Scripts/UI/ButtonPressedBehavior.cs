using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressedBehavior : StateMachineBehaviour
{
    public static Dictionary<string, System.Action> buttonFunctionTable;

    private void Awake()
    {
        buttonFunctionTable = new Dictionary<string, System.Action>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //選択アニメーションをに入ると他のInputを無効化
        UIInput.Instance.DisableAllUIInputs();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Animator animatorは
        //現在アニメーションを実行しているボタンのanimatorを返すため
        //そのオブジェクトの名前を取得する
        buttonFunctionTable[animator.gameObject.name].Invoke();
    }
}
