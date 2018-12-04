using System.Collections.Generic;

namespace ReposeRecord
{
    public class Guard
    {
        public Guard()
        {
            MinutesAsleep = new List<int>();
        }

        public string Id { get; set; }
        public int TotalTimeAsleep { get; set; }
        public List<int> MinutesAsleep { get; set; }
    }
}
