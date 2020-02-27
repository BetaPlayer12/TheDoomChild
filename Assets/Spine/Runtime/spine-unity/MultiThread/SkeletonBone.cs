using Spine;
using System;
using UnityEngine;

namespace DChild
{
    #region Jobs


    public struct SkeletonBone
    {
        #region Variables
        public SkeletonBone(Bone bone)
        {
            if (bone != null)
            {
                hashCode = bone.GetHashCode();
                transformMode = bone.data.transformMode;

                ax = bone.x;
                ay = bone.y;
                arotation = bone.rotation;
                ascaleX = bone.scaleX;
                ascaleY = bone.scaleY;
                ashearX = bone.shearX;
                ashearY = bone.shearY;
                appliedValid = false;

                skeletonScaleX = bone.skeleton.ScaleX;
                skeletonScaleY = bone.skeleton.ScaleY;
                skeletonX = bone.skeleton.x;
                skeletonY = bone.skeleton.y;

                hasParent = bone.parent != null;
                parent = new SkeletonBoneIndex(bone.parent);
                dataLength = bone.data.length;

                x = bone.x;
                y = bone.y;
                rotation = bone.rotation;
                scaleX = bone.scaleX;
                scaleY = bone.scaleY;
                shearX = bone.shearX;
                shearY = bone.shearY;

                a = bone.a;
                b = bone.b;
                c = bone.c;
                d = bone.d;
                worldX = bone.worldX;
                worldY = bone.worldY;
            }
            else
            {
                hashCode = 0;
                transformMode = TransformMode.Normal;

                ax = 0;
                ay = 0;
                arotation = 0;
                ascaleX = 0;
                ascaleY = 0;
                ashearX = 0;
                ashearY = 0;
                appliedValid = false;

                skeletonScaleX = 0;
                skeletonScaleY = 0;
                skeletonX = 0;
                skeletonY = 0;

                hasParent = false;
                parent = new SkeletonBoneIndex(null);
                dataLength = 0f;

                x = 0;
                y = 0;
                rotation = 0;
                scaleX = 0;
                scaleY = 0;
                shearX = 0;
                shearY = 0;

                a = 0;
                b = 0;
                c = 0;
                d = 0;
                worldX = 0;
                worldY = 0;
            }
        }

        public int hashCode { get; }
        public bool hasParent { get; }
        public SkeletonBoneIndex parent;

        public float dataLength { get; }

        public float skeletonScaleX { get; }
        public float skeletonScaleY { get; }
        public float skeletonX { get; }
        public float skeletonY { get; }


        public float ax { get; private set; }
        public float ay { get; private set; }
        public float arotation { get; private set; }
        public float ascaleX { get; private set; }
        public float ascaleY { get; private set; }

        public float ashearX { get; private set; }
        public float ashearY { get; private set; }
        public bool appliedValid;

        public TransformMode transformMode { get; }

        public float x, y, rotation, scaleX, scaleY, shearX, shearY;
        public float a, b, c, d;
        public float worldX, worldY;
        #endregion

        public void LocalToWorld(float localX, float localY, out float worldX, out float worldY)
        {
            worldX = localX * a + localY * b + this.worldX;
            worldY = localX * c + localY * d + this.worldY;
        }

        public void UpdateWorldTransform(SkeletonBone parent)
        {
            UpdateWorldTransform(x, y, rotation, scaleX, scaleY, shearX, shearY, parent);
        }

        public void UpdateWorldTransform(float x, float y, float rotation, float scaleX, float scaleY, float shearX, float shearY, SkeletonBone parent)
        {
            ax = x;
            ay = y;
            arotation = rotation;
            ascaleX = scaleX;
            ascaleY = scaleY;
            ashearX = shearX;
            ashearY = shearY;
            appliedValid = true;

            if (hasParent)
            { // Not Root bone.

                float pa = parent.a, pb = parent.b, pc = parent.c, pd = parent.d;
                worldX = pa * x + pb * y + parent.worldX;
                worldY = pc * x + pd * y + parent.worldY;

                switch (transformMode)
                {
                    case TransformMode.Normal:
                        {
                            float rotationY = rotation + 90 + shearY;
                            float la = MathUtils.CosDeg(rotation + shearX) * scaleX;
                            float lb = MathUtils.CosDeg(rotationY) * scaleY;
                            float lc = MathUtils.SinDeg(rotation + shearX) * scaleX;
                            float ld = MathUtils.SinDeg(rotationY) * scaleY;
                            a = pa * la + pb * lc;
                            b = pa * lb + pb * ld;
                            c = pc * la + pd * lc;
                            d = pc * lb + pd * ld;
                            //Debug.Log($"{GetHashCode()} =X= {pa}-{pb}-{pc}-{pd}");
                            return;
                        }
                    case TransformMode.OnlyTranslation:
                        {
                            float rotationY = rotation + 90 + shearY;
                            a = MathUtils.CosDeg(rotation + shearX) * scaleX;
                            b = MathUtils.CosDeg(rotationY) * scaleY;
                            c = MathUtils.SinDeg(rotation + shearX) * scaleX;
                            d = MathUtils.SinDeg(rotationY) * scaleY;
                            break;
                        }
                    case TransformMode.NoRotationOrReflection:
                        {
                            float s = pa * pa + pc * pc, prx;
                            if (s > 0.0001f)
                            {
                                s = Math.Abs(pa * pd - pb * pc) / s;
                                pb = pc * s;
                                pd = pa * s;
                                prx = MathUtils.Atan2(pc, pa) * MathUtils.RadDeg;
                            }
                            else
                            {
                                pa = 0;
                                pc = 0;
                                prx = 90 - MathUtils.Atan2(pd, pb) * MathUtils.RadDeg;
                            }
                            float rx = rotation + shearX - prx;
                            float ry = rotation + shearY - prx + 90;
                            float la = MathUtils.CosDeg(rx) * scaleX;
                            float lb = MathUtils.CosDeg(ry) * scaleY;
                            float lc = MathUtils.SinDeg(rx) * scaleX;
                            float ld = MathUtils.SinDeg(ry) * scaleY;
                            a = pa * la - pb * lc;
                            b = pa * lb - pb * ld;
                            c = pc * la + pd * lc;
                            d = pc * lb + pd * ld;
                            break;
                        }
                    case TransformMode.NoScale:
                    case TransformMode.NoScaleOrReflection:
                        {
                            float cos = MathUtils.CosDeg(rotation), sin = MathUtils.SinDeg(rotation);
                            float za = (pa * cos + pb * sin) / skeletonScaleX;
                            float zc = (pc * cos + pd * sin) / skeletonScaleY;
                            float s = (float)Math.Sqrt(za * za + zc * zc);
                            if (s > 0.00001f) s = 1 / s;
                            za *= s;
                            zc *= s;
                            s = (float)Math.Sqrt(za * za + zc * zc);
                            if (transformMode == TransformMode.NoScale
                                && (pa * pd - pb * pc < 0) != (skeletonScaleX < 0 != skeletonScaleY < 0)) s = -s;

                            float r = MathUtils.PI / 2 + MathUtils.Atan2(zc, za);
                            float zb = MathUtils.Cos(r) * s;
                            float zd = MathUtils.Sin(r) * s;
                            float la = MathUtils.CosDeg(shearX) * scaleX;
                            float lb = MathUtils.CosDeg(90 + shearY) * scaleY;
                            float lc = MathUtils.SinDeg(shearX) * scaleX;
                            float ld = MathUtils.SinDeg(90 + shearY) * scaleY;
                            a = za * la + zb * lc;
                            b = za * lb + zb * ld;
                            c = zc * la + zd * lc;
                            d = zc * lb + zd * ld;
                            break;
                        }
                }

                a *= skeletonScaleX;
                b *= skeletonScaleX;
                c *= skeletonScaleY;
                d *= skeletonScaleY;
            }
            else
            {
                float rotationY = rotation + 90 + shearY;
                a = MathUtils.CosDeg(rotation + shearX) * scaleX * skeletonScaleX;
                b = MathUtils.CosDeg(rotationY) * scaleY * skeletonScaleX;
                c = MathUtils.SinDeg(rotation + shearX) * scaleX * skeletonScaleY;
                d = MathUtils.SinDeg(rotationY) * scaleY * skeletonScaleY;
                worldX = x * skeletonScaleX + skeletonX;
                worldY = y * skeletonScaleY + skeletonY;
            }


        }

        public void UpdateAppliedTransform(SkeletonBone parent)
        {
            appliedValid = true;
            if (hasParent)
            {
                float pa = parent.a, pb = parent.b, pc = parent.c, pd = parent.d;
                float pid = 1 / (pa * pd - pb * pc);
                float dx = worldX - parent.worldX, dy = worldY - parent.worldY;
                ax = (dx * pd * pid - dy * pb * pid);
                ay = (dy * pa * pid - dx * pc * pid);
                float ia = pid * pd;
                float id = pid * pa;
                float ib = pid * pb;
                float ic = pid * pc;
                float ra = ia * a - ib * c;
                float rb = ia * b - ib * d;
                float rc = id * c - ic * a;
                float rd = id * d - ic * b;
                ashearX = 0;
                ascaleX = (float)Math.Sqrt(ra * ra + rc * rc);
                if (ascaleX > 0.0001f)
                {
                    float det = ra * rd - rb * rc;
                    ascaleY = det / ascaleX;
                    ashearY = MathUtils.Atan2(ra * rb + rc * rd, det) * MathUtils.RadDeg;
                    arotation = MathUtils.Atan2(rc, ra) * MathUtils.RadDeg;
                }
                else
                {
                    ascaleX = 0;
                    ascaleY = (float)Math.Sqrt(rb * rb + rd * rd);
                    ashearY = 0;
                    arotation = 90 - MathUtils.Atan2(rd, rb) * MathUtils.RadDeg;
                }
            }
            else
            {
                ax = worldX;
                ay = worldY;
                arotation = MathUtils.Atan2(c, a) * MathUtils.RadDeg;
                ascaleX = (float)Math.Sqrt(a * a + c * c);
                ascaleY = (float)Math.Sqrt(b * b + d * d);
                ashearX = 0;
                ashearY = MathUtils.Atan2(a * b + c * d, a * d - b * c) * MathUtils.RadDeg;
            }
        }
    }
    #endregion

}