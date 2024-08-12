using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GameState", menuName = "Game/Create new StateTracker")]
public class GameStateSO : ObjectIDSO
{
    public GameState state;

    public GameState State {  get { return state; } set { state = value; } }
}
