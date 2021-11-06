using UnityEngine;

public class MoveCamera : MonoBehaviour {

    [SerializeField]public Transform player;

    void Update() {
        transform.position = player.transform.position;
    }
}
