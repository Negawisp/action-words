using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayService
{
    /// <summary>
    /// Generates a sequence of random unrepeating integers.
    /// </summary>
    /// <param name="array">array to fill</param>
    /// <param name="lowerGap">Minimum random number (inclusive)</param>
    /// <param name="higherGap">Maximum random number (exclusive)</param>
    /// <returns>The sequence</returns>
    public static int[] GenerateRandomUnrepeatingSequence(int[] array, int lowerGap, int higherGap)
    {
        int i = 0;
        int length = array.Length;
        int poolCapacity = higherGap - lowerGap;
        ArrayList numbersPool = new ArrayList(poolCapacity);
        for (i = 0; i < poolCapacity; i++)
        {
            numbersPool.Add(lowerGap + i);
            Debug.LogFormat("NumbersPool[{0}]: {1}", i, numbersPool[i]);
        }
        for (i = 0; i < length; i++)
        {
            int j = UnityEngine.Random.Range(0, poolCapacity - i);
            array[i] = (int)numbersPool[j];
            numbersPool.RemoveAt(j);
            Debug.LogFormat("retArray[{0}]: {1}", i, array[i]);
        }
        return array;
    }


    public static bool SequenceIsOrdered (in int[] sequence)
    {
        if (null == sequence)
        {
            throw new NullReferenceException();
        }

        int sequenceLength = sequence.Length;
        int delta = 0;
        int prevDelta = 0;
        bool retVal = true;


        for (int i = 1; i < sequenceLength; i++)
        {
            if (delta != 0)
            {
                if (prevDelta != 0 && delta * prevDelta < 0)
                {
                    retVal = false;
                    break;
                }
                prevDelta = delta;
            }
            delta = sequence[i] - sequence[i - 1];
            Debug.LogFormat("Delta: {0}, prevDelta: {1}", delta, prevDelta);
        }

        return retVal;
    }

    public static bool SequenceIsIncreasing(int[] sequence)
    {
        if (null == sequence)
        {
            throw new NullReferenceException();
        }

        int sequenceLength = sequence.Length;
        int delta = 0;
        bool retVal = true;

        for (int i = 1; i < sequenceLength; i++)
        {
            delta = sequence[i] - sequence[i - 1];
            if (delta < 0)
            {
                retVal = false;
                break;
            }
        }

        return retVal;
    }
}
