
// Code from https://forum.unity.com/threads/cant-run-my-coroutine-ondestroy.785756/
// Credit: Kurt-Dekker (https://forum.unity.com/members/kurt-dekker.225647/)
using UnityEngine;
using System.Collections;
 
public class CallAfterDelay : MonoBehaviour
{
    float delay;
    System.Action action;
   
    public static CallAfterDelay Create( float delay, System.Action action)
    {
        CallAfterDelay cad = new GameObject("CallAfterDelay").AddComponent<CallAfterDelay>();
        cad.delay = delay;
        cad.action = action;
        return cad;
    }
   
    IEnumerator Start()
    {
        yield return new WaitForSeconds( delay);
        action();
        Destroy ( gameObject);
    }
}