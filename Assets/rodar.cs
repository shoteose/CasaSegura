using UnityEngine;

public class rodar : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(new Vector3(0,0 , 45) * Time.deltaTime);

    }
}
