using Spine;

namespace DChild
{
    public struct SkeletonBoneIndex
    {
        public int hashcode { get; }
        public int index;

        public SkeletonBoneIndex(Bone bone)
        {
            hashcode = bone?.GetHashCode() ?? 0;
            index = -1;
        }
    }
}