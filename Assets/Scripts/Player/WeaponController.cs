using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//enum EWeaponState {iShooting,CanShoot,CannotShoot,Reloading}
public class WeaponController : MonoBehaviour
{
    [SerializeField] List<Weapon> weapons;
    [SerializeField] Transform weaponPoint;
    private Weapon currentWeapon;
    public bool canShoot { get; set;} //player
    void Start()
    {
        currentWeapon = weapons[0];
        currentWeapon.Equip(weaponPoint);
        canShoot = true;
    }  
    public void PerfomShoot()
    {
        if (canShoot)
            StartCoroutine(currentWeapon.Shoot());
    }
}  
