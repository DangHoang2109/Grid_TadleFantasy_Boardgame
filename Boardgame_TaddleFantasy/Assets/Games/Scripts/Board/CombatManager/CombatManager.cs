using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    void Awake() => Instance = this;
    public static int[] D6 = new int[] { 1, 2, 3, 4, 5, 6 };

    public static int GenerateRollingAttackDiceResult(int dices)
    {
        int total = 0;
        for (int i = 0; i < dices; i++) { total += D6.Random(); }
        return total;
    }
}
