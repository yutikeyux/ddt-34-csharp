using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MetroFramework.Localization
{
	internal class MetroLocalize
	{
		private DataSet dataSet_0;

		internal static MetroLocalize Grv83vh53MyypkjBmRF;

		public string DefaultLanguage()
		{
			return "en";
		}

		public string CurrentLanguage()
		{
			string text = Application.CurrentCulture.TwoLetterISOLanguageName;
			if (((string)(object)text).Length == 0)
			{
				text = DefaultLanguage();
			}
			return ((string)(object)text).ToLower();
		}

		public MetroLocalize(string ctrlName)
		{
			method_0(ctrlName);
		}

		public MetroLocalize(Control ctrl)
		{
			method_0(ctrl.Name);
		}

		private void method_0(string string_0)
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			string text = "";
			Stream stream = null;
			if (entryAssembly != null)
			{
				text = string.Concat((string[])(object)new string[6]
				{
					(string)(object)entryAssembly.GetName().Name,
					".Localization.",
					(string)(object)CurrentLanguage(),
					".",
					(string)(object)string_0,
					".xml"
				});
				stream = entryAssembly.GetManifestResourceStream(text);
			}
			if (stream == null)
			{
				entryAssembly = Assembly.GetCallingAssembly();
				text = string.Concat((string[])(object)new string[6]
				{
					(string)(object)entryAssembly.GetName().Name,
					".Localization.",
					(string)(object)CurrentLanguage(),
					".",
					(string)(object)string_0,
					".xml"
				});
				stream = entryAssembly.GetManifestResourceStream(text);
				if (stream == null)
				{
					text = string.Concat((string[])(object)new string[6]
					{
						(string)(object)entryAssembly.GetName().Name,
						".Localization.",
						(string)(object)DefaultLanguage(),
						".",
						(string)(object)string_0,
						".xml"
					});
					stream = entryAssembly.GetManifestResourceStream(text);
				}
			}
			if (dataSet_0 == null)
			{
				dataSet_0 = new DataSet();
			}
			if (stream != null)
			{
				DataSet dataSet = new DataSet();
				dataSet.ReadXml(stream);
				dataSet_0.Merge(dataSet);
				stream.Close();
			}
		}

		private string method_1(object object_0)
		{
			if (object_0 != null)
			{
				return ((object)object_0).ToString();
			}
			return "";
		}

		public string translate(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return "";
			}
			if (dataSet_0 != null)
			{
				if (dataSet_0.Tables["Localization"] != null)
				{
					DataRow[] array = dataSet_0.Tables["Localization"].Select("Key='" + key + "'");
					if (array.Length <= 0)
					{
						return "~" + key;
					}
					return ((object)array[0]["Value"]).ToString();
				}
				return "&" + key;
			}
			return "&" + key;
		}

		public string translate(string key, object var1)
		{
			string text = translate(key);
			return ((string)(object)text).Replace("#1", method_1(var1));
		}

		public string translate(string key, object var1, object var2)
		{
			string text = translate(key);
			text = ((string)(object)text).Replace("#1", method_1(var1));
			return ((string)(object)text).Replace("#2", method_1(var2));
		}

		public string getValue(string key, object var1, object var2, object var3)
		{
			string text = translate(key);
			text = ((string)(object)text).Replace("#1", method_1(var1));
			text = ((string)(object)text).Replace("#2", method_1(var2));
			return ((string)(object)text).Replace("#3", method_1(var3));
		}

		public string getValue(string key, object var1, object var2, object var3, object var4)
		{
			string text = translate(key);
			text = ((string)(object)text).Replace("#1", method_1(var1));
			text = ((string)(object)text).Replace("#2", method_1(var2));
			text = ((string)(object)text).Replace("#3", method_1(var3));
			return ((string)(object)text).Replace("#4", method_1(var4));
		}

		public string getValue(string key, object var1, object var2, object var3, object var4, object var5)
		{
			string text = translate(key);
			text = ((string)(object)text).Replace("#1", method_1(var1));
			text = ((string)(object)text).Replace("#2", method_1(var2));
			text = ((string)(object)text).Replace("#3", method_1(var3));
			text = ((string)(object)text).Replace("#4", method_1(var4));
			return ((string)(object)text).Replace("#5", method_1(var5));
		}

		internal static bool kh6XqDhEQqcUCiepMW3()
		{
			return Grv83vh53MyypkjBmRF == null;
		}

		internal static void hvjwlrh2S7eVDD0Alyo()
		{
		}
	}
}
