using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffects : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float disappearTime = 1.0f;
    [SerializeField] private float disappearSpeed = 3.0f;
    [SerializeField] private Color popupColor = Color.white;
    [SerializeField] private Transform damagePopupPrefab;
    [SerializeField] private Transform parentGameObject;

    public DamagePopup Create(Vector3 position, int damage)
    {
        // instantiate damage prefab
        Transform damagePopupTransform = Instantiate(damagePopupPrefab, position, Quaternion.identity);

        // set the parent game object
        if (parentGameObject != null)
        {
            damagePopupTransform.SetParent(parentGameObject);
        }

        // get damage popup script to set damage text
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.setText(damage, popupColor, moveSpeed, disappearTime, disappearSpeed);

        // return damage popup
        return damagePopup;
    }
}
