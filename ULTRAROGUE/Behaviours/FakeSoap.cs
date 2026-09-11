using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class FakeSoap : MonoBehaviour
{
    // Token: 0x06001DA0 RID: 7584 RVA: 0x000E8435 File Offset: 0x000E6635
    private void Start()
    {
        this.itid = base.GetComponent<ItemIdentifier>();
        this.rb = base.GetComponent<Rigidbody>();
    }

    // Token: 0x06001DA1 RID: 7585 RVA: 0x000E844F File Offset: 0x000E664F
    private void FixedUpdate()
    {
        if (this.rb)
        {
            this.velocityBeforeCollision = this.rb.velocity;
        }
    }

    public void FuckingExplode()
    {
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }

    public GameObject explosion;

    // Token: 0x06001DA2 RID: 7586 RVA: 0x000E8470 File Offset: 0x000E6670
    private void OnCollisionEnter(Collision collision)
    {
        if (!this.itid.pickedUp && this.velocityBeforeCollision.magnitude > 15f)
        {
            EnemyIdentifierIdentifier enemyIdentifierIdentifier;
            if ((collision.gameObject.layer == 11 || collision.gameObject.layer == 10) && collision.gameObject.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
            {
                if (enemyIdentifierIdentifier.eid)
                {
                    FuckingExplode();
                }
                this.rb.velocity = Vector3.zero;
                return;
            }
            Breakable breakable;
            if (collision.gameObject.TryGetComponent<Breakable>(out breakable) && !breakable.specialCaseOnly)
            {
                breakable.Break();
                this.rb.velocity = Vector3.zero;
                return;
            }
            Bleeder bleeder;
            if (collision.gameObject.TryGetComponent<Bleeder>(out bleeder))
            {
                bleeder.GetHit(base.transform.position, GoreType.Head, false);
            }
        }
    }

    // Token: 0x06001DA3 RID: 7587 RVA: 0x000E8570 File Offset: 0x000E6770
    public void HitWith(GameObject target)
    {
        EnemyIdentifierIdentifier enemyIdentifierIdentifier;
        if (target.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
        {
            enemyIdentifierIdentifier.eid.DeliverDamage(target, Vector3.zero, target.transform.position, 999999f, true, 0f, null, false, false);
        }
    }

    // Token: 0x04002659 RID: 9817
    private ItemIdentifier itid;

    // Token: 0x0400265A RID: 9818
    private Rigidbody rb;

    // Token: 0x0400265B RID: 9819
    private Vector3 velocityBeforeCollision;
}
