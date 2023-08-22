namespace Static
{
    public static class UniqueIDGenerator
    {
        private static int _currentID = 0;

        public static int GetUniqueID()
        {
            _currentID++;
            return _currentID;
        }
    }
}
