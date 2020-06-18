using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnFlippedFog {
    public static List<int> fogs = new List<int>() {8, 11, 12, 13, 16, 32, 42, 44, 46, 47, 48, 49, 56, 63, 64};
    public static List<string> fogOutcomes = new List<string>() {"event", "event", "event", "event", "event", "strength", "gold", "gold", "gold", "gor", "gor", "wineskin", "witches", "wp2", "wp3"};
    public static string RemoveFromList(int num) {
        fogs.Remove(num);
        //randomize outcome then remove ou;come option from list
        string outcome;
        int randomOutcome = UnityEngine.Random.Range(0, fogOutcomes.Count);
        outcome = fogOutcomes[randomOutcome];
        fogOutcomes.Remove(outcome);
        //return "witches";
        return outcome; //replace with return "witches" to test the witches brew and herbs
        

    }
}