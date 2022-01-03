using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCRUD.Shared.Utilities
{
	public class DelegateExposer
	{
		private Delegate _del;

		public DelegateExposer(Delegate del)
		{
			_del = del;
		}

		public void Add(Delegate del)
		{
			_del = Delegate.Combine(del, _del);
		}

		public void Remove(Delegate del)
		{
			_del = Delegate.Remove(_del, del)!;
		}
	}
}
