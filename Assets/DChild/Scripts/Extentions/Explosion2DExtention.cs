using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public static class Explosion2DExtention
    {
        public static void AddExplosiveForce(this Rigidbody2D rigidbody, float force, float radius, float upliftModified)
        {
            rigidbody.AddExplosiveForce(force, rigidbody.position, radius, upliftModified);
        }

        public static void AddExplosiveForce(this Rigidbody2D rigidbody, float force, Vector2 explosionPosition, float radius, float upliftModified)
        {
            List<Rigidbody2D> affectedRigidbodies = CastExplosion(rigidbody, explosionPosition, radius);
            ApplyExposiveForce(force, explosionPosition, radius, upliftModified, affectedRigidbodies);
            CastExplosiveForce(rigidbody, force, explosionPosition, radius, Physics2D.GetLayerCollisionMask(rigidbody.gameObject.layer));
        }

        public static void CastExplosiveForce(this Rigidbody2D rigidbody, float force, float radius)
        {
            CastExplosiveForce(rigidbody, force, rigidbody.position, radius, Physics2D.GetLayerCollisionMask(rigidbody.gameObject.layer));
        }

        public static void CastExplosiveForce(this Rigidbody2D rigidbody, float force, float radius, int layer)
        {
            CastExplosiveForce(rigidbody, force, rigidbody.position, radius, layer);
        }

        public static void CastExplosiveForce(this Rigidbody2D rigidbody, float force, Vector2 explosionPosition, float radius, int layer)
        {
            List<IExplosionReact> ignoreList = new List<IExplosionReact>();
            var affectedColliders = Physics2D.OverlapCircleAll(explosionPosition, radius, layer);
            for (int i = 0; i < affectedColliders.Length; i++)
            {
                var reactant = affectedColliders[i].GetComponentInParent<IExplosionReact>();
                if (reactant != null)
                {
                    if (IsIgnored(ignoreList, reactant) == false)
                    {
                        var impactForce = CalculateImpactForce(reactant.transform.position, explosionPosition, radius, force);
                        reactant.React(explosionPosition, impactForce);
                        ignoreList.Add(reactant);
                    }
                }
            }
        }

        private static Vector2 CalculateImpactForce(Vector2 bodyPosition, Vector2 explosionPosition, float radius, float force)
        {
            var dir = ((Vector2)bodyPosition - explosionPosition);
            float calc = 1 - (dir.magnitude / radius);
            if (calc <= 0)
            {
                calc = 0;
            }
            return dir.normalized * force * calc;
        }


        private static bool IsIgnored(List<IExplosionReact> ignoreList, IExplosionReact reactant)
        {
            for (int j = 0; j < ignoreList.Count; j++)
            {
                if (ignoreList[j] == reactant)
                {
                    return true;
                }
            }

            return false;
        }

        private static void ApplyExposiveForce(float force, Vector2 explosionPosition, float radius, float upliftModified, List<Rigidbody2D> affectedRigidbodies)
        {
            for (int i = 0; i < affectedRigidbodies.Count; i++)
            {
                //Calculations is taken from 2D Explosion Force Asset
                var body = affectedRigidbodies[i];
                var dir = ((Vector2)body.position - explosionPosition);
                float calc = 1 - (dir.magnitude / radius);
                if (calc <= 0)
                {
                    calc = 0;
                }
                body.AddForce(dir.normalized * force * calc, ForceMode2D.Impulse);
                body.AddForce(Vector2.up * upliftModified * calc, ForceMode2D.Impulse);
            }
        }

        private static List<Rigidbody2D> CastExplosion(Rigidbody2D rigidbody, Vector2 explisionPosition, float radius)
        {
            List<Rigidbody2D> affectedRigidbodies = new List<Rigidbody2D>();
            var affectedColliders = Physics2D.OverlapCircleAll(explisionPosition, radius, Physics2D.GetLayerCollisionMask(rigidbody.gameObject.layer));
            for (int i = 0; i < affectedColliders.Length; i++)
            {
                var affectedRigidbody = affectedColliders[i].GetComponentInParent<Rigidbody2D>();
                if (affectedRigidbody != null && affectedRigidbody.bodyType == RigidbodyType2D.Dynamic)
                {
                    affectedRigidbodies.Add(affectedRigidbody);
                }
            }

            return affectedRigidbodies;
        }
    }

}