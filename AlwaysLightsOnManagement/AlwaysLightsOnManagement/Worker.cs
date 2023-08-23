using System;
/**
* author: KATONA TAMAS - katonatomi@msn.com
* 2023.
*/
namespace AlwaysLightsOnManagement
{
    public class Worker
    {
        public int WorkerID { get; set; }
        public string FullName { get; set; }

        /// <summary>
        /// Creates a Worker with Name.
        /// </summary>
        /// <param name="fullName">Full name of Worker</param>
        public Worker(string fullName)
        {
            FullName = fullName;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
