using UnityEngine;

namespace Ultrarogue.Behaviours
{
    public class BossArmor : MonoBehaviour
    {
        public float Armor = 0.25f;

        public float DamageThreshold = 10f;

        public float Scaling = 0.5f;

        public float CalculateDamage(float damage)
        {
            if (damage <= 0f)
                return 0f;

            if (damage <= DamageThreshold)
            {
                return Mathf.Max(damage * (1f - Armor), 1f);
            }

            float excessDamage = damage - DamageThreshold;

            float extraReduction = excessDamage / (excessDamage + DamageThreshold);
            extraReduction *= Scaling;

            float totalReduction = Armor + extraReduction;

            totalReduction = Mathf.Clamp(totalReduction, 0f, 0.9f);

            float finalDamage = damage * (1f - totalReduction);

            return Mathf.Max(finalDamage, 1f);
        }
    }
}