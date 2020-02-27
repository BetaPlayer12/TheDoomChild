namespace DChild
{
    #region Jobs

    public struct SkeletonUpdateInstruction
        {

            public enum Type
            {
                Bone,
                IKConstraint,
                TransformConstraint
            }
            public SkeletonUpdateInstruction(Type type, int index) : this()
            {
                this.type = type;
                this.index = index;
            }

            public Type type { get; }
            public int index { get; }
        }
        #endregion

}