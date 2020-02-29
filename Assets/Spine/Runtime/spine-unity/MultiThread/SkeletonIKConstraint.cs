using Spine;
using System;

namespace DChild
{
    #region Jobs


    public struct SkeletonIKConstraint
        {
            public SkeletonIKConstraint(IkConstraint constraint)
            {
                targetHashCode = constraint.target.GetHashCode();
                boneCount = constraint.bones.Count;
                if (boneCount > 0)
                {
                    boneHashCode1 = constraint.bones.Items[0].GetHashCode();
                    if (boneCount == 2)
                    {
                        boneHashCode2 = constraint.bones.Items[1].GetHashCode();
                    }
                    else
                    {
                        boneHashCode2 = 0;
                    }
                }
                else
                {
                    boneHashCode1 = 0;
                    boneHashCode2 = 0;
                }

                compress = constraint.compress;
                stretch = constraint.stretch;
                uniform = constraint.data.uniform;
                alpha = constraint.mix;
                bendDir = constraint.bendDirection;
                softness = constraint.softness;

                targetIndex = 0;
                boneIndex1 = 0;
                boneIndex2 = 0;
            }

            public int targetHashCode { get; }
            public int targetIndex;

            public int boneCount { get; }
            public int boneHashCode1 { get; }
            public int boneHashCode2 { get; }

            public int boneIndex1;

            public int boneIndex2;

            public bool compress { get; }

            public bool stretch { get; }

            public bool uniform { get; }

            public float alpha { get; }

            public int bendDir { get; }

            public float softness { get; private set; }

            public void Apply(SkeletonBone targetBone, ref SkeletonBone childBone, SkeletonBone childBoneParent)
            {
                Apply(ref childBone, targetBone.worldX, targetBone.worldY, compress, stretch, uniform, alpha, childBoneParent);
            }

            public void Apply(ref SkeletonBone bone, float targetX, float targetY, bool compress, bool stretch, bool uniform,
                                float alpha, SkeletonBone parent)
            {
                if (!bone.appliedValid) bone.UpdateAppliedTransform(parent);

                float pa = parent.a, pb = parent.b, pc = parent.c, pd = parent.d;
                float rotationIK = -bone.ashearX - bone.arotation;
                float tx = 0, ty = 0;

                switch (bone.transformMode)
                {
                    case TransformMode.OnlyTranslation:
                        tx = targetX - bone.worldX;
                        ty = targetY - bone.worldY;
                        break;
                    case TransformMode.NoRotationOrReflection:
                        {
                            rotationIK += (float)Math.Atan2(pc, pa) * MathUtils.RadDeg;
                            float ps = Math.Abs(pa * pd - pb * pc) / (pa * pa + pc * pc);
                            pb = -pc * ps;
                            pd = pa * ps;
                            float x = targetX - parent.worldX, y = targetY - parent.worldY;
                            float d = pa * pd - pb * pc;
                            tx = (x * pd - y * pb) / d - bone.ax;
                            ty = (y * pa - x * pc) / d - bone.ay;
                            break;
                        }
                    default:
                        {
                            float x = targetX - parent.worldX, y = targetY - parent.worldY;
                            float d = pa * pd - pb * pc;
                            tx = (x * pd - y * pb) / d - bone.ax;
                            ty = (y * pa - x * pc) / d - bone.ay;
                            break;
                        }
                }

                rotationIK += (float)Math.Atan2(ty, tx) * MathUtils.RadDeg;
                if (bone.ascaleX < 0) rotationIK += 180;
                if (rotationIK > 180)
                    rotationIK -= 360;
                else if (rotationIK < -180) //
                    rotationIK += 360;

                float sx = bone.ascaleX, sy = bone.ascaleY;
                if (compress || stretch)
                {
                    switch (bone.transformMode)
                    {
                        case TransformMode.NoScale:
                            tx = targetX - bone.worldX;
                            ty = targetY - bone.worldY;
                            break;
                        case TransformMode.NoScaleOrReflection:
                            tx = targetX - bone.worldX;
                            ty = targetY - bone.worldY;
                            break;
                    }
                    float b = bone.dataLength * sx, dd = (float)Math.Sqrt(tx * tx + ty * ty);
                    if ((compress && dd < b) || (stretch && dd > b) && b > 0.0001f)
                    {
                        float s = (dd / b - 1) * alpha + 1;
                        sx *= s;
                        if (uniform) sy *= s;
                    }
                }
                bone.UpdateWorldTransform(bone.ax, bone.ay, bone.arotation + rotationIK * alpha, sx, sy, bone.ashearX, bone.ashearY, parent);
            }

            public void Apply(SkeletonBone targetBone, ref SkeletonBone parentBone, SkeletonBone parentBoneParent, ref SkeletonBone childBone, SkeletonBone childBoneParent)
            {
                if (alpha == 0)
                {
                    childBone.UpdateWorldTransform(childBoneParent);
                    return;
                }
                if (!parentBone.appliedValid) parentBone.UpdateAppliedTransform(parentBoneParent);
                if (!childBone.appliedValid) childBone.UpdateAppliedTransform(childBoneParent);
                float px = parentBone.ax, py = parentBone.ay, psx = parentBone.ascaleX, sx = psx, psy = parentBone.ascaleY, csx = childBone.ascaleX;
                int os1, os2, s2;
                if (psx < 0)
                {
                    psx = -psx;
                    os1 = 180;
                    s2 = -1;
                }
                else
                {
                    os1 = 0;
                    s2 = 1;
                }
                if (psy < 0)
                {
                    psy = -psy;
                    s2 = -s2;
                }
                if (csx < 0)
                {
                    csx = -csx;
                    os2 = 180;
                }
                else
                    os2 = 0;
                float cx = childBone.ax, cy, cwx, cwy, a = parentBone.a, b = parentBone.b, c = parentBone.c, d = parentBone.d;
                bool u = Math.Abs(psx - psy) <= 0.0001f;
                if (!u)
                {
                    cy = 0;
                    cwx = a * cx + parentBone.worldX;
                    cwy = c * cx + parentBone.worldY;
                }
                else
                {
                    cy = childBone.ay;
                    cwx = a * cx + b * cy + parentBone.worldX;
                    cwy = c * cx + d * cy + parentBone.worldY;
                }
                a = parentBoneParent.a;
                b = parentBoneParent.b;
                c = parentBoneParent.c;
                d = parentBoneParent.d;
                float id = 1 / (a * d - b * c), x = cwx - parentBoneParent.worldX, y = cwy - parentBoneParent.worldY;
                float dx = (x * d - y * b) * id - px, dy = (y * a - x * c) * id - py;
                float l1 = (float)Math.Sqrt(dx * dx + dy * dy), l2 = childBone.dataLength * csx, a1, a2;
                if (l1 < 0.0001f)
                {

                    Apply(ref parentBone, targetBone.worldX, targetBone.worldY, false, stretch, false, alpha, parentBoneParent);
                    childBone.UpdateWorldTransform(cx, cy, 0, childBone.ascaleX, childBone.ascaleY, childBone.ashearX, childBone.ashearY, childBoneParent);
                    return;
                }
                x = targetBone.worldX - parentBoneParent.worldX;
                y = targetBone.worldY - parentBoneParent.worldY;
                float tx = (x * d - y * b) * id - px, ty = (y * a - x * c) * id - py;
                float dd = tx * tx + ty * ty;
                if (softness != 0)
                {
                    softness *= psx * (csx + 1) / 2;
                    float td = (float)Math.Sqrt(dd), sd = td - l1 - l2 * psx + softness;
                    if (sd > 0)
                    {
                        float p = Math.Min(1, sd / (softness * 2)) - 1;
                        p = (sd - softness * (1 - p * p)) / td;
                        tx -= p * tx;
                        ty -= p * ty;
                        dd = tx * tx + ty * ty;
                    }
                }
                if (u)
                {
                    l2 *= psx;
                    float cos = (dd - l1 * l1 - l2 * l2) / (2 * l1 * l2);
                    if (cos < -1)
                        cos = -1;
                    else if (cos > 1)
                    {
                        cos = 1;
                        if (stretch) sx *= ((float)Math.Sqrt(dd) / (l1 + l2) - 1) * alpha + 1;
                    }
                    a2 = (float)Math.Acos(cos) * bendDir;
                    a = l1 + l2 * cos;
                    b = l2 * (float)Math.Sin(a2);
                    a1 = (float)Math.Atan2(ty * a - tx * b, tx * a + ty * b);
                }
                else
                {
                    a = psx * l2;
                    b = psy * l2;
                    float aa = a * a, bb = b * b, ta = (float)Math.Atan2(ty, tx);
                    c = bb * l1 * l1 + aa * dd - aa * bb;
                    float c1 = -2 * bb * l1, c2 = bb - aa;
                    d = c1 * c1 - 4 * c2 * c;
                    if (d >= 0)
                    {
                        float q = (float)Math.Sqrt(d);
                        if (c1 < 0) q = -q;
                        q = -(c1 + q) / 2;
                        float r0 = q / c2, r1 = c / q;
                        float r = Math.Abs(r0) < Math.Abs(r1) ? r0 : r1;
                        if (r * r <= dd)
                        {
                            y = (float)Math.Sqrt(dd - r * r) * bendDir;
                            a1 = ta - (float)Math.Atan2(y, r);
                            a2 = (float)Math.Atan2(y / psy, (r - l1) / psx);
                            goto break_outer; // break outer;
                        }
                    }
                    float minAngle = MathUtils.PI, minX = l1 - a, minDist = minX * minX, minY = 0;
                    float maxAngle = 0, maxX = l1 + a, maxDist = maxX * maxX, maxY = 0;
                    c = -a * l1 / (aa - bb);
                    if (c >= -1 && c <= 1)
                    {
                        c = (float)Math.Acos(c);
                        x = a * (float)Math.Cos(c) + l1;
                        y = b * (float)Math.Sin(c);
                        d = x * x + y * y;
                        if (d < minDist)
                        {
                            minAngle = c;
                            minDist = d;
                            minX = x;
                            minY = y;
                        }
                        if (d > maxDist)
                        {
                            maxAngle = c;
                            maxDist = d;
                            maxX = x;
                            maxY = y;
                        }
                    }
                    if (dd <= (minDist + maxDist) / 2)
                    {
                        a1 = ta - (float)Math.Atan2(minY * bendDir, minX);
                        a2 = minAngle * bendDir;
                    }
                    else
                    {
                        a1 = ta - (float)Math.Atan2(maxY * bendDir, maxX);
                        a2 = maxAngle * bendDir;
                    }
                }
            break_outer:
                float os = (float)Math.Atan2(cy, cx) * s2;
                float rotation = parentBone.arotation;
                a1 = (a1 - os) * MathUtils.RadDeg + os1 - rotation;
                if (a1 > 180)
                    a1 -= 360;
                else if (a1 < -180) a1 += 360;
                parentBone.UpdateWorldTransform(px, py, rotation + a1 * alpha, sx, parentBone.ascaleY, 0, 0, parentBoneParent);
                rotation = childBone.arotation;
                a2 = ((a2 + os) * MathUtils.RadDeg - childBone.ashearX) * s2 + os2 - rotation;
                if (a2 > 180)
                    a2 -= 360;
                else if (a2 < -180) a2 += 360;
                childBone.UpdateWorldTransform(cx, cy, rotation + a2 * alpha, childBone.ascaleX, childBone.ascaleY, childBone.ashearX, childBone.ashearY, childBoneParent);
            }
        }
        #endregion

}