using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatSO", menuName = "Variables/Create new Float", order = 0)]
public class FloatSO : ObjectIDSO
{
    private FloatReference varValue;

    public FloatReference Value { get { return varValue; } set { varValue = value; } }
}

[System.Serializable]
public class FloatReference
{
    public bool isConstant;
    public static float value;
    public float varValue;

    public float Float { get { return isConstant ? value : varValue; } set { if (!isConstant) varValue = value; } }
}