using Ceras;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RTCV.NetCore
{
	public class Extensions
	{   /// <summary>
		/// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
		/// Provides a method for performing a deep copy of an object.
		/// Binary Serialization is used to perform the copy.
		/// </summary>
		public static class ObjectCopier
		{
			/// <summary>
			/// Perform a deep Copy of the object.
			/// </summary>
			/// <typeparam name="T">The type of object being copied.</typeparam>
			/// <param name="source">The object instance to copy.</param>
			/// <returns>The copied object.</returns>
			public static T Clone<T>(T source)
			{
				if (!typeof(T).IsSerializable)
				{
					throw new ArgumentException("The type must be serializable.", "source");
				}

				// Don't serialize a null object, simply return the default for that object
				if (Object.ReferenceEquals(source, null))
				{
					return default(T);
				}

				IFormatter formatter = new BinaryFormatter();
				Stream stream = new MemoryStream();
				using (stream)
				{
					formatter.Serialize(stream, source);
					stream.Seek(0, SeekOrigin.Begin);
					return (T)formatter.Deserialize(stream);
				}
			}
		}

		public static class ConsoleHelper
		{
			public static ConsoleCopy con;
			public static void CreateConsole(string path)
			{
				ReleaseConsole();
				AllocConsole();
				con = new ConsoleCopy(path);

				//Disable the X button on the console window
				EnableMenuItem(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_DISABLED);
			}

			private static bool ConsoleVisible = true;
			public static void ShowConsole()
			{
				var handle = GetConsoleWindow();
				ShowWindow(handle, SW_SHOW);
				ConsoleVisible = true;
			}

			public static void HideConsole()
			{
				var handle = GetConsoleWindow();
				ShowWindow(handle, SW_HIDE);
				ConsoleVisible = false;
			}

			public static void ToggleConsole()
			{
				if (ConsoleVisible)
					HideConsole();
				else
					ShowConsole();
			}
			public static void ReleaseConsole()
			{
				var handle = GetConsoleWindow();
				CloseHandle(handle);
			}
			// P/Invoke required:
			internal const int SW_HIDE = 0;
			internal const int SW_SHOW = 5;

			internal const int SC_CLOSE = 0xF060;           //close button's code in Windows API
			internal const int MF_ENABLED = 0x00000000;     //enabled button status
			internal const int MF_GRAYED = 0x1;             //disabled button status (enabled = false)
			internal const int MF_DISABLED = 0x00000002;    //disabled button status

			private const UInt32 StdOutputHandle = 0xFFFFFFF5;
			[DllImport("kernel32.dll")]
			private static extern IntPtr GetStdHandle(UInt32 nStdHandle);
			[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
			public static extern bool CloseHandle(IntPtr handle);
			[DllImport("kernel32.dll")]
			private static extern void SetStdHandle(UInt32 nStdHandle, IntPtr handle);
			[DllImport("kernel32")]
			static extern bool AllocConsole();
			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern IntPtr GetConsoleWindow();
			[DllImport("user32.dll")]
			public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
			[DllImport("user32.dll")]
			public static extern IntPtr GetSystemMenu(IntPtr HWNDValue, bool isRevert);
			[DllImport("user32.dll")]
			public static extern int EnableMenuItem(IntPtr tMenu, int targetItem, int targetStatus);
			public class ConsoleCopy : IDisposable
			{
				FileStream fileStream;
				public StreamWriter FileWriter;
				TextWriter doubleWriter;
				TextWriter oldOut;

				class DoubleWriter : TextWriter
				{

					TextWriter one;
					TextWriter two;

					public DoubleWriter(TextWriter one, TextWriter two)
					{
						this.one = one;
						this.two = two;
					}

					public override Encoding Encoding
					{
						get { return one.Encoding; }
					}

					public override void Flush()
					{
						one.Flush();
						two.Flush();
					}

					public override void Write(char value)
					{
						one.Write(value);
						two.Write(value);
					}

				}

				public ConsoleCopy(string path)
				{
					oldOut = Console.Out;

					try
					{
						File.Create(path).Close();
						fileStream = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.Read);
						FileWriter = new StreamWriter(fileStream);
						FileWriter.AutoFlush = true;

						doubleWriter = new DoubleWriter(FileWriter, oldOut);
					}
					catch (Exception e)
					{
						Console.WriteLine("Cannot open file for writing");
						Console.WriteLine(e.Message);
						return;
					}
					Console.SetOut(doubleWriter);
					Console.SetError(doubleWriter);
				}

				public void Dispose()
				{
					Console.SetOut(oldOut);
					if (FileWriter != null)
					{
						FileWriter.Flush();
						FileWriter.Close();
						FileWriter = null;
					}
					if (fileStream != null)
					{
						fileStream.Close();
						fileStream = null;
					}
				}
			}
		}


		//Thanks to Riki, dev of Ceras for writing this
		public class HashSetFormatterThatKeepsItsComparer : Ceras.Formatters.IFormatter<HashSet<byte[]>>
		{
			// Sub-formatters are automatically set by Ceras' dependency injection
			public Ceras.Formatters.IFormatter<byte[]> _byteArrayFormatter;
			public Ceras.Formatters.IFormatter<IEqualityComparer<byte[]>> _comparerFormatter; // auto-implemented by Ceras using DynamicObjectFormatter

			public void Serialize(ref byte[] buffer, ref int offset, HashSet<byte[]> set)
			{
				// What do we need?
				// - The comparer
				// - Number of entries
				// - Actual content

				// Comparer
				_comparerFormatter.Serialize(ref buffer, ref offset, set.Comparer);

				// Count
				// We could use a 'IFormatter<int>' field, but Ceras will resolve it to this method anyway...
				SerializerBinary.WriteInt32(ref buffer, ref offset, set.Count);

				// Actual content
				foreach (var array in set)
					_byteArrayFormatter.Serialize(ref buffer, ref offset, array);
			}

			public void Deserialize(byte[] buffer, ref int offset, ref HashSet<byte[]> set)
			{
				IEqualityComparer<byte[]> equalityComparer = null;
				_comparerFormatter.Deserialize(buffer, ref offset, ref equalityComparer);

				// We can already create the hashset
				set = new HashSet<byte[]>(equalityComparer);

				// Read content...
				int count = SerializerBinary.ReadInt32(buffer, ref offset);
				for (int i = 0; i < count; i++)
				{
					byte[] ar = null;
					_byteArrayFormatter.Deserialize(buffer, ref offset, ref ar);

					set.Add(ar);
				}
			}
		}
		

		public static bool IsGDIEnhancedScalingAvailable()
		{
			return (Environment.OSVersion.Version.Major == 10 &
					Environment.OSVersion.Version.Build >= 17763);
		}

		public enum DPI_AWARENESS
		{
			DPI_AWARENESS_INVALID = -1,
			DPI_AWARENESS_UNAWARE = 0,
			DPI_AWARENESS_SYSTEM_AWARE = 1,
			DPI_AWARENESS_PER_MONITOR_AWARE = 2
		}

		public enum DPI_AWARENESS_CONTEXT
		{
			DPI_AWARENESS_CONTEXT_DEFAULT = 0, 
			DPI_AWARENESS_CONTEXT_UNAWARE = -1, 
			DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = -2,
			DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = -3,
			DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4,
			DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED = -5
		}

		[DllImport("User32.dll")]
		public static extern DPI_AWARENESS_CONTEXT GetThreadDpiAwarenessContext();

		[DllImport("User32.dll")]
		public static extern DPI_AWARENESS_CONTEXT GetWindowDpiAwarenessContext(
			IntPtr hwnd);

		[DllImport("User32.dll")]
		public static extern DPI_AWARENESS_CONTEXT SetThreadDpiAwarenessContext(
			DPI_AWARENESS_CONTEXT dpiContext);

	}

	public class SafeJsonTypeSerialization
	{
		public class JsonKnownTypesBinder : ISerializationBinder
		{
			public IList<Type> KnownTypes { get; set; }

			public JsonKnownTypesBinder()
			{
				KnownTypes = new List<Type>();
			}

			public Type BindToType(string assemblyName, string typeName)
			{
				return KnownTypes.SingleOrDefault(t => t.Name == typeName);
			}

			public void BindToName(Type serializedType, out string assemblyName, out string typeName)
			{
				assemblyName = null;
				typeName = serializedType.Name;
			}
		}

		//https://stackoverflow.com/a/38340375
		public class UntypedToTypedValueContractResolver : DefaultContractResolver
		{
			// As of 7.0.1, Json.NET suggests using a static instance for "stateless" contract resolvers, for performance reasons.
			// http://www.newtonsoft.com/json/help/html/ContractResolver.htm
			// http://www.newtonsoft.com/json/help/html/M_Newtonsoft_Json_Serialization_DefaultContractResolver__ctor_1.htm
			// "Use the parameterless constructor and cache instances of the contract resolver within your application for optimal performance."
			// See also https://stackoverflow.com/questions/33557737/does-json-net-cache-types-serialization-information
			static UntypedToTypedValueContractResolver instance;

			// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
			static UntypedToTypedValueContractResolver() { instance = new UntypedToTypedValueContractResolver(); }

			public static UntypedToTypedValueContractResolver Instance { get { return instance; } }

			protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
			{
				var contract = base.CreateDictionaryContract(objectType);

				if (contract.DictionaryValueType == typeof(object) && contract.ItemConverter == null)
				{
					contract.ItemConverter = new UntypedToTypedValueConverter();
				}

				return contract;
			}
		}

		public class UntypedToTypedValueConverter : JsonConverter
		{
			public override bool CanConvert(Type objectType)
			{
				throw new NotImplementedException("This converter should only be applied directly via ItemConverterType, not added to JsonSerializer.Converters");
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				if (reader.TokenType == JsonToken.Null)
					return null;
				var value = serializer.Deserialize(reader, objectType);
				if (value is TypeWrapper)
				{
					return ((TypeWrapper)value).ObjectValue;
				}
				return value;
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				if (serializer.TypeNameHandling == TypeNameHandling.None)
				{
					Debug.WriteLine("ObjectItemConverter used when serializer.TypeNameHandling == TypeNameHandling.None");
					serializer.Serialize(writer, value);
				}
				// Handle a couple of simple primitive cases where a type wrapper is not needed
				else if (value is string)
				{
					writer.WriteValue((string)value);
				}
				else if (value is bool)
				{
					writer.WriteValue((bool)value);
				}
				else
				{
					var contract = serializer.ContractResolver.ResolveContract(value.GetType());
					if (contract is JsonPrimitiveContract)
					{
						var wrapper = TypeWrapper.CreateWrapper(value);
						serializer.Serialize(writer, wrapper, typeof(object));
					}
					else
					{
						serializer.Serialize(writer, value);
					}
				}
			}
		}

		public abstract class TypeWrapper
		{
			protected TypeWrapper() { }

			[JsonIgnore]
			public abstract object ObjectValue { get; }

			public static TypeWrapper CreateWrapper<T>(T value)
			{
				if (value == null)
					return new TypeWrapper<T>();
				var type = value.GetType();
				if (type == typeof(T))
					return new TypeWrapper<T>(value);
				// Return actual type of subclass
				return (TypeWrapper)Activator.CreateInstance(typeof(TypeWrapper<>).MakeGenericType(type), value);
			}
		}

		public sealed class TypeWrapper<T> : TypeWrapper
		{
			public TypeWrapper() : base() { }

			public TypeWrapper(T value)
				: base()
			{
				this.Value = value;
			}

			public override object ObjectValue { get { return Value; } }

			public T Value { get; set; }
		}

	}
}
