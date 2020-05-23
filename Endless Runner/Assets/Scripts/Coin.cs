using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(70 * Time.deltaTime, 0, 0);
    }

    private void PlayCoinSound()
    {
        AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("coin_collect"), this.gameObject.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayCoinSound();
            PlayerManager.numberOfCoins += 1;
            Destroy(gameObject);
        }
    }
}
