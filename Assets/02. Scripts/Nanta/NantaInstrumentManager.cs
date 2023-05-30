using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NantaInstrumentManager : MonoBehaviour
{
    public AbstractNantaInstrument[] Instruments;
    private List<IEnumerator> changeRoutines = new List<IEnumerator>();
    public void Initialize()
    {
        foreach(var instrument in Instruments)
        {
            instrument.Initialize();
            instrument.gameObject.SetActive(false);
        }
    }

    public void ChangeInstrument(float time, int instrumentIndex)
    {
        IEnumerator routine = ChangeRoutine(time, instrumentIndex);
        changeRoutines.Add(routine);
        StartCoroutine(routine);
    }

    IEnumerator ChangeRoutine(float time, int instrumentIndex)
    {
        yield return new WaitForSeconds(time);
        foreach(var instrument in Instruments)
        {
            instrument.gameObject.SetActive(false);
        }
        Instruments[instrumentIndex].gameObject.SetActive(true);
    }

    public void ResetAll()
    {
        foreach(var routine in changeRoutines)
        {
            StopCoroutine(routine);
        }
        changeRoutines.Clear();

        foreach(var instrument in Instruments)
        {
            instrument.gameObject.SetActive(false);
        }
    }
}
