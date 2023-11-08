using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency
{
    public static string DisplauCurrency(int gold)
    {
        char secondChar = '\0';
        char thirdChar = '\0';
        if (gold == 0)
        {
            return "0";
        }

        int goldLenght = gold.ToString().Length;
        if (goldLenght > 1)
        {
            secondChar = gold.ToString()[1];
            if (goldLenght > 2)
            {
                thirdChar = gold.ToString()[2];
            }
        }

        char firstChar = gold.ToString()[0];


        switch (goldLenght)
        {
            case 1:
            case 2:
            case 3:
                return gold.ToString();
            case 4:
                return $"{firstChar}K";
                break;
            case 5:
                return $"{firstChar}{secondChar}K";
            case 6:
                return $"{firstChar}{secondChar}{thirdChar}K";
            case 7:
                return $"{firstChar}.{secondChar}{thirdChar}M";
                break;
            case 8:
                if (gold >= 10000000 && gold <= 99999999)
                {
                    return $"{firstChar}{secondChar}.{thirdChar}M";
                }

                break;
            case 9:
                if (gold >= 100000000 && gold <= 999999999)
                {
                    return $"{firstChar}{secondChar}{thirdChar}M";
                }

                break;
            case 10:
                if (gold >= 1000000000 && gold <= 9999999999)
                {
                    return $"{firstChar}.{secondChar}{thirdChar}B";
                }

                break;
        }

        return String.Empty;
    }
}