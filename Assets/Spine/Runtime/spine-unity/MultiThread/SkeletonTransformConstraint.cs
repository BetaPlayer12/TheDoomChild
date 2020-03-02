using Spine;
using System;

namespace DChild
{
    public struct SkeletonTransformConstraint
    {
        public struct WorldData
        {
            public float rotateMix, translateMix, scaleMix, shearMix;
            public float ta, tb, tc, td;
            public float degRadReflect;
            public float offsetRotation, offsetShearY;

            public WorldData(SkeletonBone target, SkeletonTransformConstraint constraint)
            {
                rotateMix = constraint.rotateMix;
                translateMix = constraint.translateMix;
                scaleMix = constraint.scaleMix;
                shearMix = constraint.shearMix;
                ta = target.a;
                tb = target.b;
                tc = target.c;
                td = target.d;
                degRadReflect = ta * td - tb * tc > 0 ? MathUtils.DegRad : -MathUtils.DegRad;
                offsetRotation = constraint.dataOffsetRotation * degRadReflect;
                offsetShearY = constraint.dataOffsetShearY * degRadReflect;
            }
        }

        public struct LocalData
        {
            public float rotateMix, translateMix, scaleMix, shearMix;

            public LocalData(SkeletonTransformConstraint constraint)
            {
                rotateMix = constraint.rotateMix;
                translateMix = constraint.translateMix;
                scaleMix = constraint.scaleMix;
                shearMix = constraint.shearMix;
            }
        }

        public int minBoneIndex { get; }
        public int maxBoneIndex { get; }

        public bool dataLocal, dataRelative;
        public float rotateMix, translateMix, scaleMix, shearMix;
        public SkeletonBoneIndex target;
        public float dataOffsetRotation, dataOffsetShearY, dataOffsetX, dataOffsetY, dataOffsetScaleX, dataOffsetScaleY;

        public SkeletonTransformConstraint(TransformConstraint constraint, int minBoneIndex, int maxBoneIndex)
        {
            this.minBoneIndex = minBoneIndex;
            this.maxBoneIndex = maxBoneIndex;
            if (constraint == null)
            {
                dataLocal = false;
                dataRelative = false;
                rotateMix = 0;
                translateMix = 0;
                scaleMix = 0;
                shearMix = 0;

                target = new SkeletonBoneIndex(null);

                dataOffsetRotation = 0;
                dataOffsetShearY = 0;
                dataOffsetX = 0;
                dataOffsetY = 0;
                dataOffsetScaleX = 0;
                dataOffsetScaleY = 0;
            }
            else
            {
                var data = constraint.data;
                dataLocal = data.local;
                dataRelative = data.relative;
                rotateMix = constraint.rotateMix;
                translateMix = constraint.translateMix;
                scaleMix = constraint.scaleMix;
                shearMix = constraint.shearMix;

                target = new SkeletonBoneIndex(constraint.target);

                dataOffsetRotation = data.offsetRotation;
                dataOffsetShearY = data.offsetShearY;
                dataOffsetX = data.offsetX;
                dataOffsetY = data.offsetY;
                dataOffsetScaleX = data.offsetScaleX;
                dataOffsetScaleY = data.offsetScaleY;
            }
        }

        public WorldData CreateWorldData(SkeletonBone target) => new WorldData(target, this);
        public LocalData CreateLocalData() => new LocalData(this);

        public void ApplyAbsoluteWorld(WorldData data, SkeletonBone target, ref SkeletonBone bone)
        {
            bool modified = false;

            if (rotateMix != 0)
            {
                float a = bone.a, b = bone.b, c = bone.c, d = bone.d;
                float r = MathUtils.Atan2(data.tc, data.ta) - MathUtils.Atan2(c, a) + data.offsetRotation;
                if (r > MathUtils.PI)
                    r -= MathUtils.PI2;
                else if (r < -MathUtils.PI) r += MathUtils.PI2;
                r *= rotateMix;
                float cos = MathUtils.Cos(r), sin = MathUtils.Sin(r);
                bone.a = cos * a - sin * c;
                bone.b = cos * b - sin * d;
                bone.c = sin * a + cos * c;
                bone.d = sin * b + cos * d;
                modified = true;
            }

            if (translateMix != 0)
            {
                float tx, ty; //Vector2 temp = this.temp;
                target.LocalToWorld(dataOffsetX, dataOffsetY, out tx, out ty); //target.localToWorld(temp.set(data.offsetX, data.offsetY));
                bone.worldX += (tx - bone.worldX) * translateMix;
                bone.worldY += (ty - bone.worldY) * translateMix;
                modified = true;
            }

            if (scaleMix > 0)
            {
                float s = (float)Math.Sqrt(bone.a * bone.a + bone.c * bone.c);
                if (s != 0) s = (s + ((float)Math.Sqrt(data.ta * data.ta + data.tc * data.tc) - s + dataOffsetScaleX) * scaleMix) / s;
                bone.a *= s;
                bone.c *= s;
                s = (float)Math.Sqrt(bone.b * bone.b + bone.d * bone.d);
                if (s != 0) s = (s + ((float)Math.Sqrt(data.tb * data.tb + data.td * data.td) - s + dataOffsetScaleY) * scaleMix) / s;
                bone.b *= s;
                bone.d *= s;
                modified = true;
            }

            if (shearMix > 0)
            {
                float b = bone.b, d = bone.d;
                float by = MathUtils.Atan2(d, b);
                float r = MathUtils.Atan2(data.td, data.tb) - MathUtils.Atan2(data.tc, data.ta) - (by - MathUtils.Atan2(bone.c, bone.a));
                if (r > MathUtils.PI)
                    r -= MathUtils.PI2;
                else if (r < -MathUtils.PI) r += MathUtils.PI2;
                r = by + (r + data.offsetShearY) * shearMix;
                float s = (float)Math.Sqrt(b * b + d * d);
                bone.b = MathUtils.Cos(r) * s;
                bone.d = MathUtils.Sin(r) * s;
                modified = true;
            }

            if (modified) bone.appliedValid = false;
        }

        public void ApplyRelativeWorld(WorldData data, SkeletonBone target, ref SkeletonBone bone)
        {
            bool modified = false;

            if (rotateMix != 0)
            {
                float a = bone.a, b = bone.b, c = bone.c, d = bone.d;
                float r = MathUtils.Atan2(data.tc, data.ta) + data.offsetRotation;
                if (r > MathUtils.PI)
                    r -= MathUtils.PI2;
                else if (r < -MathUtils.PI) r += MathUtils.PI2;
                r *= rotateMix;
                float cos = MathUtils.Cos(r), sin = MathUtils.Sin(r);
                bone.a = cos * a - sin * c;
                bone.b = cos * b - sin * d;
                bone.c = sin * a + cos * c;
                bone.d = sin * b + cos * d;
                modified = true;
            }

            if (translateMix != 0)
            {
                float tx, ty; //Vector2 temp = this.temp;
                target.LocalToWorld(dataOffsetX, dataOffsetY, out tx, out ty); //target.localToWorld(temp.set(data.offsetX, data.offsetY));
                bone.worldX += tx * translateMix;
                bone.worldY += ty * translateMix;
                modified = true;
            }

            if (scaleMix > 0)
            {
                float s = ((float)Math.Sqrt(data.ta * data.ta + data.tc * data.tc) - 1 + dataOffsetScaleX) * scaleMix + 1;
                bone.a *= s;
                bone.c *= s;
                s = ((float)Math.Sqrt(data.tb * data.tb + data.td * data.td) - 1 + dataOffsetScaleY) * scaleMix + 1;
                bone.b *= s;
                bone.d *= s;
                modified = true;
            }

            if (shearMix > 0)
            {
                float r = MathUtils.Atan2(data.td, data.tb) - MathUtils.Atan2(data.tc, data.ta);
                if (r > MathUtils.PI)
                    r -= MathUtils.PI2;
                else if (r < -MathUtils.PI) r += MathUtils.PI2;
                float b = bone.b, d = bone.d;
                r = MathUtils.Atan2(d, b) + (r - MathUtils.PI / 2 + data.offsetShearY) * shearMix;
                float s = (float)Math.Sqrt(b * b + d * d);
                bone.b = MathUtils.Cos(r) * s;
                bone.d = MathUtils.Sin(r) * s;
                modified = true;
            }

            if (modified) bone.appliedValid = false;
        }

        public void ApplyAbsoluteLocal(LocalData data, SkeletonBone target, ref SkeletonBone bone, SkeletonBone boneParent)
        {
            if (!bone.appliedValid) bone.UpdateAppliedTransform(boneParent);

            float rotation = bone.arotation;
            if (rotateMix != 0)
            {
                float r = target.arotation - rotation + dataOffsetRotation;
                r -= (16384 - (int)(16384.499999999996 - r / 360)) * 360;
                rotation += r * rotateMix;
            }

            float x = bone.ax, y = bone.ay;
            if (translateMix != 0)
            {
                x += (target.ax - x + dataOffsetX) * translateMix;
                y += (target.ay - y + dataOffsetY) * translateMix;
            }

            float scaleX = bone.ascaleX, scaleY = bone.ascaleY;
            if (scaleMix != 0)
            {
                if (scaleX != 0) scaleX = (scaleX + (target.ascaleX - scaleX + dataOffsetScaleX) * scaleMix) / scaleX;
                if (scaleY != 0) scaleY = (scaleY + (target.ascaleY - scaleY + dataOffsetScaleY) * scaleMix) / scaleY;
            }

            float shearY = bone.ashearY;
            if (shearMix != 0)
            {
                float r = target.ashearY - shearY + dataOffsetShearY;
                r -= (16384 - (int)(16384.499999999996 - r / 360)) * 360;
                shearY += r * shearMix;
            }

            bone.UpdateWorldTransform(x, y, rotation, scaleX, scaleY, bone.ashearX, shearY, boneParent);
        }

        public void ApplyRelativeLocal(LocalData data, SkeletonBone target, ref SkeletonBone bone, SkeletonBone boneParent)
        {
            if (!bone.appliedValid) bone.UpdateAppliedTransform(boneParent);

            float rotation = bone.arotation;
            if (rotateMix != 0) rotation += (target.arotation + dataOffsetRotation) * rotateMix;

            float x = bone.ax, y = bone.ay;
            if (translateMix != 0)
            {
                x += (target.ax + dataOffsetX) * translateMix;
                y += (target.ay + dataOffsetY) * translateMix;
            }

            float scaleX = bone.ascaleX, scaleY = bone.ascaleY;
            if (scaleMix != 0)
            {
                scaleX *= ((target.ascaleX - 1 + dataOffsetScaleX) * scaleMix) + 1;
                scaleY *= ((target.ascaleY - 1 + dataOffsetScaleY) * scaleMix) + 1;
            }

            float shearY = bone.ashearY;
            if (shearMix != 0) shearY += (target.ashearY + dataOffsetShearY) * shearMix;

            bone.UpdateWorldTransform(x, y, rotation, scaleX, scaleY, bone.ashearX, shearY, boneParent);
        }
    }
}