using UnityEngine;

[CreateAssetMenu(fileName = "BoolSO", menuName = "Variables/Create new Bool", order = 1)]
public class BoolSO : ObjectIDSO
{
    public BoolReference varValue;
    public BoolReference Value { get { return varValue; } set { varValue = value; } }
}

[System.Serializable]
public class BoolReference
{
    public bool isConstant = true;
    public bool constantValue;
    public bool varValue;
    public bool Bool { get { return isConstant ? constantValue : varValue; } set { if (!isConstant) varValue = value; } }
}


