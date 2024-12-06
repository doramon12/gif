using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class dameZone : MonoBehaviour
{

    //vung sat thuong
    public Collider dameCollider;

    public int dame = 20;

    public List<Collider> ListDame = new List<Collider>();

    public string targetTag;

    // Start is called before the first frame update
    void Start()
    {
        dameCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(targetTag) && !ListDame.Contains(other))
        {
            ListDame.Add(other);
            other.GetComponent<Enemy>().TakeDame(dame);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) && !ListDame.Contains(other))
        {
            ListDame.Add(other);
            other.GetComponent<Enemy>().TakeDame(dame);
        }
    }

    public void beginDame()
    {
        ListDame.Clear();
        dameCollider.enabled = true;
    }

    public void endDame()
    {
        ListDame.Clear();
        dameCollider.enabled = false;
    }
}
