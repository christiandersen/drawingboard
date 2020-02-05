using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#pragma warning disable 0693

namespace DrawingBoard2.Utils
{
    /// <summary>
    /// Generic clone utility.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class CloneUtil<T>
    {
        /// <summary>
        /// Creates a deep copy of the object 
        /// source : http://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-an-object-in-net-c-specifically
        /// </summary>
        /// <typeparam name="T">Generic T Type</typeparam>
        /// <param name="obj">Object to be cloned</param>
        /// <returns>Cloned object</returns>
        public static T DeepClone<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
