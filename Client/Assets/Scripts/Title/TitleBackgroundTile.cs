using UnityEngine;

public class TitleBackgroundTile : MonoBehaviour {

    public Rigidbody Rigidbody;

    private Vector3 mDefaultPosition;

    void Awake()
    {
        mDefaultPosition = transform.localPosition;
        
    }

    public void Reset()
    {
        transform.localPosition = mDefaultPosition;
        transform.rotation = Quaternion.identity;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
    }

    public void PlayAnimation()
    {
        float x = Random.Range(-200.0f, 200.0f);
        float y = Random.Range(-200.0f, 200.0f);
        float z = Random.Range(-200.0f, 200.0f);
        Rigidbody.AddForce(new Vector3(x, y, z));
    }
}
