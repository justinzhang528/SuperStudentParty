using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaughtyCharacter
{
    public class WeaponCollider : MonoBehaviour
    {
        public AudioSource destroyAudio;
        public GameObject destroyEffect;
        public GameObject damageEffect;
        private float timer, preTime = 0;
        private bool isInAttack = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (timer - preTime > 0.2f && timer - preTime < 0.5f)
                isInAttack = true;
            else
                isInAttack = false;
            timer += Time.deltaTime;
            if (Input.GetMouseButtonDown(0) && (timer - preTime) > 0.6f)
            {
                preTime = timer;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy" && isInAttack)
            {
                destroyAudio.Play();
                Destroy(other.gameObject);
                Instantiate(destroyEffect, other.gameObject.transform.position, Quaternion.identity);
            }

            if (other.gameObject.tag == "Player" && isInAttack)
            {
                other.gameObject.GetComponent<Animator>().SetTrigger("damage");
                GameObject effect = Instantiate(damageEffect, other.gameObject.transform.position, Quaternion.identity);
                Destroy(effect, 1f);
            }
        }
    }
}

