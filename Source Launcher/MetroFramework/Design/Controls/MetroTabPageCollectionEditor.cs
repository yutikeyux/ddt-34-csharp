using System;
using System.ComponentModel.Design;
using MetroFramework.Controls;

namespace MetroFramework.Design.Controls
{
	internal class MetroTabPageCollectionEditor : CollectionEditor
	{
		private static MetroTabPageCollectionEditor twonbXNsbpN3Qm7l5c4;

		protected override CollectionForm CreateCollectionForm()
		{
			CollectionForm collectionForm = base.CreateCollectionForm();
			collectionForm.Text = "MetroTabPage Collection Editor";
			return collectionForm;
		}

		public MetroTabPageCollectionEditor(Type type)
			: base(type)
		{
		}

		protected override Type CreateCollectionItemType()
		{
			return Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(MetroTabPage).TypeHandle);
		}

		protected override Type[] CreateNewItemTypes()
		{
			return new Type[1] { Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(MetroTabPage).TypeHandle) };
		}

		internal static bool m0ED9INNn6eHYyLnaAZ()
		{
			return twonbXNsbpN3Qm7l5c4 == null;
		}

		internal static void puHM7JNeX4RSqyiP6VU()
		{
		}
	}
}
