using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.IMGEditor.GUI
{
	[Serializable]
	/// <summary>
	/// Defines a method to be used sorting files inside an archive
	/// </summary>
	public enum SortMethod
	{
		DEFAULT = 0,
		ALPHABETICAL = 1,
		SIZE = 2,
		OFFSET = 3
	}
}
