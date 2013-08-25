namespace DataMerger
{
    interface IMerge
    {
        void Merge(User incomingUser);
        User FindUser(int uniqueId);
        int UserCount { get; }
    }
}
