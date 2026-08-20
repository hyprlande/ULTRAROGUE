using UnityEngine;

public class FilthBallBehaviour : MonoBehaviour
{
    private float speed = 15f;
    private Vector3 velocity;

    private EnemyIdentifier eid;

    private GameObject filthPrefab;

    private float nextFilthSpawn = 98f;

    private SphereCollider sphereCollider;
    private LayerMask environmentMask;

    private enum State { Roaming, Pausing, Dashing }
    private State state = State.Roaming;

    private float detectionRange = 9f;
    private float damageRange = 4f;
    private float pauseDuration = 0.4f;
    private float dashDuration = 5f;
    private float dashSpeedMultiplier = 3f;

    private float stateTimer;
    private Transform player;

    void Start()
    {
        velocity = Random.onUnitSphere * speed;
        eid = GetComponent<EnemyIdentifier>();
        nextFilthSpawn = eid.Health - 2;
        sphereCollider = GetComponent<SphereCollider>();
        environmentMask = LayerMaskDefaults.Get(LMD.Environment);

        filthPrefab = AssetsManager.GetEnemiesOfType(EnemyType.Filth)[0].gameObject;

        if (NewMovement.Instance != null)
            player = NewMovement.Instance.transform;

        GetComponent<HitEffectTriggerer>().OnHit.AddListener((dmg) =>
        {
            float hpBefore = eid.Health;
            float hpAfter = hpBefore - dmg;

            while (hpAfter <= nextFilthSpawn)
            {
                Instantiate(filthPrefab, transform.position, Quaternion.identity);

                nextFilthSpawn /= 1.28f;

                if (nextFilthSpawn <= 0.1f)
                    break;
            }
            if (hpAfter <= 0)
            {
                Destroy(gameObject);
            }
        });
    }

    void Update()
    {
        switch (state)
        {
            case State.Roaming:
                UpdateRoaming();
                break;

            case State.Pausing:
                UpdatePausing();
                break;

            case State.Dashing:
                UpdateDashing();
                break;
        }
    }

    private void UpdateRoaming()
    {
        Move(speed);

        // Check if the player has come into range.
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            state = State.Pausing;
            stateTimer = pauseDuration;
            velocity = Vector3.zero; // freeze in place during the pause
            Instantiate(AssetsManager.BlueFlash, transform.position, Quaternion.identity);
        }
    }

    private void UpdatePausing()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            if (player != null)
            {
                Vector3 dir = (player.position - transform.position).normalized;
                velocity = dir * speed * dashSpeedMultiplier;
            }
            else
            {
                velocity = Random.onUnitSphere * speed * dashSpeedMultiplier;
            }

            state = State.Dashing;
            stateTimer = dashDuration;
        }
    }

    bool HitPlayer = false;

    private void UpdateDashing()
    {
        Move(speed * dashSpeedMultiplier);

        stateTimer -= Time.deltaTime;

        if(player != null && Vector3.Distance(transform.position, player.position) <= damageRange && !HitPlayer)
        {
            HitPlayer = true;
            NewMovement.Instance.GetHurt(20, false);
        }

        if (stateTimer <= 0f)
        {
            velocity = velocity.normalized * speed;
            state = State.Roaming;
            HitPlayer = false;
        }
    }

    private void Move(float currentSpeed)
    {
        Vector3 direction = velocity.normalized;
        float distance = currentSpeed * Time.deltaTime;

        float radius = sphereCollider.radius *
                       Mathf.Max(
                           transform.lossyScale.x,
                           transform.lossyScale.y,
                           transform.lossyScale.z
                       );

        if (Physics.SphereCast(
            transform.position,
            radius,
            direction,
            out RaycastHit hit,
            distance,
            environmentMask,
            QueryTriggerInteraction.Ignore))
        {
            transform.position = hit.point + hit.normal * radius;

            velocity = Vector3.Reflect(velocity, hit.normal).normalized * currentSpeed;
        }
        else
        {
            transform.position += direction * distance;
        }
    }
}