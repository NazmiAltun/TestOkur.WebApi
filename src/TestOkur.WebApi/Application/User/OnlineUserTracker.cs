//namespace TestOkur.WebApi.Application.User
//{
//    using System;
//    using System.Collections.Concurrent;
//    using System.Collections.Generic;
//    using System.Linq;

//    public static class OnlineUserTracker
//    {
//        private static readonly ConcurrentDictionary<string, DateTime> OnlineUserDictionary = new ConcurrentDictionary<string, DateTime>();

//        public static IEnumerable<string> GetOnlineUsers() => OnlineUserDictionary.Keys.Distinct().ToList();

//        public static void Add(string userEmail)
//        {
//            OnlineUserDictionary.AddOrUpdate(userEmail, DateTime.UtcNow, (key, existingValue) => DateTime.UtcNow);
//        }
//    }
//}
