using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        BasicEnemy basicEnemy = other.GetComponent<BasicEnemy>();
        ResourceManager.instance.Health -= basicEnemy.damage;
        Destroy(other.gameObject);
    }
}
