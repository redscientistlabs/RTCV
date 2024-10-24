namespace RTCV.NetCore.NetCoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using Ceras;

    public static class ObjectCopier
    {
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            //Return default of a null object
            if (source == null)
            {
                return default(T);
            }

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    //Thanks to Riki, dev of Ceras for writing this
    public class HashSetFormatterThatKeepsItsComparer : Ceras.Formatters.IFormatter<HashSet<byte[]>>
    {
        // Sub-formatters are automatically set by Ceras' dependency injection
        public Ceras.Formatters.IFormatter<byte[]> ByteArrayFormatter { get; private set; }
        public Ceras.Formatters.IFormatter<IEqualityComparer<byte[]>> ComparerFormatter { get; private set; } // auto-implemented by Ceras using DynamicObjectFormatter

        public void Serialize(ref byte[] buffer, ref int offset, HashSet<byte[]> set)
        {
            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            // What do we need?
            // - The comparer
            // - Number of entries
            // - Actual content

            // Comparer
            ComparerFormatter.Serialize(ref buffer, ref offset, set.Comparer);

            // Count
            // We could use a 'IFormatter<int>' field, but Ceras will resolve it to this method anyway...
            SerializerBinary.WriteInt32(ref buffer, ref offset, set.Count);

            // Actual content
            foreach (var array in set)
            {
                ByteArrayFormatter.Serialize(ref buffer, ref offset, array);
            }
        }

        public void Deserialize(byte[] buffer, ref int offset, ref HashSet<byte[]> set)
        {
            IEqualityComparer<byte[]> equalityComparer = null;
            ComparerFormatter.Deserialize(buffer, ref offset, ref equalityComparer);

            // We can already create the hashset
            set = new HashSet<byte[]>(equalityComparer);

            // Read content...
            var count = SerializerBinary.ReadInt32(buffer, ref offset);
            for (var i = 0; i < count; i++)
            {
                byte[] ar = null;
                ByteArrayFormatter.Deserialize(buffer, ref offset, ref ar);

                set.Add(ar);
            }
        }
    }

    public class NullableByteHashSetFormatterThatKeepsItsComparer : Ceras.Formatters.IFormatter<HashSet<byte?[]>>
    {
        // Sub-formatters are automatically set by Ceras' dependency injection
        public Ceras.Formatters.IFormatter<byte?[]> ByteArrayFormatter { get; }
        public Ceras.Formatters.IFormatter<IEqualityComparer<byte?[]>> ComparerFormatter { get; } // auto-implemented by Ceras using DynamicObjectFormatter

        public void Serialize(ref byte[] buffer, ref int offset, HashSet<byte?[]> set)
        {
            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            // What do we need?
            // - The comparer
            // - Number of entries
            // - Actual content

            // Comparer
            ComparerFormatter.Serialize(ref buffer, ref offset, set.Comparer);

            // Count
            // We could use a 'IFormatter<int>' field, but Ceras will resolve it to this method anyway...
            SerializerBinary.WriteInt32(ref buffer, ref offset, set.Count);

            // Actual content
            foreach (var array in set)
            {
                ByteArrayFormatter.Serialize(ref buffer, ref offset, array);
            }
        }

        public void Deserialize(byte[] buffer, ref int offset, ref HashSet<byte?[]> set)
        {
            IEqualityComparer<byte?[]> equalityComparer = null;
            ComparerFormatter.Deserialize(buffer, ref offset, ref equalityComparer);

            // We can already create the hashset
            set = new HashSet<byte?[]>(equalityComparer);

            // Read content...
            var count = SerializerBinary.ReadInt32(buffer, ref offset);
            for (var i = 0; i < count; i++)
            {
                byte?[] ar = null;
                ByteArrayFormatter.Deserialize(buffer, ref offset, ref ar);

                set.Add(ar);
            }
        }
    }

    public enum DPIAwarenessContext
    {
        Default = 0,
        Unaware = -1,
        SystemAware = -2,
        PerMonitorAware = -3,
        PerMonitorAwareV2 = -4,
        UnawareGDIScaled = -5
    }

    //https://stackoverflow.com/a/47744757/10923568
    public static class StackFrameHelper
    {
        private delegate object DGetStackFrameHelper();

        private static DGetStackFrameHelper _getStackFrameHelper = null;

        private static FieldInfo _frameCount = null;
        private static volatile bool initialized = false;

        public static void OneTimeSetup()
        {
            if (initialized)
            {
                return;
            }

            try
            {
                Type stackFrameHelperType =
                                 typeof(object).Assembly.GetType("System.Diagnostics.StackFrameHelper");

                // ReSharper disable once PossibleNullReferenceException
                MethodInfo getStackFramesInternal =
                   Type.GetType("System.Diagnostics.StackTrace, mscorlib").GetMethod(
                                "GetStackFramesInternal", BindingFlags.Static | BindingFlags.NonPublic);
                if (getStackFramesInternal == null)
                {
                    return;  // Unknown mscorlib implementation
                }

                DynamicMethod dynamicMethod = new DynamicMethod(
                          "GetStackFrameHelper", typeof(object), new Type[0], typeof(StackTrace), true);

                ILGenerator generator = dynamicMethod.GetILGenerator();
                generator.DeclareLocal(stackFrameHelperType);

                bool newDotNet = false;

                ConstructorInfo constructorInfo =
                         stackFrameHelperType.GetConstructor(new Type[] { typeof(bool), typeof(Thread) });
                if (constructorInfo != null)
                {
                    generator.Emit(OpCodes.Ldc_I4_0);
                }
                else
                {
                    constructorInfo = stackFrameHelperType.GetConstructor(new Type[] { typeof(Thread) });
                    if (constructorInfo == null)
                    {
                        return; // Unknown mscorlib implementation
                    }

                    newDotNet = true;
                }

                generator.Emit(OpCodes.Ldnull);
                generator.Emit(OpCodes.Newobj, constructorInfo);
                generator.Emit(OpCodes.Stloc_0);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Ldc_I4_0);

                if (newDotNet)
                {
                    generator.Emit(OpCodes.Ldc_I4_0);  // Extra parameter
                }

                generator.Emit(OpCodes.Ldnull);
                generator.Emit(OpCodes.Call, getStackFramesInternal);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Ret);

                _getStackFrameHelper =
                      (DGetStackFrameHelper)dynamicMethod.CreateDelegate(typeof(DGetStackFrameHelper));

                _frameCount = stackFrameHelperType.GetField("iFrameCount",
                                                        BindingFlags.NonPublic | BindingFlags.Instance);
                initialized = true;
            }
            catch
            { }  // _frameCount remains null, indicating unknown mscorlib implementation
        }

        public static int GetCallStackDepth()
        {
            if (!initialized)
            {
                OneTimeSetup();
            }

            if (_frameCount == null)
            {
                return 0;  // Unknown mscorlib implementation
            }

            return (int)_frameCount.GetValue(_getStackFrameHelper());
        }
    }
}
